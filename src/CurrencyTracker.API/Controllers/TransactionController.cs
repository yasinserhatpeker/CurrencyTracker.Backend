using CurrencyTracker.Application.DTOs.Transactions;
using CurrencyTracker.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService=transactionService;
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateTransactionsDTO createTransactionsDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
               await _transactionService.CreateTransactionAsync(createTransactionsDTO);
               return Ok(new{message ="Transaction created succesfully!"});
            }
            catch(Exception ex)
            {
                return BadRequest (new {message = ex.Message});
            }

        }
    }
}
