namespace Skelly.WebApi.Domain.WorkItemAggregate;

public class WorkItem(string title)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = title;
}
