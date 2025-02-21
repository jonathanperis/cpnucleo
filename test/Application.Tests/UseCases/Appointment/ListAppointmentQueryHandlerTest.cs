namespace Application.Tests.UseCases.Appointment;

public class ListAppointmentQueryHandlerTest : IDisposable
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Domain.Entities.Appointment>> _mockAppointmentRepo;
    private readonly ListAppointmentQueryHandler _handler;

    public ListAppointmentQueryHandlerTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockAppointmentRepo = new Mock<IRepository<Domain.Entities.Appointment>>();
        _mockUnitOfWork.Setup(u => u.GetRepository<Domain.Entities.Appointment>())
            .Returns(_mockAppointmentRepo.Object);
        _handler = new ListAppointmentQueryHandler(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfAppointments_WhenAppointmentsExist()
    {
        // Arrange
        var pagination = new PaginationParams { PageNumber = 1, PageSize = 10 };
        var appointments = new List<Domain.Entities.Appointment>
        {
            Domain.Entities.Appointment.Create("Test Appointment 1", DateTime.UtcNow, 1, BaseEntity.GetNewId(), BaseEntity.GetNewId()),
            Domain.Entities.Appointment.Create("Test Appointment 2", DateTime.UtcNow, 2, BaseEntity.GetNewId(), BaseEntity.GetNewId())
        };
        var paginatedResult = new PaginatedResult<Domain.Entities.Appointment?>
        {
            Data = appointments,
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };

        _mockAppointmentRepo.Setup(r => r.GetAllAsync(pagination))
            .ReturnsAsync(paginatedResult)
            .Verifiable();

        var query = new ListAppointmentQuery(pagination);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.Result.Data);
        Assert.Equal(appointments.Count, result.Result.Data.Count());
        _mockAppointmentRepo.Verify(repo => repo.GetAllAsync(pagination), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoAppointmentsExist()
    {
        // Arrange
        var pagination = new PaginationParams { PageNumber = 1, PageSize = 10 };
        var paginatedResult = new PaginatedResult<Domain.Entities.Appointment?>
        {
            Data = [],
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };

        _mockAppointmentRepo.Setup(r => r.GetAllAsync(pagination))
            .ReturnsAsync(paginatedResult)
            .Verifiable();

        var query = new ListAppointmentQuery(pagination);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.NotNull(result.Result.Data);
        Assert.Empty(result.Result.Data);
        _mockAppointmentRepo.Verify(repo => repo.GetAllAsync(pagination), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenListAppointmentsReturnsNull()
    {
        // Arrange
        var pagination = new PaginationParams { PageNumber = 1, PageSize = 10 };
        var paginatedResult = new PaginatedResult<Domain.Entities.Appointment?>
        {
            Data = null,
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };

        _mockAppointmentRepo.Setup(r => r.GetAllAsync(pagination))
            .ReturnsAsync(paginatedResult)
            .Verifiable();

        var query = new ListAppointmentQuery(pagination);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.Null(result.Result.Data);
        _mockAppointmentRepo.Verify(repo => repo.GetAllAsync(pagination), Times.Once);
    }

    public void Dispose()
    {
        _mockUnitOfWork.Verify();
        _mockAppointmentRepo.Verify();
        GC.SuppressFinalize(this);
    }
}
