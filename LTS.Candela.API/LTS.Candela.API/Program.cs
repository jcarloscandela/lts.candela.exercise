using LTS.Candela.API.Data;
using LTS.Candela.API.Services;
using LTS.Candela.API.Repositories;
using Microsoft.EntityFrameworkCore;
using LTS.Candela.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// Database
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseInMemoryDatabase("CandelaInMemoryDb"));
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
}

// Services
builder.Services.AddControllers();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseMiddleware<ExceptionHandling>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

// Seed or migrate database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (app.Environment.IsDevelopment())
    {
        SeedDevelopmentData(dbContext);
    }
    else
    {
        dbContext.Database.Migrate();
    }
}

app.Run();

void SeedDevelopmentData(ApplicationDbContext dbContext)
{
    if (dbContext.Users.Any()) return;

    var users = Enumerable.Range(1, 15).Select(i =>
        new LTS.Candela.API.Models.User
        {
            Id = Guid.NewGuid(),
            Name = $"Test User {i}",
            Email = $"testuser{i}@test.com",
            TranslationCredits = i * 100,
            DateCreated = DateTime.UtcNow,
            DateModified = DateTime.UtcNow
        }
    );
    dbContext.Users.AddRange(users);
    dbContext.SaveChanges();
}
