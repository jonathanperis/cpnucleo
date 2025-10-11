using WebApi.Endpoints.Appointment.CreateAppointment;

namespace WebApi.Integration.Tests.Endpoints.Appointment;

[Collection<AppointmentCollection>]
[Priority(1)]
public class CreateAppointmentTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _appointmentId = state.AppointmentId;
    private readonly Guid _assignmentId = state.AssignmentId;
    private readonly Guid _userId = state.UserId;

    [Fact, Priority(1)]
    public async Task Appointments_ShouldCreateAnAppointment()
    {
        var (rsp, err) = await app.Client.POSTAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Name = "New appointment",
            Description = "New appointment description",
            KeepDate = DateTime.UtcNow,
            AmountHours = 2,
            AssignmentId = _assignmentId,
            UserId = _userId,
            Id = _appointmentId
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
