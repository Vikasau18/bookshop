using BookShopping_Project.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eshop.Models.ViewModels
{
    public class Myproductvm
    {
        public Product Product { get; set; }
        public IEnumerable<Product> ProductList { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}
