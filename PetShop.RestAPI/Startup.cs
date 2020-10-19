using System;
using System.Reflection;
using System.IO;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PetShop.Core.ApplicationService;
using PetShop.Core.ApplicationService.Implementations;
using PetShop.Core.ApplicationService.Interfaces;
using PetShop.Core.DomainService;
using PetShop.Core.HelperClasses.Implementations;
using PetShop.Core.HelperClasses.Interfaces;
using PetShop.Infrastructure.Data.EntityFramework;
using PetShop.Infrastructure.Data.EntityFramework.Authentication;
using PetShop.Infrastructure.Data.EntityFramework.Repositories;

namespace PetShop.RestAPI
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

            // Create a byte array with random values. This byte array is used
            // to generate a key for signing JWT tokens.
            Byte[] secretBytes = new byte[40];
            Random rand = new Random();
            rand.NextBytes(secretBytes);

            // Add JWT based authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    //ValidAudience = "TodoApiClient",
                    ValidateIssuer = false,
                    //ValidIssuer = "TodoApi",
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretBytes),
                    ValidateLifetime = true, //validate the expiration and not before values in the token
                    ClockSkew = TimeSpan.FromMinutes(5) //5 minute tolerance for the expiration date
                };
            });

            // Register the AuthenticationHelper in the helpers folder for dependency
            // injection. It must be registered as a singleton service. The AuthenticationHelper
            // is instantiated with a parameter. The parameter is the previously created
            // "secretBytes" array, which is used to generate a key for signing JWT tokens,
            services.AddSingleton<IAuthenticationHelper>(new
                AuthenticationHelper(secretBytes));

            services.AddCors(options => options.AddDefaultPolicy(
                builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();

                })
            );
            services.AddScoped<IPetRepository, PetRepository>();
            services.AddScoped<IPetService, PetService>();
            services.AddScoped<IOwnerRepository, OwnerRepository>();
            services.AddScoped<IOwnerService, OwnerService>();
            services.AddScoped<IPetTypeRepository, PetTypeRepository>();
            services.AddScoped<IPetTypeService, PetTypeService>();
            services.AddScoped<IColorRepository, ColorRepository>();
            services.AddScoped<IPetColorRepository, PetColorRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IParser, Parser>();
           
            services.AddControllers().AddNewtonsoftJson(o =>
            {
                o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                o.SerializerSettings.MaxDepth = 5;

            });

            services.AddDbContext<PetShopDbContext>(
                opt =>
                {
                    opt.UseSqlite("Data Source=PetShop.db");
                }, ServiceLifetime.Transient);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "PetShop",
                    Description = "An api used to manage a petshop",
                    Contact = new OpenApiContact
                    {
                        Name = "Kacper Bujko",
                        Email = "kacperbujko@hotmail.dk",
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            //if (env.IsDevelopment())
            //{
            app.UseDeveloperExceptionPage();
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var petRepo = scope.ServiceProvider.GetService<IPetRepository>();
                    var ownerRepo = scope.ServiceProvider.GetService<IOwnerRepository>();
                    var petTypeRepo = scope.ServiceProvider.GetService<IPetTypeRepository>();
                    var colorRepo = scope.ServiceProvider.GetService<IColorRepository>();
                    var petColorRepo = scope.ServiceProvider.GetService<IPetColorRepository>();
                    var userRepo = scope.ServiceProvider.GetService<IUserRepository>();
                    var authHelp = scope.ServiceProvider.GetService<IAuthenticationHelper>();
                    var context = scope.ServiceProvider.GetService<PetShopDbContext>();
                    
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();
                    DataInitializer dataInitializer = new DataInitializer(petRepo, petTypeRepo, ownerRepo, colorRepo, petColorRepo, userRepo, authHelp);
                    dataInitializer.InitData();

                    // new DataInitializer(petRepo, ownerRepo, petTypeRepo).InitData(); 
                }
            //}
            app.UseSwaggerUI(c =>
            {

                c.SwaggerEndpoint("/swagger/v1/swagger.json", "PetShop V1");
                c.RoutePrefix = string.Empty;

            });


            app.UseHttpsRedirection();

            app.UseRouting();

            // Enable CORS (after UseRouting and before UseAuthorization).
            app.UseCors();

            // Use authentication
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
