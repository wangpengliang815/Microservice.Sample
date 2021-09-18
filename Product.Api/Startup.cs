using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Product.Api
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

            services.AddDbContext<ProductContext>(opt => opt.UseSqlServer(Configuration["ConnectionString"]));

            services.AddCap(x =>
            {
                x.UseEntityFramework<ProductContext>().UseRabbitMQ(option =>
                {
                    option.HostName = "192.168.31.191";
                    option.UserName = "guest";
                    option.Password = "guest";
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment()) { } else { }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // ЗўЮёзЂВс
            app.RegisterConsul(Configuration, lifetime);
        }
    }
}
