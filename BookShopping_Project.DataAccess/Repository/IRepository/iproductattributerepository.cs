using BookShopping_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopping_Project.DataAccess.Repository.IRepository
{
   public  interface iproductattributerepository:IRepository<pc>
    {
        void Update(pc category);
    }
}
