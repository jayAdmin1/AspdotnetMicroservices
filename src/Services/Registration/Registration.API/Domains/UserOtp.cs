using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Registration.API.Domains
{
    public class UserOtp
    {
        [Key]
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int Otp { get;set; }
        public DateTime SendDateTime { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
