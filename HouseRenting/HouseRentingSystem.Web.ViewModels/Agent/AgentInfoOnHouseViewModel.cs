using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Web.ViewModels.Agent
{
    public class AgentInfoOnHouseViewModel
    {
        public string Email { get; set; } = null!;
        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; } = null!;

    }
}
