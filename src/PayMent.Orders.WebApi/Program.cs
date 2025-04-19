using PayMent.Orders.WebApi.Extensions;
using PayMents.Orders.Application.Settings;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<AppSettings>
    (builder.Configuration.GetSection(nameof(AppSettings)));
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection(nameof(EmailSettings)));

builder.AddSwagger()
    .AddData()
    .AddApplicationService()
    .AddIntegrationService()
    .AddBearerAuthorizetion(builder.Configuration)
    .AddHangfire(builder.Configuration);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    DashboardTitle = "Hangfire Dashboard"
});

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
