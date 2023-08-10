using datingAppreal.Data;
using datingAppreal.Extensions;
using datingAppreal.InterFace;
using datingAppreal.MiddleWare;
using datingAppreal.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// in this line below we cut all the stuff  that make a mess and put it in folder Extensions in class AppServicesExtension.cs 
builder.Services.Addapplicationservices(builder.Configuration);//read the comment above to understand this line
//add builder for authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)//we could do the  same here and put that in folde Extensions and create new calss to put i in to reduse  the mess but we do not do it
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true ,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"])), 
            ValidateIssuer=false,
            ValidateAudience=false,
        };
    }
    ) ;
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
//app.UseMiddleware<ExceptionMiddleware>();
using var scope = app.Services.CreateScope();//this code is for seed in database
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(context);
}
catch(Exception ex)
{
    var logger = services.GetService<Logger<Program>>();
    //logger.LogError(ex, "An error occured during Migration");
}
app.Run();
