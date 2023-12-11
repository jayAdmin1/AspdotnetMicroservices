using System.ComponentModel.DataAnnotations;

namespace Registration.API.Domains
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<UserOtp> UserOtp { get; set; }
        protected User() { }
        public User(string name, string emailaddress, string mobileno, string address, string password, byte[] passwordsalt, bool isdeleted)
        {
            Name = name;
            EmailAddress = emailaddress;
            MobileNo = mobileno;
            Address = address;
            Password = password;
            PasswordSalt = passwordsalt;
            IsDeleted = isdeleted;
        }
    }
}
