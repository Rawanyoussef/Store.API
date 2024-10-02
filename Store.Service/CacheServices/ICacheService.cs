using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.CacheServices
{
    public interface ICacheService
    {
        Task SetCscheRespponseAsync(string Key, object response, TimeSpan timetolive);
        Task<string> GetCscheRespponseAsync(string key);
    }
}
