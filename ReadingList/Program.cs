using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ReadingList.Data.Abstractions;
using ReadingList.Data.Repositories;
using ReadingList.DataBase;
using Serilog;
using Serilog.Events;

namespace ReadingList
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((ctx, lc) => lc
                .WriteTo.Console()
                .WriteTo.File(GetPathToLogFile(),
                    LogEventLevel.Information));

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddDbContext<ReadingListDbContext>(
                optionBuilder => optionBuilder.UseSqlServer(connectionString));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add business services

            // Add repositories

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        /// <summary>
        ///     Returns the path for log file recording.
        /// </summary>
        /// <returns>A string whose value contains a path to the log file</returns>
        private static string GetPathToLogFile()
        {
            var sb = new StringBuilder();
            sb.Append(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            sb.Append(@"\logs\");
            sb.Append($"{DateTime.Now:yyyyMMddhhmmss}");
            sb.Append("data.log");
            return sb.ToString();
        }
    }
}