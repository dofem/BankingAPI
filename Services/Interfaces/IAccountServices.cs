using BankingAPI.Model;

namespace BankingAPI.Services.Interfaces
{
    public interface IAccountServices
    {
        Account Authenticate(string AccountNumber, string Pin);
        IEnumerable<Account> GetAllAccounts();
        Account Create(Account account, string Pin, string ConfirmPin);
        void Update(Account account, string Pin = null);
        void Delete(int id);
        Account GetAccountById(int id);
        Account GetByAccountNumber(string AccountNumber);
    }
}
