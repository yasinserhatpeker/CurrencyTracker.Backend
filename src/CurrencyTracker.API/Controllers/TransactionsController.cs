using CurrencyTracker.Application.DTOs.Portfolios;
using CurrencyTracker.Application.DTOs.Transactions;
using CurrencyTracker.Application.Interfaces;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : CustomBaseController
    {
        private readonly ITransactionService _transactionService;
        private readonly IPortfolioService _portfolioService;
        public TransactionsController(ITransactionService transactionService, IPortfolioService portfolioService)
        {
            _transactionService = transactionService;
            _portfolioService = portfolioService;
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateTransactionsDTO createTransactionsDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var portfolio = await _portfolioService.GetByIdAsync(createTransactionsDTO.PortfolioId);
                if (portfolio is null || portfolio.UserId != GetCurrentUserId())
                {
                    return Unauthorized(new { message = "You have no right to create a transaction." });

                }
                await _transactionService.CreateTransactionAsync(createTransactionsDTO);
                return Ok(new { message = "Transaction created succesfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var transactionDto = await _transactionService.GetByIdAsync(id);
                var portfolio = await _portfolioService.GetByIdAsync(transactionDto.PortfolioId);
                if (portfolio is null || portfolio.UserId != GetCurrentUserId())
                {
                    return Unauthorized(new { message = "You dont have an access to do it." });
                }
                return Ok(transactionDto);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }

        }

        [HttpGet("portfolio/{portfolioId}")]
        public async Task<IActionResult> GetByPortfolio(Guid portfolioId)
        {
            try
            {
                var portfolio = await _portfolioService.GetByIdAsync(portfolioId);
                if (portfolio is null || portfolio.UserId != GetCurrentUserId())
                {
                    return Unauthorized(new { message = "You dont have an access to do it." });
                }
                var transactions = await _transactionService.GetTransactionsByPortfolioAsync(portfolioId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });

            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var transactionDto = await _transactionService.GetByIdAsync(id);
                var portfolio = await _portfolioService.GetByIdAsync(transactionDto.PortfolioId);

                if (portfolio is null || portfolio.UserId != GetCurrentUserId())
                {
                    return Unauthorized(new{message="You dont have an access to do it"});
                }

                await _transactionService.DeleteTransactionAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateTransactionDTO updateTransactionDTO)
        {

             
            if (updateTransactionDTO.Id != id)
            {
                return BadRequest("Transaction is not found");
            }
            try
            {   
                var existingTransaction = await _transactionService.GetByIdAsync(id);
                var portfolio = await _portfolioService.GetByIdAsync(existingTransaction.PortfolioId);

                if(portfolio is null || portfolio.UserId != GetCurrentUserId())
                {
                    return Unauthorized(new{message ="You dont have an access to do it"});
                }
                await _transactionService.UpdateTransactionAsync(id, updateTransactionDTO);
                return Ok(new { message = "Transaction updated succesfully" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
