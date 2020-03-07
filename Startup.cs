using System.IO;
using System.Reflection;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.Execution.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SampleApp.Models;
using SampleApp.Resolvers;

namespace SampleApp
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

            services.AddControllersWithViews();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddGraphQL(sp =>
                {
                    var schema = SchemaBuilder.New()
                        .AddDocumentFromString(this.ReadGraphQLSchema())
                        // This schema type does not have a model counterpart, so just bind
                        // the resolver directly to the type name as it appears in schema.graphql
                        .BindResolver<QueryResolver>(c => c.To("Query"))
                        // This schema type does have a corresponding data model, so bind the
                        // resolver to the model that matches the schema.graphql type name
                        .BindResolver<WeatherForecastResolver>(c => c.To<WeatherForecast>())
                        .AddServices(sp)
                        .Create();

                    schema.MakeExecutable(new QueryExecutionOptions
                    {
                        IncludeExceptionDetails = true,
                    });

                    return schema;
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseGraphQL("/graphql");

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }

        private string ReadGraphQLSchema()
        {
            string schema;
            using (var schemaStream = Assembly.
                GetExecutingAssembly().
                GetManifestResourceStream("SampleApp.schema.graphql"))
            {
                if (schemaStream == null)
                {
                    throw new FileNotFoundException("Unable to load GraphQL schema");
                }

                using var reader = new StreamReader(schemaStream);
                schema = reader.ReadToEnd();
            }

            return schema;
        }
    }
}
