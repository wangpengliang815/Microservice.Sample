using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Order.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<OrderContext>(opt => opt.UseSqlServer(Configuration["ConnectionString"]));

            services.AddCap(x =>
            {
                x.UseEntityFramework<OrderContext>().UseRabbitMQ(option =>
                {
                    option.HostName = "192.168.201.191";
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
            //app.RegisterConsul(Configuration, lifetime);
        }
    }
}
