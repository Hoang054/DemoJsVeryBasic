using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using ProjectTeamNET.Models;
using ProjectTeamNET.Models.Entity;
using ProjectTeamNET.Repository;
using ProjectTeamNET.Repository.Implement;
using ProjectTeamNET.Repository.Interface;
using ProjectTeamNET.Service;
using ProjectTeamNET.Service.Implement;
using ProjectTeamNET.Service.Interface;
using ProjectTeamNET.Service.ManCheckHour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET
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
            services.AddSession();
            services.AddMvc();
            services.AddControllersWithViews();
            services.AddEntityFrameworkNpgsql().AddDbContext<ProjectDbContext>(opt =>
                           opt.UseNpgsql(Configuration.GetConnectionString("DbConnection")));
            //services.AddTransient(typeof(DbContextOptions<ProjectDbContext>));
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IManhourInputService, ManhourInputService>();
            services.AddScoped<IManhourReportService, ManhourReportService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IManhourInputService, ManhourInputService>();         
            services.AddScoped<IManhourUpdateService, ManhourUpdateService>();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(3000);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");
            });
        }
    }
}
