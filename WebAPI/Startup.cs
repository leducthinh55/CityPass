using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Infrastructure.Data;
using Infrastructure.Infrastructure;
using Service;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Infrastructure.Repositories;
using WebAPI.Utils;
using Microsoft.AspNetCore.Http.Features;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using System.IO;
using Sgw.KebabCaseRouteTokens;

namespace WebAPI
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
            var emailConfig = Configuration.GetSection("EmailConfig").Get<EmailConfig>();
            services.AddSingleton(emailConfig);

            services.AddScoped<IMailService, MailService>();
            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

            services.AddAutoMapper(typeof(Startup));

            services.AddDbContext<CityPassContext>();
            #region DI
            // add for data
            //services.AddScoped<IDbFactory, DbFactory>();
            //services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IAttractionRepository, AttractionRepository>();
            services.AddTransient<IAtrractionService, AtrractionService>();

            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<ICategoryService, CategoryService>();

            services.AddTransient<ICityRepository, CityRepository>();
            services.AddTransient<ICityService, CityService>();

            services.AddTransient<IUserPassRepository, UserPassRepository>();
            services.AddTransient<IUserPassService, UserPassService>();

            services.AddTransient<ITicketRepository, TicketRepository>();
            services.AddTransient<ITicketService, TicketService>();

            services.AddTransient<ITicketTypeRepository, TicketTypeRepository>();
            services.AddTransient<ITicketTypeService, TicketTypeService>();

            services.AddTransient<ICollectionRepository, CollectionRepository>();
            services.AddTransient<ICollectionService, CollectionService>();

            services.AddTransient<IPassRepository, PassRepository>();
            services.AddTransient<IPassService, PassService>();
            
            services.AddTransient<ITicketTypeInCollectionRepository, TicketTypeInCollectionRepository>();
            services.AddTransient<ITicketTypeInCollectionService, TicketTypeInCollectionService>();
            #endregion

            services.AddRouting(option => { option.LowercaseUrls = true; });
            services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddCors(options => options.AddPolicy("AllowAll", builder =>
                builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()
            ));

            var pathToKey = Path.Combine(Directory.GetCurrentDirectory(), "firebase_admin_sdk.json");

            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile(pathToKey)
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireClaim("admin"));
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(builder =>
            {
                var firebaseProjectName = Configuration["FirebaseProjectId"];
                builder.Authority = "https://securetoken.google.com/" + firebaseProjectName;
                builder.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = "https://securetoken.google.com/" + firebaseProjectName,
                    ValidateAudience = true,
                    ValidAudience = firebaseProjectName,
                    ValidateLifetime = true
                };
            });

            services.AddMvc().AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.PropertyNamingPolicy = null;
                o.JsonSerializerOptions.DictionaryKeyPolicy = null;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
            });
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

            app.UseCors("AllowAll");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
           {
               c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyAPI");
           });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
