using Microsoft.OpenApi.Models;

namespace Store.Web.Extentions
{
    public static class SwaggerServiceExtension
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Store API",
                    Version = "V1",
                    Contact = new OpenApiContact
                    {
                        Name = "Rawana",
                        Email = "rawan@gmail.com"
                    }
                });

                var securityScheme = new OpenApiSecurityScheme
                {
                    Description = "Enter 'Bearer' followed by a space and then your token in the Authorization header. Example: 'Bearer your_token'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Id = "bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                };

                options.AddSecurityDefinition("bearer", securityScheme);

                var securityRequirement = new OpenApiSecurityRequirement
                {
                    { securityScheme, new[] { "bearer" } }
                };

                options.AddSecurityRequirement(securityRequirement);
            });

            return services;
        }
    }
}
