using HouseRentingSystem.Web.ViewModels.House;

namespace HouseRentingSystem.Services.Data.Models.House
{
    public class AllHousesFilteredAndPagedServiceModel
	{
        public AllHousesFilteredAndPagedServiceModel()
        {
            Houses = new HashSet<HouseAllViewModel>();    
        }

        public int TotalHousesCount { get; set; }

        public IEnumerable<HouseAllViewModel> Houses { get; set; }
    }
}