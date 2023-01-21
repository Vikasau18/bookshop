using BookShopping_Project.DataAccess.Repository.IRepository;
using BookShopping_Project.Models;
using BookShopping_Project1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopping_Project.DataAccess.Repository
{
     public  class ProductAttributeRepository:Repository<pc>,iproductattributerepository
    {
        private readonly ApplicationDbContext _context;
        public ProductAttributeRepository(ApplicationDbContext context):base(context)
        {
            _context = context;

        }
        public void Update(pc obj)
        {
            _context.Update(obj);
        }
    }
}
