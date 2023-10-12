using HouseRentingSystem.Web.ViewModels.Agent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Web.ViewModels.House
{
    public class HouseDetailsViewModel : HouseAllViewModel
    {
        public string Description { get; set; } = null!;
        public string Category { get; set; } = null!;
        public AgentInfoOnHouseViewModel Agent { get; set; } = null!;

    }
}
