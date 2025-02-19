using Microsoft.EntityFrameworkCore;
using Skelly.WebApi.Infrastructure.Persistence;

namespace Skelly.WebApi.IntegrationTests.TestHelper.Fixtures;

public class AppDbContextFixture : IDisposable
{
    public AppDbContext DbContext { get; private set; }

    public AppDbContextFixture()
    {
        var options = CreateNewContextOptions();
        DbContext = new AppDbContext(options);
        DbContext.Database.EnsureCreated();
    }

    public void ResetDatabase()
    {
        DbContext.Database.EnsureDeleted();
        DbContext.Database.EnsureCreated();
    }

    private static DbContextOptions<AppDbContext> CreateNewContextOptions()
    {
        return new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
    }

    public void Dispose()
    {
        DbContext?.Dispose();
        GC.SuppressFinalize(this);
    }
}