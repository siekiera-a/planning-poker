using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Server.Dtos.Incoming;
using Server.DAOs;
using Server.Services.Authentication;
using Server.Services.Authorization;
using Server.Services.DataAccess;
using Server.Services.Team;

namespace Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private void AddDAOs(IServiceCollection services)
        {
            services.AddScoped<UserDAO>();
            services.AddScoped<RefreshTokenDAO>();
            services.AddScoped<TeamDAO>();
            services.AddScoped<TeamMemberDAO>();
            services.AddScoped<RolesDAO>();
            services.AddScoped<MeetingDAO>();
        }

        private void AddServices(IServiceCollection services)
        {
            services.AddSingleton<IConnectionFactory, ConnectionFactory>();
            services.AddSingleton<IPasswordEncoder, PasswordEncoder>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserProvider, UserProvider>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IUserAuthorization, UserAuthorization>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            string tokenKey = Configuration.GetSection("TokenKey").Value;

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddSwaggerDocument();
            services.AddHttpContextAccessor();

            AddDAOs(services);
            AddServices(services);

            services.AddSingleton<IJwtTokenManager>(new JwtTokenManager(tokenKey));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
