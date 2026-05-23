namespace Application.Unit.Tests;

public class CreateProjectHandlerTests
{
    [Test]
    public async Task ExecuteAsync_ShouldReturnValidationFailure_WhenNameIsEmpty()
    {
        var store = A.Fake<IProjectCreateStore>();
        var handler = new CreateProjectHandler(store, NullLogger<CreateProjectHandler>.Instance);

        var result = await handler.ExecuteAsync(new CreateProjectRequest(Guid.CreateVersion7(), " ", Guid.CreateVersion7()));

        result.Success.ShouldBeFalse();
        result.Message.ShouldBe("Name is required.");
        result.Project.ShouldBeNull();
        A.CallTo(() => store.ExistsAsync(A<Guid>._, A<CancellationToken>._)).MustNotHaveHappened();
        A.CallTo(() => store.AddAsync(A<Project>._, A<CancellationToken>._)).MustNotHaveHappened();
    }

    [Test]
    public async Task ExecuteAsync_ShouldReturnValidationFailure_WhenOrganizationIdIsEmpty()
    {
        var store = A.Fake<IProjectCreateStore>();
        var handler = new CreateProjectHandler(store, NullLogger<CreateProjectHandler>.Instance);

        var result = await handler.ExecuteAsync(new CreateProjectRequest(Guid.CreateVersion7(), "New Project", Guid.Empty));

        result.Success.ShouldBeFalse();
        result.Message.ShouldBe("OrganizationId is required.");
        result.Project.ShouldBeNull();
        A.CallTo(() => store.ExistsAsync(A<Guid>._, A<CancellationToken>._)).MustNotHaveHappened();
        A.CallTo(() => store.AddAsync(A<Project>._, A<CancellationToken>._)).MustNotHaveHappened();
    }

    [Test]
    public async Task ExecuteAsync_ShouldReturnConflict_WhenProjectIdAlreadyExists()
    {
        var projectId = Guid.CreateVersion7();
        var store = A.Fake<IProjectCreateStore>();
        A.CallTo(() => store.ExistsAsync(projectId, A<CancellationToken>._)).Returns(true);
        var handler = new CreateProjectHandler(store, NullLogger<CreateProjectHandler>.Instance);

        var result = await handler.ExecuteAsync(new CreateProjectRequest(projectId, "Existing Project", Guid.CreateVersion7()));

        result.Success.ShouldBeFalse();
        result.Message.ShouldBe("this Id is already in use!");
        result.Project.ShouldBeNull();
        A.CallTo(() => store.AddAsync(A<Project>._, A<CancellationToken>._)).MustNotHaveHappened();
    }

    [Test]
    public async Task ExecuteAsync_ShouldCreateProject_WhenProjectIdIsAvailable()
    {
        var projectId = Guid.CreateVersion7();
        var organizationId = Guid.CreateVersion7();
        var createdProject = Project.Create("New Project", organizationId, projectId);
        var store = A.Fake<IProjectCreateStore>();
        A.CallTo(() => store.ExistsAsync(projectId, A<CancellationToken>._)).Returns(false);
        A.CallTo(() => store.AddAsync(A<Project>.That.Matches(project =>
                project.Id == projectId &&
                project.Name == "New Project" &&
                project.OrganizationId == organizationId),
            A<CancellationToken>._)).Returns(createdProject);
        var handler = new CreateProjectHandler(store, NullLogger<CreateProjectHandler>.Instance);

        var result = await handler.ExecuteAsync(new CreateProjectRequest(projectId, "New Project", organizationId));

        result.Success.ShouldBeTrue();
        result.Message.ShouldBe("Project created successfully.");
        result.Project.ShouldNotBeNull();
        result.Project.Id.ShouldBe(projectId);
        result.Project.Name.ShouldBe("New Project");
        result.Project.OrganizationId.ShouldBe(organizationId);
    }
}
