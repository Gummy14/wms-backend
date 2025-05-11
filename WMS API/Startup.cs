using WMS_API.Layers.Services;
using WMS_API.Layers.Services.Interfaces;
using WMS_API.Layers.Data;
using WMS_API.Layers.Data.Interfaces;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using WMS_API.DbContexts;
using WMS_API.Layers.Data.Interfaces;
using WMS_API.Layers.Data;
using WMS_API.Layers.Services.Interfaces;
using WMS_API.Layers.Services;

namespace WMS_API
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
            string mySqlConnectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContextPool<MyDbContext>(x => x.UseMySql(mySqlConnectionString, ServerVersion.AutoDetect(mySqlConnectionString)));

            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IContainerService, ContainerService>();
            services.AddScoped<IContainerRepository, ContainerRepository>();
            services.AddScoped<IBoxService, BoxService>();
            services.AddScoped<IBoxRepository, BoxRepository>();
            services.AddScoped<IShipmentService, ShipmentService>();
            services.AddScoped<IShipmentRepository, ShipmentRepository>();
            services.AddScoped<ITruckService, TruckService>();
            services.AddScoped<ITruckRepository, TruckRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(builder => builder
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowAnyOrigin()
            );

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}