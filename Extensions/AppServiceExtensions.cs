﻿using API.Data;
using API.DTOs;
using API.Helpers;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class AppServiceExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration conf)
    {
        services.AddDbContext<DataContext>(opt =>
        {
            opt.UseSqlite(conf.GetConnectionString("DefaultConnection"));
        });
        services.AddCors();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IImageService, ImageService>();
        services.AddScoped<LogUserActivity>();
        services.AddScoped<IlikesRepository, LikesRepository>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.Configure<CloudinarySettings>(conf.GetSection("CloudinarySettings"));

        return services;
    }
}