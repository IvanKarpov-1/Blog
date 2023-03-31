using Blog.API.Extensions;
using Blog.DAL;
using Blog.DAL.DataContext;
using Microsoft.EntityFrameworkCore;
using System.Text;

Console.OutputEncoding = Encoding.Default;
Console.InputEncoding = Encoding.Default;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<BlogDataContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedData(context);
}
catch (Exception e)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(e, "Помилка відбулась піч час міграції");
}

app.Run();
