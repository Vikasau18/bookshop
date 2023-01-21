
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopping_Project.Models.ViewModels
{
    public class pcvm
    {
        public Category Category { get; set; }
        public List<string> SelectedFruits { get; set; }
        public List<string> getting { get; set; }
        public List<SelectListItem> Cat { get; set; }
        public IList<SelectListItem> Attributesproducts { get; set; }
        public ICollection<pc> ap { get; set; } = new HashSet<pc>();


        public pcvm()
        {
            SelectedFruits = new List<string>();
            getting = new List<string>();
            Attributesproducts = new List<SelectListItem>();
        }
    }
}
