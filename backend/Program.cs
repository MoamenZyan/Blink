using System.Text.Json.Serialization;
using StackExchange.Redis;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

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

// Configure Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocal", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Dependancy Injection
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost"));
builder.Services.AddScoped<IRedisCache, RedisCache>();
builder.Services.AddScoped<IRepository<Post>, PostsRepository>();
builder.Services.AddScoped<IRepository<User>, UsersRepository>();
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<UserService>();

// To prevent reference cycle
builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                options.JsonSerializerOptions.WriteIndented = true;
            });

// Loading Environment Variables
Env.Load();

builder.Services.AddControllers();

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseMySql(Env.GetString("DATABASE_CONNECTION_STRING"), new MySqlServerVersion(new Version()));
});

var app = builder.Build();

app.UseCors("AllowLocal");
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blink API Documentation");
});

app.Run();
