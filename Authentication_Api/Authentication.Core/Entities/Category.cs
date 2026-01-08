using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Core.Entities
{
    public class Category:BasicEntity<int>
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public virtual ICollection<Products> Products { get; set; } = new HashSet<Products>();
    }
}
