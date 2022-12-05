using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ReadingList.Business.ServiceImplementations;
using ReadingList.Core.Abstractions;
using ReadingList.Data.Abstractions;
using ReadingList.Data.Abstractions.Repositories;
using ReadingList.Data.Repositories;
using ReadingList.DataBase;
using ReadingList.DataBase.Entities;
using Serilog;
using Serilog.Events;

namespace ReadingList.WebAPI
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

            var myCorsPolicyName = "ReactApp";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(myCorsPolicyName, policyBuilder =>
                {
                    policyBuilder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                });
            });

            var connectionString = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddDbContext<ReadingListDbContext>(
                optionBuilder => optionBuilder.UseSqlServer(connectionString));

            builder.Services.AddControllers();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.IncludeXmlComments(builder.Configuration["APIXmlDocumentation"]);
            });

            // Add business services
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IAuthorService, AuthorService>();
            builder.Services.AddScoped<IBookService, BookService>();

            // Add repositories
            builder.Services.AddScoped<IRepository<Author>, Repository<Author>>();
            builder.Services.AddScoped<IRepository<Category>, Repository<Category>>();
            builder.Services.AddScoped<IRepository<Book>, Repository<Book>>();
            builder.Services.AddScoped<IRepository<BookNote>, Repository<BookNote>>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors(myCorsPolicyName);
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