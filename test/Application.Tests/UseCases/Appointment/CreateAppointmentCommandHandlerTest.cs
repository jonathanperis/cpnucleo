namespace Application.Tests.UseCases.Appointment;

public class CreateAppointmentCommandHandlerTest
{
    private readonly Mock<IApplicationDbContext> _dbContextMock;
    private readonly CreateAppointmentCommandHandler _handler;
    private readonly Mock<DbSet<Domain.Entities.Appointment>> _mockAppointmentsDbSet;

    public CreateAppointmentCommandHandlerTest()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        _mockAppointmentsDbSet = new Mock<DbSet<Domain.Entities.Appointment>>();
        _dbContextMock.Setup(db => db.Appointments).Returns(_mockAppointmentsDbSet.Object);

        _handler = new CreateAppointmentCommandHandler(_dbContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenAppointmentIsCreatedSuccessfully()
    {
        // Arrange
        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var command = new CreateAppointmentCommand("Test Appointment", DateTime.UtcNow, 1, Ulid.NewUlid(), Ulid.NewUlid());

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

        var command = new CreateAppointmentCommand("Test Appointment", DateTime.UtcNow, 1, Ulid.NewUlid(), Ulid.NewUlid());

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
        var command = new CreateAppointmentCommand(string.Empty, DateTime.UtcNow, 1, Ulid.NewUlid(), Ulid.NewUlid());
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
        var command = new CreateAppointmentCommand("Test Appointment", default, 1, Ulid.NewUlid(), Ulid.NewUlid());
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
        var command = new CreateAppointmentCommand("Test Appointment", DateTime.UtcNow, 0, Ulid.NewUlid(), Ulid.NewUlid());
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
        var command = new CreateAppointmentCommand("Test Appointment", DateTime.UtcNow, 1, Ulid.Empty, Ulid.NewUlid());
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
        var command = new CreateAppointmentCommand("Test Appointment", DateTime.UtcNow, 1, Ulid.NewUlid(), Ulid.Empty);
        var validator = new CreateAppointmentCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "UserId"));
    }
}
