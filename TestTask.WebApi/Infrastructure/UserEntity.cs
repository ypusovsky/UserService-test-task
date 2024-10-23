﻿namespace TestTask.WebApi.Infrastructure
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string PasswordHash { get; set; } = String.Empty;
        public string? Role { get; set; }
    }
}
