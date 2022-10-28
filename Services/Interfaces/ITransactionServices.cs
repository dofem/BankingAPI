using BankingAPI.Model;

namespace BankingAPI.Services.Interfaces
{
    public interface ITransactionServices
    {
        Response CreateNewTransaction(Transaction transaction);
        Response FindTransactionByDate(DateTime Date);
        Response MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin);
        Response MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin);
        Response MakeFundTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin);

    }
}
