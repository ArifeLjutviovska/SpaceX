namespace SpacexServer.Api
{
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using SpacexServer.Api.Commands.RefreshTokens;
    using SpacexServer.Api.Commands.SpacexLaunches;
    using SpacexServer.Api.Commands.Users;
    using SpacexServer.Api.Common;
    using SpacexServer.Api.Common.Interfaces;
    using SpacexServer.Api.Common.Models;
    using SpacexServer.Api.Common.Security;
    using SpacexServer.Api.Contracts.SpacexLaunches.Responses;
    using SpacexServer.Api.Contracts.SpacexLaunches.Services;
    using SpacexServer.Storage.Common.Context;
    using SpacexServer.Storage.RefreshTokens.Repositories;
    using SpacexServer.Storage.Users.Repositories;
    using System.Text;

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
            builder.Services.AddTransient<IQueryDispatcher, QueryDispatcher>();
            builder.Services.AddHttpClient<SpaceXLaunchService>();
            builder.Services.AddMemoryCache();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<ISpacexDbContext, SpacexDbContext>();

            var connectionStringTemplate = builder.Configuration.GetConnectionString("DefaultConnection");
            string connectionString = connectionStringTemplate ?? string.Empty;

            if (connectionStringTemplate != null && connectionStringTemplate.Contains("${SQL_SERVER}"))
            {
                connectionString = connectionStringTemplate
                        .Replace("${SQL_SERVER}", Environment.GetEnvironmentVariable("SQL_SERVER"))
                        .Replace("${DB_USERNAME}", Environment.GetEnvironmentVariable("DB_USERNAME"))
                        .Replace("${DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD"));
            }

            builder.Services.AddDbContext<SpacexDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddScoped<IUsersRepository, UsersRepository>();
            builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            builder.Services.AddTransient<ICommandHandler<SignUpUserCommand, Result>, SignUpUserCommandHandler>();
            builder.Services.AddTransient<ICommandHandler<LoginUserCommand, Result>, LoginUserCommandHandler>();
            builder.Services.AddTransient<ICommandHandler<RefreshTokenCommand, Result>, RefreshTokenCommandHandler>();
            builder.Services.AddTransient<ICommandHandler<UpdatePasswordCommand, Result>, UpdatePasswordCommandHandler>();
            builder.Services.AddTransient<ICommandHandler<ForgotPasswordCommand, Result>, ForgotPasswordCommandHandler>();
            builder.Services.AddTransient<ICommandHandler<ResetPasswordCommand, Result>, ResetPasswordCommandHandler>();

            builder.Services.AddTransient<IQueryHandler<GetSpacexLaunchesQuery, PagedResult<SpaceXLaunchDto>>, GetSpacexLaunchesQueryHandler>();

            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? builder.Configuration["JwtSettings:Secret"];

            builder.Configuration["JwtSettings:Secret"] = jwtSecret;

            var key = Encoding.UTF8.GetBytes(jwtSecret);

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = ctx =>
                        {
                            ctx.Request.Cookies.TryGetValue("accessToken", out var accessToken);
                            if (!string.IsNullOrEmpty(accessToken))
                            {
                                ctx.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            builder.Services.AddAuthorization();
            builder.Services.AddScoped<JwtService>();

            builder.Services.AddCors();

            var app = builder.Build();

            app.UseCors(policy => policy
                .WithOrigins("http://localhost:4300", "http://localhost:4200")
                .AllowCredentials()
                .AllowAnyMethod()
                .AllowAnyHeader());

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}