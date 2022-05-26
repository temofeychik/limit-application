using LimiterApplication.Data;
using LimiterApplication.Middlewares;
using LimiterApplication.Services;
using LimiterApplication.Services.Interfaces;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IUserService, UserService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ApiTokenMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    serviceScope.ServiceProvider.GetService<ApplicationDbContext>().Database.Migrate();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRateLimiter(new RateLimiterOptions
{
    Limiter = PartitionedRateLimiter.Create<HttpContext, string>(_ =>
    {
        return RateLimitPartition.CreateTokenBucketLimiter<string>("Limiter",
            _ => new TokenBucketRateLimiterOptions(10, QueueProcessingOrder.NewestFirst, 0, TimeSpan.FromMinutes(10), 10));
    }),
    DefaultRejectionStatusCode = (int)HttpStatusCode.TooManyRequests
});

app.MapControllers();

app.Run();
