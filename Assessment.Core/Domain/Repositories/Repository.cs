using Assessment.Core.DbContexts;
using Assessment.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.Core.Domain.Repositories
{
    public class Repository<T> : IRepository<T> where T : ModelBase
    {
        private readonly AssessmentContext _assessmentContext;

        public Repository(AssessmentContext assessmentContext)
        {
            ArgumentNullException.ThrowIfNull(assessmentContext, nameof(assessmentContext));

            _assessmentContext = assessmentContext;
        }
        public async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _assessmentContext.Set<T>().FindAsync(id, cancellationToken);
        }

        public virtual async Task<int> CreateAsync(T entity, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));

            _assessmentContext.Set<T>().Add(entity);

            await _assessmentContext.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
