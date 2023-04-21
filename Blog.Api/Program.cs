using Blog.API.Extensions;
using Blog.API.Middleware;
using Blog.API.SignalR;
using System.Text;

Console.OutputEncoding = Encoding.Default;
Console.InputEncoding = Encoding.Default;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<CommentHub>("/comment");

//using var scope = app.Services.CreateScope();
//var services = scope.ServiceProvider;

//try
//{
//    var context = services.GetRequiredService<BlogDataContext>();
//    var userManager = services.GetRequiredService<UserManager<User>>();
//    await context.Database.MigrateAsync();
//    await Seed.SeedData(context, userManager);
//}
//catch (Exception e)
//{
//    var logger = services.GetRequiredService<ILogger<Program>>();
//    logger.LogError(e, "Помилка відбулась піч час міграції");
//}

app.Run();
