using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerMVP.Business.DTO
{
    public class RoleAddDto
    {
        [Required]
        public string Name { get; set; } = null!;
    }
    public class RoleListDto : RoleAddDto
    {
    }
}
