﻿using BookShopping_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopping_Project.DataAccess.Repository.IRepository
{
   public  interface ICategoryRepository:IRepository<Category>
    {
        void Update(Category category);
    }
}
