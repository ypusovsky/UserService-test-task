using TestTask.WebApi.Infrastructure;
using TestTask.WebApi.Interfaces;
using TestTask.WebApi.Models;

public class UserService(
    IUserRepository userRepository,
    IUserValidator userValidator) : IUserService
{
    public Task<OperationResult<Guid>> Create(UserDto user)
    {
        var userValidation = userValidator.ValidateUserDto(user);
        if (!userValidation.IsSuccess && userValidation.Message is not null)
        {
            return Task.FromResult(OperationResult<Guid>.Failure(userValidation.Message));
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
        return userRepository.Create(new UserEntity
        {
            Id = Guid.NewGuid(),
            Email = user.Email,
            PasswordHash = passwordHash,
            Name = user.Name,
            Role = user.Role
        });
    }

    public Task<IEnumerable<string>> GetAllNames()
    {
        return userRepository.GetAllNames();
    }

    public Task<OperationResult<Guid>> UpdateRole(Guid userId, UserRole newRole)
    {
        var roleValidation = userValidator.ValidateRole(newRole);
        if (!roleValidation.IsSuccess && roleValidation.Message is not null)
        {
            return Task.FromResult(OperationResult<Guid>.Failure(roleValidation.Message));
        }

        return userRepository.UpdateRole(userId, newRole);
    }
}