using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Store.Data.Contexts;
using Store.Data.Entities.IdinitiesEntities;
using System.Text;

namespace Store.Web.Extentions
{
    public  static class IDentityServicesExtentions
    {
        public static IServiceCollection AddIdentiyServices(this IServiceCollection services ,IConfiguration _configuration)
        { 
            var builder = services.AddIdentityCore<AppUser>();
            builder = new IdentityBuilder(builder.UserType ,builder.Services);
            builder.AddEntityFrameworkStores<StoreIdentityDbContext>();
            builder.AddSignInManager<SignInManager<AppUser>>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(option =>
                {
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokrn:key"])),
                        ValidateIssuer=true,
                        ValidIssuer = _configuration["Token:Issuer"],
                        ValidateAudience=false
                    };
                });
            return services;
        }
    }
}
