
using TextProcessing.Api.Application;
using TextProcessing.Api.Infrastructure;
using TextProcessing.Api.Middleware;

namespace TextProcessing.Api
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);
      // Kestrel tuned for long-lived streaming responses
      builder.WebHost.ConfigureKestrel(options =>
      {
        options.Limits.KeepAliveTimeout = TimeSpan.FromHours(4);
        options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(2);
      });

      // Add services to the container.

      builder.Services.AddControllers();
      // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
      builder.Services.AddEndpointsApiExplorer();
      builder.Services.AddSwaggerGen();
      
      builder.Services.AddScoped<IProcessorService, ProcessorService>();
      
      builder.Services.AddCors(options =>
      {
        options.AddDefaultPolicy(policy =>
        {
          policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
      });

      var app = builder.Build();

      // Configure the HTTP request pipeline.
      if (app.Environment.IsDevelopment())
      {
        app.UseSwagger();
        app.UseSwaggerUI();
      }
      app.UseMiddleware<ErrorHandlingMiddleware>();

      app.UseRouting();
      app.UseCors();


      app.MapControllers();

      app.Run();
    }
  }
}
