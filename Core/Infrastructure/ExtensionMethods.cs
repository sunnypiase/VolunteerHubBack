using Domain;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class ExtensionMethods
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationContext>(optionsBuilder =>
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), 
            x => x.MigrationsAssembly("Infrastructure")));
            
            services.AddScoped<IGenericRepository<User>, SqlGenericRepository<User>>();
            services.AddScoped<IGenericRepository<Tag>, SqlGenericRepository<Tag>>();
            services.AddScoped<IGenericRepository<Post>, SqlGenericRepository<Post>>();
        }
    }
}
