using Blog.BLL.Core;
using Blog.BLL.Services.Users;
using Blog.DAL.Contracts;
using Blog.DAL.DataContext;
using Blog.DAL.Repositories;
using Blog.DAL.UnitOfWork;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using Create = Blog.BLL.Services.Articles.Create;


namespace Blog.Infrastructure.Dependency;

public static class ApplicationServiceExtension
{
    public static IServiceCollection AddDalServices(this IServiceCollection services, IConfiguration cfg)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddDbContext<BlogDataContext>(option =>
        {
            option.UseSqlite(cfg.GetConnectionString("DefaultConnection"));
        });

        return services;
    }

    public static IServiceCollection AddBllServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(List.Handler)));
        services.AddScoped<MapperlyMapper>();
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<Create>();
        services.AddScoped<ClaimsPrincipal>();
        services.AddSignalR(opt =>
        {
            opt.EnableDetailedErrors = true;
        });

        return services;
    }
}