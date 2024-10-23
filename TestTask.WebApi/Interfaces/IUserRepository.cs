using TestTask.WebApi.Infrastructure;
using TestTask.WebApi.Models;

namespace TestTask.WebApi.Interfaces
{
    public interface IUserRepository
    {
        Task<OperationResult<Guid>> Create(UserEntity user);
        Task<OperationResult<Guid>> UpdateRole(Guid userId, UserRole newRole);
        Task<IEnumerable<string>> GetAllNames();
        Task<bool> EmailExists(string email);
        Task<bool> IdExists(Guid id);
    }
}
