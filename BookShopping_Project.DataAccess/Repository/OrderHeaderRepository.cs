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
     public  class OrderHeaderRepository:Repository<OrderHeader>,IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderHeaderRepository(ApplicationDbContext context):base(context)
        {
            _context = context;

        }

        public void Update(OrderHeader orderHeader)
        {
            _context.Update(orderHeader);
        }
    }
}
