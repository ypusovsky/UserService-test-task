using TestTask.WebApi.Interfaces;
using TestTask.WebApi.Models;

namespace TestTask.WebApi.Services.Helpers
{
    public class UserValidator : IUserValidator
    {
        public OperationResult<bool> ValidateUserDto(UserDto user)
        {
            if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                return OperationResult<bool>.Failure("Invalid input");
            }

            var emailRegex = new System.Text.RegularExpressions.Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!emailRegex.IsMatch(user.Email))
            {
                return OperationResult<bool>.Failure("Invalid email");
            }

            return OperationResult<bool>.Success(true);
        }

        public OperationResult<bool> ValidateRole(UserRole newRole)
        {
            if (newRole != UserRole.Admin && newRole != UserRole.User)
            {
                return OperationResult<bool>.Failure("Invalid role");
            }

            return OperationResult<bool>.Success(true);
        }
    }
}
