using WebApi.Endpoints.Appointment.ListAppointments;

namespace WebApi.Integration.Tests.Endpoints.Appointment;

[Collection<AppointmentCollection>]
[Priority(2)]
public class ListAppointmentsTests(WebAppFixture app, WebAppState state) : TestBase<WebAppFixture, WebAppState>
{
    [Fact, Priority(1)]
    public async Task Appointments_ShouldReturnAllAppointments()
    {
        var (rsp, err) = await app.Client.GETAsync<Endpoint, Request, ErrorResponse>(new Request
        {
            Pagination = new PaginationParams
            {
                PageNumber = 1,
                PageSize = 10,
                SortColumn = "Name",
                SortOrder = "ASC"
            }
        });

        rsp.StatusCode.ShouldBe(HttpStatusCode.OK);
        err.Errors.ShouldBeEmpty();
    }
}
