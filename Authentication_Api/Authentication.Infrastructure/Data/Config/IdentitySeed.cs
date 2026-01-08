using Authentication.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Infrastructure.Data.Config
{
    public class IdentitySeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                //create new user
                var user = new AppUser
                {
                    DisplayName = "Sonu",
                    Email = "Sonurajmca@gmail.com",
                    UserName = "Sonurajmca",
                    address = new Address
                    {
                        FirstName = "sonu",
                        LastName = "Raj",
                        City = "Patna",
                        State = "Bihar",
                        Street = "AX",
                        PostalCode = "1212121"

                    }
                };
                await userManager.CreateAsync(user, "Password@123");

            }
        }
    }
}
