using SimpleToDoAppWebMVC.Services;

namespace SimpleToDoAppWebMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            /* Add Http Client factory
             * These services are used to call the API
             * A uri for the API is configured in appsettings.json
             * Learn more from: https://learn.microsoft.com/en-us/dotnet/core/extensions/httpclient-factory#typed-clients
             */

            string? uri;
            if (builder.Environment.IsDevelopment())
                uri = builder.Configuration.GetConnectionString("LocalHttpClient");
            else
                uri = builder.Configuration.GetConnectionString("AzureHttpClient");
            if (uri == null)
                throw new Exception("No HttpClient available");

            // Add the service handling categories
            builder.Services.AddHttpClient<CategoryService>(client =>
                {
                    client.BaseAddress = new Uri(uri);
                });
            builder.Services.AddHttpClient<TaskService>(client =>
            {
                client.BaseAddress = new Uri(uri);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
