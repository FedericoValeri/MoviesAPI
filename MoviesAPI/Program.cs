using Microsoft.EntityFrameworkCore;
using MoviesAPI.Controllers.ApiBehavior;
using MoviesAPI.Filters;
using MoviesAPI.Models.Services.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Prevent "No 'Access-Control-Allow-Or igin' header is present on the requested resource" error
// (N.B. This is only needed for fronted applications running inside a browser!)
builder.Services.AddCors(options =>
{
    var frontedURL = builder.Configuration.GetValue<string>("frontend_url");
    options.AddDefaultPolicy(builder =>
    {
        builder
        .WithOrigins(frontedURL)
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ParseBadRequest));
}).ConfigureApiBehaviorOptions(BadRequestBehavior.Parse);

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

app.UseRouting();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();