using PayMents.Orders.Application.Service;
using PayMent.Orders.WebApi.Extensions;
using PayMent.Orders.Domain.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<AppSettings>
    (builder.Configuration.GetSection(nameof(AppSettings)));

builder.AddSwagger()
    .AddData()
    .AddApplicationService()
    .AddIntegrationService()
    .AddBearerAuthorizetion(builder.Configuration);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
