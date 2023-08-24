using datingAppreal.Data;
using datingAppreal.Helpers;
using datingAppreal.InterFace;
using datingAppreal.Services;
using datingAppreal.SignalR;
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
            
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<LogUserAcivity>();
           
            
            services.AddSignalR();
            services.AddSingleton<PresenceTracker>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
