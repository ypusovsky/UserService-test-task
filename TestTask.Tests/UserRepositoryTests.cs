using FluentAssertions;
using Moq;
using TestTask.WebApi.Infrastructure;
using TestTask.WebApi.Interfaces;
using TestTask.WebApi.Models;

public class UserRepositoryTests
{
    private readonly Mock<IUserDbContext> _dbContextMock;
    private readonly UserRepository _userRepository;

    public UserRepositoryTests()
    {
        _dbContextMock = new Mock<IUserDbContext>();
        _userRepository = new UserRepository(_dbContextMock.Object);
    }

    [Fact]
    public async Task Create_ShouldReturnSuccess_WhenUserIsCreated()
    {
        var userEntity = new UserEntity
        {
            Id = Guid.NewGuid(),
            Name = "Test User",
            Email = "test@example.com",
            PasswordHash = "hashed_password",
            Role = nameof(UserRole.User)
        };

        _dbContextMock.Setup(db => db.Create(userEntity))
                       .ReturnsAsync(OperationResult<Guid>.Success(userEntity.Id));

        var result = await _userRepository.Create(userEntity);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(userEntity.Id);
        _dbContextMock.Verify(db => db.Create(userEntity), Times.Once);
    }

    [Fact]
    public async Task GetAllNames_ShouldReturnNames_WhenUsersExist()
    {
        var userNames = new List<string> { "User1", "User2" };
        _dbContextMock.Setup(db => db.GetAllNames())
                       .ReturnsAsync(userNames);

        var result = await _userRepository.GetAllNames();

        result.Should().BeEquivalentTo(userNames);
        _dbContextMock.Verify(db => db.GetAllNames(), Times.Once);
    }

    [Fact]
    public async Task UpdateRole_ShouldReturnSuccess_WhenRoleIsUpdated()
    {
        var userId = Guid.NewGuid();
        var newRole = UserRole.Admin;

        _dbContextMock.Setup(db => db.UpdateRole(userId, newRole))
                       .ReturnsAsync(OperationResult<Guid>.Success(userId));

        var result = await _userRepository.UpdateRole(userId, newRole);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(userId);
        _dbContextMock.Verify(db => db.UpdateRole(userId, newRole), Times.Once);
    }

    [Fact]
    public async Task EmailExists_ShouldReturnTrue_WhenEmailExists()
    {
        string email = "test@example.com";
        _dbContextMock.Setup(db => db.EmailExists(email))
                       .ReturnsAsync(true);

        var result = await _userRepository.EmailExists(email);

        result.Should().BeTrue();
        _dbContextMock.Verify(db => db.EmailExists(email), Times.Once);
    }

    [Fact]
    public async Task IdExists_ShouldReturnTrue_WhenIdExists()
    {
        var userId = Guid.NewGuid();
        _dbContextMock.Setup(db => db.IdExists(userId))
                       .ReturnsAsync(true);

        var result = await _userRepository.IdExists(userId);

        result.Should().BeTrue();
        _dbContextMock.Verify(db => db.IdExists(userId), Times.Once);
    }
}
