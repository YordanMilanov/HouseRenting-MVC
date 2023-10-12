using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Web.ViewModels.House
{
    public class HousePreDeleteDetailsViewModel
    {
        public string Title { get; set; } = null!;
        public string Address { get; set; } = null!;

        [Display(Name = "Image Link")] 
        public string ImageUrl { get; set; } = null!;
    }
}
