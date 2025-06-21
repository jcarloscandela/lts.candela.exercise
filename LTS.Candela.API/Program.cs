using LTS.Candela.API.Data;
using LTS.Candela.API.Services;
using LTS.Candela.API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LTS.Candela.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

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

        builder.Services.AddControllers();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Add health checks
        builder.Services.AddHealthChecks();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();            
            app.UseSwaggerUI();
        }

        app.UseCors("AllowAll");

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        // Map health check endpoint
        app.MapHealthChecks("/health");

        // Apply migrations or seed data based on environment
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            if (app.Environment.IsDevelopment())
            {
                // Seed development data
                if (!dbContext.Users.Any())
                {
                    dbContext.Users.AddRange(
                        new Models.User { Id = Guid.NewGuid(), Name = "Test User 1", Email = "testuser1@test.com", TranslationCredits = 100 },
                        new Models.User { Id = Guid.NewGuid(), Name = "Test User 2", Email = "testuser2@test.com", TranslationCredits = 200 },
                        new Models.User { Id = Guid.NewGuid(), Name = "Test User 3", Email = "testuser3@test.com", TranslationCredits = 300 },
                        new Models.User { Id = Guid.NewGuid(), Name = "Test User 4", Email = "testuser4@test.com", TranslationCredits = 400 },
                        new Models.User { Id = Guid.NewGuid(), Name = "Test User 5", Email = "testuser5@test.com", TranslationCredits = 500 },
                        new Models.User { Id = Guid.NewGuid(), Name = "Test User 6", Email = "testuser6@test.com", TranslationCredits = 600 },
                        new Models.User { Id = Guid.NewGuid(), Name = "Test User 7", Email = "testuser7@test.com", TranslationCredits = 700 },
                        new Models.User { Id = Guid.NewGuid(), Name = "Test User 8", Email = "testuser8@test.com", TranslationCredits = 800 },
                        new Models.User { Id = Guid.NewGuid(), Name = "Test User 9", Email = "testuser9@test.com", TranslationCredits = 900 },
                        new Models.User { Id = Guid.NewGuid(), Name = "Test User 10", Email = "testuser10@test.com", TranslationCredits = 1000 },
                        new Models.User { Id = Guid.NewGuid(), Name = "Test User 11", Email = "testuser11@test.com", TranslationCredits = 1100 },
                        new Models.User { Id = Guid.NewGuid(), Name = "Test User 12", Email = "testuser12@test.com", TranslationCredits = 1200 },
                        new Models.User { Id = Guid.NewGuid(), Name = "Test User 13", Email = "testuser13@test.com", TranslationCredits = 1300 },
                        new Models.User { Id = Guid.NewGuid(), Name = "Test User 14", Email = "testuser14@test.com", TranslationCredits = 1400 },
                        new Models.User { Id = Guid.NewGuid(), Name = "Test User 15", Email = "testuser15@test.com", TranslationCredits = 1500 }
                    );

                    dbContext.SaveChanges();
                }
            }
            else
            {
                // Apply migrations in production
                dbContext.Database.Migrate();
            }
        }

        app.Run();
    }
}
