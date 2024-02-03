using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerMVP.Business.DTO
{
    public class LoginDto
    {
        public string EmailAddress { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
