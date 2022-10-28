using BankingAPI.DAL;
using BankingAPI.Model;
using BankingAPI.Services.Interfaces;
using System.Text;

namespace BankingAPI.Services.Implementation
{
    public class AccountServices : IAccountServices
    {
        private DataContext _context;

        public AccountServices(DataContext context)
        {
            _context = context;
        }
        public Account Authenticate(string AccountNumber, string pin)
        {
            var account = _context.Accounts.Where(x=>x.AccountNumberGenerated == AccountNumber).FirstOrDefault();
            if (account == null)
                return null;


            if (!VerifyPinHash(pin, account.PinHash, account.PinSalt))
                return null;

            return account;
        }

        private static bool VerifyPinHash(string pin, byte[] pinHash, byte[] pinSalt)
        {
            if(string.IsNullOrEmpty(pin))
                throw new ArgumentNullException(nameof(pin));
            using (var hmac = new System.Security.Cryptography.HMACSHA512(pinSalt))
            {
                var computedPinHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pin));
                {
                    for (var i = 0; i < computedPinHash.Length; i++)
                        if (computedPinHash[i] != pinHash[i])
                            return false;
                }
            }
            return true;
        }

        public Account Create(Account account, string Pin, string ConfirmPin)
        {
            if (_context.Accounts.Any(x => x.Email == account.Email))
                throw new ApplicationException("An Account already exist with this email");

            //Pin Validation
            if (!Pin.Equals(ConfirmPin))
                throw new ApplicationException("Pins do not match");
            //After Validation,create account
            //Hashing/encryption to be done first
            Byte[] pinHash, pinSalt;
            CreatePinHash(Pin, out pinHash, out pinSalt);// Let us create this crypto methood
            account.PinHash = pinHash;
            account.PinSalt = pinSalt;

            _context.Accounts.Add(account);
            _context.SaveChanges();


            return account;
        }

        private static void CreatePinHash(string pin,out byte[] pinHash, out byte[] pinSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                pinSalt = hmac.Key;
                pinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pin));
            }
        }


        public void Delete(int id)
        {
            var account = _context.Accounts.FirstOrDefault(x => x.Id == id);
            if(account != null)
                _context.Accounts.Remove(account);
        }

        public Account GetAccountById(int id)
        {
            var account = _context.Accounts.Where(x => x.Id == id).FirstOrDefault();
            if (account == null)
                return null;

            return account;
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            var account = _context.Accounts.ToList();
            return account;
        }

        public Account GetByAccountNumber(string AccountNumber)
        {
            var account = _context.Accounts.Where(x=>x.AccountNumberGenerated == AccountNumber).FirstOrDefault();
            if (account == null)
                return null;

            return account;
        }

        public void Update(Account account, string Pin = null)
        {
            var AccountToBeUpdated = _context.Accounts.Where(x => x.Email == account.Email).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(account.Email))
            {
                if (_context.Accounts.Any(x => x.Email == account.Email))
                    throw new ApplicationException($"This Email {account.Email} exist already");
                AccountToBeUpdated.Email = account.Email;
            }

            if (!string.IsNullOrWhiteSpace(account.PhoneNumber))
            {
                if (_context.Accounts.Any(x => x.PhoneNumber == account.PhoneNumber))
                    throw new ApplicationException($"This Email {account.PhoneNumber} exist already");
                AccountToBeUpdated.PhoneNumber = account.PhoneNumber;
            }

            if (!string.IsNullOrWhiteSpace(Pin))
            {
                byte[] pinHash, pinSalt;
                CreatePinHash(Pin, out pinHash, out pinSalt);

                AccountToBeUpdated.PinHash = pinHash;
                AccountToBeUpdated.PinSalt = pinSalt;

            }
            AccountToBeUpdated.DateLastUpdated = DateTime.Now;
            _context.Accounts.Update(account);
            _context.SaveChanges();
        }
    }
}
