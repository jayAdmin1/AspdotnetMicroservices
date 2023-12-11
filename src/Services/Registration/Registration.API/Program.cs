using Microsoft.EntityFrameworkCore;
using Registration.API.Configuration;
using Registration.API.Data;
using Registration.API.Extensions;
using Registration.API.Mappings;
using Registration.API.Repositories.Abstration;
using Registration.API.Repositories.Implementation;
using Registration.API.Services.Abstration;
using Registration.API.Services.Implementation;
using Registration.API.ViewModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IHelperService, HelperService>();
builder.Services.AddAutoMapper(typeof(RegistrationMapping));
builder.Services.JWTConfiguration(builder.Configuration);
builder.Services.AddSwaggerConfiguration(builder.Configuration,builder.Environment);
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
var app = builder.Build();

app.MigrateDatabase<DataContext>((context, services) =>
{
    var logger = services.GetService<ILogger<DataContext>>();
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
