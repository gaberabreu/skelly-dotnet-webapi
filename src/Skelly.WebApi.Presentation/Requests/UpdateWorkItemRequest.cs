using Microsoft.AspNetCore.Mvc;

namespace Skelly.WebApi.Presentation.Requests;

public class UpdateWorkItemRequest
{
    [FromBody]
    public required string Title { get; set; }
}
