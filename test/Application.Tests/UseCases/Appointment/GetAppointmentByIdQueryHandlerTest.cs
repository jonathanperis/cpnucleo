namespace Application.Tests.UseCases.Appointment;

public class GetAppointmentByIdQueryHandlerTest : IDisposable
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Domain.Entities.Appointment>> _mockAppointmentRepo;
    private readonly GetAppointmentByIdQueryHandler _handler;
    
    public GetAppointmentByIdQueryHandlerTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockAppointmentRepo = new Mock<IRepository<Domain.Entities.Appointment>>();
        _mockUnitOfWork.Setup(u => u.GetRepository<Domain.Entities.Appointment>())
            .Returns(_mockAppointmentRepo.Object);
        _handler = new GetAppointmentByIdQueryHandler(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAppointment_WhenAppointmentExists()
    {
        // Arrange
        var appointment = Domain.Entities.Appointment.Create(
            "Test Appointment",
            DateTime.UtcNow,
            1,
            BaseEntity.GetNewId(),
            BaseEntity.GetNewId(),
            BaseEntity.GetNewId());

        _mockAppointmentRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(appointment)
            .Verifiable();

        var query = new GetAppointmentByIdQuery(BaseEntity.GetNewId());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.Appointment);
        _mockAppointmentRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenAppointmentDoesNotExist()
    {
        // Arrange
        _mockAppointmentRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null, TimeSpan.FromMilliseconds(1))
            .Verifiable();

        var query = new GetAppointmentByIdQuery(BaseEntity.GetNewId());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.Null(result.Appointment);
        _mockAppointmentRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
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
    
    public void Dispose()
    {
        _mockUnitOfWork.Verify();
        _mockAppointmentRepo.Verify();
        GC.SuppressFinalize(this);
    }
}
