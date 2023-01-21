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
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Category = new CategoryRepository(_context);
            CoverType = new CoverTypeRepository(_context);
            SP_CALL = new SP_CALL(_context);
            Product=new  ProductRepository(_context);
            ApplicationUser = new ApplicationUserRepository(_context);
            Company = new CompanyRepository(_context);
            ShoppingCart = new ShoppingCartRepository(_context);
            OrderHeader = new OrderHeaderRepository(_context);
            OrderDetails = new OrderDetailsRepository(_context);
            pc = new ProductAttributeRepository(_context);

        }
        public IShoppingCartRepository ShoppingCart { get; private set; }

        public IOrderHeaderRepository OrderHeader { get; private set; }

        public IOrderDetailsRepository OrderDetails { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public ICoverTypeRepository CoverType { get; private set; }
        public ISP_CALL SP_CALL { get; private set; }

        public IProductRepository Product { get; private set; }

        public IApplicationUserRepository ApplicationUser { get; private set; }

        public ICompanyRepository Company { get; private set; }

        public iproductattributerepository pc { get; private set; }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
