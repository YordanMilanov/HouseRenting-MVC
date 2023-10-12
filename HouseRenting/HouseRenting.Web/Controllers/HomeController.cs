


using HouseRentingSystem.Services.Interfaces;
using HouseRentingSystem.Web.Infrastructure.Extensions;
using HouseRentingSystem.Web.ViewModels.Home;
using HouseRentingSystem.Web.ViewModels.House;
using Microsoft.Extensions.Primitives;

namespace HouseRenting.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;

    using HouseRenting.Web.Models;


    public class HomeController : Controller
    {
        private readonly IHouseService houseService;
        private readonly IAgentService agentService;

        public HomeController(IHouseService houseService, IAgentService agentService)
        {
            this.houseService = houseService;
            this.agentService = agentService;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<IndexViewModel> viewModel =
               await this.houseService.LastThreeHousesAsync();

            return View(viewModel);
        }

       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statusCode)
        {
            if (statusCode == 400 || statusCode == 404)
            {
                return this.View("Error404");
            }

            return View();
        }
    }
}
