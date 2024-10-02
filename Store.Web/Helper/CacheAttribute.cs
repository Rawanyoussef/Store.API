using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Store.Service.CacheServices;
using System.Text;

namespace Store.Web.Helper
{
    public class CacheAttribute : Attribute, IAsyncActionFilter

    {
        private readonly int _timetolivesconds;

        public CacheAttribute(int timetolivesconds)
        {
            _timetolivesconds = timetolivesconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var _cacheservice = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            var cachekey= GeneratedCacheKey(context.HttpContext.Request);
            var cachedresposed = await _cacheservice.GetCscheRespponseAsync(cachekey);
            if (!string.IsNullOrEmpty(cachedresposed) )
            {
                var ContentResult = new ContentResult
                {
                    Content = cachedresposed,
                    ContentType = "application/json",
                    StatusCode = 200,
                };
                context.Result= ContentResult;
                return;
            }
            var executedcontext=await next();
            if (executedcontext.Result is OkObjectResult response) 
                await _cacheservice.SetCscheRespponseAsync(cachekey,response.Value,TimeSpan.FromSeconds(_timetolivesconds));
            
        }
        private string GeneratedCacheKey(HttpRequest request)
        {
            StringBuilder cachekey = new StringBuilder();
            cachekey.Append($"{request.Path}");
            foreach ( var item in request.Query.OrderBy(x=>x.Key ))
                cachekey.Append($"|{item.Key}-{item.Value}");
            return cachekey.ToString();
            
        }
    }
}