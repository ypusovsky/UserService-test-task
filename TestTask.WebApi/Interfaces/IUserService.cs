using TestTask.WebApi.Models;

namespace TestTask.WebApi.Interfaces
{
    public interface IUserService
    {
        Task<OperationResult<Guid>> Create(UserDto user);
        Task<OperationResult<Guid>> UpdateRole(Guid userId, UserRole newRole);
        Task<IEnumerable<string>> GetAllNames();
    }
}
