using CRUDExample.Filters.ActionFilters;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using ServiceContracts;
using Services;

namespace CRUDExample
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ResponseHeaderActionFilter>();

            services.AddControllersWithViews(options =>
               {
                   //options.Filters.Add<ResponseHeaderActionFilter>(5);

                   //var logger = builder.Services.BuildServiceProvider()
                   //.GetRequiredService<ILogger<ResponseHeaderActionFilter>>();
                   var logger = services.BuildServiceProvider()
                   .GetRequiredService<ILogger<ResponseHeaderActionFilter>>();
                   options.Filters.Add(new ResponseHeaderActionFilter(logger)
                   {
                       _Key = "my-key-from-global",
                       _Value = "my-value-from-controller",
                       Order = 2
                   });
               });
            services.AddScoped<ICountriesAdderService, CountriesAdderService>();
            services.AddScoped<ICountriesGetterService, CountriesGetterService>();
            services.AddScoped<ICountriesUploaderService, CountriesUploaderService>();
            services.AddScoped<IPersonsDeleterService, PersonsDeleteService>();
            services.AddScoped<IPersonsGetterService,PersonsGetterServiceChild>();
            services.AddScoped<PersonsGetterService, PersonsGetterService>();
            services.AddScoped<IPersonsAdderService, PersonsAdderService>();
            services.AddScoped<IPersonsUpdaterService, PersonsUpdaterService>();
            services.AddScoped<IPersonsSorterService, PersonsSorterService>();
            services.AddScoped<ICountriesRepository, CountriesRepository>();
            services.AddScoped<IPersonsRepository, PersonsRepository>();


            services.AddDbContext<ApplicationDbContext>
                   (
                       options =>
                       {
                           options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                       }
                   );
            services.AddTransient<PersonsListActionFilter>();
            services.AddHttpLogging(options =>
               {
                   options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties
                   | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
               });
            return services;
        }
    }
}
