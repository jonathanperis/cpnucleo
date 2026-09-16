using Domain.Entities;
using Domain.Models;

namespace Application.Unit.Tests;

public class DomainContractTests
{
    [TestCase(0, 10)]
    [TestCase(1, -1)]
    [TestCase(1, 101)]
    [TestCase(int.MaxValue, 10)]
    public void Pagination_RejectsUnboundedOrInvalidRequests(int page, int size)
    {
        Should.Throw<ArgumentOutOfRangeException>(() => new PaginationParams { PageNumber = page, PageSize = size });
    }

    [Test]
    public void Assignment_RejectsInvalidChangesWithoutMutatingTheEntity()
    {
        var start = DateTime.UtcNow;
        var assignment = Assignment.Create("Learn transactions", "", start, start.AddDays(1), 2,
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        Should.Throw<ArgumentException>(() => Assignment.Update(assignment, "Changed", "", start, start.AddDays(-1), 2,
            assignment.ProjectId, assignment.WorkflowId, assignment.UserId, assignment.AssignmentTypeId));
        assignment.Name.ShouldBe("Learn transactions");
        Should.Throw<ArgumentOutOfRangeException>(() => Appointment.Create("Invalid hours", start, 0, assignment.Id, assignment.UserId));
    }
}
