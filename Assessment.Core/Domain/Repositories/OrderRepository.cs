using Assessment.Core.DbContexts;
using Assessment.Core.Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Core.Domain.Repositories
{
    public class OrderRepository : Repository<Order>
    {
        private readonly AssessmentContext _assessmentContext;

        public OrderRepository(AssessmentContext assessmentContext) : base(assessmentContext)
        {
            _assessmentContext = assessmentContext;
        }

        public async override Task<int> CreateAsync(Order entity, CancellationToken cancellationToken)
        {
            var cmd = _assessmentContext.Database.GetDbConnection()
               .CreateCommand();

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "spCreateOrder";

            await _assessmentContext?.Orders.FromSqlRaw("spCreateOrder @CustomerId, @ProductId, @Quantity",
                    entity.CustomerId, entity.ProductId, entity.Quantity).ToListAsync(cancellationToken);

            var newId = (await cmd.ExecuteScalarAsync(cancellationToken)) as int?;

            if (newId is null)
                throw new Exception("Entity could not be created");

            return newId.Value;
        }
    }
}
