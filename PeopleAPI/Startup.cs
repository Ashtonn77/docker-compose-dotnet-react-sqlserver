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
using Microsoft.OpenApi.Models;
using PeopleAPI.Data;
using PeopleAPI.Models;

namespace PeopleAPI
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PeopleAPI", Version = "v1" });
            });

            // services.AddDbContext<AppDbContext>(option => {

            //     option.UseSqlServer(Configuration.GetConnectionString("sqlConnection"));

            // });
            // "ConnectionStrings": {    
            //     "sqlConnection": "Server=ms-sql-server,1433;Database=master;User Id=sa;Password=Pa55w0rd2019;"
            // },

            var server = Configuration["DBServer"] ?? "localhost"; //"ms-sql-server" -> env in docker-compose file   
            var port = Configuration["DBPort"] ?? "1433";    
            var user = Configuration["DBUser"] ?? "SA";    
            var password = Configuration["DBPassword"] ?? "Pa55w0rd2019"; //with docker compose
            //var password_2 = Configuration["DBPassword"] ?? "Pa$$w0rd2019";  //without docker compose    
            var database = Configuration["Database"] ?? "People";    

            services.AddDbContext<AppDbContext>(option => {

                option.UseSqlServer($"Server={server},{port};Initial Catalog={database};User ID={user};Password={password}");

            });

            services.AddCors(o =>
            {
                o.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PeopleAPI v1"));
            }

            PrepDB.PrepPopulation(app);

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
