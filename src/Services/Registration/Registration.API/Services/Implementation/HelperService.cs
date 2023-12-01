using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using Registration.API.Domains;
using Registration.API.Services.Abstration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Registration.API.Services.Implementation
{
    public class HelperService : IHelperService
    {
        private readonly IConfiguration _configuration;
        public HelperService(IConfiguration configuration)
        {
            _configuration = configuration;
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
    }
}
