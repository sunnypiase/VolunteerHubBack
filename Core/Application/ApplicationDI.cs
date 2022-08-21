using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;
using Application.Services;

namespace Application
{
    public static class ApplicationDI
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient<HashingService>();
        }
    }
}
