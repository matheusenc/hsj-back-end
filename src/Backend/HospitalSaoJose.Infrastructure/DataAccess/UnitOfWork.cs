using HospitalSaoJose.Domain.Repositories;

namespace HospitalSaoJose.Infrastructure.DataAccess;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly HospitalSaoJoseDbContext _dbContext;

    public UnitOfWork(HospitalSaoJoseDbContext dbContext) => _dbContext = dbContext;

    public async Task Commit() => await _dbContext.SaveChangesAsync();
}
