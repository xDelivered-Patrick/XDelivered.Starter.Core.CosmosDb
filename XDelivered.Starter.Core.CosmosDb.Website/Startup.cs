using System;
using System.IO;
using System.Linq;
using System.Text;
using Autofac;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Swashbuckle.AspNetCore.Swagger;
using XDelivered.StarterKits.NgCoreCosmosDb.Data;
using XDelivered.StarterKits.NgCoreCosmosDb.Exceptions;
using XDelivered.StarterKits.NgCoreCosmosDb.Helpers;
using XDelivered.StarterKits.NgCoreCosmosDb.Services;
using XDelivered.StarterKits.NgCoreCosmosDb.Settings;
using User = XDelivered.StarterKits.NgCoreCosmosDb.Data.User;
using AspNetCore.Identity.MongoDB;
using XDelivered.Starter.Core.CosmosDb.Website.Data;

namespace XDelivered.StarterKits.NgCoreCosmosDb
{
    public class Startup
    {
        public IContainer ApplicationContainer { get; private set; }
        private IHostingEnvironment _env;

        public IConfiguration Configuration { get; set; }


        public Startup(IConfiguration configuration, IHostingEnvironment appEnv)
        {
            _env = appEnv;
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(x =>
                {
                    x.Filters.Add<WrapOperationalResult>();
                    x.Filters.Add<HandleUserExceptions>();
                })
                .AddControllersAsServices()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddCors();
            services.AddOptions();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {Version = "v1", Title = "XDelivered API",});

                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "SwaggerProject.xml");
                c.IncludeXmlComments(xmlPath);
                c.EnableAnnotations();
            });

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/dist"; });


            // Register dependencies, populate the services from
            // the collection, and build the container.
            ConfigureSettingsAndIdentity(services, _env);
        }


        private void ConfigureSettingsAndIdentity(IServiceCollection services, IHostingEnvironment env)
        {
            ConfigureSettings(services, env);
            ConfigureIdentity(services);

            //DI
            services.AddScoped<IUserService, UserService>();
        }

        private void ConfigureSettings(IServiceCollection services, IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            services.Configure<AppConfiguration>(options =>
                Configuration.GetSection(nameof(AppConfiguration)).Bind(options));

            this.Configuration = builder.Build();
        }

        private void ConfigureIdentity(IServiceCollection services)
        {
            // Add DocumentDb client singleton instance (it's recommended to use a singleton instance for it)

            services.Configure<MongoDbSettings>(Configuration.GetSection("MongoDb"));
            services.AddSingleton<IUserStore<User>>(provider =>
            {
                var options = provider.GetService<IOptions<MongoDbSettings>>();
                var client = new MongoClient(options.Value.ConnectionString);
                var database = client.GetDatabase(options.Value.DatabaseName);

                return MongoUserStore<User>.CreateAsync(database).GetAwaiter().GetResult();
            });
            services.AddIdentity<User>()
                .AddDefaultTokenProviders();

            //services.AddIdentityWithDocumentDBStores<User, IdentityRole>(client, x=>new DocumentCollection() { Id = "main"}, options => { })
            //    .AddRoleManager<RoleManager<IdentityRole>>()
            //    .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 4;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                var signingKey = Configuration.GetSection("AppConfiguration:SigningKey").Value;
                var siteUrl = Configuration.GetSection("AppConfiguration:SiteUrl").Value;

                cfg.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
                    ValidAudience = siteUrl,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = siteUrl,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            DependencyInjectionHelper.ApplicationServices = app.ApplicationServices;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseCors(b =>
            {
                b.WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();


            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("v1/swagger.json", "Restaurant Review API"); });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
            
            if (env.IsDevelopment() || ServerHelper.IntegrationTests)
            {
                using (IServiceScope serviceScope = app.ApplicationServices
                    .GetRequiredService<IServiceScopeFactory>()
                    .CreateScope())
                {
                    MongoUserStore<User> context = serviceScope.ServiceProvider.GetService<MongoUserStore<User>>();
                    UserManager<User> userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();

                    var users = userManager.Users.ToList();
                    Seed.SeedDb(context, userManager).Wait();
                }
            }
        }


        //private async Task CreateRolesThatDoNotExist(IServiceProvider serviceProvider)
        //{
        //    var roleManager = serviceProvider.GetRequiredService<RoleManager<DocumentDbIdentityRole>>();

        //    foreach (var roleName in Enum.GetNames(typeof(Roles)))
        //    {
        //        var roleExist = await roleManager.RoleExistsAsync(roleName);
        //        if (!roleExist)
        //        {
        //            await roleManager.CreateAsync(new IdentityRole() { Name = roleName});
        //        }
        //    }
        //}
    }
}
