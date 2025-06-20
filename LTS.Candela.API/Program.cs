using LTS.Candela.API.Data;
using Microsoft.EntityFrameworkCore;

namespace LTS.Candela.API
{
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
                            new Models.User { Id = Guid.NewGuid(), Name = "Test User 2", Email = "testuser1@test.com", TranslationCredits = 200 }
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
}
