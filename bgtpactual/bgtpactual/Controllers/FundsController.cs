using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bgtpactual.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FundsController : ControllerBase
    {

        [HttpPost("subscribe")]
        public async Task<IActionResult> SubscribeToFund([FromBody] DTO.Request.SubcribeFundRequest request, [FromServices] Services.Abstractions.IFundService fundService)
        {
            try
            {
                await fundService.SubscribeToFundAsyc(request.FundId, request.ClientId, request.Amount, request.ComunicationChannel);
                return Ok(new { Message = "Suscripción exitosa" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("unsubscribe")]
        public async Task<IActionResult> UnsubscribeFromFund([FromBody] DTO.Request.UnsubcribeFundRequest request, [FromServices] Services.Abstractions.IFundService fundService)
        {
            try
            {
                await fundService.UnsubscribeFromFundAsync(request.FundId, request.ClientId);
                return Ok(new { Message = "Cancelación de suscripción exitosa" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{clientId}/history")]
        public async Task<IActionResult> GetTransactionHistory(string clientId, [FromServices] Services.Abstractions.ITransactionService transactionService)
        {
            try
            {
                var history = await transactionService.GetTransactionHistoryAsync(clientId);
                return Ok(history);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("funds")]
        public async Task<IActionResult> GetFunds([FromServices] Services.Abstractions.IFundService fundService)
        {
            try
            {
                var funds = await fundService.GetFundsAsync();
                return Ok(funds);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        [HttpGet("{clientId}")]
        public async Task<IActionResult> GetClient(string clientId, [FromServices] Services.Abstractions.IFundService fundService)
        {
            var client = await fundService.GetClientAsync(clientId);
            if (client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }
    }
}
