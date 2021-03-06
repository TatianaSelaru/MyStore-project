using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyStore.Data;
using MyStore.Domain.Entities;
using MyStore.Domain.Infrastructure;
using MyStore.Infrastructure;
using MyStore.Services;
using System;
using static MyStore.Data.OrderRepository;

namespace MyStore
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
            /*services.AddControllers().AddJsonOptions(x =>
             x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);*/
            services.AddControllers().AddNewtonsoftJson(options =>
  options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "MyStore", Version = "v1" });
            });

            services.AddDbContext<StoreContext>(
                options=> options.UseSqlServer(Configuration.GetConnectionString("StoreDb"))
                
                );
            //load the appsettins in a strongly-typed class
            services.Configure<MySettings>(Configuration.GetSection("MySettings"));

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddAutoMapper(typeof(ProductProfile));

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<ISupplierService, SupplierService>();

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();


            services.AddScoped<IReportsRepository, ReportsRepository>();
            services.AddScoped<IReportsService, ReportsService>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Take1 v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<SecurityHeaderMiddleWare>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            
        }
    }
}
