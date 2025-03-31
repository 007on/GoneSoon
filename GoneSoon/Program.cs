using GoneSoon.Infrastructure;
using GoneSoon.Infrastructure.GoneSoon.Data;
using GoneSoon.NotificationStrategies;
using GoneSoon.Repositories;
using GoneSoon.Services;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Net;

internal class Program
{
    private static async Task Main(string[] args)
    {
        await Task.Delay(10000);
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("Redis"));

        builder.Services.AddDbContext<GoneSoonDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });


        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetSection("Redis:ConnectionString").Value;
        });

        builder.Services.AddSingleton<INotificationStrategyFactory, NotificationStrategyFactory>();

        builder.Services.AddScoped<IRedisStorageService, RedisStorageService>();
        builder.Services.AddScoped<INoteRepository, RedisNoteRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<INotificationMethodRepository, NotificationMethodRepository>();
        builder.Services.AddScoped<INoteService, NoteService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<INotificationMethodService, NotificationMethodService>();
        builder.Services.AddScoped<INoteManager, NoteManager>();
        builder.Services.AddScoped<RedisKeyExpirationWatcher>();
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

        builder.WebHost.ConfigureKestrel(options =>
        {
            options.Listen(IPAddress.Any, 5000);
        });


        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var dbContext = services.GetRequiredService<GoneSoonDbContext>();
                dbContext.Database.Migrate();
                Console.WriteLine("✅ Migrations completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error running migrations: {ex.Message}");
                throw;
            }
        }


        app.Run();
    }
}
