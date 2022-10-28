using BankingAPI.DAL;
using BankingAPI.Model;
using BankingAPI.Services.Implementation.Utils;
using BankingAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace BankingAPI.Services.Implementation
{
    public class TransactionServices : ITransactionServices
    {
        private DataContext _context;
        ILogger<TransactionServices> _logger;
        private AppSettings _settings;
        private static string _ourBankSettlementAccount;
        private readonly IAccountServices _accountServices;

        public TransactionServices(DataContext context, ILogger<TransactionServices> logger,IOptions<AppSettings> settings,IAccountServices accountServices)
        {
            _context = context;
            _logger = logger;
            _settings = settings.Value;
            _ourBankSettlementAccount = _settings.OurBankSettlementAccount;
            _accountServices = accountServices;

        }

        public Response CreateNewTransaction(Transaction transaction)
        {
            //create a new transaction
            Response response = new Response();
            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            response.ResponseCode = "00";
            response.ResponseMessage = "Transaction Successfully Created";
            response.Data = null;

            return response;    
        }

        public Response FindTransactionByDate(DateTime Date)
        {
            Response response = new Response();
            var transaction = _context.Transactions.Where(x => x.TransactionDate == Date).ToList();
            response.ResponseCode = "00";
            response.ResponseMessage = "Transaction Created Successfully";
            response.Data = transaction;

            return response;
        }

        public Response MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin)
        {
            //make deposit
            Response response = new Response();
            Account sourceAccount;
            Account? destinationAccount;
            Transaction transaction = new Transaction();

            //validating source account by authenticating
            var userauth = _accountServices.Authenticate(AccountNumber,TransactionPin);
            if (userauth == null)
                throw new Exception("Invalid Credentials");

            try
                {
                    //for deposit,our bankSettlementAccount is the source giving money to the user account
                    sourceAccount = _accountServices.GetAccountById(2);
                    destinationAccount = _accountServices.GetByAccountNumber(AccountNumber);

                    //Now let us update our account balance
                    sourceAccount.CurrentAccountBalance -= Amount;
                    destinationAccount.CurrentAccountBalance += Amount;

                    //check if there is update
                    if ((_context.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
                        (_context.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                    {
                        //Transaction is Successful
                        transaction.TransactionStatus = TransStatus.Successful;
                        response.ResponseCode = "00";
                        response.ResponseMessage = "Transaction Successful";
                        response.Data = null;
                    }
                    else
                    {
                        //Failed Transaction
                        transaction.TransactionStatus = TransStatus.Failed;
                        response.ResponseCode = "02";
                        response.ResponseMessage = "Transaction Failed";
                        response.Data = null;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"An Error Occured....=>{ex.Message}");
                }
            

            //set other props of transaction
            transaction.TransactionType =TransType.Deposit;
            transaction.TransactionSourceAccount = "2000000000";
            transaction.TransactionDestinationAccount = AccountNumber;
            transaction.TransactionAmount = Amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = $"NEW TRANSACTION FROM =>{JsonConvert.SerializeObject(transaction.TransactionSourceAccount)}" +
                $"TO DESTINATION ACCOUNT=> {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} CREATED ON DATE" +
                $"{JsonConvert.SerializeObject(transaction.TransactionDate)} FOR AMOUNT=>{JsonConvert.SerializeObject(transaction.TransactionAmount)} " +
                $"TRANSACTION TYPE => {JsonConvert.SerializeObject(transaction.TransactionType)},TRANSACTION STATUS=>{JsonConvert.SerializeObject(transaction.TransactionStatus)}";

            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return response;

        }

        public Response MakeFundTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin)
        {
            //make Fund Transfer
            Response response = new Response();
            Account sourceAccount;
            Account destinationAccount;
            Transaction transaction = new Transaction();

            //validating source account by authenticating
            var authUser = _accountServices.Authenticate(FromAccount, TransactionPin);
            if (authUser == null)
                throw new Exception("Invalid Credentials");

            try
            {
                //for withdrawal,our bankSettlementAccount is the destination giving money to the user account
                sourceAccount = _accountServices.GetByAccountNumber(FromAccount);
                destinationAccount = _accountServices.GetByAccountNumber(ToAccount);

                //Now let us update our account balance
                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;

                //check if there is update
                if ((_context.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
                    (_context.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    //Transaction is Successful
                    transaction.TransactionStatus = TransStatus.Successful;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Fund Transfer Successful";
                    response.Data = null;
                }
                else
                {
                    //Failed Transaction
                    transaction.TransactionStatus = TransStatus.Failed;
                    response.ResponseCode = "02";
                    response.ResponseMessage = "Fund Transfer Failed";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An Error Occured....=>{ex.Message}");
            }

            //set other props of transaction
            transaction.TransactionType = TransType.Transfer;
            transaction.TransactionSourceAccount = FromAccount;
            transaction.TransactionDestinationAccount = ToAccount;
            transaction.TransactionAmount = Amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = $"NEW TRANSACTION FROM =>{JsonConvert.SerializeObject(transaction.TransactionSourceAccount)}" +
                $"TO DESTINATION ACCOUNT=> {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} CREATED ON DATE" +
                $"{JsonConvert.SerializeObject(transaction.TransactionDate)} FOR AMOUNT=>{JsonConvert.SerializeObject(transaction.TransactionAmount)} " +
                $"TRANSACTION TYPE => {JsonConvert.SerializeObject(transaction.TransactionType)},TRANSACTION STATUS=>{JsonConvert.SerializeObject(transaction.TransactionStatus)}";

            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return response;

        }




        public Response MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin)
        {
            {
                //make withdrawal
                Response response = new Response();
                Account sourceAccount;
                Account destinationAccount;
                Transaction transaction = new Transaction();

                //validating source account by authenticating
                var authUser = _accountServices.Authenticate(AccountNumber, TransactionPin);
                if (authUser == null)
                    throw new Exception("Invalid Credentials");

                try
                {
                    //for withdrawal,our bankSettlementAccount is the destination giving money to the user account
                    sourceAccount = _accountServices.GetByAccountNumber(AccountNumber);
                    destinationAccount = _accountServices.GetAccountById(2);

                    //Now let us update our account balance
                    sourceAccount.CurrentAccountBalance -= Amount;
                    destinationAccount.CurrentAccountBalance += Amount;

                    //check if there is update
                    if ((_context.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
                        (_context.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                    {
                        //Transaction is Successful
                        transaction.TransactionStatus = TransStatus.Successful;
                        response.ResponseCode = "00";
                        response.ResponseMessage = "Transaction Withdrawal Successful";
                        response.Data = null;
                    }
                    else
                    {
                        //Failed Transaction
                        transaction.TransactionStatus = TransStatus.Failed;
                        response.ResponseCode = "02";
                        response.ResponseMessage = "Transaction Withdrawal Failed";
                        response.Data = null;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"An Error Occured....=>{ex.Message}");
                }

                //set other props of transaction
                transaction.TransactionType = TransType.Withdrawal;
                transaction.TransactionSourceAccount = AccountNumber;
                transaction.TransactionDestinationAccount = "2000000000";
                transaction.TransactionAmount = Amount;
                transaction.TransactionDate = DateTime.Now;
                transaction.TransactionParticulars = $"NEW TRANSACTION FROM =>{JsonConvert.SerializeObject(transaction.TransactionSourceAccount)}" +
                    $"TO DESTINATION ACCOUNT=> {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} CREATED ON DATE" +
                    $"{JsonConvert.SerializeObject(transaction.TransactionDate)} FOR AMOUNT=>{JsonConvert.SerializeObject(transaction.TransactionAmount)} " +
                    $"TRANSACTION TYPE => {JsonConvert.SerializeObject(transaction.TransactionType)},TRANSACTION STATUS=>{JsonConvert.SerializeObject(transaction.TransactionStatus)}";

                _context.Transactions.Add(transaction);
                _context.SaveChanges();

                return response;

            }
        }
    }
}
