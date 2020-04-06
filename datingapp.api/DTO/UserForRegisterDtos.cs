using System.ComponentModel.DataAnnotations;

namespace datingapp.api.DTO
{
    public class UserForRegisterDtos
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(8,MinimumLength=4,ErrorMessage = "You Must specify password between 4 and 8")]
        public string Password { get; set; }
    }
}