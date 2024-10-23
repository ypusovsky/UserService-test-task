using TestTask.WebApi.Infrastructure;
using TestTask.WebApi.Interfaces;
using TestTask.WebApi.Models;

public class UserService(
    IUserRepository userRepository,
    IUserValidator userValidator,
    IWebSocketService webSocketService) : IUserService
{
    public async Task<OperationResult<Guid>> Create(UserDto user)
    {
        var userValidation = await userValidator.ValidateUserDto(user);
        if (!userValidation.IsSuccess && userValidation.Message is not null)
        {
            return OperationResult<Guid>.Failure(userValidation.Message);
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
        OperationResult<Guid> result = await userRepository.Create(new UserEntity
        {
            Id = Guid.NewGuid(),
            Email = user.Email,
            PasswordHash = passwordHash,
            Name = user.Name,
            Role = nameof(user.Role)
        });
        await webSocketService.NotifyClients($"User with {user.Email} email successfully created.");

        return result;
    }

    public async Task<IEnumerable<string>> GetAllNames()
    {
        IEnumerable<string> result = await userRepository.GetAllNames();
        await webSocketService.NotifyClients($"{result.Count()} user names were successfully received.");

        return result;
    }

    public async Task<OperationResult<Guid>> UpdateRole(Guid userId, UserRole newRole)
    {
        var roleValidation = await userValidator.ValidateRoleUpdate(userId, newRole);
        if (!roleValidation.IsSuccess && roleValidation.Message is not null)
        {
            return OperationResult<Guid>.Failure(roleValidation.Message);
        }

        OperationResult<Guid> result = await userRepository.UpdateRole(userId, newRole);
        await webSocketService.NotifyClients($"The role of user with {userId} was successfully changed to {nameof(newRole)}");

        return result;
    }
}