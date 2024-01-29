using Microsoft.AspNetCore.Identity;

namespace HouseRentingSystem.Data.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            RentedHouse = new HashSet<House>();
            Id = Guid.NewGuid();
        }

        public virtual ICollection<House> RentedHouse { get; set; }
    }
}
