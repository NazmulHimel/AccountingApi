﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Acc.Context;
using Acc.HelpersAndUtilities.Connection;
using Acc.HelpersAndUtilities.JwtSecurity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace AccountingApi
{
    public class Startup
    {
        public IServiceCollection _services { get; private set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _services = services;

            services.Configure<ConnectionStringList>(Configuration.GetSection("ConnectionString"));
            services.AddDbContext<SqlServerContext>(options => options.UseSqlServer(Configuration["ConnectionString:MsSqlConnection"]));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMvc().AddJsonOptions(
                                                options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                                            );
            RegisterCorsPolicies();
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Test Title version 1 ",
                    Description = "Test Description",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "Ali Ben Chaabene",
                        Email = "alibenchaabene@gmail.com",
                        Url = ""
                    }

                });

                c.SwaggerDoc("v2", new Info
                {
                    Version = "v2",
                    Title = "Test Title version 2 ",
                    Description = "Test Description",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "Ali Ben Chaabene",
                        Email = "alibenchaabene@gmail.com",
                        Url = ""
                    }

                });

                //Locate the XML file being generated by ASP.NET...
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                //... and tell Swagger to use those XML comments.
                c.IncludeXmlComments(xmlPath);
            });


            // configure jwt authentication

            var jwtTokenManagementSection = Configuration.GetSection("JwtTokenManagement");
            services.Configure<JwtTokenManagement>(jwtTokenManagementSection);

            var jwtTokenManagement = jwtTokenManagementSection.Get<JwtTokenManagement>();
            var key = Encoding.ASCII.GetBytes(jwtTokenManagement.Secret);

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
                    ValidateAudience = false,
                    ValidIssuer = jwtTokenManagement.Issuer,
                    ValidAudience = jwtTokenManagement.Audience
                };
            });

            // DI inject
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpContextAccessor();

        
            services.AddTransient<IJwtSecurityService, JwtSecurityService>();

            

        }


        private void RegisterCorsPolicies()
        {
            string[] localHostOrigins = new string[] {

                "https://localhost:4200",
                "https://localhost:4201",
                "http://localhost:4200",
                "http://localhost:4201"
            };

            string[] productionHostOrigins = new string[] {
                "https://admin.ivabd.com",
                "http://admin.ivabd.com",
                "https://ivabd.com",
                "http://ivabd.com",
                "https://localhost:4200",
                "https://localhost:4201",
                "http://localhost:4200",
                "http://localhost:4201",
                "http://127.0.0.1:5500",
                "http://127.0.0.1:5501",
                "http://demo.ivabd.com"
            };

            _services.AddCors(options =>    // CORS middleware must precede any defined endpoints
            {
                options.AddPolicy("DevelopmentCorsPolicy", builder =>
                {
                    builder.WithOrigins(localHostOrigins)
                            .AllowAnyHeader().AllowAnyMethod();
                });
                options.AddPolicy("ProductionCorsPolicy", builder =>
                {
                    builder.WithOrigins(productionHostOrigins)
                            .AllowAnyHeader().AllowAnyMethod();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseCors("DevelopmentCorsPolicy");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();

            }

            //app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Test Version 1");
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "API Test Version 2");
            });

            app.UseCors("ProductionCorsPolicy");
            //app.UseMiddleware<AuthenticationMiddleware>();

            app.UseStaticFiles();
            // app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();

        }
    }
}
