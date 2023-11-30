using ITentikaTest.BackgroundServices;
using ITentikaTest.Clients;
using ITentikaTest.Domain.Data;
using ITentikaTest.Domain.IRepositories;
using ITentikaTest.Domain.Repositories;
using ITentikaTest.IClients;

namespace ITentikaTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<AppDbContext>();
            builder.Services.AddControllers();
            builder.Services.AddScoped<IIncidentRepository, IncidentRepository>();
            builder.Services.AddHttpClient<IProcessorClient, ProcessorClient>();
            builder.Services.AddHostedService<GeneratorHostedService>();
            builder.Services.AddHostedService<ProcessorHostedService>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
