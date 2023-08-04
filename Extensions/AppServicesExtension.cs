﻿using datingAppreal.Data;
using datingAppreal.InterFace;
using datingAppreal.Services;
using Microsoft.EntityFrameworkCore;

namespace datingAppreal.Extensions
{
    public static class AppServicesExtension
    {
        public static IServiceCollection Addapplicationservices(this IServiceCollection services,IConfiguration config) 
            
        {
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });
            //add bulider for token 
            services.AddScoped<ITokenServices, TokenServices>();
            return services;
        }
    }
}