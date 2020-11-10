using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using TodoMysqlApi.Models;
using TodoMysqlApi.Services;

namespace TodoMysqlApi
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
      services.AddDbContextPool<TodoContext>(
        dbContextOptions => dbContextOptions
          .UseMySql(
            Configuration.GetConnectionString("TodoDatabase"),
            // Replace with your server version and type.
            mySqlOptions => mySqlOptions
              .ServerVersion(new Version(5, 7, 31), ServerType.MySql)
              .CharSetBehavior(CharSetBehavior.NeverAppend))
          // Everything from this point on is optional but helps with debugging.
          .UseLoggerFactory(
            LoggerFactory.Create(
              logging => logging
                .AddConsole()
                .AddFilter(level => level >= LogLevel.Information)))
          .EnableSensitiveDataLogging()
          .EnableDetailedErrors()
      );
      services.AddControllers();

      services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(o =>
          o.Events.OnRedirectToLogin = context =>
          {
              context.Response.StatusCode = StatusCodes.Status401Unauthorized;
              return Task.CompletedTask;
          });

      // register a class with the DI
      services.AddScoped<IUserService, UserService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
