using Assessment.Core.DbContexts;
using Assessment.Core.Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Assessment.Core.Domain.Repositories
{
    public class OrderRepository : Repository<Order>
    {
        private readonly AssessmentContext _assessmentContext;
        private readonly IUnitOfWork _unitOfWork;

        public OrderRepository(AssessmentContext assessmentContext, IUnitOfWork unitOfWork) : base(assessmentContext)
        {
            _assessmentContext = assessmentContext;
            _unitOfWork = unitOfWork;
        }

        public async override Task<int> CreateAsync(Order entity, CancellationToken cancellationToken)
        {
            using var tran = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            using var cmd = _assessmentContext.Database.GetDbConnection().CreateCommand();

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Transaction = tran.GetDbTransaction();
            cmd.CommandText = "spCreateOrder";
            cmd.Parameters.Add(new SqlParameter("@CustomerId", entity.CustomerId));
            cmd.Parameters.Add(new SqlParameter("@ProductId", entity.ProductId));
            cmd.Parameters.Add(new SqlParameter("@Quantity", entity.Quantity));
          
            var newId = (await cmd.ExecuteScalarAsync(cancellationToken)) as int?;

            await tran.CommitAsync(cancellationToken);

            return newId ?? 0;
        }
    }
}
