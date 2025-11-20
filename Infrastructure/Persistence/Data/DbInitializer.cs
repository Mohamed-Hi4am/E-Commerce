using Domain.Contracts;
using Domain.Entities.ProductModule;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Persistence.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly StoreDbContext _dbContext;

        public DbInitializer(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InitializeAsync()
        {
            try
            {
                // Create the database if it doesn't exist & apply any pending migrations

                if (_dbContext.Database.GetPendingMigrations().Any())
                    await _dbContext.Database.MigrateAsync();


                // Apply Data seeding

                if (!_dbContext.ProductTypes.Any())
                {
                    // Read Types From File As String

                    var typeData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\DataSeeding\types.json");

                    // Transform From Json into C# Object

                    var types = JsonSerializer.Deserialize<List<ProductType>>(typeData);

                    // Add Types Into DB & Save Changes

                    if (types is not null && types.Any())
                    {
                        await _dbContext.ProductTypes.AddRangeAsync(types);
                        // await _dbContext.SaveChangesAsync();
                    }

                }

                if (!_dbContext.ProductBrands.Any())
                {
                    // Read Brands From File As String

                    var brandData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\DataSeeding\brands.json");

                    // Transform From Json into C# Object

                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);

                    // Add Brands Into DB & Save Changes

                    if (brands is not null && brands.Any())
                    {
                        await _dbContext.ProductBrands.AddRangeAsync(brands);
                        // await _dbContext.SaveChangesAsync();
                    }

                }

                if (!_dbContext.Products.Any())
                {
                    // Read Products From File As String

                    var productData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\DataSeeding\products.json");

                    // Transform From Json into C# Object

                    var products = JsonSerializer.Deserialize<List<Product>>(productData);

                    // Add Products Into DB & Save Changes

                    if (products is not null && products.Any())
                    {
                        await _dbContext.Products.AddRangeAsync(products);
                        // await _dbContext.SaveChangesAsync();
                    }

                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
