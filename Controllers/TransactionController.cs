using AutoMapper;
using BankingAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace BankingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private ITransactionServices _transactionServices;
        private IMapper _mapper;

        public TransactionController(ITransactionServices transactionServices,IMapper mapper)
        {
            _transactionServices = transactionServices;
            _mapper = mapper;
        }


        [HttpPost]
        [Route("Make_deposit")]
        public IActionResult MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin)
        {
            if (Regex.IsMatch(AccountNumber, @"^[0][1-9]\d{9}$^[1-9]\d{9}$"))
                return BadRequest("Account number must be equal to 10");
            return Ok(_transactionServices.MakeDeposit(AccountNumber, Amount, TransactionPin));
            
        }


        [HttpPost]
        [Route("Make_Withdrawal")]
        public IActionResult MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin)
        {
            if (Regex.IsMatch(AccountNumber, @"^[0][1-9]\d{9}$^[1-9]\d{9}$"))
                return BadRequest("Account number must be equal to 10");
            return Ok(_transactionServices.MakeWithdrawal(AccountNumber, Amount, TransactionPin));

        }

        [HttpPost]
        [Route("Fund_Transfer")]
        public IActionResult MakeFundTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin)
        {
            if (Regex.IsMatch(FromAccount, @"^[0][1-9]\d{9}$^[1-9]\d{9}$")) 
                return BadRequest("Account number must be equal to 10");
            if (Regex.IsMatch(ToAccount, @"^[0][1-9]\d{9}$^[1-9]\d{9}$"))
                return BadRequest("Account number must be equal to 10");
            return Ok(_transactionServices.MakeFundTransfer(FromAccount, ToAccount, Amount, TransactionPin));

        }



    }
}
