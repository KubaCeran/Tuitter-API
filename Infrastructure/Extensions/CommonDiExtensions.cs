using Core.DTOs.Options.Auth;
using Core.Entities;
using Infrastructure.DataContext;
using Infrastructure.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Core;
using System.Text;

namespace Infrastructure.Extensions
{
    public static class CommonDiExtensions
    {
        public static WebApplication CommonApiDiSetup(this WebApplicationBuilder builder)
        {
            //Database Configuration
            builder.Services.AddDataContext(builder.Configuration);

            //Identity Configuration
            builder.Services.AddIdentityServices(builder.Configuration);

            //Automapper auto registration of all profiles within solution
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //Api Configuration
            builder.Services.AddControllers(options =>
            {
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            });
            builder.Services.AddEndpointsApiExplorer();

            //Swagger Configuration
            builder.Services.AddSwagger();

            //Cors Configuration
            builder.Services.AddCors();

            //Serilog Configuration
            builder.Logging.ClearProviders();
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();
            builder.Host.UseSerilog(logger);

            //ExceptionsHandlingMiddleware
            builder.Services.AddHttpLogging(o => { });
            builder.Services.AddExceptionHandler<ExceptionsHandlingMiddleware>();
            builder.Services.AddProblemDetails();

            return builder.Build();
        }

        private static IServiceCollection AddCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
                });
            });

            return services;
        }

        private static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "JWTToken_Auth_API",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
                c.OperationFilter<SwaggerFileOperationFilter>();
            });

            return services;
        }

        private static IServiceCollection AddDataContext(this IServiceCollection services, IConfiguration config)
        {
            var dbConnection = config.GetConnectionString("DbConnection");
            services.AddDbContext<TuitterContext>(options => options.UseSqlServer(dbConnection));

            return services;
        }

        private static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            // For Identity
            services.AddIdentity<User, IdentityRole<int>>()
                .AddEntityFrameworkStores<TuitterContext>()
                .AddSignInManager()
                .AddRoles<IdentityRole<int>>();

            const string authSection = "JWT";
            services.Configure<AuthConfiguration>(config.GetSection(authSection));

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            });

            // Adding Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            // Adding Jwt Bearer
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidAudience = config[$"{authSection}:ValidAudience"],
                    ValidIssuer = config[$"{authSection}:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config[$"{authSection}:Secret"]!))
                };
            });

            return services;
        }
    }
}
