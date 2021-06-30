using Annie.Authorization;
using Annie.Authorization.Filter;
using Annie.Authorization.JWToken;
using Annie.Data;
using Annie.Data.DatabaseProvider;
using Annie.Store.Core;
using Annie.Store.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Store
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(configuration.GetValue<string>(WebHostDefaults.ContentRootKey))
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages(options =>
            {
                options.Conventions
                    .AddPageApplicationModelConvention("/StreamedSingleFileUploadDb",
                        model =>
                        {
                            model.Filters.Add(
                                new GenerateAntiforgeryTokenCookieAttribute());
                            model.Filters.Add(
                                new DisableFormValueModelBindingAttribute());
                        });
                options.Conventions
                    .AddPageApplicationModelConvention("/StreamedSingleFileUploadPhysical",
                        model =>
                        {
                            model.Filters.Add(
                                new GenerateAntiforgeryTokenCookieAttribute());
                            model.Filters.Add(
                                new DisableFormValueModelBindingAttribute());
                        });
            });

            services.Configure<AppSettings>(Configuration, c => c.BindNonPublicProperties = true);
            services.AddHttpClient();

            #region PostgreSQL
            var sqlConnectionString = Configuration.GetSection("PostgreSQLSettings:ConnectionString").Value;

            services.AddDbContext<DomainModelContext>(options =>
                options.UseNpgsql(
                    sqlConnectionString
                //b => b.MigrationsAssembly("Annie")
                )
            );

            services.AddTransient<StaticRepository>();
            services.AddTransient<FileRepository>();


            #region JWToken
            services.AddSingleton<IJwtSigningEncodingKey>(JwtSettings.Parameters.SigningKey);

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtSettings.Parameters.JwtSchemeName;
                auth.DefaultChallengeScheme = JwtSettings.Parameters.JwtSchemeName;
            })
            .AddJwtBearer(token =>
            {
                token.RequireHttpsMetadata = false;
                token.SaveToken = JwtSettings.Parameters.SaveToken;
                token.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = JwtSettings.Parameters.ValidateIssuerSigningKey,
                    //Same Secret key will be used while creating the token
                    IssuerSigningKey = JwtSettings.Parameters.IssuerSigningKey,
                    ValidateIssuer = JwtSettings.Parameters.ValidateIssuer,
                    //Usually, this is your application base URL
                    ValidIssuer = JwtSettings.Parameters.ValidIssuer,
                    ValidateAudience = JwtSettings.Parameters.ValidateAudience,
                    //Here, we are creating and using JWT within the same application.
                    //In this case, base URL is fine.
                    //If the JWT is created using a web service, then this would be the consumer URL.
                    ValidAudience = JwtSettings.Parameters.ValidAudience,
                    RequireExpirationTime = JwtSettings.Parameters.RequireExpirationTime,
                    ValidateLifetime = JwtSettings.Parameters.ValidateLifetime,
                    ClockSkew = TimeSpan.Zero
                };
            });

            #endregion


            services.AddHttpContextAccessor();
            services.AddTransient<IDbConnection>(db => new NpgsqlConnection(sqlConnectionString));
            services.AddTransient<IDbConnectionFactory>(db => new SqlConnectionFactory(sqlConnectionString));
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "/store")),
                RequestPath = new PathString("/store")
            });

            app.UseRouting();

            app.Use(async (context, next) =>
            {
                var JWToken = context.Request.Cookies[JwtSettings.Parameters.JwtName];
                if (!string.IsNullOrEmpty(JWToken) && !context.Request.Headers.ContainsKey("Authorization"))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
                }
                await next();
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
