using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopping_Project.Models
{
   public class pc
    {
        public int Id { get; set; }
        [Required]

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string Name { get; set; }
    }
}
