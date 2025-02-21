using System.ComponentModel.DataAnnotations;

namespace HouseBrokerMVP.Business.DTO
{
    public class UserDto
    {
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public string? PhoneNumber { get; set; }
    }
}
