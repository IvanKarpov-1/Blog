using Blog.DAL.Contracts;
using Blog.DAL.DataContext;
using Blog.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.DAL.Extensions;

public static class ApplicationServiceExtension
{
    public static IServiceCollection AddDalServices(this IServiceCollection services, IConfiguration cfg)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddDbContext<BlogDataContext>(option =>
        {
            option.UseSqlite(cfg.GetConnectionString("DefaultConnection"));
        });

        return services;
    }
}