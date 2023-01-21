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
     public  class ShoppingCartRepository:Repository<ShoppingCart>,IShoppingCartRepository
    {
        private readonly ApplicationDbContext _context;
        public ShoppingCartRepository(ApplicationDbContext context):base(context)
        {
            _context = context;

        }
      

        public void Update(ShoppingCart shoppingCart)
        {
            _context.Update(shoppingCart);
        }
    }
}
