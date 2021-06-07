using AutoMapper;
using CrossCutting.IoC;
using Domain.Common;
using Domain.EntityMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;
using System;
using System.Text;

namespace MicroShopsApi
{
  public class Startup
  {
    private readonly Container _container = new Container();

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddCors();
      services.AddControllers(options =>
      {
        options.RespectBrowserAcceptHeader = true; // false by default
      });
      services.AddRouting(options => options.LowercaseUrls = true);
      AddSwagger(services);

      var appSettingsSection = Configuration.GetSection("AppSettings");
      services.Configure<AppSettings>(appSettingsSection);

      var appSettings = appSettingsSection.Get<AppSettings>();
      var key = Encoding.ASCII.GetBytes(appSettings.Secret);
      services.AddAuthentication(x =>
      {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer(x =>
      {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(key),
          ValidateIssuer = false,
          ValidateAudience = false
        };
      });

      services.AddLogging();
      IntegrateSimpleInjector(services);
      services.AddAutoMapper(c => c.AddProfile<MicroShopsProfile>(), typeof(Startup));
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
      loggerFactory.AddFile("Logs/micro-shops-{Date}.log");

      InitializeContainer();

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      //app.UseHttpsRedirection();

      app.UseSwagger();

      // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Micro Shops API V1");
      });

      app.UseRouting();

      app.UseCors(x => x
          .AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });

      _container.Verify();
    }

    private void IntegrateSimpleInjector(IServiceCollection services)
    {
      _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

      services.AddSingleton<IControllerActivator>(
          new SimpleInjectorControllerActivator(_container));

      services.AddSimpleInjector(_container, o =>
      {
        o.AddAspNetCore().AddControllerActivation();
        o.AutoCrossWireFrameworkComponents = true;
        o.AddLogging();
      });
      services.UseSimpleInjectorAspNetRequestScoping(_container);
    }

    private void InitializeContainer()
    {
      // Add application services. For instance:
      _container.RegisterDependenciesScoped();
    }

    private void AddSwagger(IServiceCollection services)
    {
      services.AddSwaggerGen(options =>
      {
        var groupName = "v1";

        options.SwaggerDoc(groupName, new OpenApiInfo
        {
          Title = $"Micro Shops {groupName}",
          Version = groupName,
          Description = "Micro Shops API",
          Contact = new OpenApiContact
          {
            Name = "Micro Shops Company",
            Email = string.Empty,
            Url = new Uri("https://micro-shops.com/"),
          }
        });
      });
    }
  }
}