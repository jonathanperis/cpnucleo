using WebApi.Endpoints.Appointment.UpdateAppointment;

namespace WebApi.Integration.Tests.Endpoints.Appointment;

[Collection<AppointmentCollection>]
[Priority(4)]
public class UpdateAppointmentTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _appointmentId = state.AppointmentId;
    private readonly Guid _assignmentId = state.AssignmentId;
    private readonly Guid _userId = state.UserId;

    [Fact, Priority(1)]
    public async Task Appointments_ShouldUpdateAnAppointment()
    {
        var (rsp, err) = await app.Client.PATCHAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Name = "Updated appointment",
            Description = "Updated appointment description",
            KeepDate = DateTime.UtcNow,
            AmountHours = 4,
            AssignmentId = _assignmentId,
            UserId = _userId,
            Id = _appointmentId
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
