using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "Blink API Documentation",
            Version = "v1"
        }
     );
     var filePath = Path.Combine(AppContext.BaseDirectory, "MyApi.xml");
     c.IncludeXmlComments(filePath);
});
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IRepository<User>, UserRepository>();
// Loading Environment Variables
DotNetEnv.Env.Load();

builder.Services.AddControllers();

builder.Services.AddDbContext<DatabaseContext>(options => 
    options.UseMySql(DotNetEnv.Env.GetString("DATABASE_CONNECTION_STRING"), new MySqlServerVersion(new Version())));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    // Database Initialization
    DB.Init(serviceProvider);
}


app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blink API Documentation");
});

app.Run();
