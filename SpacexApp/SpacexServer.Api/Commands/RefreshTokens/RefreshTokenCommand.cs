namespace SpacexServer.Api.Commands.RefreshTokens
{
    using SpacexServer.Api.Common.Interfaces;
    using SpacexServer.Api.Common.Models;
    using SpacexServer.Api.Common.Security;
    using SpacexServer.Storage.Users.Repositories;
    using System.Threading.Tasks;
    using SpacexServer.Storage.RefreshTokens.Entities;
    using SpacexServer.Storage.RefreshTokens.Repositories;
    using SpacexServer.Storage.Users.Entities;

    /// <summary>
    /// Handles user authentication token renewal by validating the provided refresh token.
    /// If valid, it generates a new access token and a new refresh token while revoking the old one.
    /// </summary>
    public class RefreshTokenCommand() : ICommand<Result>{}

    public class RefreshTokenCommandHandler(IRefreshTokenRepository refreshTokenRepository,
                                            IUsersRepository userRepository,
                                            JwtService jwtService,
                                            IHttpContextAccessor httpContextAccessor)
        : ICommandHandler<RefreshTokenCommand, Result>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
        private readonly IUsersRepository _userRepository = userRepository;
        private readonly JwtService _jwtService = jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result> ExecuteAsync(RefreshTokenCommand command)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return Result.Unauthorized("Invalid request.");
            }

            string? refreshToken = httpContext.Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Result.Unauthorized("No refresh token found.");
            }

            RefreshToken? storedToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
            if (storedToken == null || storedToken.RevokedAt.HasValue || storedToken.ExpiresAt < DateTime.UtcNow)
            {
                return Result.Unauthorized("Invalid or expired refresh token.");
            }

            User? user = await _userRepository.GetByIdAsync(storedToken.UserFk);

            if (user == null)
            {
                return Result.Unauthorized("User not found.");
            }

            string newAccessToken = _jwtService.GenerateAccessToken(user);
            string newRefreshToken = _jwtService.GenerateRefreshToken();

            storedToken.Revoke();
            await _refreshTokenRepository.UpdateAsync(storedToken);

            var newRefreshTokenEntity = RefreshToken.Create(user.Id, newRefreshToken, DateTime.UtcNow.AddSeconds(30));
            await _refreshTokenRepository.InsertAsync(newRefreshTokenEntity);

            SetAuthCookies(httpContext.Response, newAccessToken, newRefreshToken);

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