using bgtpactual.Models;
using bgtpactual.Services.Abstractions;
using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace bgtpactual.Services
{
    public class FundService : IFundService
    {

        private readonly ILogger<FundService> _logger;
        private readonly IMongoCollection<Fund> _fundCollection;
        private readonly IMongoCollection<Client> _clientCollection;

        public FundService(ILogger<FundService> logger, IMongoDatabase mongoDatabase)
        {
            _logger = logger;
            _fundCollection = mongoDatabase.GetCollection<Fund>("fondos");
            _clientCollection = mongoDatabase.GetCollection<Client>("clientes");
        }

        public async Task<Client> GetClientAsync(string clientId)
        {
            return await _clientCollection.Find(c => c.Id == clientId).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Fund>> GetFundsAsync()
        {
            return await _fundCollection.Find(_ => true).ToListAsync();
        }

        public async Task SubscribeToFundAsyc(string fundId, string userId, decimal ammount)
        {
            var client = await _clientCollection.Find(c => c.Id == userId).FirstOrDefaultAsync();

            var fund = await _fundCollection.Find(f => f.Id == fundId).FirstOrDefaultAsync();

            if (client == null)
            {
                throw new Exception("Cliente no encontrado");
            }

            if (fund == null)
            {
                throw new Exception("Fondo no encontrado");
            }

            if (ammount < fund.MinimumAmount)
            {
                throw new Exception($"El monto minimo para este fondo es de {fund.MinimumAmount}");
            }

            if (client.Balance < ammount)
            {
                throw new Exception("No tiene fondos suficientes para esta transacción");
            }

            client.Funds ??= new List<SubscribedFund>();

            client.Funds.Add(new SubscribedFund
            {
                FundId = fund.Id,
                FundName = fund.Name,
                SubscriptionAmount = ammount,
                SubscriptionDate = DateTime.UtcNow
            });

            client.Transactions ??= new List<Transaction>();

            client.Transactions.Add(new Transaction
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Type = "subscription",
                Amount = ammount,
                Date = DateTime.UtcNow,
                FundId = fund.Id,
                FundName = fund.Name
            });

            client.Balance -= ammount;

            await _clientCollection.ReplaceOneAsync(c => c.Id == client.Id, client);
        }

        public async Task UnsubscribeFromFundAsync(string fundId, string userId)
        {
            var client = await _clientCollection.Find(c => c.Id == userId).FirstOrDefaultAsync();

            if (client == null)
            {
                throw new Exception("Cliente no encontrado");
            }

            var fundToCancel = client.Funds.FirstOrDefault(f => f.FundId == fundId);
            if (fundToCancel == null)
            {
                throw new Exception("Fondo no encontrado");
            }

            client.Balance += fundToCancel.SubscriptionAmount;
            client.Funds.Remove(fundToCancel);
            client.Transactions.Add(new Transaction
            {
                Id = ObjectId.GenerateNewId().ToString(),
                FundId = fundId,
                Amount = fundToCancel.SubscriptionAmount,
                Date = DateTime.UtcNow,
                Type = "cancellation",
                FundName = fundToCancel.FundName
            });

            await _clientCollection.ReplaceOneAsync(c => c.Id == userId, client);
        }
    }
}
