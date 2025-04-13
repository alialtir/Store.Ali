using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Persistence
{
    public class DbInitializer : IDbInitializer
    {
        private readonly StoreDbContext _context;

        public DbInitializer(StoreDbContext context)
        {
           _context = context;
        }

        public async Task InitializeAsync()
        {
            try
            {
                // Create Database If it doesn't Exists && Apply To Any Pending Migrations
                if (_context.Database.GetPendingMigrations().Any())
                {
                    await _context.Database.MigrateAsync();
                }


                // Data Seeding

                // Seeding ProductTypes From Json Files

                if (!_context.ProductTypes.Any())
                {

                    // 1. Read All Data from Types Json File as String

                    var typesData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\types.json");

                    // 2. Transform String To C# Objects [List<ProductTypes>]
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);


                    // 3. Add List<ProductTypes> To Database

                    if (types is not null && types.Any())
                    {
                        await _context.ProductTypes.AddRangeAsync(types);
                        await _context.SaveChangesAsync();
                    }
                }





                // Seeding ProductBrands From Json Files

                if (!_context.ProductsBrands.Any())
                {

                    // 1. Read All Data from Types Json File as String

                    var brandsData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\brands.json");

                    // 2. Transform String To C# Objects [List<ProductsBrands>]
                    var brand = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);


                    // 3. Add List<ProductsBrands> To Database

                    if (brand is not null && brand.Any())
                    {
                        await _context.ProductsBrands.AddRangeAsync(brand);
                        await _context.SaveChangesAsync();
                    }
                }


                // Seeding Product From Json Files

                if (!_context.Products.Any())
                {

                    // 1. Read All Data from Types Json File as String

                    var productsData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\products.json");

                    // 2. Transform String To C# Objects [List<Product>]
                    var product = JsonSerializer.Deserialize<List<Product>>(productsData);


                    // 3. Add List<Product> To Database

                    if (product is not null && product.Any())
                    {
                        await _context.Products.AddRangeAsync(product);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch(Exception) {
                throw;
            }


        }
    }
}

// ..\Infrastructure\Persistence\Data\Seeding\types.json