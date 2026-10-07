using Domain.Common;
using Domain.Common.Security;
using Domain.Entities;
using Domain.Models;

namespace Application.Unit.Tests;

public class DomainContractTests
{
    [TestCase(0, 10)]
    [TestCase(1, -1)]
    [TestCase(1, 0)]
    [TestCase(1, 101)]
    [TestCase(int.MaxValue, 10)]
    public void Pagination_RejectsUnboundedOrInvalidRequests(int page, int size)
    {
        // Binders and serializers can always build the object; Require enforces the bounds.
        var pagination = new PaginationParams { PageNumber = page, PageSize = size };

        pagination.Problems().ShouldNotBeEmpty();
        Should.Throw<DomainException>(() => PaginationParams.Require(pagination));
    }

    [Test]
    public void Pagination_BoundsSearchAndIds()
    {
        Should.Throw<DomainException>(() => PaginationParams.Require(new PaginationParams { Search = new string('x', 129) })).Field.ShouldBe("search");
        Should.Throw<DomainException>(() => PaginationParams.Require(new PaginationParams { Ids = "not-a-uuid" })).Field.ShouldBe("ids");
        Should.Throw<DomainException>(() => PaginationParams.Require(new PaginationParams { Ids = string.Join(',', Enumerable.Range(0, 101).Select(_ => Guid.NewGuid())) }));
        Should.Throw<DomainException>(() => PaginationParams.Require(null)).Field.ShouldBe("pagination");

        var id = Guid.NewGuid();
        var valid = PaginationParams.Require(new PaginationParams { Ids = $"{id}, {id}", Search = "  term  " });
        valid.GetIds().ShouldBe([id]);
        valid.Search.ShouldBe("term");
        valid.Problems().ShouldBeEmpty();
    }

    [TestCase("50%", "%50\\%%")]
    [TestCase("snake_case", "%snake\\_case%")]
    [TestCase("back\\slash", "%back\\\\slash%")]
    public void Pagination_EscapesLikeWildcardsInSearch(string search, string pattern)
    {
        new PaginationParams { Search = search }.GetSearchPattern().ShouldBe(pattern);
    }

    [Test]
    public void Assignment_RejectsInvalidChangesWithoutMutatingTheEntity()
    {
        var start = DateTime.UtcNow;
        var assignment = Assignment.Create("Learn transactions", "Description", start, start.AddDays(1), 2,
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        var error = Should.Throw<DomainException>(() => Assignment.Update(assignment, "Changed", "Description", start, start.AddDays(-1), 2,
            assignment.ProjectId, assignment.WorkflowId, assignment.UserId, assignment.AssignmentTypeId));
        error.Field.ShouldBe("EndDate");
        assignment.Name.ShouldBe("Learn transactions");
        Should.Throw<DomainException>(() => Assignment.Create("Zero hours", "Description", start, start, 0,
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid())).Field.ShouldBe("AmountHours");
        Should.Throw<DomainException>(() => Appointment.Create("Invalid hours", start, 0, assignment.Id, assignment.UserId));
    }

    [Test]
    public void Timestamps_AreNormalizedToUtc()
    {
        var unspecified = new DateTime(2064, 6, 9, 0, 0, 0, DateTimeKind.Unspecified);
        var local = new DateTime(2064, 6, 9, 12, 0, 0, DateTimeKind.Local);
        var assignment = Assignment.Create("Dates", "Description", unspecified, local, 1, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        assignment.StartDate.Kind.ShouldBe(DateTimeKind.Utc);
        assignment.StartDate.ShouldBe(new DateTime(2064, 6, 9, 0, 0, 0, DateTimeKind.Utc));
        assignment.EndDate.ShouldBe(local.ToUniversalTime());
    }

    public static IEnumerable<TestCaseData> InvalidEntities()
    {
        var id = Guid.NewGuid();
        yield return new TestCaseData((Action)(() => Organization.Create(" ", null))).SetName("Organization requires a name");
        yield return new TestCaseData((Action)(() => Workflow.Create("Planned", 0))).SetName("Workflow order must be positive");
        yield return new TestCaseData((Action)(() => Workflow.Create("", 1))).SetName("Workflow requires a name");
        yield return new TestCaseData((Action)(() => Impediment.Create(null))).SetName("Impediment requires a name");
        yield return new TestCaseData((Action)(() => AssignmentType.Create(" "))).SetName("AssignmentType requires a name");
        yield return new TestCaseData((Action)(() => Project.Create("Project", Guid.Empty))).SetName("Project requires an organization");
        yield return new TestCaseData((Action)(() => User.Create("Name", " ", new PasswordHash("h", "")))).SetName("User requires a login");
        yield return new TestCaseData((Action)(() => UserProject.Create(Guid.Empty, id))).SetName("UserProject requires a user");
        yield return new TestCaseData((Action)(() => UserAssignment.Create(id, Guid.Empty))).SetName("UserAssignment requires an assignment");
        yield return new TestCaseData((Action)(() => AssignmentImpediment.Create("", id, id))).SetName("AssignmentImpediment requires a description");
        yield return new TestCaseData((Action)(() => Organization.Create(new string('n', Guard.NameMaxLength + 1), null))).SetName("Names are bounded");
    }

    [TestCaseSource(nameof(InvalidEntities))]
    public void Factories_EnforceTheSameInvariantsForEveryTransport(Action create)
    {
        Should.Throw<DomainException>(create);
    }

    [Test]
    public void Remove_IsIdempotent()
    {
        var workflow = Workflow.Create("Planned", 1);
        Workflow.Remove(workflow);
        var deletedAt = workflow.DeletedAt;

        Workflow.Remove(workflow);

        workflow.Active.ShouldBeFalse();
        workflow.DeletedAt.ShouldBe(deletedAt);
    }

    [TestCase("Short1!", false)]
    [TestCase("alllowercase1!", false)]
    [TestCase("NoDigits!!", false)]
    [TestCase("Valid@123", true)]
    public void PasswordPolicy_IsSharedByBothTransports(string password, bool valid)
    {
        PasswordPolicy.IsSatisfiedBy(password).ShouldBe(valid);
        if (!valid) Should.Throw<DomainException>(() => PasswordPolicy.Validate(password));
    }

    [Test]
    public void SecurityStamp_ChangesWithPasswordOrLogin()
    {
        var stamp = SecurityStamp.Compute("hash", "Jane");
        SecurityStamp.Compute("hash", " jane ").ShouldBe(stamp, "logins are compared case- and whitespace-insensitively");
        SecurityStamp.Compute("other-hash", "jane").ShouldNotBe(stamp);
        SecurityStamp.Compute("hash", "john").ShouldNotBe(stamp);
    }

    [Test]
    public void BatchIds_AreDistinctAndBounded()
    {
        var id = Guid.NewGuid();
        BatchIds.Normalize([id, id]).ShouldBe([id]);
        Should.Throw<DomainException>(() => BatchIds.Normalize([]));
        Should.Throw<DomainException>(() => BatchIds.Normalize([Guid.Empty]));
        Should.Throw<DomainException>(() => BatchIds.Normalize(Enumerable.Range(0, 101).Select(_ => Guid.NewGuid())));
    }
}
