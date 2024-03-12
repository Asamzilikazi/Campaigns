using Campaigns.DataAccessLibrary;
using Campaigns.DataAccessLibrary.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Campaigns
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

            //for authentication to function fully, it needs to be integrated with the client-side implementation. 
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration["Auth0:Authority"];
                    options.Audience = Configuration["Auth0:Audience"];
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Auth0:Issuer"],
                        ValidAudiences = Configuration.GetSection("Auth0:Audiences").Get<string[]>(),
                        IssuerSigningKeyResolver = (string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters) =>
                        {
                            // Retrieve the issuer signing keys from Auth0
                            var json = new WebClient().DownloadString($"{Configuration["Auth0:Authority"]}.well-known/jwks.json");
                            var keys = JsonConvert.DeserializeObject<JsonWebKeySet>(json).Keys;
                            return keys;
                        }
                    };
                });

            services.AddHttpContextAccessor();
            services.AddControllers();
            services.AddDistributedMemoryCache(); // Add memory-based session state
            services.AddSession(options =>
            {
                // Set session timeout (default is 20 minutes)
                options.IdleTimeout = TimeSpan.FromMinutes(1);

                // Set session cookie options
                options.Cookie.Name = ".Campaigns.Session";
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true; // Make the session cookie essential

            });     
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Campaign Management System", Version = "v1" });

                // Configure JWT authentication information
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                // Configure JWT authentication requirement
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
             });
            services.AddDbContext<CampaignsDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Configuring Dependancy Injection
            services.AddScoped<ICampaignRepository, CampaignRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Campaigns v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication(); 
            app.UseAuthorization();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSession();

        }
    }
}
