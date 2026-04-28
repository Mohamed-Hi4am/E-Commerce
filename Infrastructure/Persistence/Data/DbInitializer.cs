using Domain.Contracts;
using Domain.Entities.IdentityModule;
using Domain.Entities.OrderModule;
using Domain.Entities.ProductModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Identity;
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
        private readonly StoreIdentityContext _identityContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(StoreDbContext dbContext, StoreIdentityContext identityContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _identityContext = identityContext;
            _userManager = userManager;
            _roleManager = roleManager;
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

                if (!_dbContext.DeliveryMethods.Any())
                {
                    // Read Products From File As String
                    var methodsData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\DataSeeding\delivery.json");

                    // Transform From Json into C# Object
                    var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(methodsData);

                    // Add Products Into DB & Save Changes
                    if (methods is not null && methods.Any())
                    {
                        await _dbContext.DeliveryMethods.AddRangeAsync(methods);
                    }
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task InitializeIdentityAsync()
        {

            // Create the database if it doesn't exist & apply any pending migrations
            if (_identityContext.Database.GetPendingMigrations().Any())
                await _identityContext.Database.MigrateAsync();


            // Set Default Users & Roles

            // Seed Roles
            if (!_roleManager.Roles.Any())
            {
                // Admin & SuperAdmin
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            }

            // Seed Users
            if (!_userManager.Users.Any())
            {
                var adminUser = new User()
                {
                    DisplayName = "Admin",
                    Email = "Admin@gmail.com",
                    UserName = "Admin@gmail.com",
                    PhoneNumber = "0123456789"
                };


                var superAdminUser = new User()
                {
                    DisplayName = "Super Admin",
                    Email = "SuperAdmin@gmail.com",
                    UserName = "SuperAdmin@gmail.com",
                    PhoneNumber = "0123456789"
                };
    

                await _userManager.CreateAsync(adminUser, "Passw0rd#");
                await _userManager.CreateAsync(superAdminUser, "Passw0rd#");

                // Assign Role To Users
                await _userManager.AddToRoleAsync(adminUser, "Admin");
                await _userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
            }
        }
    }
}