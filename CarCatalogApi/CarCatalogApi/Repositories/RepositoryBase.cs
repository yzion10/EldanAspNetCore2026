using CarCatalogApi.Data;

namespace CarCatalogApi.Repositories;

public abstract class RepositoryBase
{
    protected readonly CarCatalogDbContext Context;

    protected RepositoryBase(CarCatalogDbContext context)
    {
        Context = context;
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await Context.SaveChangesAsync() > 0;
    }
}
