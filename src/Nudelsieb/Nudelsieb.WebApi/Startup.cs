using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureADB2C.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Nudelsieb.Domain;
using Nudelsieb.Persistence;
using Microsoft.Azure.Cosmos;
using Microsoft.IdentityModel.Logging;

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
            // TODO implement cosmos db persistence 
            //var cosmosDbContainer = InitializeCosmosDbContainerAsync(
            //    Configuration.GetSection("Persistence").GetSection("CosmosDb"))
            //    .Result;

            // add as singleton to enable in-memory data for dummy repository
            services.AddSingleton<INeuronRepository, DummyNeuronRepository>();
            //services.AddSingleton<Container>(_ => cosmosDbContainer);

            services
                .AddAuthentication(AzureADB2CDefaults.BearerAuthenticationScheme)
                .AddAzureADB2CBearer(options => Configuration.Bind("AzureAdB2C", options));

            services.AddControllers();
            services.AddHealthChecks();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = ApiName, Version = ApiVersion });

                // TODO directly integrate Azure AD B2c into Swagger UI or use alternative UI
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                IdentityModelEventSource.ShowPII = true;
            }

            app.UseHttpsRedirection();

            app.UseRouting();

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

        private async Task<Container> InitializeCosmosDbContainerAsync(IConfigurationSection configSection)
        {
            // var b = new CosmosClientBuilder("",""). ...

            var accountConfig = configSection.GetSection("Account");
            CosmosClient client = new CosmosClient(
                accountConfig.GetValue<string>("Endpoint"),
                accountConfig.GetValue<string>("Key"));

            Database database = await client.CreateDatabaseIfNotExistsAsync(
                configSection.GetValue<string>("Database"));

            Container container = await database.CreateContainerIfNotExistsAsync(
                configSection.GetValue<string>("Container"),
                configSection.GetValue<string>("PartitionKeyPath"));

            return container;
        }
    }
}
