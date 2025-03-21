using PayMent.Orders.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.AddSwagger()
    .AddData()
    .AddAplicationService()
    .AddIntegrationService();

var app = builder.Build();




app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
