namespace SpacexServer.Api.Commands.Users
{
    using SpacexServer.Storage.Users.Entities;
    using SpacexServer.Storage.Users.Repositories;
    using SpacexServer.Api.Common.Interfaces;
    using SpacexServer.Api.Common.Models;
    using SpacexServer.Api.Contracts.Users.Requests;
    using SpacexServer.Api.Common.Security;
    using System.Text.RegularExpressions;

    public class SignUpUserCommand(SignUpUserRequest request) : ICommand<Result>
    {
        public SignUpUserRequest Request { get; set; } = request;
    }

    public class SignUpUserCommandHandler(IUsersRepository userRepository) : ICommandHandler<SignUpUserCommand, Result>
    {
        private readonly IUsersRepository _userRepository = userRepository;

        public async Task<Result> ExecuteAsync(SignUpUserCommand command)
        {
            Result validateRequestResult = ValidateRequest(command.Request);

            if (validateRequestResult.IsFailure)
            {
                return validateRequestResult;
            }

            var existingUser = await _userRepository.GetByEmailAsync(command.Request.Email);

            if (existingUser != null)
            {
                return Result.Conflicted("The email is already registered.");
            }

            string hashedPassword = PasswordHasher.HashPassword(command.Request.Password);

            var user = User.Create(email: command.Request.Email,
                                   firstName: command.Request.FirstName,
                                   lastName: command.Request.LastName,
                                   password: hashedPassword);

            await _userRepository.InsertAsync(user);

            return Result.Ok();
        }

        private static Result ValidateRequest(SignUpUserRequest Request)
        {
            if (string.IsNullOrWhiteSpace(Request.FirstName))
            {
                return Result.Invalid("First name is required.");
            }

            if (string.IsNullOrWhiteSpace(Request.LastName))
            {
                return Result.Invalid("Last name is required.");
            }

            if (string.IsNullOrWhiteSpace(Request.Email))
            {
                return Result.Invalid("Email is required.");
            }

            if (string.IsNullOrWhiteSpace(Request.Password))
            {
                return Result.Invalid("Password is required.");
            }

            if (Request.FirstName.Length > 150)
            {
                return Result.Invalid("First name cannot be longer than 150 characters.");
            }

            if (Request.LastName.Length > 150)
            {
                return Result.Invalid("Last name cannot be longer than 150 characters.");
            }

            if (Request.Email.Length > 75)
            {
                return Result.Invalid("Email cannot be longer than 75 characters.");
            }

            if (Request.Password.Length < 8 || Request.Password.Length > 50)
            {
                return Result.Invalid("Password must be between 8 and 50 characters.");
            }

            if (!IsValidEmail(Request.Email))
            {
                return Result.Invalid("Invalid email format.");
            }

            if (IsDisposableEmail(Request.Email))
            {
                return Result.Invalid("Disposable email addresses are not allowed.");
            }

            if (!IsValidPassword(Request.Password))
            {
                return Result.Invalid("Password must have at least 8 characters, one uppercase, one lowercase, one number, and one special character.");
            }

            return Result.Ok();
        }

        private static bool IsValidEmail(string email)
        {
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }

        private static bool IsDisposableEmail(string email)
        {
            var disposableDomains = new HashSet<string>
            {
                "tempmail.com", "10minutemail.com", "mailinator.com", "guerrillamail.com"
            };

            var domain = email.Split('@').Last();
            return disposableDomains.Contains(domain);
        }

        private static bool IsValidPassword(string password)
        {
            var passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            return Regex.IsMatch(password, passwordPattern);
        }
    }
}
