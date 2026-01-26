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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var transaction = await _transactionService.GetByIdAsync(id);
                return Ok(transaction);
            }
            catch(Exception ex)
            {
                return NotFound(new{message=ex.Message});
            }

        }

        [HttpGet("portfolio/{portfolioId}")]
        public async Task<IActionResult> GetTransactionsByPortfolio(Guid portfolioId)
        {
            try
            {
                var transactions = await _transactionService.GetTransactionsByPortfolioAsync(portfolioId);
                return Ok(transactions);
            }
            catch(Exception ex)
            {
                return NotFound(new{message=ex.Message});

            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _transactionService.RemoveTransactionAsync(id);
                return NoContent();
            }
            catch(Exception ex)
            {
                return NotFound(new{message = ex.Message});
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateTransactionDTO updateTransactionDTO)
        {
            if(updateTransactionDTO.Id != id)
            {
                return BadRequest("Transaction is not found");
            }
            try
            {
                await _transactionService.UpdateTransactionAsync(id,updateTransactionDTO);
                return Ok(new{message ="Transaction updated succesfully"});
            }
            catch(Exception ex)
            {
                return NotFound(new{message=ex.Message});
            }
        }
    }
}
