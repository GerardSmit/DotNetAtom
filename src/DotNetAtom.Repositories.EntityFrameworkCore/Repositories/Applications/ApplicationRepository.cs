using System;
using System.Threading.Tasks;
using DotNetAtom.Application;
using Microsoft.EntityFrameworkCore;

namespace DotNetAtom.EntityFrameworkCore.Repositories.Applications;

public class ApplicationRepository : IApplicationRepository
{
    private readonly IDbContextFactory<AtomDbContext> _dbContextFactory;

    public ApplicationRepository(IDbContextFactory<AtomDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<Guid?> GetApplicationId()
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var application = await dbContext.AspNetApplication
            .FirstOrDefaultAsync(a => a.LoweredApplicationName == "dotnetnuke");

        return application?.Id;
    }
}
