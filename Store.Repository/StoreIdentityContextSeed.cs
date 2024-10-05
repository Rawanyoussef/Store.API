using Microsoft.AspNetCore.Identity;
using Store.Data.Entities.IdinitiesEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository
{
    public class StoreIdentityContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager .Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "RAwanAbdeLrhman",
                    Email = "rawan@gmail.com",
                    UserName = "Rawana",
                    Address = new Address
                    {
                        FirstName = "Rawan",
                        LastName ="Abdelrhman",
                        city="Helwan",
                        state="cairo",
                        Street="5",
                        PostalCode="10",
                    }

                };
                await userManager.CreateAsync(user,"password123!");

            }
        }
    }
}
