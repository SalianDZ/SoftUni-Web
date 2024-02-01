using HouseRentingSystem.Web.ViewModels.House.Enums;
using System.ComponentModel.DataAnnotations;
using static HouseRentingSystem.Common.GeneralApplicationConstants;

namespace HouseRentingSystem.Web.ViewModels.House
{
	public class AllHousesQueryModel
	{
        public AllHousesQueryModel()
        {
            CurrentPage = DefaultPage;
            HousesPerPage = EntitiesPerPage;
            Categories = new HashSet<string>();
            Houses = new HashSet<HouseAllViewModel>();
        }

        public string? Category { get; set; }

        public string? SearchString { get; set; }

        public HouseSorting HouseSorting { get; set; }

        public int CurrentPage { get; set; }

        [Display(Name = "Show houses on page")]
        public int HousesPerPage { get; set; }

        public int TotalHouses { get; set; }

        public IEnumerable<string> Categories { get; set; } = null!;

        public IEnumerable<HouseAllViewModel> Houses { get; set; }
    }
}
