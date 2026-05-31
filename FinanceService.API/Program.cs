using FinanceService.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplicationServices()
    .AddSwagger();

builder.Services.AddControllers();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();