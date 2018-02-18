using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using TodoAPI.Filters;
using TodoAPI.Models;
using TodoAPI.Repositories;
using TodoAPI.Services;
using VersionRouting;

namespace TodoAPI
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
            var apiPrefix = "api";
            services.AddDbContext<TodoContext>(options => options.UseSqlServer("Server=DESKTOP-B051CI4;Database=TodoDatabase;Trusted_Connection=True;MultipleActiveResultSets=true"));
            services.AddTransient<ITodosRepository, TodosRepository>();
            services.AddTransient<ITodosService, TodosService>();
            services.AddAutoMapper();
            services.AddIdentity<ApplicationUser,IdentityRole>()
                .AddEntityFrameworkStores<TodoContext>()
                .AddDefaultTokenProviders();
            /* services.AddAuthentication(sharedOptions =>
             {
                 sharedOptions.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                 sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                 // sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
             });*/
            /* services.ConfigureApplicationCookie(options =>
             {
                 // Cookie settings
                 options.Cookie.HttpOnly = true;
                 options.Cookie.Expiration = TimeSpan.FromDays(150);
                 options.LoginPath = "/api/v1/Account/Login"; // If the LoginPath is not set here, ASP.NET Core will default to /Account/Login
                 options.LogoutPath = "/api/v1/Account/Logout"; // If the LogoutPath is not set here, ASP.NET Core will default to /Account/Logout
                // options.AccessDeniedPath = "/api/v1/Account/AccessDenied"; // If the AccessDeniedPath is not set here, ASP.NET Core will default to /Account/AccessDenied
                 options.SlidingExpiration = true;
             });*/
            services.AddResponseCaching(options =>
            {
                options.UseCaseSensitivePaths = true;
                options.MaximumBodySize = 1024;
            });
            services.AddAuthorization();
            services.AddMvc(options =>
            {
      //          options.Conventions.Add(new NameSpaceVersionRoutingConvention(apiPrefix));
                options.Filters.Add(new ValidationActionFilter());
                options.Filters.Add(typeof(ControllerExceptionFilter));
            }
            );
            
            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseETagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseAuthentication();
            //app.UseCookieAuthentication()
            app.UseResponseCaching();
            app.UseMvc();
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("MVC does not found anything :(");
            });
        }
    }
}
