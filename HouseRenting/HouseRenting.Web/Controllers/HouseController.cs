﻿using HouseRentingSystem.Services.Data.Models.House;
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
        private readonly IHouseService houseService;

        public HouseController(ICategoryService categoryService, IAgentService agentService, IHouseService houseService)
        {
            this.categoryService = categoryService;
            this.agentService = agentService;
            this.houseService = houseService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> All([FromQuery] AllHousesQueryModel queryModel)
        {
            AllHousesFilteredAndPagedServiceModel serviceModel =
                await this.houseService.AllAsync(queryModel);

            queryModel.Houses = serviceModel.Houses;
            queryModel.TotalHouses = serviceModel.TotalHousesCount;
            queryModel.Categories = await this.categoryService.AllCategoryNamesAsync();


            return this.View(queryModel);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            //TODO
            return this.Ok();
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

            try
            {
                HouseFormModel formModel = new HouseFormModel()
                {
                    Categories = await this.categoryService.AllCategoriesAsync()
                };

                return View(formModel);
            }
            catch (Exception)
            {
                this.TempData[ErrorMessage] = "Unexpected error occurred! ";

                return this.RedirectToAction("Index", "Home");
            }
           
        }

        [HttpPost]
        public async Task<IActionResult> Add(HouseFormModel model)
        {
            bool isAgent = await this.agentService
                .AgentExistsByUserIdAsync(this.User.GetId());
            if (!isAgent)
            {
                TempData[ErrorMessage] = "You must become an agent in order to add new houses!";

                return this.RedirectToAction("Become", "Agent");
            }

            bool categoryExists = await this.categoryService
                .ExistsById(model.CategoryId);
            if (!categoryExists)
            {
                //Adding model error to ModelState automatically makes ModelState invalid!
                ModelState.AddModelError(nameof(model.CategoryId), "Selected category does not exist!");
            }


            if (!this.ModelState.IsValid)
            {
                model.Categories = await this.categoryService.AllCategoriesAsync();
                return this.View(model);
            }

            try
            {
                string? agentId = await this.agentService
                    .GetAgentIdByUserIdAsync(this.User.GetId()!)!;

               string houseId = 
                   await this.houseService.CreateAndReturnIdAsync(model, agentId!);
               return this.RedirectToAction("Details", "House", new { id = houseId });
            }
            catch (Exception e)
            {
                this.ModelState.AddModelError(string.Empty,"Unexpected error!");
                model.Categories = await this.categoryService.AllCategoriesAsync();

                return this.View(model);
            }

            
        }

        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            List<HouseAllViewModel> myHouses = new List<HouseAllViewModel>();

            string userId = this.User.GetId()!;
            bool isUserAgent = await this.agentService
                .AgentExistsByUserIdAsync(userId);

            if (isUserAgent)
            {
                string? agentId =
                    await this.agentService.GetAgentIdByUserIdAsync(userId);

                myHouses.AddRange(await this.houseService.AllByAgentIdAsync(agentId!));

            }

            else
            {
                myHouses.AddRange(await this.houseService.AllByUserIdAsync(userId));
            }

            return View(myHouses);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(string id)
        {
            bool houseExists = await this.houseService.ExistsByIdAsync(id);
            if (!houseExists)
            {
                this.TempData[ErrorMessage] = "House with the provided id does not exist!";
                return this.RedirectToAction("All", "House");
            }

            try
            {
                HouseDetailsViewModel viewModel = await this.houseService
                    .GetDetailsByIdAsync(id);
                return View(viewModel);

            }
            catch (Exception)
            {
                this.TempData[ErrorMessage] = "Unexpected error occurred! ";

                return this.RedirectToAction("Index", "Home");
            }

        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            bool isUserAgent = await this.agentService
                .AgentExistsByUserIdAsync(this.User.GetId()!);

            if (!isUserAgent)
            {
                this.TempData[ErrorMessage] = "You must become an agent in order to edit house info!";

                return this.RedirectToAction("Become", "Agent");
            }



            string agentId = 
                await this.agentService.GetAgentIdByUserIdAsync(this.User.GetId()!);

            bool isAgentOwner = await this.houseService
                .IsAgentWithIdOwnerOfHouseWithIdAsync(id, agentId);

            if (!isAgentOwner)
            {
                this.TempData[ErrorMessage] = "You must the agent owner in order to edit house info!";
                return this.RedirectToAction("Mine", "House");

            }

            bool houseExists = await this.houseService.ExistsByIdAsync(id);
            if (!houseExists)
            {
                this.TempData[ErrorMessage] = "House with the provided id does not exist!";
                return this.RedirectToAction("Index", "Home");
            }

            try
            {
                HouseFormModel formModel = await this.houseService
                    .GetHouseForEditByIdAsync(id);
                formModel.Categories = await this.categoryService
                    .AllCategoriesAsync();
                return this.View(formModel);
            }
            catch (Exception)
            {
                this.TempData[ErrorMessage] = "Unexpected error occurred! ";

                return this.RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, HouseFormModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.Categories = await this.categoryService.AllCategoriesAsync();
                return this.View(model);
            }

            bool isUserAgent = await this.agentService
                .AgentExistsByUserIdAsync(this.User.GetId()!);

            if (!isUserAgent)
            {
                this.TempData[ErrorMessage] = "You must become an agent in order to edit house info!";

                return this.RedirectToAction("Become", "Agent");
            }



            string? agentId =
                await this.agentService.GetAgentIdByUserIdAsync(this.User.GetId()!);

            bool isAgentOwner = await this.houseService
                .IsAgentWithIdOwnerOfHouseWithIdAsync(id, agentId);

            if (!isAgentOwner)
            {
                this.TempData[ErrorMessage] = "You must the agent owner in order to edit house info!";
                return this.RedirectToAction("Mine", "House");

            }

            bool houseExists = await this.houseService.ExistsByIdAsync(id);
            if (!houseExists)
            {
                this.TempData[ErrorMessage] = "House with the provided id does not exist!";
                return this.RedirectToAction("All", "House");
            }

            try
            {
                await this.houseService.EditHouseByIdAndFormModel(id, model);
            }
            catch (Exception)
            {
                this.ModelState.AddModelError(string.Empty, "Unexpected error occurred while trying to update the house. Please try again!");

                model.Categories = await this.categoryService.AllCategoriesAsync();
                return this.View(model);
            }

            return this.RedirectToAction("Details", "House", new { id = id });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            bool isUserAgent = await this.agentService
                .AgentExistsByUserIdAsync(this.User.GetId()!);

            if (!isUserAgent)
            {
                this.TempData[ErrorMessage] = "You must become an agent in order to edit house info!";

                return this.RedirectToAction("Become", "Agent");
            }



            string agentId =
                await this.agentService.GetAgentIdByUserIdAsync(this.User.GetId()!);

            bool isAgentOwner = await this.houseService
                .IsAgentWithIdOwnerOfHouseWithIdAsync(id, agentId);

            if (!isAgentOwner)
            {
                this.TempData[ErrorMessage] = "You must the agent owner in order to edit house info!";
                return this.RedirectToAction("Mine", "House");

            }

            bool houseExists = await this.houseService.ExistsByIdAsync(id);
            if (!houseExists)
            {
                this.TempData[ErrorMessage] = "House with the provided id does not exist!";
                return this.RedirectToAction("Index", "Home");
            }

            try
            {
                HousePreDeleteDetailsViewModel viewModel = await this.houseService
                    .GetHouseForDeleteByIdAsync(id);
                return this.View(viewModel);
            }
            catch (Exception)
            {
                this.TempData[ErrorMessage] = "Unexpected error!";

                return this.RedirectToAction("Index", "Home");

            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id, HousePreDeleteDetailsViewModel model)
        {
            bool isUserAgent = await this.agentService
                .AgentExistsByUserIdAsync(this.User.GetId()!);

            if (!isUserAgent)
            {
                this.TempData[ErrorMessage] = "You must become an agent in order to edit house info!";

                return this.RedirectToAction("Become", "Agent");
            }



            string agentId =
                await this.agentService.GetAgentIdByUserIdAsync(this.User.GetId()!);

            bool isAgentOwner = await this.houseService
                .IsAgentWithIdOwnerOfHouseWithIdAsync(id, agentId);

            if (!isAgentOwner)
            {
                this.TempData[ErrorMessage] = "You must the agent owner in order to edit house info!";
                return this.RedirectToAction("Mine", "House");

            }

            bool houseExists = await this.houseService.ExistsByIdAsync(id);
            if (!houseExists)
            {
                this.TempData[ErrorMessage] = "House with the provided id does not exist!";
                return this.RedirectToAction("Index", "Home");
            }

            try
            {
                await this.houseService.DeleteHouseByIdAsync(id);
                this.TempData[WarningMessage] = "The house was deleted!";
                return this.RedirectToAction("Mine", "House");
            }
            catch (Exception)
            {
                this.TempData[WarningMessage] = "Unexpected error!";
                return this.RedirectToAction("Mine", "House");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Rent(string id)
        {
            bool houseExists = await this.houseService.ExistsByIdAsync(id);
            if (!houseExists)
            {
                this.TempData[WarningMessage] = "House with provided id does not exists";
                RedirectToAction("All", "House");
            }

            bool isHouseRented = await this.houseService.IsRented(id);

            if (isHouseRented)
            {
                this.TempData[WarningMessage] = "house is rented";
                RedirectToAction("All", "House");
            }

            bool isUserAgent = await this.agentService.AgentExistsByUserIdAsync(this.User.GetId()!);

            if (isUserAgent)
            {
                this.TempData[ErrorMessage] = "Agents can't rent houses";
                return RedirectToAction("Index", "Home");
            }

            try
            {
                await this.houseService.RentHouseAsync(id, this.User.GetId()!);
            }
            catch (Exception)
            {
                this.TempData[ErrorMessage] = "Unexpected error";
                return RedirectToAction("Index", "Home");
            }
            return this.RedirectToAction("Mine", "House");
        }

        [HttpPost]
        public async Task<IActionResult> Leave(string id)
        {
            bool houseExists = await this.houseService.ExistsByIdAsync(id);
            if (!houseExists)
            {
                this.TempData[WarningMessage] = "House with provided id does not exists";
                RedirectToAction("All", "House");
            }

            bool isHouseRented = await this.houseService.IsRented(id);

            if (!isHouseRented)
            {
                this.TempData[WarningMessage] = "house is not rented";
                RedirectToAction("All", "House");
            }

            bool isUserAgent = await this.agentService.AgentExistsByUserIdAsync(this.User.GetId()!);

            if (isUserAgent)
            {
                this.TempData[ErrorMessage] = "Agents can't rent houses";
                return RedirectToAction("Index", "Home");
            }

            bool isCurrentUserRenterOfTheHouse =
                await this.houseService.IsRentedByUserWithIdAsync(id, this.User.GetId()!);

            if (!isCurrentUserRenterOfTheHouse)
            {
                this.TempData["ErrorMessage"] = "You are not renter of the house!";
                return RedirectToAction("Mine", "House");
            }

            try
            {
                await this.houseService.LeaveHouseAsync(id);
                return RedirectToAction("Mine", "House");
            }
            catch (Exception)
            {
                this.TempData["ErrorMessage"] = "Unexpected error!";
                return RedirectToAction("Mine", "House");
            }
        }
    }
}
