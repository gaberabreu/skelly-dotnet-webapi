using Microsoft.AspNetCore.Mvc;

namespace Skelly.WebApi.Presentation.Requests;

public class CreateWorkItemRequest
{
    [FromBody]
    public required string Title { get; set; }
}
