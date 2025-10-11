using Infrastructure.Common.Context;
using Microsoft.EntityFrameworkCore;

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
        
        var fakeDbContext = A.Fake<IApplicationDbContext>();
        var fakeDbSet = A.Fake<DbSet<Impediment>>();
        
        A.CallTo(() => fakeDbContext.Impediments).Returns(fakeDbSet);
        A.CallTo(() => fakeDbSet.FindAsync(A<object[]>._, A<CancellationToken>._)).Returns(new ValueTask<Impediment?>(impediment));

        var ep = Factory.Create<WebApi.Endpoints.Impediment.GetImpedimentById.Endpoint>(fakeDbContext);
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
        
        var fakeDbContext = A.Fake<IApplicationDbContext>();
        var fakeDbSet = A.Fake<DbSet<Impediment>>();
        
        A.CallTo(() => fakeDbContext.Impediments).Returns(fakeDbSet);
        A.CallTo(() => fakeDbSet.FindAsync(A<object[]>._, A<CancellationToken>._)).Returns(new ValueTask<Impediment?>((Impediment?)null));

        var ep = Factory.Create<WebApi.Endpoints.Impediment.GetImpedimentById.Endpoint>(fakeDbContext);
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
        
        var fakeDbContext = A.Fake<IApplicationDbContext>();
        var fakeDbSet = A.Fake<DbSet<Impediment>>();
        
        A.CallTo(() => fakeDbContext.Impediments).Returns(fakeDbSet);
        A.CallTo(() => fakeDbSet.FindAsync(A<object[]>._, A<CancellationToken>._)).Returns(new ValueTask<Impediment?>(impediment));
        A.CallTo(() => fakeDbContext.SaveChangesAsync(A<CancellationToken>._)).Returns(true);

        var ep = Factory.Create<WebApi.Endpoints.Impediment.UpdateImpediment.Endpoint>(fakeDbContext);
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
        
        var fakeDbContext = A.Fake<IApplicationDbContext>();
        var fakeDbSet = A.Fake<DbSet<Impediment>>();
        
        A.CallTo(() => fakeDbContext.Impediments).Returns(fakeDbSet);
        A.CallTo(() => fakeDbSet.FindAsync(A<object[]>._, A<CancellationToken>._)).Returns(new ValueTask<Impediment?>(impediment));
        A.CallTo(() => fakeDbContext.SaveChangesAsync(A<CancellationToken>._)).Returns(true);

        var ep = Factory.Create<WebApi.Endpoints.Impediment.RemoveImpediment.Endpoint>(fakeDbContext);
        var req = new WebApi.Endpoints.Impediment.RemoveImpediment.Request { Ids = new List<Guid> { impedimentId } };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.Response.ShouldNotBeNull();
        ep.Response.Success.ShouldBeTrue();
    }
}
