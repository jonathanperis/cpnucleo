namespace Application.Tests.UseCases.Appointment;

public class CreateAppointmentCommandHandlerTest
{
    private readonly Mock<IApplicationDbContext> _dbContextMock;
    private readonly CreateAppointmentCommandHandler _handler;

    public CreateAppointmentCommandHandlerTest()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        Mock<DbSet<Domain.Entities.Appointment>> mockAppointmentsDbSet = new();
        _dbContextMock.Setup(db => db.Appointments).Returns(mockAppointmentsDbSet.Object);

        _handler = new CreateAppointmentCommandHandler(_dbContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenAppointmentIsCreatedSuccessfully()
    {
        // Arrange
        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var command = new CreateAppointmentCommand("Test Appointment", DateTime.UtcNow, 1, BaseEntity.GetNewId(), BaseEntity.GetNewId());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result);
        _dbContextMock.Verify(db => db.Appointments!.AddAsync(It.IsAny<Domain.Entities.Appointment>(), It.IsAny<CancellationToken>()), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailed_WhenSaveChangesFails()
    {
        // Arrange
        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var command = new CreateAppointmentCommand("Test Appointment", DateTime.UtcNow, 1, BaseEntity.GetNewId(), BaseEntity.GetNewId());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Failed, result);
        _dbContextMock.Verify(db => db.Appointments!.AddAsync(It.IsAny<Domain.Entities.Appointment>(), It.IsAny<CancellationToken>()), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void Handle_ShouldFail_WhenDescriptionIsEmpty()
    {
        // Arrange
        var command = new CreateAppointmentCommand(string.Empty, DateTime.UtcNow, 1, BaseEntity.GetNewId(), BaseEntity.GetNewId());
        var validator = new CreateAppointmentCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Description"));
    }

    [Fact]
    public void Handle_ShouldFail_WhenKeepDateIsEmpty()
    {
        // Arrange
        var command = new CreateAppointmentCommand("Test Appointment", default, 1, BaseEntity.GetNewId(), BaseEntity.GetNewId());
        var validator = new CreateAppointmentCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "KeepDate"));
    }

    [Fact]
    public void Handle_ShouldFail_WhenAmountHoursIsZero()
    {
        // Arrange
        var command = new CreateAppointmentCommand("Test Appointment", DateTime.UtcNow, 0, BaseEntity.GetNewId(), BaseEntity.GetNewId());
        var validator = new CreateAppointmentCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "AmountHours"));
    }

    [Fact]
    public void Handle_ShouldFail_WhenAssignmentIdIsEmpty()
    {
        // Arrange
        var command = new CreateAppointmentCommand("Test Appointment", DateTime.UtcNow, 1, Guid.Empty, BaseEntity.GetNewId());
        var validator = new CreateAppointmentCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "AssignmentId"));
    }

    [Fact]
    public void Handle_ShouldFail_WhenUserIdIsEmpty()
    {
        // Arrange
        var command = new CreateAppointmentCommand("Test Appointment", DateTime.UtcNow, 1, BaseEntity.GetNewId(), Guid.Empty);
        var validator = new CreateAppointmentCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "UserId"));
    }
}
