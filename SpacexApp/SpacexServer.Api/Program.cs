namespace SpacexServer.Api
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using SpacexServer.Common;
    using SpacexServer.Common.Interfaces;
    using SpacexServer.Common.Models;
    using SpacexServer.Contracts.Common.Repositories;
    using SpacexServer.Contracts.User.Repositories;
    using SpacexServer.Services.User.Commands;
    using SpacexServer.Storage.Common.Context;
    using SpacexServer.Storage.Common.UnitOfWork;
    using SpacexServer.Storage.User.Repositories;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(7005);
            });


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddTransient<ICommandDispatcher, CommandDispatcher>();

            builder.Services.AddScoped<ISpacexDbContext, SpacexDbContext>();

            var connectionStringTemplate = builder.Configuration.GetConnectionString("DefaultConnection");

            string connectionString = connectionStringTemplate
                .Replace("${SQL_SERVER}", Environment.GetEnvironmentVariable("SQL_SERVER") ?? "127.0.0.1,1433")
                .Replace("${DB_USERNAME}", Environment.GetEnvironmentVariable("DB_USERNAME") ?? "sa")
                .Replace("${DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "a123456!");

            builder.Services.AddDbContext<SpacexDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddTransient<ICommandHandler<AddNewUserCommand, Result>, AddNewUserCommandHandler>();

            builder.Services.AddCors();

            var app = builder.Build();

            app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());

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