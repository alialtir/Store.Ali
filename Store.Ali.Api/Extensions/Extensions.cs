using Services;

using Microsoft.AspNetCore.Mvc;
using Persistence;
using Shared.ErrorModels;
using Domain.Contracts;
using Store.Ali.Api.Middlewares;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Persistence.Identity;

namespace Store.Ali.Api.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection RegisterAllServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddBuiltInServices();

            services.AddSwaggerServices();
    

            services.AddInfrastructureServices(configuration);

            services.AddIdentityServices();

            services.AddApplicationServices();


            services.ConfigureServices();

            return services;
        }


        private static IServiceCollection AddBuiltInServices(this IServiceCollection services)
        {
            services.AddControllers();


            return services;
        }



        private static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, IdentityRole>()
                 .AddEntityFrameworkStores<StoreIdentityDbContext>();


            return services;
        }


        private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();


            return services;
        }

        private static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(config =>
            {
                config.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(m => m.Value.Errors.Any())
                                              .Select(m => new ValidationError()
                                              {
                                                  Field = m.Key,
                                                  Errors = m.Value.Errors.Select(errors => errors.ErrorMessage)
                                              }
                                              );

                    var response = new ValidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(response);
                };
            }
             );


            return services;
        }


        public static async Task<WebApplication> ConfigureMiddlewares(this WebApplication app)
        {
          await  app.InitializeDatabaseAsync();


            app.UseGlobalErrorHandling();




            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();

            app.UseHttpsRedirection();



            app.UseAuthorization();


            app.MapControllers();

            return app;
        }


        private static async Task<WebApplication> InitializeDatabaseAsync(this WebApplication app)
        {
     

            using var scope = app.Services.CreateScope();

            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>(); // ASK CLR Create Object From IDbInitializer
            await dbInitializer.InitializeAsync();

            await dbInitializer.InitializeIdentityAsync();
   

            return app;
        }

        private static  WebApplication UseGlobalErrorHandling(this WebApplication app)
        {


            app.UseMiddleware<GlobalErrorHandlingMiddleware>();



            return app;
        }

    }
}
