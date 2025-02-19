using Microsoft.EntityFrameworkCore;
using Skelly.WebApi.Application.Common;

namespace Skelly.WebApi.Infrastructure.Persistence.Extensions;

public static class EfExtensions
{
    public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> queryable, int pageNumber, int pageSize, CancellationToken cancellationToken = default) where T : class
    {
        var count = await queryable.CountAsync(cancellationToken);

        var items = await queryable.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}
