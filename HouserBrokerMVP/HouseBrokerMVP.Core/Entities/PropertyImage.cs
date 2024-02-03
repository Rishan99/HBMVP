using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HouseBrokerMVP.Core.Model;

namespace HouseBrokerMVP.Core.Entities
{
    public class PropertyImage : DateTimeModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ImageName { get; set; } = null!;
        public int PropertyId { get;set; }

        public virtual Property Property { get; set; } = null!;
    }
}
