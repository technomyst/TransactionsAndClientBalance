using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using Unistream_T4.Dto;
using Unistream_T4.Services.Abstractions;

namespace Unistream_T4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _service;
        public TransactionsController(ITransactionService service)
        {
            _service=service;
        }

        [HttpPost("/credit")]
        public async Task<ActionResult<AddTransactionResponseDto>> AddCreditTransaction(TransactionDto transactionDto)
        {
            var response = await  _service.AddCreditTransaction(transactionDto);
            return Ok(response);
        }
        [HttpPost("/debit")]
        public async Task<ActionResult<AddTransactionResponseDto>> AddDebitTransaction(TransactionDto transactionDto)
        {
            var response = await _service.AddDebitTransaction(transactionDto);
            return Ok(response);
        }

        [HttpPost("/revert")]
        public async Task<ActionResult<RevertTransactionResponseDto>> RevertTransaction(Guid id)
        {
            var response =await _service.RevertTransaction(id);
            return Ok(response);
        }


        [HttpGet("/balance")]
        public async Task<ActionResult<AddTransactionResponseDto>> GetBalance(Guid id)
        {
            var response = await _service.GetClientBalance(id);
            return Ok(response);
        }
       
    }
}
