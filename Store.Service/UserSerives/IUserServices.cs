using Store.Service.UserSerives.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.UserSerives
{
    public interface IUserServices
    {
        Task<UserDto>Login(LoginDto input);
        Task<UserDto> Register(RegisterDto input);
    }
}
