using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleToDoAppAPI.Models;
using SimpleToDoAppAPI.Services;

namespace SimpleToDoAppAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers(options =>
            {
                // An option to add Json Validation
                options.ModelMetadataDetailsProviders.Add(new SystemTextJsonValidationMetadataProvider());
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            /*
             * Registering Services:
             * DbContext Injection allows for an easily accessible tool to manipulate a database
             * Services are then added which contained DB operations. These services are used in controllers
             * Learn more about dependency injection at: https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection
             */

            // DbContext Injection
            if (builder.Environment.IsDevelopment())
                builder.Services.AddDbContext<SimpleToDoAppDbContext>(options =>
                    options.UseSqlServer(
                        // Db Connection is set in appsettings json. In production, a Key Vaults secret is ideal
                        builder.Configuration.GetConnectionString("LocalDbConnection")));

            // Add Services, which handles DB Operations
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ITaskService, TaskService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
