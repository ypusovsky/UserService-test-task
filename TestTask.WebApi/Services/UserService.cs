using TestTask.WebApi.Infrastructure;
using TestTask.WebApi.Interfaces;
using TestTask.WebApi.Models;

public class UserService(
    IUserRepository userRepository,
    IUserValidator userValidator) : IUserService
{
    public async Task<OperationResult<Guid>> Create(UserDto user)
    {
        var userValidation = await userValidator.ValidateUserDto(user);
        if (!userValidation.IsSuccess && userValidation.Message is not null)
        {
            return OperationResult<Guid>.Failure(userValidation.Message);
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
        return await userRepository.Create(new UserEntity
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

    public async Task<OperationResult<Guid>> UpdateRole(Guid userId, UserRole newRole)
    {
        var roleValidation = await userValidator.ValidateRoleUpdate(userId, newRole);
        if (!roleValidation.IsSuccess && roleValidation.Message is not null)
        {
            return OperationResult<Guid>.Failure(roleValidation.Message);
        }

        return await userRepository.UpdateRole(userId, newRole);
    }
}