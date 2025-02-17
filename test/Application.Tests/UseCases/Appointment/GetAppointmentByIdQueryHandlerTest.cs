namespace Application.Tests.UseCases.Appointment;

public class GetAppointmentByIdQueryHandlerTest
{
    private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
    private readonly GetAppointmentByIdQueryHandler _handler;

    public GetAppointmentByIdQueryHandlerTest()
    {
        _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
        _handler = new GetAppointmentByIdQueryHandler(_appointmentRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAppointment_WhenAppointmentExists()
    {
        // Arrange
        var appointmentDto = new AppointmentDto("Test Appointment", DateTime.UtcNow, 1, Guid.NewGuid(), Guid.NewGuid())
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };

        _appointmentRepositoryMock
            .Setup(repo => repo.GetAppointmentById(It.IsAny<Guid>()))
            .ReturnsAsync(appointmentDto);

        var query = new GetAppointmentByIdQuery(Guid.NewGuid());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.Appointment);
        _appointmentRepositoryMock.Verify(repo => repo.GetAppointmentById(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenAppointmentDoesNotExist()
    {
        // Arrange
        _appointmentRepositoryMock
            .Setup(repo => repo.GetAppointmentById(It.IsAny<Guid>()))
            .ReturnsAsync((AppointmentDto?)null);

        var query = new GetAppointmentByIdQuery(Guid.NewGuid());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.Null(result.Appointment);
        _appointmentRepositoryMock.Verify(repo => repo.GetAppointmentById(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public void Handle_ShouldFail_WhenIdIsEmpty()
    {
        // Arrange
        var query = new GetAppointmentByIdQuery(Guid.Empty);
        var validator = new GetAppointmentByIdQueryValidator();

        // Act
        var result = validator.Validate(query);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors.Find(e => e.PropertyName == "Id"));
    }
}
