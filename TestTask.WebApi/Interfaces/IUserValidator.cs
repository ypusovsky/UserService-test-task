using TestTask.WebApi.Models;

namespace TestTask.WebApi.Interfaces
{
    public interface IUserValidator
    {
        Task<OperationResult<bool>> ValidateUserDto(UserDto user);
        Task<OperationResult<bool>> ValidateRoleUpdate(Guid userId, UserRole newRole);
    }
}
