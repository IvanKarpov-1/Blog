using Blog.BLL.Core;
using Blog.BLL.Services.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Blog.BLL.Extensions;

public static class ApplicationServiceExtension
{
    public static IServiceCollection AddDalServices(this IServiceCollection services, IConfiguration cfg)
    {
        Blog.DAL.Extensions.ApplicationServiceExtension.AddDalServices(services, cfg);

        return services;
    }

    public static IServiceCollection AddBllServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(List.Handler)));
        services.AddScoped<MapperlyMapper>();

        return services;
    }
}