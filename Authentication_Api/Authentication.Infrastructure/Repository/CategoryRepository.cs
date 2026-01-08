using Authentication.Core.Entities;
using Authentication.Core.Interface;
using Authentication.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Infrastructure.Repository
{
   
        public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
        {
            public CategoryRepository(AppDbContext context) : base(context)
            {
            }
        }
    
}
