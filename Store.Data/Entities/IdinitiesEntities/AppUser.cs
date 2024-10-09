using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.Entities.IdinitiesEntities
{
    public class AppUser:IdentityUser
    {
        public string DisplayName { get; set; }
        public Address Address { get; set; }

        public static implicit operator AppUser(AppUser v)
        {
            throw new NotImplementedException();
        }
    }   
}
