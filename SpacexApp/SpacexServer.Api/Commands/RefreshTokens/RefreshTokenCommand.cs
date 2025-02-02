namespace SpacexServer.Api.Commands.RefreshTokens
{
    using SpacexServer.Api.Common.Interfaces;
    using SpacexServer.Api.Common.Models;
    using SpacexServer.Api.Common.Security;
    using SpacexServer.Api.Contracts.Users.Responses;
    using SpacexServer.Storage.Users.Repositories;
    using System.Threading.Tasks;
    using SpacexServer.Api.Contracts.RefreshTokens.Requests;
    using SpacexServer.Storage.RefreshTokens.Entities;
    using SpacexServer.Storage.RefreshTokens.Repositories;

    /// <summary>
    /// Handles user authentication token renewal by validating the provided refresh token.
    /// If valid, it generates a new access token and a new refresh token while revoking the old one.
    /// </summary>
    public class RefreshTokenCommand(RefreshTokenRequest request) : ICommand<Result<LoginUserResponse>>
    {
        public RefreshTokenRequest Request { get; set; } = request;
    }

    public class RefreshTokenCommandHandler(IRefreshTokenRepository refreshTokenRepository, IUsersRepository userRepository, JwtService jwtService)
        : ICommandHandler<RefreshTokenCommand, Result<LoginUserResponse>>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
        private readonly IUsersRepository _userRepository = userRepository;
        private readonly JwtService _jwtService = jwtService;

        /// <summary>
        /// Executes the refresh token logic: 
        /// - Validates the provided refresh token.
        /// - Revokes the old refresh token.
        /// - Generates a new access token and a new refresh token.
        /// - Stores the new refresh token in the database.
        /// </summary>
        /// <param name="command">The refresh token request containing the token to validate.</param>
        /// <returns>
        /// Returns a `LoginUserResponse` containing the new access and refresh tokens if successful,
        /// otherwise returns an unauthorized error.
        /// </returns>
        public async Task<Result<LoginUserResponse>> ExecuteAsync(RefreshTokenCommand command)
        {
            RefreshToken? storedToken = await _refreshTokenRepository.GetByTokenAsync(command.Request.RefreshToken);
            if (storedToken == null || storedToken.RevokedAt.HasValue || storedToken.ExpiresAt < DateTime.UtcNow)
            {
                return Result.Unauthorized<LoginUserResponse>("Invalid or expired refresh token.");
            }

            var user = await _userRepository.GetByIdAsync(storedToken.UserFk);

            if (user == null)
            {
                return Result.Unauthorized<LoginUserResponse>("User not found.");
            }

            string newAccessToken = _jwtService.GenerateAccessToken(user);
            string newRefreshToken = _jwtService.GenerateRefreshToken();

            storedToken.Revoke();
            await _refreshTokenRepository.UpdateAsync(storedToken);

            var newRefreshTokenEntity = RefreshToken.Create(user.Id, newRefreshToken, DateTime.UtcNow.AddDays(7));
            await _refreshTokenRepository.InsertAsync(newRefreshTokenEntity);

            return Result.Ok(new LoginUserResponse(newAccessToken, newRefreshToken));
        }
    }
}