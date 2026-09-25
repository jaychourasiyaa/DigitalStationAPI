using DigitalStation.Contract.Interfaces;
using DigitalStation.Contract.Models;
using DigitalStation.Contract.Utility;
using DigitalStation.Provider.Services;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
namespace DigitalStation.Provider
{
    public class Bootstrapper
    {

        public static void AddBootstapperServices(WebApplicationBuilder builder)
        {
            //Interface Dependency
            builder.Services.AddScoped<IEmailServices, EmailServices>();
            builder.Services.AddScoped<IJwtServices, JwtServices>();
            builder.Services.AddScoped<IUserServices, UserServices>();
            builder.Services.AddScoped<IAuthorizationServices, AuthServices>();
            builder.Services.AddScoped<IBrandServices,BrandServices>();
            builder.Services.AddScoped<ICategoryServices,CategoryServices>();
            builder.Services.AddScoped<IProductServices,ProductServices>();
            builder.Services.AddScoped<IAddressServices,AddressServices>();
            builder.Services.AddScoped<ICartServices,CartServices>();
            builder.Services.AddScoped<IWishlistServices,WishlistServices>();
            builder.Services.AddScoped<IOrderServices,OrderServices>();
            builder.Services.AddScoped<AuthServices>();
            //swagger services
            builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.UseInlineDefinitionsForEnums();
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Digital Station Api's",
                    Version = "v1"
                });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            //cloudinary
            builder.Services.Configure<CloudinarySettingsConfiguration>(builder.Configuration.GetSection("Cloudinary"));

            // Register Cloudinary
            builder.Services.AddSingleton(provider =>
            {
                var config = provider.GetRequiredService<IOptions<CloudinarySettingsConfiguration>>().Value;
                return new Cloudinary(new Account(config.CloudName, config.ApiKey, config.ApiSecret));
            });
            builder.Services.AddScoped<CloudinaryServices>();

        }
    }
}
