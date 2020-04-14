using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using MWMS.Messaging.Infrastructure;
using MWMS.Messaging.Infrastructure.RabbitMQ;
using MWMS.Services.Maintenance.API.CommandHandlers;
using MWMS.Services.Maintenance.API.Queries;
using MWMS.Services.Maintenance.InfrastructureLayer.Util;

namespace MWMS.Services.Maintenance.API
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //Configure Mongodb
            services.Configure<DatabaseSettings>(options =>
            {
                options.ConnectionString
                    = Configuration.GetSection("MongoConnection:ConnectionString").Value;
                options.DatabaseName
                    = Configuration.GetSection("MongoConnection:Database").Value;
            });

            // add messagepublisher classes
            var configSection = Configuration.GetSection("RabbitMQ");
            string host = configSection["Host"];
            string userName = configSection["UserName"];
            string password = configSection["Password"];
            services.AddTransient<IMessagePublisher>((sp) => new RabbitMQMessagePublisher(host, userName, password, "MWMS"));

            //swagger

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "MWMS API", Version = "v1" });
            });
            //Add AutoMapper
            services.AddAutoMapper(typeof(Startup));

            //Register DI
            InfrastructureLayerDIRegistration.Register(services);
            // MessagingDIRegistration.Register(services);
            CommandHandlersDIRegistration.Register(services);
            QueriesDIRegistration.Register(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MWMS API V1");
            });
        }
    }
}
