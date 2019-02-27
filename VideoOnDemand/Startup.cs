﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VideoOnDemand;
using VideoOnDemand.Data;
using VideoOnDemand.Entities;
using VideoOnDemand.Models;
using VideoOnDemand.Modules.DTOModels;
using VideoOnDemand.Repositories;
using VideoOnDemand.Services;

namespace VideoOnDemand
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            // Adding Created Reading Services
            services.AddSingleton<IReadRepository, SqlReadRepository>();

            // DTO Mapper To Entity
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Video, VideoDTO>();

                cfg.CreateMap<Download, DownloadDTO>()
                    .ForMember(dest => dest.DownloadUrl, src => src.MapFrom(s => s.Url))
                    .ForMember(dest => dest.DownloadTitle, src => src.MapFrom(s => s.Title));

                cfg.CreateMap<Instructor, InstructorDTO>()
                    .ForMember(dest => dest.InstructorName, src => src.MapFrom(s => s.Name))
                    .ForMember(dest => dest.InstructorDescription, src => src.MapFrom(s => s.Description))
                    .ForMember(dest => dest.InstructorAvatar, src => src.MapFrom(s => s.Thumbnail));

                cfg.CreateMap<Course, CourseDTO>()
                    .ForMember(dest => dest.CourseDescription, src => src.MapFrom(s => s.Description))
                    .ForMember(dest => dest.CourseId, src => src.MapFrom(s => s.Id))
                    .ForMember(dest => dest.CourseTitle, src => src.MapFrom(s => s.Title))
                    .ForMember(dest => dest.CourseImageUrl, src => src.MapFrom(s => s.ImageUrl))
                    .ForMember(dest => dest.MarqueeImageUrl, src => src.MapFrom(s => s.MarqueeImageUrl));

                cfg.CreateMap<Module, ModuleDTO>()
                    .ForMember(dest => dest.ModuleTitle, src => src.MapFrom(s => s.Title));

            });

            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            ApplicationDbContext context)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            DbInitializer.Initialize(context);
        }
    }
}
