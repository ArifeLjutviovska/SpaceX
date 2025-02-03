namespace SpacexServer.Api.Commands.Users
{
    using SpacexServer.Api.Common.Interfaces;
    using SpacexServer.Api.Common.Models;
    using SpacexServer.Api.Common.Security;
    using SpacexServer.Api.Contracts.Users.Requests;
    using SpacexServer.Storage.Users.Repositories;
    using System.Threading.Tasks;
    using SpacexServer.Storage.RefreshTokens.Entities;
    using SpacexServer.Storage.RefreshTokens.Repositories;

    /// <summary>
    /// Command to handle user login.
    /// Takes an email and password as input and returns a JWT access token and refresh token upon successful authentication.
    /// </summary>
    /// <param name="request">The login request containing user credentials.</param>
    public class LoginUserCommand(LoginRequest request) : ICommand<Result>
    {
        public LoginRequest Request { get; set; } = request;
    }

    public class LoginUserCommandHandler(IUsersRepository userRepository,
                                         IRefreshTokenRepository refreshTokenRepository,
                                         JwtService jwtService,
                                         IHttpContextAccessor httpContextAccessor)
        : ICommandHandler<LoginUserCommand, Result>
    {
        private readonly IUsersRepository _userRepository = userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
        private readonly JwtService _jwtService = jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result> ExecuteAsync(LoginUserCommand command)
        {
            var user = await _userRepository.GetByEmailAsync(command.Request.Email);

            if (user == null || !PasswordHasher.VerifyPassword(command.Request.Password, user.Password))
            {
                return Result.Unauthorized("Invalid email or password.");
            }

            string accessToken = _jwtService.GenerateAccessToken(user);
            string refreshToken = _jwtService.GenerateRefreshToken();

            var refreshTokenEntity = RefreshToken.Create(user.Id, refreshToken, DateTime.UtcNow.AddDays(7));
            await _refreshTokenRepository.InsertAsync(refreshTokenEntity);

            var httpContext = _httpContextAccessor.HttpContext;

            SetAuthCookies(httpContext!.Response, accessToken, refreshToken);

            return Result.Ok();
        }

        private static void SetAuthCookies(HttpResponse response, string accessToken, string refreshToken)
        {
            response.Cookies.Append("accessToken", accessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,  // Change to `true` in production
                SameSite = SameSiteMode.Strict,
                Path = "/",
                Expires = DateTime.UtcNow.AddMinutes(15)
            });

            response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Strict,
                Path = "/",
                Expires = DateTime.UtcNow.AddDays(7)
            });
        }
    }
}