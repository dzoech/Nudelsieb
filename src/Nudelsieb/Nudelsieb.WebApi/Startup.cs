using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureADB2C.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Nudelsieb.Domain;
using Microsoft.IdentityModel.Logging;
using Nudelsieb.Persistence.Relational;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Reflection;
using Nudelsieb.Notifications;
using Nudelsieb.Application.Persistence;
using Nudelsieb.Application;

namespace Nudelsieb.WebApi
{
    public class Startup
    {
        const string ApiName = "Nudelsieb";
        const string ApiVersion = "V1";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddTransient<INeuronRepository, RelationalDbNeuronRepository>()
                .AddApplicationLayer()
                .AddNotificationServices(options => Configuration.Bind(NotificationsOptions.SectionName, options))
                .AddAuthentication(AzureADB2CDefaults.BearerAuthenticationScheme)
                .AddAzureADB2CBearer(options => Configuration.Bind("AzureAdB2C", options));

            services.AddHttpClient();
            services.AddControllers();
            services.AddHealthChecks();
            services.AddCors(o => o
                .AddDefaultPolicy(p => p
                    .WithOrigins("http://localhost:4200", "https://localhost:4200")
                    .AllowAnyMethod()
                    .AllowCredentials()));

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = ApiName, Version = ApiVersion });

                var xmlDocPath = Path.Combine(
                    AppContext.BaseDirectory,
                    $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");

                options.IncludeXmlComments(xmlDocPath);

                // TODO directly integrate Azure AD B2c into Swagger UI or use alternative UI
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new List<string>()
                    }
                });
            });

            services.AddDbContext<BraindumpDbContext>(options =>
            {
                var connStr = Configuration
                    .GetSection("Persistence")
                    .GetSection("Relational")
                    .GetSection("SqlConnectionString")
                    .Value;

                options.UseSqlServer(connStr);
                options.EnableSensitiveDataLogging();
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <remarks>
        /// <see href="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-3.1#middleware-order">
        /// See the documentation for the correct order of middlewares.
        /// </see>
        /// </remarks>
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            BraindumpDbContext braindumpContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                IdentityModelEventSource.ShowPII = true;
            }

            braindumpContext.Database.Migrate();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("health");
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{ApiName} {ApiVersion}");
            });
        }
    }
}
