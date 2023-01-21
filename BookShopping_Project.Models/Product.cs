using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopping_Project.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Discription { get; set; }
        [Required]
      
        public double ListPrice { get; set; }
        [Required]
        [Range(1,100000)]
        public double Price50 { get; set; }
        [Required]
        [Range(1,100000)]
        public double Price100 { get; set; }
        [Required]
        [Range(1,100000)]
        public double Price { get; set; }
        [Required]
        [Display(Name ="ImageUrl")]
        public string ImageUrl { get; set; }
        [Display(Name ="Category")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        [Display(Name ="CoverType")]
        public int CoverTypeId { get; set; }
        [ForeignKey("CoverTypeId")]
        public CoverType CoverType { get; set; }
    }
}
