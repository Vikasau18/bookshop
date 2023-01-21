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
     public  class OrderDetailsRepository:Repository<OrderDetails>,IOrderDetailsRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderDetailsRepository(ApplicationDbContext context):base(context)
        {
            _context = context;

        }

        public void Update(OrderDetails orderDetails)
        {
            _context.Update(orderDetails);
        }
    }
}
