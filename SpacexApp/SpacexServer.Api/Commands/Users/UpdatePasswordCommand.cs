namespace SpacexServer.Api.Commands.Users
{
    using SpacexServer.Api.Common.Interfaces;
    using SpacexServer.Api.Common.Models;
    using SpacexServer.Api.Common.Security;
    using SpacexServer.Api.Contracts.Users.Requests;
    using SpacexServer.Storage.Users.Entities;
    using SpacexServer.Storage.Users.Repositories;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    /// <summary>
    /// Handles the process of updating a user's password.
    /// This includes verifying the current password, validating the new password's strength, 
    /// hashing the new password, and updating the database.
    /// </summary>
    public class UpdatePasswordCommand(UpdatePasswordRequest request) : ICommand<Result>
    {
        public UpdatePasswordRequest Request { get; set; } = request;
    }

    public class UpdatePasswordCommandHandler(IUsersRepository userRepository)
        : ICommandHandler<UpdatePasswordCommand, Result>
    {
        private readonly IUsersRepository _userRepository = userRepository;

        public async Task<Result> ExecuteAsync(UpdatePasswordCommand command)
        {
            User? user = await _userRepository.GetByEmailAsync(command.Request.Email);

            if (user == null)
            {
                return Result.Failed("User not found.");
            }

            if (!PasswordHasher.VerifyPassword(command.Request.CurrentPassword, user.Password))
            {
                return Result.Failed("The current password you entered is incorrect.");
            }

            if (string.IsNullOrWhiteSpace(command.Request.NewPassword))
            {
                return Result.Invalid("New password is required to change the current password.");
            }

            if (command.Request.NewPassword.Length < 8 || command.Request.NewPassword.Length > 50)
            {
                return Result.Invalid("New password must be between 8 and 50 characters.");
            }

            if (!IsValidPassword(command.Request.NewPassword))
            {
                return Result.Invalid("New password must have at least 8 characters, one uppercase, one lowercase, one number, and one special character.");
            }

            if (PasswordHasher.VerifyPassword(command.Request.NewPassword, user.Password))
            {
                return Result.Invalid("New password cannot be the same as the current password.");
            }

            string hashedPassword = PasswordHasher.HashPassword(command.Request.NewPassword);
            user.UpdatePassword(hashedPassword);
            await _userRepository.UpdateAsync(user);

            return Result.Ok();
        }

        private static bool IsValidPassword(string password)
        {
            var passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[.@$!%*?&])[A-Za-z\d.@$!%*?&]{8,}$";
            return Regex.IsMatch(password, passwordPattern);
        }
    }
}