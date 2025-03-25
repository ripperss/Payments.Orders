using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PayMent.Orders.Domain.Data;
using PayMents.Orders.Application.Abstractions;
using PayMents.Orders.Application.MapperProfiels;
using PayMents.Orders.Application.MapperProfile;
using PayMents.Orders.Application.Service;

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

        return builder;
    }

    public static WebApplicationBuilder AddAplicationService(this WebApplicationBuilder builder) 
    {
        builder.Services.AddScoped<ICartService, CartService>();
        builder.Services.AddScoped<IOrderService,OrderService>();

        builder.Services.AddAutoMapper(
              typeof(OrderProfile)
            , typeof(CartItemProfile)
            , typeof(OrderProfile));
        
        return builder;
    }

    public static WebApplicationBuilder AddIntegrationService(this WebApplicationBuilder builder)
    {
        return builder;
    }
}
