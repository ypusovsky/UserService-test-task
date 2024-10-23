using TestTask.WebApi.Models;

namespace TestTask.WebApi.Interfaces
{
    public interface IUserValidator
    {
        OperationResult<bool> ValidateUserDto(UserDto user);
        OperationResult<bool> ValidateRole(UserRole newRole);
    }
}
