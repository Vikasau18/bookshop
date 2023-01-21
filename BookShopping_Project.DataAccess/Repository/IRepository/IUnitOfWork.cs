using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopping_Project.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IShoppingCartRepository ShoppingCart { get; }
        IOrderHeaderRepository OrderHeader { get; }
        IOrderDetailsRepository OrderDetails { get; }
        ICategoryRepository Category{get;}
        ICoverTypeRepository CoverType { get; }
        ISP_CALL SP_CALL { get; }
        IProductRepository Product { get; }
        IApplicationUserRepository ApplicationUser { get; }
        ICompanyRepository Company { get; }
        iproductattributerepository pc { get; }

        void Save();
    
    }
}
