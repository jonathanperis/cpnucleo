namespace WebApi.Integration.Tests.Hosts;

[Priority(1)]
public class OrganizationCollection : TestCollection<WebAppFixture> { }

[Priority(2)]
public class ProjectCollection : TestCollection<WebAppFixture> { }

[Priority(3)]
public class UserCollection : TestCollection<WebAppFixture> { }

[Priority(4)]
public class WorkflowCollection : TestCollection<WebAppFixture> { }

[Priority(5)]
public class ImpedimentCollection : TestCollection<WebAppFixture> { }

[Priority(6)]
public class AssignmentTypeCollection : TestCollection<WebAppFixture> { }

[Priority(7)]
public class UserProjectCollection : TestCollection<WebAppFixture> { }

[Priority(8)]
public class AssignmentCollection : TestCollection<WebAppFixture> { }

[Priority(9)]
public class AssignmentImpedimentCollection : TestCollection<WebAppFixture> { }

[Priority(10)]
public class UserAssignmentCollection : TestCollection<WebAppFixture> { }

[Priority(11)]
public class AppointmentCollection : TestCollection<WebAppFixture> { }
