namespace Application.Tests.UseCases.Appointment;

public class UpdateAppointmentCommandHandlerTest
{
    private readonly Mock<ApplicationDbContext> _dbContextMock;
    private readonly UpdateAppointmentCommandHandler _handler;
    private readonly List<Domain.Entities.Appointment> _appointments;

    public UpdateAppointmentCommandHandlerTest()
    {
        _dbContextMock = new Mock<ApplicationDbContext>();

        _appointments =
        [
            Domain.Entities.Appointment.Create("Test Appointment 1", DateTime.UtcNow, 1, Ulid.NewUlid(),
                Ulid.NewUlid()),
            Domain.Entities.Appointment.Create("Test Appointment 2", DateTime.UtcNow, 2, Ulid.NewUlid(), Ulid.NewUlid())
        ];

        _dbContextMock.Setup(db => db.Appointments).ReturnsDbSet(_appointments);

        _handler = new UpdateAppointmentCommandHandler(_dbContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenAppointmentIsUpdatedSuccessfully()
    {
        // Arrange
        var appointment = _appointments.First();
        var command = new UpdateAppointmentCommand(appointment.Id, "Updated Appointment", appointment.KeepDate, 2, appointment.AssignmentId, appointment.UserId);

        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result);
        Assert.Equal("Updated Appointment", appointment.Description);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailed_WhenSaveChangesFails()
    {
        // Arrange
        var appointment = _appointments.First();
        var command = new UpdateAppointmentCommand(appointment.Id, "Updated Appointment", appointment.KeepDate, 2, appointment.AssignmentId, appointment.UserId);

        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Failed, result);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenAppointmentDoesNotExist()
    {
        // Arrange
        var command = new UpdateAppointmentCommand(Ulid.NewUlid(), "Updated Appointment", DateTime.UtcNow, 2, Ulid.NewUlid(), Ulid.NewUlid());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public void Handle_ShouldFail_WhenIdIsEmpty()
    {
        // Arrange
        var command = new UpdateAppointmentCommand(Ulid.Empty, "Test Appointment", DateTime.UtcNow, 2, Ulid.NewUlid(), Ulid.NewUlid());
        var validator = new UpdateAppointmentCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }

    [Fact]
    public void Handle_ShouldFail_WhenDescriptionIsEmpty()
    {
        // Arrange
        var command = new UpdateAppointmentCommand(Ulid.NewUlid(), string.Empty, DateTime.UtcNow, 2, Ulid.NewUlid(), Ulid.NewUlid());
        var validator = new UpdateAppointmentCommandValidator();

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
        var command = new UpdateAppointmentCommand(Ulid.NewUlid(), "Test Appointment", default, 2, Ulid.NewUlid(), Ulid.NewUlid());
        var validator = new UpdateAppointmentCommandValidator();

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
        var command = new UpdateAppointmentCommand(Ulid.NewUlid(), "Test Appointment", DateTime.UtcNow, 0, Ulid.NewUlid(), Ulid.NewUlid());
        var validator = new UpdateAppointmentCommandValidator();

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
        var command = new UpdateAppointmentCommand(Ulid.NewUlid(), "Test Appointment", DateTime.UtcNow, 2, Ulid.Empty, Ulid.NewUlid());
        var validator = new UpdateAppointmentCommandValidator();

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
        var command = new UpdateAppointmentCommand(Ulid.NewUlid(), "Test Appointment", DateTime.UtcNow, 2, Ulid.NewUlid(), Ulid.Empty);
        var validator = new UpdateAppointmentCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "UserId"));
    }
}
