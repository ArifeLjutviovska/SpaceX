namespace SpacexServer.Services.User.Commands
{
    using SpacexServer.Common.Interfaces;
    using SpacexServer.Common.Models;
    using SpacexServer.Contracts.Common.Repositories;
    using SpacexServer.Contracts.User.Repositories;
    using SpacexServer.Contracts.User.Requests;
    using SpacexServer.Entities.User.Domain;

    public class AddNewUserCommand(CreateUserRequest userRequest) : ICommand<Result>
    {
        public CreateUserRequest UserRequest { get; set; } = userRequest;
    }

    public class AddNewUserCommandHandler(IUserRepository userRepository, IUnitOfWork _unitOfWork) : ICommandHandler<AddNewUserCommand, Result>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUnitOfWork _unitOfWork = _unitOfWork;

        public async Task<Result> ExecuteAsync(AddNewUserCommand command)
        {
            User user = User.Create(name: command.UserRequest.Name, createdOn: DateTime.UtcNow);

            _userRepository.Insert(user: user);

            await _unitOfWork.SaveAsync();

            return Result.Ok();
        }
    }
}