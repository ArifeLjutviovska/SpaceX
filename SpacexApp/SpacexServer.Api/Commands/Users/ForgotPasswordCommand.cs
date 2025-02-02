﻿namespace SpacexServer.Api.Commands.Users
{
    using SpacexServer.Api.Common.Interfaces;
    using SpacexServer.Api.Common.Models;
    using SpacexServer.Api.Contracts.Users.Requests;
    using SpacexServer.Storage.Users.Entities;
    using SpacexServer.Storage.Users.Repositories;
    using System.Threading.Tasks;

    public class ForgotPasswordCommand(ForgotPasswordRequest request) : ICommand<Result>
    {
        public ForgotPasswordRequest Request { get; set; } = request;
    }

    public class ForgotPasswordCommandHandler(IUsersRepository userRepository)
        : ICommandHandler<ForgotPasswordCommand, Result>
    {
        private readonly IUsersRepository _userRepository = userRepository;

        public async Task<Result> ExecuteAsync(ForgotPasswordCommand command)
        {
            User? user = await _userRepository.GetByEmailAsync(command.Request.Email);

            if (user == null)
            {
                return Result.Failed("User not found.");
            }

            //This is for local testing so I am just sending is Ok result, for production environment
            //configuration should be done to sent reset link to the email of the user

            return Result.Ok();
        }
    }
}