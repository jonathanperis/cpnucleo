namespace Integration.Tests.Modules;

[Collection("Scenarios"), Order(11)]
public class AppointmentModuleTests(WebAppFixture fixture)
{
    private readonly IAlbaHost _host = fixture.AlbaHost;
    private readonly Guid _appointmentId = fixture.AppointmentId;
    private readonly Guid _assignmentId = fixture.AssignmentId;
    private readonly Guid _userId = fixture.UserId;
    
    [Fact, Order(51)]
    public async Task Appointments_ShouldCreateAnAppointment()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Post.Json(new CreateAppointmentCommand(
                $"Integration Test Appointment #{DateTime.UtcNow.Ticks.ToString()}", 
                DateTime.UtcNow,
                6,
                _assignmentId,
                _userId,
                _appointmentId)).ToUrl("/api/appointments");
            s.StatusCodeShouldBe(201);
        });
    }    
    
    [Fact, Order(52)]
    public async Task Appointments_ShouldReturnAllAppointments()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Get.Json(new PaginationParams { PageNumber = 1, PageSize = 10, SortColumn = "Id", SortOrder = "ASC"}).ToUrl("/api/appointments");
            s.StatusCodeShouldBeOk();
        });
    }    
    
    [Fact, Order(53)]
    public async Task Appointments_ShouldGetAnAppointment()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Get.Json(new{ }).ToUrl("/api/appointments/" + _appointmentId);
            s.StatusCodeShouldBe(200);
        });      
    }
    
    [Fact, Order(54)]
    public async Task Appointments_ShouldUpdateAnAppointment()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Patch.Json(new UpdateAppointmentCommand(_appointmentId,
                $"Integration Test Appointment UPDATED #{DateTime.UtcNow.Ticks.ToString()}", 
                DateTime.UtcNow,
                6,
                _assignmentId,
                _userId)).ToUrl("/api/appointments/" + _appointmentId);
            s.StatusCodeShouldBe(204);
        });
    }
    
    [Fact, Order(55)]
    public async Task Appointments_ShouldDeleteAnAppointment()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Delete.Json(new{ }).ToUrl("/api/appointments/" + _appointmentId);
            s.StatusCodeShouldBe(204);
        });    
    }
}