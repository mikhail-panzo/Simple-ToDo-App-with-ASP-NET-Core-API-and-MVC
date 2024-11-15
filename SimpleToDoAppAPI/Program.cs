using Microsoft.EntityFrameworkCore;
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

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // DbContext Injection
            if (builder.Environment.IsDevelopment())
                builder.Services.AddDbContext<SimpleToDoAppDbContext>(options =>
                    options.UseSqlServer(
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
