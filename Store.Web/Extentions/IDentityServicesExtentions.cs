using Microsoft.AspNetCore.Identity;
using Store.Data.Contexts;
using Store.Data.Entities.IdinitiesEntities;

namespace Store.Web.Extentions
{
    public  static class IDentityServicesExtentions
    {
        public static IServiceCollection AddIdentiyServices(this IServiceCollection services)
        { 
            var builder = services.AddIdentityCore<AppUser>();
            builder = new IdentityBuilder(builder.UserType ,builder.Services);
            builder.AddEntityFrameworkStores<StoreIdentityDbContext>();
            builder.AddSignInManager<SignInManager<AppUser>>();
            services.AddAuthentication();
            return services;
        }
    }
}
