using GoneSoon.Infrastructure;
using GoneSoon.Infrastructure.GoneSoon.Data;
using GoneSoon.Repositories;
using GoneSoon.Services;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost"));

        // Настройка DbContext
        builder.Services.AddDbContext<GoneSoonDbContext>(options =>
        {
            string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connectionString);
        });


        builder.Services.AddStackExchangeRedisCache(options =>
        {
            IConfigurationSection configurationSection = builder.Configuration.GetSection("Redis:ConnectionString");
            options.Configuration = configurationSection.Value;
        });
        
        builder.Services.AddScoped<IRedisStorageService, RedisStorageService>();
        builder.Services.AddScoped<INoteRepository, RedisNoteRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<INotificationMethodRepository, NotificationMethodRepository>();
        builder.Services.AddScoped<INoteService, NoteService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<INotificationMethodService, NotificationMethodService>();
        builder.Services.AddScoped<INoteManager, NoteManager>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
