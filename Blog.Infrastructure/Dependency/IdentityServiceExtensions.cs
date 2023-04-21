using Blog.BLL.Services;
using Blog.DAL.DataContext;
using Blog.DAL.Models;
using Blog.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Blog.Infrastructure.Dependency;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration cfg)
    {
        services.AddIdentityCore<User>(opt =>
        {
            opt.Password.RequireNonAlphanumeric = false;
            opt.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<BlogDataContext>();

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(cfg["TokenKey"]!));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                opt.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/comment"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization(opt =>
        {
            opt.AddPolicy("IsArticleAuthor", policy =>
            {
                policy.Requirements.Add(new IsArticleAuthorRequirement());
            });
        });

        services.AddAuthorization(opt =>
        {
            opt.AddPolicy("IsCommentAuthor", policy =>
            {
                policy.Requirements.Add(new IsCommentAuthorRequirement());
            });
        });

        services.AddTransient<IAuthorizationHandler, IsArticleAuthorRequirementHandler>();
        services.AddTransient<IAuthorizationHandler, IsCommentAuthorRequirementHandler>();

        services.AddScoped<TokenService>();

        return services;
    }
}