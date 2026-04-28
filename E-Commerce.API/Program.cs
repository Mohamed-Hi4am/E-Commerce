using E_Commerce.API.Extensions;

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

            builder.Services.AddCoreServices(builder.Configuration);

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

            app.UseCors("Development");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            #endregion

            app.Run();

        }
    }
}