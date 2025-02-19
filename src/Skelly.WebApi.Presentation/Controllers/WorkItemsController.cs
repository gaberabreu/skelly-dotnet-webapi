using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Skelly.WebApi.Application.WorkItemAggregate.Create;
using Skelly.WebApi.Application.WorkItemAggregate.Delete;
using Skelly.WebApi.Application.WorkItemAggregate.Get;
using Skelly.WebApi.Application.WorkItemAggregate.List;
using Skelly.WebApi.Application.WorkItemAggregate.Update;
using Skelly.WebApi.Presentation.Requests;

namespace Skelly.WebApi.Presentation.Controllers;

[Authorize]
public class WorkItemsController(ProblemDetailsFactory problemDetailsFactory, IMediator mediator) : BaseController(problemDetailsFactory)
{
    [HttpGet]
    public async Task<IActionResult> ListWorkItems([FromQuery] ListWorkItemsRequest request)
    {
        var query = new ListWorkItemsQuery(request.PageNumber, request.PageSize);
        var result = await mediator.Send(query);
        return ToActionResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetWorkItem(Guid id)
    {
        var query = new GetWorkItemQuery(id);
        var result = await mediator.Send(query);
        return ToActionResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateWorkItem([FromBody] CreateWorkItemRequest request)
    {
        var command = new CreateWorkItemCommand(request.Title);
        var result = await mediator.Send(command);
        return ToActionResult(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateWorkItem(Guid id, [FromBody] UpdateWorkItemRequest request)
    {
        var command = new UpdateWorkItemCommand(id, request.Title);
        var result = await mediator.Send(command);
        return ToActionResult(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteWorkItem(Guid id)
    {
        var command = new DeleteWorkItemCommand(id);
        var result = await mediator.Send(command);
        return ToActionResult(result);
    }
}
