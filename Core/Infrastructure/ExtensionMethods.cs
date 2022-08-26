using Application.Repositories;
using Application.Repositories.Abstractions;
using Application.Services;
using Infrastructure.Repositories;
using Infrastructure.Services;
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

            services.AddTransient<IHashingService, HashingService>();

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ITagRepository, TagRepository>();
            services.AddTransient<IPostRepository, PostRepository>();
            services.AddTransient<IPostConnectionRepository, PostConnectionRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }
    }
}
