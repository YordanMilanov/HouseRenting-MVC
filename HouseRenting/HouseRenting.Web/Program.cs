using HouseRentingSystem.Services;
using HouseRentingSystem.Services.Interfaces;
using HouseRentingSystem.Web.Infrastructure.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HouseRenting.Web
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    
    using HouseRentingSystem.Data;
    using HouseRentingSystem.Data.Models;
    using static HouseRentingSystem.Web.Infrastructure.Extensions.WebApplicationBuilderExtensions;

    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            string connectionString = 
                builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            
            builder.Services
                .AddDbContext<HouseRentingDbContext>(options =>
                options.UseSqlServer(connectionString));

            //Identity Options
            builder.Services
                .AddDefaultIdentity<ApplicationUser>(options =>
                {
                        options.SignIn.RequireConfirmedAccount = 
                            builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedAccount");

                    options.Password.RequireLowercase = 
                        builder.Configuration.GetValue<bool>("Identity:Password:RequireLowercase"); ;
                    options.Password.RequireUppercase =
                        builder.Configuration.GetValue<bool>("Identity:Password:RequireUppercase");
                    options.Password.RequireNonAlphanumeric = 
                        builder.Configuration.GetValue<bool>("Identity:Password:RequireNonAlphanumeric");
                    options.Password.RequiredLength = 
                        builder.Configuration.GetValue<int>("Identity:Password:RequiredLength");
                })
                .AddEntityFrameworkStores<HouseRentingDbContext>();

            //Register services
            builder.Services.AddScoped<IHouseService, HouseService>();
            builder.Services.AddScoped<IAgentService, AgentService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IUSerService, UserService>();

            //Registers the controllers and views
            builder.Services
                .AddControllersWithViews()
                .AddMvcOptions(options =>
                {
                    options
                        .ModelBinderProviders
                        .Insert(0, new DecimalModelBinderProvider());

                    //turn on CSRF safety
                    options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
                });
                    
            //very important! - our custom modelBinder must be INSERTED at position 0 not added
            //because the they are used in their order in the list
            //and if  you add your custom at the end if some other binder succeed to
            //complete the task all other are not used (including our custom binder)


            //Build - order does matter
            var app = builder.Build();

            // Configure the HTTP request pipeline. - the environments
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error/500");
                app.UseStatusCodePagesWithRedirects("/Home/Error?statusCode={0}");
                
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
            app.MapRazorPages();


            app.Run();
        }
    }
}
