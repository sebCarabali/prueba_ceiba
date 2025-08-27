using bgtpactual.Models;

namespace bgtpactual.Services.Abstractions
{
    public interface ITransactionService
    {
        Task<ICollection<Transaction>> GetTransactionHistoryAsync(string userId);
    }
}
