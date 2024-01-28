using Microsoft.AspNetCore.Identity;

namespace HouseRentingSystem.Data.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            RentedHouse = new HashSet<House>();
        }

        public virtual ICollection<House> RentedHouse { get; set; }
    }
}
