using Microsoft.AspNetCore.Mvc;
using Store.Repository.Interfaces;
using Store.Repository.Repositories;
using Store.Service.Services.ProductServices.Dtos;
using Store.Service.Services.ProductServices;
using Store.Service.Services;
using Store.Service.HandelResponse;
using Store.Service.CacheServices;
using Store.Service.Services.basketService;
using Store.Service.Services.BasketServiceDtos;
using Store.Service.Services.basketService.CustomerBasketDto;
using Store.Repository.Basket;
using Store.Service.TokenService;
using Store.Service.Services.OrderService;
using Store.Service.Services.OrderService.Dtos;

namespace Store.Web.Extentions
{
    public static class ApplicationServiceExetension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<ITokenService, TokenService>();
           services.AddScoped<IOrderService, OrderService>(); 
            services.AddScoped<IBasketRepository, BasketReposatpry>();       
            services.AddAutoMapper(typeof(BasketProfile));
            services.AddAutoMapper(typeof(ProductProfile));
            services.AddAutoMapper(typeof(OrderProfile));


            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState.
                                Where(model => model.Value?.Errors.Count > 0)
                                .SelectMany(model => model.Value?.Errors)
                                .Select(error => error.ErrorMessage).ToList();


                    var errorresponse = new ValidationErrorResponse
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(errorresponse);
                };
            });
            return services;


        }
    }
}
