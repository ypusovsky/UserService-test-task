using FluentAssertions;
using Moq;
using TestTask.WebApi.Infrastructure;
using TestTask.WebApi.Interfaces;
using TestTask.WebApi.Models;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUserValidator> _userValidatorMock;
    private readonly Mock<IWebSocketService> _webSocketServiceMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userValidatorMock = new Mock<IUserValidator>();
        _webSocketServiceMock = new Mock<IWebSocketService>();
        _userService = new UserService(_userRepositoryMock.Object, _userValidatorMock.Object, _webSocketServiceMock.Object);
    }

    [Fact]
    public async Task Create_ShouldReturnFailure_WhenUserValidationFails()
    {
        var userDto = new UserDto { Email = "test@example.com", Password = "password", Name = "Test User", Role = UserRole.User };

        _userValidatorMock.Setup(v => v.ValidateUserDto(userDto))
                          .ReturnsAsync(OperationResult<bool>.Failure("Invalid user"));

        var result = await _userService.Create(userDto);

        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("Invalid user");
        _userRepositoryMock.Verify(r => r.Create(It.IsAny<UserEntity>()), Times.Never);
    }

    [Fact]
    public async Task Create_ShouldNotifyClients_WhenUserIsSuccessfullyCreated()
    {
        var userDto = new UserDto { Email = "test@example.com", Password = "Passw0rd1", Name = "Test User", Role = UserRole.User };
        var userEntity = new UserEntity { Id = Guid.NewGuid(), Email = userDto.Email, PasswordHash = "hashed_password", Name = userDto.Name, Role = nameof(userDto.Role) };

        _userValidatorMock.Setup(v => v.ValidateUserDto(userDto))
                          .ReturnsAsync(OperationResult<bool>.Success(true));

        _userRepositoryMock.Setup(r => r.Create(It.IsAny<UserEntity>()))
                           .ReturnsAsync(OperationResult<Guid>.Success(userEntity.Id));

        var result = await _userService.Create(userDto);

        result.IsSuccess.Should().BeTrue();
        _webSocketServiceMock.Verify(ws => ws.NotifyClients($"User with {userDto.Email} email successfully created."), Times.Once);
    }

    [Fact]
    public async Task GetAllNames_ShouldNotifyClients_WhenNamesAreRetrieved()
    {
        var names = new List<string> { "User1", "User2" };
        _userRepositoryMock.Setup(r => r.GetAllNames())
                           .ReturnsAsync(names);

        var result = await _userService.GetAllNames();

        result.Should().BeEquivalentTo(names);
        _webSocketServiceMock.Verify(ws => ws.NotifyClients($"2 user names were successfully received."), Times.Once);
    }

    [Fact]
    public async Task UpdateRole_ShouldReturnFailure_WhenRoleValidationFails()
    {
        var userId = Guid.NewGuid();
        var newRole = UserRole.Admin;

        _userValidatorMock.Setup(v => v.ValidateRoleUpdate(userId, newRole))
                          .ReturnsAsync(OperationResult<bool>.Failure("Invalid role update"));

        var result = await _userService.UpdateRole(userId, newRole);

        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("Invalid role update");
        _userRepositoryMock.Verify(r => r.UpdateRole(It.IsAny<Guid>(), It.IsAny<UserRole>()), Times.Never);
    }

    [Fact]
    public async Task UpdateRole_ShouldNotifyClients_WhenRoleIsSuccessfullyUpdated()
    {
        var userId = Guid.NewGuid();
        var newRole = UserRole.Admin;
        _userValidatorMock.Setup(v => v.ValidateRoleUpdate(userId, newRole))
                          .ReturnsAsync(OperationResult<bool>.Success(true));

        _userRepositoryMock.Setup(r => r.UpdateRole(userId, newRole))
                           .ReturnsAsync(OperationResult<Guid>.Success(userId));

        var result = await _userService.UpdateRole(userId, newRole);

        result.IsSuccess.Should().BeTrue();
        _webSocketServiceMock.Verify(ws => ws.NotifyClients($"The role of user with {userId} was successfully changed to {nameof(newRole)}"), Times.Once);
    }
}
