using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerMVP.Business.DTO
{
    public class RegisterAdminDto
    {
        public string EmailAddress { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
        public string? PhoneNumber { get; set; }
    }
    public class RegisterUserDto : RegisterAdminDto
    {
        public new string PhoneNumber { get; set; } = null!;
    }
}
