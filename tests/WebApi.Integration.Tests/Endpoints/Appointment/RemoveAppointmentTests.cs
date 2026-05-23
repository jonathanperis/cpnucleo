using WebApi.Endpoints.Appointment.RemoveAppointment;

namespace WebApi.Integration.Tests.Endpoints.Appointment;

[Collection<AppointmentCollection>]
[Priority(5)]
public class RemoveAppointmentTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _appointmentId = state.AppointmentId;

    [Fact, Priority(1)]
    public async Task Appointments_ShouldDeleteAnAppointment()
    {
        var (rsp, err) = await app.Client.DELETEAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Ids = [_appointmentId]
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
