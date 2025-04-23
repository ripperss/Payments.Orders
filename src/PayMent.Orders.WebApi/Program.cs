using FluentValidation.AspNetCore;
using PayMent.Orders.WebApi.Extensions;
using PayMents.Orders.Application.Settings;
using Hangfire;
using PayMent.Orders.WebApi.Backgroundservices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());

builder.Services.Configure<AppSettings>
    (builder.Configuration.GetSection(nameof(AppSettings)));
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection(nameof(EmailSettings)));
builder.Services.Configure<RabbtiMqSettings>
    (builder.Configuration.GetSection(nameof(RabbtiMqSettings)));

builder.AddSwagger()
    .AddData()
    .AddApplicationService()
    .AddIntegrationService()
    .AddBearerAuthorizetion(builder.Configuration)
    .AddHangfire(builder.Configuration)
    .AddBackgroundService();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    DashboardTitle = "Hangfire Dashboard"
});

RecurringJob.AddOrUpdate<NotificationEmailBackground>(
    "notification",
    x => x.PushEmailMessage(),
    Cron.Daily);


app.MapControllers();

app.Run();
