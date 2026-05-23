using WebApi.Endpoints.Appointment.GetAppointmentById;

namespace WebApi.Integration.Tests.Endpoints.Appointment;

[Collection<AppointmentCollection>]
[Priority(3)]
public class GetAppointmentByIdTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    private readonly Guid _appointmentId = state.AppointmentId;

    [Fact, Priority(1)]
    public async Task Appointments_ShouldGetAnAppointment()
    {
        var (rsp, err) = await app.Client.GETAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Id = _appointmentId
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
