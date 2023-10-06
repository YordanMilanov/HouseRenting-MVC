

using Azure.Core;
using HouseRentingSystem.Services.Interfaces;
using HouseRentingSystem.Web.Infrastructure.Extensions;
using HouseRentingSystem.Web.ViewModels.Agent;

namespace HouseRenting.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using static HouseRentingSystem.Common.NotificationMessagesConstants;

    [Authorize]
    public class AgentController : Controller
    {
        private readonly IAgentService agentService;


        public AgentController(IAgentService agentService)
        {
            this.agentService = agentService;
        }


        [HttpGet]
        public async Task<IActionResult> Become()
        {
            string? userId = this.User.GetId();

            bool isAgent = await this.agentService.AgentExistsByUserIdAsync(userId);
            if (isAgent)
            {
                TempData[ErrorMessage] = "You are already an agent!";
                return this.RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Become(BecomeAgentFormModel model)
        {
            string? userId = this.User.GetId();
            bool isAgent = await this.agentService.AgentExistsByUserIdAsync(userId);
            if (isAgent) {
                this.TempData[ErrorMessage] = "You are already an agent!";
                return this.RedirectToAction("Index", "Home");
            }

            bool isPhoneNumberTaken = await this.agentService
                .AgentExistsByPhoneNumberAsync(model.PhoneNumber);
            if (isPhoneNumberTaken)
            {
                ModelState.AddModelError(nameof(model.PhoneNumber), "Agent with this phone already exists!");
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            bool userHasActiveRents = await this.agentService
                .UserHasRentsByUserIdAsync(userId);
            if (userHasActiveRents)
            {
                this.TempData[ErrorMessage] = "You must not have any active rents in order to become an agent!";
                return RedirectToAction("Mine", "House");
            }

            //creating all validations passed
            try
            {
                await this.agentService.Create(userId, model);

            }
            catch (Exception e)
            {
                this.TempData[ErrorMessage] = "Unexpected error occurred while registering you as an agent!";
                return this.RedirectToAction("Index", "Home");
            }

            return this.RedirectToAction("All", "House");
        }

    }
}
