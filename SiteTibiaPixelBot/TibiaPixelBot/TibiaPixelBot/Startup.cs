using Hangfire;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TibiaPixelBot.Email;

namespace TibiaPixelBot
{
    public class Startup
    {
        public static string ConnectionString { get; private set; }
        public static string nameCookie = "cokiePixelBot";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConnectionString = Configuration.GetConnectionString("DefaultConnection");

           // services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection")));

            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
              {
                  options.LoginPath = new PathString("/Account/Login");
                  options.AccessDeniedPath = new PathString("/Account/Login");
                  //options.EventsType = typeof(CustomCookieAuthenticationEvents);
                  //options.Cookie.Name = CookieAuthenticationDefaults.AuthenticationScheme;
                  //options.Cookie.Expiration = TimeSpan.FromDays(365);
              });

            //services.AddScoped<CustomCookieAuthenticationEvents>();

            services.AddMvc();

            //services.AddMvc().AddRazorPagesOptions(options =>
            //{
            //    options.Conventions.AuthorizeFolder("/");
            //    options.Conventions.AllowAnonymousToPage("/Account/Login");
            //});


            services.AddTransient<IEmailSender, EmailSender>();

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseStaticFiles();

            app.UseAuthentication();

          //  app.UseHangfireServer();

          //  app.UseHangfireDashboard();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
