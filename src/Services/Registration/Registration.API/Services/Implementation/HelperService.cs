using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using Registration.API.Domains;
using Registration.API.Services.Abstration;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Registration.API.ViewModels;
using Microsoft.Extensions.Options;

namespace Registration.API.Services.Implementation
{
    public class HelperService : IHelperService
    {
        private readonly IConfiguration _configuration;
        public EmailSettings _emailSettings { get; }
        public HelperService(IConfiguration configuration, IOptions<EmailSettings> emailSettings)
        {
            _configuration = configuration;
            _emailSettings = emailSettings.Value;
        }
        public string Authenticate(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);

            var claims = new Dictionary<string, object>() {
                { ClaimTypes.Name,user.Name},
                { ClaimTypes.Email, user.EmailAddress }
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"],
                Subject = new ClaimsIdentity(new List<Claim> { new Claim("sub", user.Id.ToString()) }),
                Claims = claims,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public (string, byte[]) PasswordSalt(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetNonZeroBytes(salt);
            }
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
                ));
            return (hashed, salt);
        }
        public bool VerifyPassword(string enteredPassword, byte[] passwordSalt, string storedPassword)
        {
            string encryptedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: storedPassword,
                    salt: passwordSalt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));
            if (enteredPassword == encryptedPassword)
                return true;
            return false;
        }

        public int GenerateRandomOtp()
        {
            int min = 10000;
            int max = 99999;
            Random random = new Random();
            return random.Next(min, max);
        }

        //Email send code using smtp client
        public async Task SendEmail(string email, string subject, string message)
        {

            MailMessage mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(_configuration["EmailCredentials:Username"], "EShop");
            mailMessage.To.Add(new MailAddress(email));
            mailMessage.Subject = subject;
            mailMessage.Body = message;
            var client = new SmtpClient(_emailSettings.Host, _emailSettings.Port)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_emailSettings.FromAddress, _emailSettings.Password)
            };
            await client.SendMailAsync(mailMessage);
        }
    }
}
