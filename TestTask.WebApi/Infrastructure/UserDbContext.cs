using System.Data.SqlClient;
using TestTask.WebApi.Interfaces;
using TestTask.WebApi.Models;

namespace TestTask.WebApi.Infrastructure
{
    public class UserDbContext(string connectionString) : IUserDbContext
    {
        public async Task<OperationResult<Guid>> Create(UserEntity user)
        {
            using (var db = new SqlConnection(connectionString))
            {
                await db.OpenAsync();
                var command = new SqlCommand($"INSERT INTO Users (Id, Name, Email, PasswordHash, Role) VALUES " +
                    $"('{user.Id}','{user.Name}', '{user.Email}', '{user.PasswordHash}', '{user.Role}')", db);
                await command.ExecuteNonQueryAsync();
            }

            return OperationResult<Guid>.Success(user.Id);
        }

        public async Task<IEnumerable<string>> GetAllNames()
        {
            var users = new List<string>();
            using (var db = new SqlConnection(connectionString))
            {
                await db.OpenAsync();
                var command = new SqlCommand("SELECT Name FROM Users", db);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        users.Add(reader.GetString(0));
                    }
                }
            }

            return users;
        }

        public async Task<OperationResult<Guid>> UpdateRole(Guid userId, UserRole newRole)
        {
            using (var db = new SqlConnection(connectionString))
            {
                await db.OpenAsync();
                var command = new SqlCommand(
                    $"UPDATE Users SET Role = '{nameof(newRole)}' WHERE Id = {userId}", db);
                await command.ExecuteNonQueryAsync();
            }

            return OperationResult<Guid>.Success(userId);
        }
    }
}
