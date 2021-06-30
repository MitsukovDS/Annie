using Annie.Authentication;
using Annie.Authorization.Filter;
using Annie.Authorization.JWToken;
using Annie.Data;
using Annie.Data.DatabaseProvider;
using Annie.Data.Static;
using Annie.Web.Models.Core;
using Annie.Web.Models.Core.Cache;
using Annie.Web.Repositories.OlympiadRepositories;
using Annie.Web.Repositories.UserRepositories;
using Annie.Web.Services.AuthServices;
using Annie.Web.Services.EmailServices;
using Annie.Web.Services.GrpcServices;
using Annie.Web.Services.OlympiadServices;
using Annie.Web.Services.UserServices;
using Grpc.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Web
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
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();//.AddRazorRuntimeCompilation();

            services.AddControllersWithViews(options =>
            {
                //options.Filters.Add(typeof(LoggerFilter));
                options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
                options.EnableEndpointRouting = false;
            }).SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddRouting(option =>
            {
                option.ConstraintMap["slugify"] = typeof(SlugifyParameterTransformer);
                option.LowercaseUrls = true;
            });

            services.AddHttpClient();

            services.Configure<AppSettings>(Configuration, c => c.BindNonPublicProperties = true);


            #region MongoDb
            //services.Configure<Logger.MongoDbSettings>(Configuration.GetSection(nameof(Logger.MongoDbSettings)));
            //services.AddSingleton<IMongoDbSettings>(sp => sp.GetRequiredService<IOptions<Logger.MongoDbSettings>>().Value);
            //services.AddSingleton<IDataAccessMongoDbProvider, DataAccessMongoDbProvider>();
            #endregion

            #region PostgreSQL
            var sqlConnectionString = Configuration.GetSection("PostgreSQLSettings:TestConnectionString").Value;

            services.AddDbContext<DomainModelContext>(options =>
                options.UseNpgsql(
                    sqlConnectionString,
                    b => b.MigrationsAssembly("Annie")
                )
            );
            services.AddTransient<IDbConnection>(db => new NpgsqlConnection(sqlConnectionString));
            services.AddTransient<IDbConnectionFactory>(db => new SqlConnectionFactory(sqlConnectionString));
            #endregion

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
                    IssuerSigningKey = JwtSettings.Parameters.IssuerSigningKey,
                    ValidateIssuer = JwtSettings.Parameters.ValidateIssuer,
                    ValidIssuer = JwtSettings.Parameters.ValidIssuer,
                    ValidateAudience = JwtSettings.Parameters.ValidateAudience,
                    ValidAudience = JwtSettings.Parameters.ValidAudience,
                    RequireExpirationTime = JwtSettings.Parameters.RequireExpirationTime,
                    ValidateLifetime = JwtSettings.Parameters.ValidateLifetime,
                    ClockSkew = TimeSpan.Zero
                };
            });

            #endregion

            #region gRPC
            var grpcConnectionString = Configuration.GetSection("GrpcSettings:HostForConnect").Value;
            services.AddGrpcClient<Greeter.GreeterClient>(o =>
            {
                o.Address = new Uri(grpcConnectionString);
            });
            #endregion

            //services.AddTransient<AuthHeadersInterceptor>();
            services.AddHttpContextAccessor();

            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IGrpcService, GrpcService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IOlympiadService, OlympiadService>();

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IOlympiadRepository, OlympiadRepository>();
            services.AddSingleton<IStaticRepository, Data.Static.StaticRepository>();
            services.AddTransient<ICache, Cache>();
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
                app.UseHsts();
            }
            //app.UseHttpsRedirection();

            // For gRPC
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            // для того, чтобы IP брал не локальный, а клиента
            // см. appsettings.json -> Microsoft.AspNetCore.HttpOverrides
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseStatusCodePagesWithReExecute("/http-status-code/status-code", "?statusCode={0}");

            app.UseStaticFiles();

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
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:slugify}/{controller:slugify}/{action:slugify}/{id:slugify?}",
                    defaults: new { controller = "Home", action = "Index" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller:slugify}/{action:slugify}/{id:slugify?}");
                    //defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
