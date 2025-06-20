﻿using GoneSoon.InteractionProtocol.Services;
using GoneSoon.InteractionProtocol.UserService;
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

        builder.Services.AddHttpClient<UserServiceClient>(client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["UserService:BaseUrl"]);
        });
        builder.Services.AddHttpClient<NoteServiceClient>(client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["NoteService:BaseUrl"]);
        });

        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetSection("Redis:ConnectionString").Value;
        });

        builder.Services.AddScoped<INoteManager, NoteManager>();
        builder.Services.AddScoped<INoteServiceClient, NoteServiceClient>();
        builder.Services.AddScoped<IUserServiceClient, UserServiceClient>();
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

        app.Run();
    }
}
