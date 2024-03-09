using System.Security.Claims;
using static HouseRentingSystem.Common.GeneralApplicationConstants;

namespace HouseRentingSystem.Web.Infrastructure.Extensions
{
	public static class ClaimsPrincipleExtensions
	{
		public static string? GetId(this ClaimsPrincipal user)
		{
			return user.FindFirstValue(ClaimTypes.NameIdentifier);
		}

		public static bool IsAdmin(this ClaimsPrincipal user)
		{
			return user.IsInRole(AdminRoleName);
		}
	}
}
