using Microsoft.EntityFrameworkCore;
using RockPaperScissors;
using RockPaperScissors.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

var constr = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<RpstestContext>(options => options.UseNpgsql(constr));

var app = builder.Build();

app.MapGrpcService<GreeterService>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();