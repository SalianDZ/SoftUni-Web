using System.ComponentModel.DataAnnotations;
using static HouseRentingSystem.Common.EntityValidationConstants.Agent;

namespace HouseRentingSystem.Web.ViewModels.Agent
{
	public class BecomeAgentFormModel
	{
		[Required]
		[StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength)]
		[Phone]
		[Display(Name = "Phone")]
		public string PhoneNumber { get; set; } = null!; 
    }
}
