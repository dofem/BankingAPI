using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;

namespace BankingAPI.Model
{
    [Table("Transactions")]
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public string TransactionUniqueReference { get; set; }
        [Precision(18,2)]
        public decimal TransactionAmount { get; set; }
        public TransStatus TransactionStatus { get; set; }  //this is an enum
        public bool IsSuccessful => TransactionStatus.Equals(TransStatus.Successful);  //this depends on the value of transaction status
        public string TransactionSourceAccount { get; set; }
        public string TransactionDestinationAccount { get; set; }
        public string TransactionParticulars { get; set; }  
        public TransType TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }

        public Transaction()
        {
            TransactionUniqueReference = $"{Guid.NewGuid().ToString().Replace("-", "").Substring(1, 27)}";
        }

    }

    public enum TransStatus
    {
       Failed,
       Successful,
       Error
    }

    public enum TransType
    {
        Deposit,
        Withdrawal,
        Transfer
    }
}
