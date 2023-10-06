using HouseRentingSystem.Services.Interfaces;
using HouseRentingSystem.Web.Infrastructure.Extensions;
using HouseRentingSystem.Web.ViewModels.House;
using static HouseRentingSystem.Common.NotificationMessagesConstants;

namespace HouseRenting.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class HouseController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IAgentService agentService;

        public HouseController(ICategoryService categoryService, IAgentService agentService)
        {
            this.categoryService = categoryService;
            this.agentService = agentService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            bool isAgent = await this.agentService
                .AgentExistsByUserIdAsync(this.User.GetId());
            if (!isAgent)
            {
                TempData[ErrorMessage] = "You must become an agent in order to add new houses!";

                return this.RedirectToAction("Become", "Agent");
            }
            HouseFormModel formModel = new HouseFormModel()
            {
                Categories = await this.categoryService.AllCategoriesAsync()
            };

            return View(formModel);
        }
    }
}
