using System.ComponentModel.DataAnnotations;

namespace PRAS_Task.Models
{
    public class LoginResponse
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
