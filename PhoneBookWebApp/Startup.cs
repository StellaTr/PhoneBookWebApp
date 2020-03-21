using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhoneBookWebApp.Data;
using PhoneBookWebApp.Entities;
using PhoneBookWebApp.Models;
using PhoneBookWebApp.Repository;

namespace PhoneBookWebApp
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
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder => {
                        builder
                        .WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });

            services.AddControllers();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddScoped<IPhoneBookRepository, PhoneBookRepository>();

            services.AddDbContext<PhoneBookContext>(options =>
              options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            var autoMapperConfig = new MapperConfiguration(cfg => {
                cfg.CreateMap<Contact, ContactDto>()
                .ForMember(dest => dest.ContactPhones, opt => opt.MapFrom(src => src.ContactPhones))
                .ReverseMap();
                cfg.CreateMap<ContactPhone, ContactPhoneDto>()
                .ReverseMap();
                cfg.CreateMap<ContactEntry, Contact>()
                .ForMember(dest => dest.ContactPhones, opt => opt.MapFrom(src => src.ContactPhones));
                cfg.CreateMap<ContactPhoneEntry, ContactPhone>();
            });

            services.AddSingleton<IMapper>(s => new Mapper(autoMapperConfig));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context => {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unaccepted error occured");
                    });
                });
            }

            app.UseCors();

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    //spa.UseReactDevelopmentServer(npmScript: "start");
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");
                }
            });
        }
    }
}
