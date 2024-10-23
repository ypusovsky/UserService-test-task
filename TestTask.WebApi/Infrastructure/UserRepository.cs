using TestTask.WebApi.Interfaces;
using TestTask.WebApi.Models;

namespace TestTask.WebApi.Infrastructure
{
    public class UserRepository(IUserDbContext db) : IUserRepository
    {
        public async Task<OperationResult<Guid>> Create(UserEntity user)
        {
            return await db.Create(user);
        }

        public async Task<IEnumerable<string>> GetAllNames()
        {
            return await db.GetAllNames();
        }

        public async Task<OperationResult<Guid>> UpdateRole(Guid userId, UserRole newRole)
        {
            return await db.UpdateRole(userId, newRole);
        }
        public async Task<bool> EmailExists(string email)
        {
            return await db.EmailExists(email);
        }
        public async Task<bool> IdExists(Guid id)
        {
            return await db.IdExists(id);
        }
    }
}
