
using Domain.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data;
using Services;
using Services.Abstractions;
using Shared.ErrorModels;
using Store.Ali.Api.Extensions;
using Store.Ali.Api.Middlewares;
using AssemblyMapping = Services.AssemblyReference;

namespace Store.Ali.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.



            builder.Services.RegisterAllServices(builder.Configuration);





           
  
            var app = builder.Build();



            await app.ConfigureMiddlewares();

            app.Run();
        }
    }
}
