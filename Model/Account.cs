using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using System.Text.Json.Serialization;

namespace BankingAPI.Model
{
    [Table("Accounts")]
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        [Precision(18,2)]
        public decimal CurrentAccountBalance { get; set; }
        public AccountType AccountType { get; set; }
        public string AccountNumberGenerated { get; set; }
        [JsonIgnore]
        public byte[] PinHash { get; set; }
        [JsonIgnore]  

        public byte[] PinSalt { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastUpdated { get; set; }



        //Generating an Account Number
        //Creating a random obj
        Random random = new Random();
        public Account()
        {
            AccountNumberGenerated = "2050";
            int i;
            for(i=1;i<7;i++)
            {
                AccountNumberGenerated += random.Next(0,9).ToString();
            }
            AccountName = String.Concat(FirstName, LastName);

        }

    }





    public enum AccountType
    {
        Savings,
        Current,
        Corporate,
        Government
    }
}
