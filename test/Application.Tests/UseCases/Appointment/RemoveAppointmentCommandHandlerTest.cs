namespace Application.Tests.UseCases.Appointment;

public class RemoveAppointmentCommandHandlerTest
{
    private readonly Mock<ApplicationDbContext> _dbContextMock;
    private readonly RemoveAppointmentCommandHandler _handler;
    private readonly List<Domain.Entities.Appointment> _appointments;

    public RemoveAppointmentCommandHandlerTest()
    {
        _dbContextMock = new Mock<ApplicationDbContext>();

        _appointments =
        [
            Domain.Entities.Appointment.Create("Test Appointment 1", DateTime.UtcNow, 1, Ulid.NewUlid(),
                Ulid.NewUlid()),
            Domain.Entities.Appointment.Create("Test Appointment 2", DateTime.UtcNow, 2, Ulid.NewUlid(), Ulid.NewUlid())
        ];

        _dbContextMock.Setup(db => db.Appointments).ReturnsDbSet(_appointments);

        _handler = new RemoveAppointmentCommandHandler(_dbContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenAppointmentIsRemovedSuccessfully()
    {
        // Arrange
        var appointmentId = _appointments.First().Id;
        var command = new RemoveAppointmentCommand(appointmentId);

        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailed_WhenSaveChangesFails()
    {
        // Arrange
        var appointmentId = _appointments.First().Id;
        var command = new RemoveAppointmentCommand(appointmentId);

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
        var appointmentId = Ulid.NewUlid();
        var command = new RemoveAppointmentCommand(appointmentId);

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
        var command = new RemoveAppointmentCommand(Ulid.Empty);
        var validator = new RemoveAppointmentCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }
}
