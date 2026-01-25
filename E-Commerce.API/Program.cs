
using Domain.Contracts;
using E_Commerce.API.Extensions;
using E_Commerce.API.Factories;
using E_Commerce.API.MiddleWares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Repositories;
using Services;
using Services.Abstraction.Contracts;
using Services.Implementations;

namespace E_Commerce.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services (DI Container)
            
            // Add services to the container.
            
            builder.Services.AddWebApisServices();

            builder.Services.AddInfrastructureServices(builder.Configuration);

            builder.Services.AddCoreServices();

            #endregion

            var app = builder.Build();

            #region Configure Kestrel MiddleWares
            
            // Configure the HTTP request pipeline.

            app.UseCustomExceptionMiddlewares();

            await app.SeedDbAsync();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddlewares();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthorization();

            app.MapControllers();

            #endregion

            app.Run();

        }
    }
}
