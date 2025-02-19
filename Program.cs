using Microsoft.EntityFrameworkCore;
using LimsTravauxService.Data;
using LimsTravauxService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TravauxContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("MySqlConnection"),
        new MySqlServerVersion(new Version(8, 0, 39)) // Use your MySQL version
    ));

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:5077") // Replace with your client's origin
               .AllowAnyMethod();
    });
});

builder.Services.AddScoped<ITypeTravauxService, TypeTravauxService>();
builder.Services.AddScoped<IAvanceeTravailService, AvanceeTravailService>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("MyCorsPolicy"); // Make sure this is called before app.UseAuthorization()

app.MapControllers(); // This line is crucial !

app.Run();
