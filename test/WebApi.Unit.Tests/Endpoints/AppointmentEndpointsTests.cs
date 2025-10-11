namespace WebApi.Unit.Tests.Endpoints;

[TestFixture]
public class AppointmentEndpointsTests
{
    [Test]
    public async Task GetAppointmentById_WithValidId_ShouldReturnAppointment()
    {
        // Arrange
        var appointmentId = Guid.NewGuid();
        var assignmentId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var appointment = Appointment.Create("Test Appointment", DateTime.UtcNow, 8, assignmentId, userId, appointmentId);
        
        var fakeRepository = A.Fake<IRepository<Appointment>>();
        A.CallTo(() => fakeRepository.GetByIdAsync(appointmentId))
            .Returns(Task.FromResult<Appointment?>(appointment));

        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        A.CallTo(() => fakeUnitOfWork.GetRepository<Appointment>()).Returns(fakeRepository);

        var ep = Factory.Create<WebApi.Endpoints.Appointment.GetAppointmentById.Endpoint>(fakeUnitOfWork);
        var req = new WebApi.Endpoints.Appointment.GetAppointmentById.Request { Id = appointmentId };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.Response.ShouldNotBeNull();
        ep.Response.Appointment.ShouldNotBeNull();
        ep.Response.Appointment.Id.ShouldBe(appointmentId);
        ep.Response.Appointment.Description.ShouldBe("Test Appointment");
    }

    [Test]
    public async Task GetAppointmentById_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var appointmentId = Guid.NewGuid();
        
        var fakeRepository = A.Fake<IRepository<Appointment>>();
        A.CallTo(() => fakeRepository.GetByIdAsync(appointmentId))
            .Returns(Task.FromResult<Appointment?>(null));

        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        A.CallTo(() => fakeUnitOfWork.GetRepository<Appointment>()).Returns(fakeRepository);

        var ep = Factory.Create<WebApi.Endpoints.Appointment.GetAppointmentById.Endpoint>(fakeUnitOfWork);
        var req = new WebApi.Endpoints.Appointment.GetAppointmentById.Request { Id = appointmentId };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.HttpContext.Response.StatusCode.ShouldBe(404);
    }

    [Test]
    public async Task ListAppointments_ShouldReturnPaginatedResults()
    {
        // Arrange
        var appointmentId1 = Guid.NewGuid();
        var appointmentId2 = Guid.NewGuid();
        var assignmentId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var appointment1 = Appointment.Create("Appointment 1", DateTime.UtcNow, 8, assignmentId, userId, appointmentId1);
        var appointment2 = Appointment.Create("Appointment 2", DateTime.UtcNow, 8, assignmentId, userId, appointmentId2);
        
        var paginatedResult = new PaginatedResult<Appointment?>
        {
            Data = new List<Appointment?> { appointment1, appointment2 },
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        var fakeRepository = A.Fake<IRepository<Appointment>>();
        A.CallTo(() => fakeRepository.GetAllAsync(A<PaginationParams>._))
            .Returns(Task.FromResult(paginatedResult));

        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        A.CallTo(() => fakeUnitOfWork.GetRepository<Appointment>()).Returns(fakeRepository);

        var ep = Factory.Create<WebApi.Endpoints.Appointment.ListAppointments.Endpoint>(fakeUnitOfWork);
        
        // Initialize response manually due to required property
        ep.Response = new WebApi.Endpoints.Appointment.ListAppointments.Response 
        { 
            Result = new PaginatedResult<WebApi.Common.Dtos.AppointmentDto?> 
            { 
                Data = new List<WebApi.Common.Dtos.AppointmentDto?>(), 
                TotalCount = 0, 
                PageNumber = 1, 
                PageSize = 10 
            } 
        };
        
        var req = new WebApi.Endpoints.Appointment.ListAppointments.Request
        {
            Pagination = new PaginationParams { PageNumber = 1, PageSize = 10, SortColumn = "Id", SortOrder = "ASC" }
        };

        // Act
        await ep.HandleAsync(req, default);
        var rsp = ep.Response;

        // Assert
        rsp.ShouldNotBeNull();
        rsp.Result.ShouldNotBeNull();
        rsp.Result.Data.ShouldNotBeNull();
        rsp.Result.Data.Count().ShouldBe(2);
        rsp.Result.TotalCount.ShouldBe(2);
    }
}
