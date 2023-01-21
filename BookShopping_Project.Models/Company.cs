using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopping_Project.Models
{
    public class Company
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string StreatAddress { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        [Display(Name ="Postal Code")]
        public string PostalCode{ get; set; }
        [Required]
        [Display(Name ="Phone Number")]
        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name ="Is Authorized Company")]
        public bool ISAuthorizedCompany { get; set; }
    }
}
