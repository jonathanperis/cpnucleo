namespace Application.Tests.UseCases.Appointment;

public class ListAppointmentQueryHandlerTest
{
    private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
    private readonly ListAppointmentQueryHandler _handler;

    public ListAppointmentQueryHandlerTest()
    {
        _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
        _handler = new ListAppointmentQueryHandler(_appointmentRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfAppointments_WhenAppointmentsExist()
    {
        // Arrange
        var appointments = new List<Domain.Entities.Appointment?>
        {
            Domain.Entities.Appointment.Create("Test Appointment 1", DateTime.UtcNow, 1, Guid.NewGuid(), Guid.NewGuid()),
            Domain.Entities.Appointment.Create("Test Appointment 2", DateTime.UtcNow, 2, Guid.NewGuid(), Guid.NewGuid())
        };

        _appointmentRepositoryMock
            .Setup(repo => repo.ListAppointments())
            .ReturnsAsync(appointments);

        var query = new ListAppointmentQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.Appointments);
        Assert.Equal(appointments.Count, result.Appointments.Count);
        _appointmentRepositoryMock.Verify(repo => repo.ListAppointments(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoAppointmentsExist()
    {
        // Arrange
        _appointmentRepositoryMock
            .Setup(repo => repo.ListAppointments())
            .ReturnsAsync([]);

        var query = new ListAppointmentQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.Appointments);
        Assert.Empty(result.Appointments);
        _appointmentRepositoryMock.Verify(repo => repo.ListAppointments(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenListAppointmentsReturnsNull()
    {
        // Arrange
        _appointmentRepositoryMock
            .Setup(repo => repo.ListAppointments())
            .ReturnsAsync(null, TimeSpan.FromMilliseconds(1));

        var query = new ListAppointmentQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.Null(result.Appointments);
        _appointmentRepositoryMock.Verify(repo => repo.ListAppointments(), Times.Once);
    }
}
