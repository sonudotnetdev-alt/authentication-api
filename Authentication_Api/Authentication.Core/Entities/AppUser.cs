using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Core.Entities
{
    public class AppUser: IdentityUser
    {
        public int Id { get; set; }        
        public string DisplayName { get; set; }

        public Address address { get; set; }

    }
}
