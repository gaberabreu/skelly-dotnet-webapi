using Microsoft.AspNetCore.Mvc.Infrastructure;
using Skelly.WebApi.Application.Common;
using Skelly.WebApi.Application.WorkItemAggregate;
using Skelly.WebApi.Application.WorkItemAggregate.Create;
using Skelly.WebApi.Application.WorkItemAggregate.Delete;
using Skelly.WebApi.Application.WorkItemAggregate.Get;
using Skelly.WebApi.Application.WorkItemAggregate.List;
using Skelly.WebApi.Application.WorkItemAggregate.Update;
using Skelly.WebApi.Presentation.Controllers;

namespace Skelly.WebApi.Presentation.UnitTests.Controllers;

public class WorkItemsControllerTests
{
    private readonly Mock<IMediator> _mediator = new();
    private readonly Mock<ProblemDetailsFactory> _factory = new();
    private readonly WorkItemsController _controller;

    public WorkItemsControllerTests()
    {
        _controller = new(_factory.Object, _mediator.Object);
    }

    [Fact]
    public async Task GivenRequest_WhenListingWorkItems_ThenReturnsOk()
    {
        // Given
        var request = new ListWorkItemsRequestFaker().Generate();
        var dtos = new WorkItemDtoFaker().GenerateBetween(1, 10);
        var pagedList = new PagedList<WorkItemDto>(dtos, dtos.Count, request.PageNumber, request.PageSize);

        _mediator.Setup(m => m.Send(It.IsAny<ListWorkItemsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(pagedList));

        // When
        var result = await _controller.ListWorkItems(request);

        // Then
        var objectResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<PagedList<WorkItemDto>>(objectResult.Value);
        Assert.Equal(pagedList, response);

        _mediator.Verify(m => m.Send(It.IsAny<ListWorkItemsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GivenRequest_WhenGettingWorkItem_ThenReturnsOk()
    {
        // Given
        var id = Guid.NewGuid();
        var dto = new WorkItemDtoFaker().Generate();

        _mediator.Setup(m => m.Send(It.IsAny<GetWorkItemQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(dto));

        // When
        var result = await _controller.GetWorkItem(id);

        // Then
        var objectResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<WorkItemDto>(objectResult.Value);
        Assert.Equal(dto, response);

        _mediator.Verify(m => m.Send(It.IsAny<GetWorkItemQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GivenRequest_WhenCreatingWorkItem_ThenReturnsOk()
    {
        // Given
        var request = new CreateWorkItemRequestFaker().Generate();
        var dto = new WorkItemDtoFaker().Generate();

        _mediator.Setup(m => m.Send(It.IsAny<CreateWorkItemCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(dto));

        // When
        var result = await _controller.CreateWorkItem(request);

        // Then
        var objectResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<WorkItemDto>(objectResult.Value);
        Assert.Equal(dto, response);

        _mediator.Verify(m => m.Send(It.IsAny<CreateWorkItemCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GivenRequest_WhenUpdatingWorkItem_ThenReturnsOk()
    {
        // Given
        var id = Guid.NewGuid();
        var request = new UpdateWorkItemRequestFaker().Generate();
        var dto = new WorkItemDtoFaker().Generate();

        _mediator.Setup(m => m.Send(It.IsAny<UpdateWorkItemCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(dto));

        // When
        var result = await _controller.UpdateWorkItem(id, request);

        // Then
        var objectResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<WorkItemDto>(objectResult.Value);
        Assert.Equal(dto, response);

        _mediator.Verify(m => m.Send(It.IsAny<UpdateWorkItemCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GivenRequest_WhenDeletingWorkItem_ThenReturnsNoContent()
    {
        // Given
        var id = Guid.NewGuid();

        _mediator.Setup(m => m.Send(It.IsAny<DeleteWorkItemCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.NoContent());

        // When
        var result = await _controller.DeleteWorkItem(id);

        // Then
        Assert.IsType<NoContentResult>(result);

        _mediator.Verify(m => m.Send(It.IsAny<DeleteWorkItemCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
