using datingAppreal.Data;
using datingAppreal.Entities;
using datingAppreal.Extensions;
using datingAppreal.InterFace;
using datingAppreal.MiddleWare;
using datingAppreal.Services;
using datingAppreal.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// in this line below we cut all the stuff  that make a mess and put it in folder Extensions in class AppServicesExtension.cs 
builder.Services.Addapplicationservices(builder.Configuration);//read the comment above to understand this line
//add builder for authentication , // in this line below we cut all the stuff
//that make a mess and put it in folder Extensions in class AddIdentityServices.
builder.Services.AddIdentityServices(builder.Configuration);//read the comment above to understand this line
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthentication();// you should add it here


app.MapControllers();
app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");

//app.UseMiddleware<ExceptionMiddleware>();
using var scope = app.Services.CreateScope();//this code is for seed in database
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
    await context.Database.MigrateAsync();
    await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [Connections]");
    await Seed.SeedUsers(userManager,roleManager);
}
catch(Exception ex)
{
    var logger = services.GetService<Logger<Program>>();
    //logger.LogError(ex, "An error occured during Migration");
}
app.Run();
