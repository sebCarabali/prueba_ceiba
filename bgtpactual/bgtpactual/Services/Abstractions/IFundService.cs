using bgtpactual.Models;

namespace bgtpactual.Services.Abstractions
{
    public interface IFundService
    {

        /// <summary>
        /// Suscribe al usuario especificado a un fondo con el monto indicado.
        /// </summary>
        /// <param name="fundId">Identificador del fondo.</param>
        /// <param name="userId">Identificador del usuario.</param>
        /// <param name="ammount">Monto a suscribir.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        Task SubscribeToFundAsyc(string fundId, string userId, decimal ammount);

        /// <summary>
        /// Cancela la suscripción del usuario especificado al fondo indicado.
        /// </summary>
        /// <param name="fundId">Identificador del fondo.</param>
        /// <param name="userId">Identificador del usuario.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        Task UnsubscribeFromFundAsync(string fundId, string userId);

        Task<ICollection<Fund>> GetFundsAsync();

        Task<Client> GetClientAsync(string clientId);
    }
}
