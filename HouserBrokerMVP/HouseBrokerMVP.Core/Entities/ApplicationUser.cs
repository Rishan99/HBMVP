using Microsoft.AspNetCore.Identity;

namespace HouseBrokerMVP.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() : base() { }

        public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}
