﻿using System;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using WebApi.Interfaces;
using WebApi.Services;

namespace WebApi
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BatteriesProjectDbContext>(
                options => options.UseNpgsql(configuration
                    .GetConnectionString("BatteriesConnectionOnRailway")));

            services.AddScoped<IBatteriesProjectDbContext>( provider => provider.GetService<BatteriesProjectDbContext>());
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            
            return services;
        }

        // private static string GetConnectionString(IConfiguration configuration)
        // {
        //     var connection = string.Empty;
        //     var connectionString = configuration.GetConnectionString("BatteriesConnection");
        //     var databaseUrl = Environment.GetEnvironmentVariable("DB_CONNECTION");
        //     var databaseUri = Environment.GetEnvironmentVariable("DATABASE_URL");
        //     
        //     // return string.IsNullOrEmpty(connectionString) 
        //     //     ? databaseUrl 
        //     //     : (string.IsNullOrEmpty(databaseUri)
        //     //         ? databaseUrl : BuildConnectionStringFromUrl(databaseUri));
        //     
        //     
        //
        //     return connection;
        // }
        
        private static string BuildConnectionStringFromUrl(string databaseUriParam)
        {
            var databaseUri = new Uri(databaseUriParam);
            var userInfo = databaseUri.UserInfo.Split(":");
            var builder = new NpgsqlConnectionStringBuilder()
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/'),
                SslMode = SslMode.Require,
                TrustServerCertificate = true
            };
            return builder.ToString();
        }
    }
}