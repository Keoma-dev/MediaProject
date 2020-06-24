using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MediaWeb.Database;
using MediaWeb.Domain;

namespace MediaWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MediaWebDbContext>(options =>
                options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MediaWebDatabase;Trusted_Connection=True;MultipleActiveResultSets=true"));
            services.AddDefaultIdentity<MediaWebUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<MediaWebDbContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            CreateRolesAndAssignUsers(serviceProvider);
        }
        private void CreateRolesAndAssignUsers(IServiceProvider serviceProvider)
        {
            CreateRoleIfNotExists(serviceProvider, "Administrator");
            CreateRoleIfNotExists(serviceProvider, "User");
            CreateRoleIfNotExists(serviceProvider, "Public");
        }

        private static void CreateRoleIfNotExists(IServiceProvider serviceProvider, string role)
        {
            var rolemanager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            if (!rolemanager.RoleExistsAsync(role).Result)
            {
                rolemanager.CreateAsync(new IdentityRole(role)).Wait();
            }
        }
    }
}
