using System;
using AspNetCore.WebApi.Dtos;
using AspNetCore.WebApi.Entities;
using AspNetCore.WebApi.Repositories;
using AspNetCore.WebApi.Services;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.Swagger;

namespace AspNetCore.WebApi
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

            services.AddOptions();
            services.AddDbContext<AppointmentDbContext>(opt => opt.UseInMemoryDatabase("AppointmentDatabase"));
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            services.AddSingleton<IInitDataService, InitDataService>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddRouting(options => options.LowercaseUrls = true);
            //services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            //services.AddScoped<IUrlHelper>(x =>
            //{
            //    var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
            //    var factory = x.GetRequiredService<IUrlHelperFactory>();
            //    return factory.GetUrlHelper(actionContext);
            //});

            //services.AddSwagger(
            //   options =>
            //   {
            //       var provider = services.BuildServiceProvider();
            //       options.SwaggerDoc(String.Empty,
            //           new Info()
            //           {
            //               Title = $"Sample API",
            //               Version = "1.0.0"
            //           });

            //   });
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
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "text/plain";
                        var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (errorFeature != null)
                        {
                            //var logger = LoggerFactory.CreateLogger("Global exception logger");
                            //logger.LogError(500, errorFeature.Error, errorFeature.Error.Message);
                        }

                        await context.Response.WriteAsync("There was an error");
                    });
                });
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseCors("AllowAllOrigins");
            AutoMapper.Mapper.Initialize(mapper =>
            {
                mapper.CreateMap<AppointmentItem, AppointmentItemDto>().ReverseMap();
                mapper.CreateMap<AppointmentItem, AppointmentUpdateStatusDto>().ReverseMap();
                mapper.CreateMap<AppointmentItem, AppointmentCreateDto>().ReverseMap();
            });
        }
    }
}
