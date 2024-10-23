namespace TestTask.WebApi.Models
{
    public class UserDto
    {
        public string Name { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public UserRole? Role { get; set; }
    }
}
