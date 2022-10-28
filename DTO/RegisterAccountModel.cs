using BankingAPI.Model;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BankingAPI.DTO
{
    public class RegisterAccountModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
     
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
      
        public AccountType AccountType { get; set; }
       
        public DateTime DateCreated { get; set; }
        public DateTime DateLastUpdated { get; set; }
        [Required]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Pin must not be must than 4 digit")]
        public string Pin { get; set; }
        [Required]
        [Compare("Pin", ErrorMessage = "Pins do not match")]
        public string confirmPin { get; set; }  
    }
}
