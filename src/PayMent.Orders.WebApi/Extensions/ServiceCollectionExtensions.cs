using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PayMent.Orders.Domain.Data;
using PayMent.Orders.Domain.Items;
using PayMent.Orders.Domain.Models;
using PayMents.Orders.Application.Abstractions;
using PayMents.Orders.Application.MapperProfiels;
using PayMents.Orders.Application.MapperProfile;
using PayMents.Orders.Application.Service;
using PayMents.Orders.Application.Settings;
using System.Runtime.CompilerServices;
using System.Text;

namespace PayMent.Orders.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Orders Api",
                Version = "v1", 
            });

            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please Enter a valid  token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
            }); 
        });

        return builder;
    }

    public static WebApplicationBuilder AddData(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<OrdersDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("DataBase"));
        });

        builder.Services.AddIdentity<UserIdentity, IdentityRoleEntity>(options =>
        {
            options.SignIn.RequireConfirmedEmail = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.User.RequireUniqueEmail = true;
        })
            .AddEntityFrameworkStores<OrdersDbContext>() 
            .AddDefaultTokenProviders();

        return builder;
    }

    public static WebApplicationBuilder AddApplicationService(this WebApplicationBuilder builder) 
    {
        builder.Services.AddAutoMapper(typeof(OrderProfile));
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddScoped<ICartService, CartService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IEmailService, EmailService>();
        builder.Services.AddScoped<IRoleInitializerService, RoleInitializerService>();

        
        return builder;
    }

    public static WebApplicationBuilder AddIntegrationService(this WebApplicationBuilder builder)
    {
        return builder;
    }

    public static WebApplicationBuilder AddBearerAuthorizetion(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        var authSettings = configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            

        }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(authSettings.TokenPrivateKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy =>
            {
                policy.RequireRole(Role.Admin);
            });

            options.AddPolicy("User", policy =>
            {
                policy.RequireRole(Role.User);
            });

            options.AddPolicy("Merchant", policy =>
            {
                policy.RequireRole(Role.Merchant);
            });
        });

        builder.Services.AddScoped<IAuthService, AuthService>();
        
        return builder;
    }

    public static WebApplicationBuilder AddHangfire(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Services.AddHangfire(options =>
        {
            options.UsePostgreSqlStorage(configuration.GetConnectionString("DataBase"));
        });

        builder.Services.AddHangfireServer();

        return builder;
    }
}
