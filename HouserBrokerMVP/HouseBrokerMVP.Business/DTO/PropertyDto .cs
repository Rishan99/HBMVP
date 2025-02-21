using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerMVP.Business.DTO
{
    public class PropertyListDto : PropertyUpdateDto
    {
        public int Id { get; set; }
        public UserDto Broker { get; set; } = null!;
        public PropertyTypeListDto PropertyType { get; set; } = null!;
        public new List<string> Images { get; set; } = new List<string>();
    }
    public class PropertyImageListDto
    {
        public string ImageName { get; set; } = null!;
    }
    public class PropertyInsertDto
    {
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public decimal Price { get; set; }
        public string Description { get; set; } = null!;
        public int PropertyTypeId { get; set; }
        public List<IFormFile> Images { get; set; } = new List<IFormFile>();

    }

    public class PropertyUpdateDto : PropertyInsertDto
    {
        public int Id { get; set; }
    }
    public class PropertyDeleteDto
    {
        public int Id { get; set; }
    }
}
