﻿using Store.Data.Entities.IdinitiesEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.TokenService
{
    public interface ITokenService
    {
        string GenerateToken(AppUser appUser);
    }
}
