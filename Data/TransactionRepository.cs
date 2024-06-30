
using System.Linq.Expressions;
using University_of_free_state_booking_Facilities.Models;


namespace University_of_free_state_booking_Facilities.Data
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }

    
}
