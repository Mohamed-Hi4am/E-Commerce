
using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Repositories;

namespace E_Commerce.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services (DI Container)
            // Add services to the container.
            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddControllers();
            builder.Services.AddScoped<IDbInitializer, DbInitializer>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(); 
            #endregion

            var app = builder.Build();

            await InitializeDbAsync(app);

            #region Configure Kestrel MiddleWares
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers(); 
            #endregion

            app.Run();

            async Task InitializeDbAsync(WebApplication app)
            {
                using var scope = app.Services.CreateScope();

                var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();

                await dbInitializer.InitializeAsync();
            }
        }
    }
}
