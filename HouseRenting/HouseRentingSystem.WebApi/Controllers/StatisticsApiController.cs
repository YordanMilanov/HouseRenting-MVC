using System.Net;
using HouseRentingSystem.Services.Data.Models.Statistics;
using HouseRentingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.WebApi.Controllers
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

        [HttpGet] //200 is HttpStatusCode.OK <- returns staisticsServiceModel for 200 response!
        [ProducesResponseType(200, Type = typeof(StatisticsServiceModel))]
        [ProducesResponseType(400)]
        [Produces("application/json")] // <- MIME type that the method will return is JSON!
        public async Task<IActionResult> GetStatistics()
        {
            try
            {
                StatisticsServiceModel serviceModel = 
                    await this.houseService.GetStatisticsAsync();
                return this.Ok(serviceModel); //this will return the serviceModel in JSON
            } catch(Exception)
            {
                return this.BadRequest("Bad request error message");
            }
        }
    }
}
