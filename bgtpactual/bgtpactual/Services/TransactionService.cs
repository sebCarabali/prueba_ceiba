using bgtpactual.Models;
using bgtpactual.Services.Abstractions;
using MongoDB.Driver;

namespace bgtpactual.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ILogger<TransactionService> _logger;
        private readonly IMongoCollection<Client> _clientsCollection;

        public TransactionService(ILogger<TransactionService> logger, IMongoDatabase mongoDatabase)
        {
            _logger = logger;
            _clientsCollection = mongoDatabase.GetCollection<Client>("clientes");
        }

        public async Task<ICollection<Transaction>> GetTransactionHistoryAsync(string clientId)
        {
            var client = _clientsCollection.Find(c => c.Id == clientId).FirstOrDefault();
            if (client == null)
            {
                throw new Exception("Cliente no encontrado");
            }
            
            return client.Transactions ?? new List<Transaction>();
        }
    }
}
