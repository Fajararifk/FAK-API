using System.ComponentModel.DataAnnotations;

namespace FAK.Domain.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string Role { get; set; }
        public string Message {  get; set; }
        public DateTime ForgotTime {  get; set; }
        public DateTime RequestResetTime { get; set;}

    }
}
