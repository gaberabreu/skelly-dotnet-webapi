using Microsoft.AspNetCore.Mvc;

namespace Skelly.WebApi.Presentation.Requests;

public class ListWorkItemsRequest
{
    [FromQuery]
    public int PageNumber { get; set; } = 1;

    [FromQuery]
    public int PageSize { get; set; } = 10;
}