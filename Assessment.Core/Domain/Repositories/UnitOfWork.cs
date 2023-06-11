using Assessment.Core.DbContexts;
using Microsoft.EntityFrameworkCore.Storage;

namespace Assessment.Core.Domain.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AssessmentContext _dbContext;

        public UnitOfWork(AssessmentContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
        {
            return _dbContext.Database.BeginTransactionAsync(cancellationToken);
        }
    }
}