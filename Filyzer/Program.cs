
using Filyzer.Domain.Interfaces;
using Filyzer.Infrastructure.Data;
using Filyzer.Infrastructure.Repositories;
using Filyzer.Middleware;
using Microsoft.EntityFrameworkCore;

namespace Filyzer;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        var app = builder.Build();

        builder.Services.AddDbContext<FilyzerDbContext>(options =>
           options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddScoped<IUserRepository, UserRepository>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseMiddleware<ApiKeyMiddleware>();
        app.UseMiddleware<RoleAuthorizationMiddleware>();
        app.MapControllers();

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<FilyzerDbContext>();
            dbContext.Database.Migrate();
        }

        app.Run();
    }
}
