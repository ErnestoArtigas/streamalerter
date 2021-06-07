using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using StreamAlerter.Business;
using StreamAlerter.Core.Interfaces.Business;
using StreamAlerter.Core.Interfaces.DatabaseRepository;
using StreamAlerter.Core.Interfaces.HttpsRepository;
using StreamAlerter.DatabaseRepository.Context;
using StreamAlerter.DatabaseRepository.Repositories;
using StreamAlerter.HttpsRepository;

namespace StreamAlerter.Api
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
            services.AddDbContext<StreamAlerterContext>(options => options.UseSqlite("Data Source=../StreamAlerter.DatabaseRepository/Database/streamAlerter.db"));
            services.AddScoped<IStreamAlerterService, StreamAlerterService>();
            services.AddScoped<IStreamAlerterDatabaseRepository, StreamAlerterRepository>();
            services.AddScoped<IStreamAlerterHttpsRepository, TwitchApiRepository>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "StreamAlerter.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "StreamAlerter.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
