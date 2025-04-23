using Domain.Contracts;
using Domain.Models;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Persistence.Data;
using Persistence.Identity;
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
        private readonly StoreIdentityDbContext _identityDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(StoreDbContext context, StoreIdentityDbContext identityDbContext, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
           _context = context;
            this._identityDbContext = identityDbContext;
            this._userManager = userManager;
            this._roleManager = roleManager;
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

        public async Task InitializeIdentityAsync()
        {
            // Create Database If it doesn't Exists && Apply To Any Pending Migrations
            if(_identityDbContext.Database.GetPendingMigrations().Any())
            {
               await _identityDbContext.Database.MigrateAsync();
            }



            if(!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name = "Admin"
                });

                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name = "SuperAdmin"
                });
            }

            // Seeding
            if(!_userManager.Users.Any())
            {
                var superAdminUser = new AppUser()
                {
                    DisplayName = "Super Admin",
                    Email = "SuperAdmin@gmail.com",
                    UserName = "SuperAdmin",
                    PhoneNumber = "0123456789"
                };
                var AdminUser = new AppUser()
                {
                    DisplayName = "Admin",
                    Email = "Admin@gmail.com",
                    UserName = "Admin",
                    PhoneNumber = "0123456789"
                };

              await  _userManager.CreateAsync(superAdminUser, "P@ssW0rd");
                await _userManager.CreateAsync(AdminUser, "P@ssW0rd");

              await  _userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
                await _userManager.AddToRoleAsync(AdminUser, "Admin");


            }

        }
    }
}

// ..\Infrastructure\Persistence\Data\Seeding\types.json