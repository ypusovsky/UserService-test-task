using TestTask.WebApi.Infrastructure;
using TestTask.WebApi.Models;

namespace TestTask.WebApi.Interfaces
{
    public interface IUserDbContext
    {
        Task<OperationResult<Guid>> Create(UserEntity user);
        Task<OperationResult<Guid>> UpdateRole(Guid userId, UserRole newRole);
        Task<IEnumerable<string>> GetAllNames();
    }
}
