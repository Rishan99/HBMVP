using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using HouseBrokerMVP.Core.Model;

namespace HouseBrokerMVP.Core.Entities
{
    public class Property : DateTimeModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public decimal Price { get; set; }
        public string Description { get; set; } = null!;
        public int PropertyTypeId { get; set; }
        public string BrokerId { get; set; } = null!;
        public virtual PropertyType PropertyType { get; set; } = null!;

        public virtual ICollection<PropertyImage> PropertyImages { get; set; } = new List<PropertyImage>();

        public virtual ApplicationUser Broker { get; set; } = null!;

    }
}
