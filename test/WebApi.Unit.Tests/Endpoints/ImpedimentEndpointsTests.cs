namespace WebApi.Unit.Tests.Endpoints;

[TestFixture]
public class ImpedimentEndpointsTests
{
    [Test]
    public async Task GetImpedimentById_WithValidId_ShouldReturnImpediment()
    {
        // Arrange
        var impedimentId = Guid.NewGuid();
        var impediment = Impediment.Create("Test Impediment", impedimentId);
        
        var fakeRepository = A.Fake<IRepository<Impediment>>();
        A.CallTo(() => fakeRepository.GetByIdAsync(impedimentId))
            .Returns(Task.FromResult<Impediment?>(impediment));

        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        A.CallTo(() => fakeUnitOfWork.GetRepository<Impediment>()).Returns(fakeRepository);

        var ep = Factory.Create<WebApi.Endpoints.Impediment.GetImpedimentById.Endpoint>(fakeUnitOfWork);
        var req = new WebApi.Endpoints.Impediment.GetImpedimentById.Request { Id = impedimentId };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.Response.ShouldNotBeNull();
        ep.Response.Impediment.ShouldNotBeNull();
        ep.Response.Impediment.Id.ShouldBe(impedimentId);
        ep.Response.Impediment.Name.ShouldBe("Test Impediment");
    }

    [Test]
    public async Task GetImpedimentById_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var impedimentId = Guid.NewGuid();
        
        var fakeRepository = A.Fake<IRepository<Impediment>>();
        A.CallTo(() => fakeRepository.GetByIdAsync(impedimentId))
            .Returns(Task.FromResult<Impediment?>(null));

        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        A.CallTo(() => fakeUnitOfWork.GetRepository<Impediment>()).Returns(fakeRepository);

        var ep = Factory.Create<WebApi.Endpoints.Impediment.GetImpedimentById.Endpoint>(fakeUnitOfWork);
        var req = new WebApi.Endpoints.Impediment.GetImpedimentById.Request { Id = impedimentId };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.HttpContext.Response.StatusCode.ShouldBe(404);
    }

    [Test]
    public async Task UpdateImpediment_WithValidData_ShouldUpdateImpediment()
    {
        // Arrange
        var impedimentId = Guid.NewGuid();
        var impediment = Impediment.Create("Original Impediment", impedimentId);
        
        var fakeRepository = A.Fake<IRepository<Impediment>>();
        A.CallTo(() => fakeRepository.GetByIdAsync(impedimentId)).Returns(Task.FromResult<Impediment?>(impediment));
        A.CallTo(() => fakeRepository.UpdateAsync(A<Impediment>._)).Returns(Task.FromResult(true));

        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        A.CallTo(() => fakeUnitOfWork.GetRepository<Impediment>()).Returns(fakeRepository);

        var ep = Factory.Create<WebApi.Endpoints.Impediment.UpdateImpediment.Endpoint>(fakeUnitOfWork);
        var req = new WebApi.Endpoints.Impediment.UpdateImpediment.Request
        {
            Id = impedimentId,
            Name = "Updated Impediment"
        };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.Response.ShouldNotBeNull();
        ep.Response.Success.ShouldBeTrue();
    }

    [Test]
    public async Task RemoveImpediment_WithValidId_ShouldDeleteImpediment()
    {
        // Arrange
        var impedimentId = Guid.NewGuid();
        var impediment = Impediment.Create("Impediment to Delete", impedimentId);
        
        var fakeRepository = A.Fake<IRepository<Impediment>>();
        A.CallTo(() => fakeRepository.GetByIdAsync(impedimentId)).Returns(Task.FromResult<Impediment?>(impediment));
        A.CallTo(() => fakeRepository.UpdateAsync(A<Impediment>._)).Returns(Task.FromResult(true));

        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        A.CallTo(() => fakeUnitOfWork.GetRepository<Impediment>()).Returns(fakeRepository);

        var ep = Factory.Create<WebApi.Endpoints.Impediment.RemoveImpediment.Endpoint>(fakeUnitOfWork);
        var req = new WebApi.Endpoints.Impediment.RemoveImpediment.Request { Ids = new List<Guid> { impedimentId } };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.Response.ShouldNotBeNull();
        ep.Response.Success.ShouldBeTrue();
    }
}
