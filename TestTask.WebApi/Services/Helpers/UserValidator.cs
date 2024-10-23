using TestTask.WebApi.Interfaces;
using TestTask.WebApi.Models;

namespace TestTask.WebApi.Services.Helpers
{
    public class UserValidator(IUserRepository userRepository) : IUserValidator
    {
        public async Task<OperationResult<bool>> ValidateUserDto(UserDto user)
        {
            user.Name = user.Name?.Trim();
            user.Email = user.Email?.Trim();
            user.Password = user.Password?.Trim();

            if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                return OperationResult<bool>.Failure("Name, Email, and Password are required.");
            }

            var emailRegex = new System.Text.RegularExpressions.Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!emailRegex.IsMatch(user.Email))
            {
                return OperationResult<bool>.Failure("Invalid email format.");
            }

            if (user.Password.Length < 8 ||
                !user.Password.Any(char.IsUpper) ||
                !user.Password.Any(char.IsLower) ||
                !user.Password.Any(char.IsDigit))
            {
                return OperationResult<bool>.Failure("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, and one number.");
            }

            var emailExists = await userRepository.EmailExists(user.Email);
            if (emailExists)
            {
                return OperationResult<bool>.Failure("Email is already in use.");
            }

            return OperationResult<bool>.Success(true);
        }

        public async Task<OperationResult<bool>> ValidateRoleUpdate(Guid userId, UserRole newRole)
        {
            var userExists = await userRepository.IdExists(userId);
            if (!userExists)
            {
                return OperationResult<bool>.Failure("If the specified user ID was correct, please try again. If you are unsure of your user ID, please contact support.");
            }

            var validRoles = new List<UserRole> { UserRole.Admin, UserRole.User };
            if (!validRoles.Contains(newRole))
            {
                return OperationResult<bool>.Failure("Invalid role specified.");
            }

            return OperationResult<bool>.Success(true);
        }
    }
}
