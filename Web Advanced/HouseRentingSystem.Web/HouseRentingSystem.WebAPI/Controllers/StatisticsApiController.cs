using HouseRentingSystem.Services.Data.Interfaces;
using HouseRentingSystem.Services.Data.Models.Statistics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HouseRentingSystem.WebAPI.Controllers
{
	[Route("api/statistics")]
	[ApiController]
	public class StatisticsApiController : ControllerBase
	{
		private readonly IHouseService houseService;

        public StatisticsApiController(IHouseService houseService)
        {
            this.houseService = houseService;
        }

		[HttpGet]
		[Produces("application/json")]
		[ProducesResponseType(200, Type = typeof(StatisticsServiceModel))]
		[ProducesResponseType(400)]
		public async Task<IActionResult> GetStatistics()
		{
			try
			{
				StatisticsServiceModel serviceModel = await houseService.GetStatisticsAsync();
				return Ok(serviceModel);
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}
    }
}
