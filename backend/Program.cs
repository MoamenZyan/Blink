using DotNetEnv;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Loading Environment Variables
DotNetEnv.Env.Load();

builder.Services.AddDbContext<DatabaseContext>(options => 
    options.UseMySql(DotNetEnv.Env.GetString("DATABASE_CONNECTION_STRING"), new MySqlServerVersion(new Version())));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    // Database Initialization
    DB.Init(serviceProvider);
}



app.Run();
