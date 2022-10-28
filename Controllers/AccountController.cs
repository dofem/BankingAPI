using AutoMapper;
using BankingAPI.DTO;
using BankingAPI.Model;
using BankingAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace BankingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAccountServices _accountServices;
        private IMapper _mapper;

        public AccountController(IAccountServices accountServices,IMapper mapper)
        {
            _accountServices = accountServices;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("Register_new_account")]
        public IActionResult RegisterNewAccount([FromBody] RegisterAccountModel registerNewAccount)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var account = _mapper.Map<Account>(registerNewAccount);
           _accountServices.Create(account, registerNewAccount.Pin, registerNewAccount.confirmPin);
            return Ok(account);
        }

        [HttpGet]
        [Route("get_all_accounts")]
        public IActionResult GetAllAccounts()
        {
            var account = _accountServices.GetAllAccounts();
            var cleaned = _mapper.Map<List<Account>>(account);
            return Ok(cleaned);
        }


        [HttpPost]
        [Route("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateModel authenticateModel)
        {
            if(!ModelState.IsValid)
                return BadRequest(authenticateModel);

            var authenticate = _accountServices.Authenticate(authenticateModel.AccountNumber, authenticateModel.Pin);
                return Ok(authenticate);    
        }


        [HttpGet]
        [Route("get_account_by_number")]
        public IActionResult GetAccountByNumber(string AccountNumber)
        {
            if (!Regex.IsMatch(AccountNumber, @"[0][1-9]\d{9}$|^[1-9]\d{9}$"))
                return BadRequest("Account Number must be 10 digits");

            var account = _accountServices.GetByAccountNumber(AccountNumber);
            var cleanedAccount = _mapper.Map<Account>(account);
            return Ok(cleanedAccount);
        }

        [HttpGet]
        [Route("get_account_by_id")]
        public IActionResult GetAccountByNumber(int Id)
        {
            if (Id == null)
                return NotFound();

            var account = _accountServices.GetAccountById(Id);
            var cleanedAccount = _mapper.Map<Account>(account);
            return Ok(cleanedAccount);
        }


        [HttpPost]
        [Route("update_account")]
        public IActionResult UpdateAccount([FromBody] UpdateAccountModel updateAccount,string Pin)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var account = _mapper.Map<Account>(updateAccount);
            _accountServices.Update(account, updateAccount.Pin);
            return Ok();
        }

    }
}
