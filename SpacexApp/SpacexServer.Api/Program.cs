namespace SpacexServer.Api
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using SpacexServer.Api.Commands.Users;
    using SpacexServer.Api.Common;
    using SpacexServer.Api.Common.Interfaces;
    using SpacexServer.Api.Common.Models;
    using SpacexServer.Storage.Common.Context;
    using SpacexServer.Storage.Users.Repositories;

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
                .Replace("${SQL_SERVER}", Environment.GetEnvironmentVariable("SQL_SERVER"))
                .Replace("${DB_USERNAME}", Environment.GetEnvironmentVariable("DB_USERNAME"))
                .Replace("${DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD"));

            builder.Services.AddDbContext<SpacexDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddScoped<IUsersRepository, UsersRepository>();

            builder.Services.AddTransient<ICommandHandler<SignUpUserCommand, Result>, SignUpUserCommandHandler>();

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