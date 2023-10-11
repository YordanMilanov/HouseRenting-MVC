using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HouseRentingSystem.Web.ViewModels.House.Enums;
using HouseRentingSystem.Common;

namespace HouseRentingSystem.Web.ViewModels.House
{
    public class AllHousesQueryModel
    {

        public AllHousesQueryModel()
        {
            this.CurrentPage = GeneralApplicationConstants.DefaultPage;
            this.HousesPerPage = GeneralApplicationConstants.EntitiesPerPage;

            this.Categories = new HashSet<string>();
            this.Houses = new HashSet<HouseAllViewModel>();
        }
        public string? Category { get; set; }

        [DisplayName("Search by word")]
        public string? SearchString { get; set; }

        [DisplayName("Sort Houses By")]
        public HouseSorting HouseSorting { get; set; }

        public int CurrentPage { get; set; }

        public int TotalHouses { get; set; }
        public int HousesPerPage {get; set; }

        public IEnumerable<string> Categories { get; set; }

        public IEnumerable<HouseAllViewModel> Houses { get; set; }
    }
}
