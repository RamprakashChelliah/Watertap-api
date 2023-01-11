using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using ParkBee.MongoDb.DependencyInjection;
using WaterTap.Api.Data;
using WaterTap.Api.Models;
using WaterTap.Api.RepositoryServices;

namespace WaterTap.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        //Configure services
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<WaterTapDatabase>(Configuration.GetSection("WaterTapDatabase"));

            //services.AddSingleton<IMongoClient, MongoClient>(s =>
            //{
            //    var url = s.GetRequiredService<IConfiguration>()["WaterTapDB"];
            //    return new MongoClient(url);
            //});
            services.AddControllers();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ITapDetailRepository, TapDetailRepository>();
            services.AddTransient<ITapEntryRepository, TapEntryRepository>();
            services.AddTransient<IMachineRepository, MachineRepository>();
            services.AddApplicationInsightsTelemetry();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "WaterTap",
                    Version = "1.0"
                });
            });
        }

        //Configure pipeline request
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //For exception
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //Enable routing
            app.UseRouting();

            //Map routing -> map request url to controller url
            app.UseEndpoints(endPoints =>
            {
                endPoints.MapControllers();
            });

            //For swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "1.0");
            });
        }
    }
}
