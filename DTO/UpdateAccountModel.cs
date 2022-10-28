using BankingAPI.Model;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BankingAPI.DTO
{
    public class UpdateAccountModel
    {
        
        public string PhoneNumber { get; set; }
        public string Email { get; set; }      
        public DateTime DateLastUpdated { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "Pin must not be must than 4 digit")]
        public string Pin { get; set; }
        [Required]
        [Compare("Pin", ErrorMessage = "Pins do not match")]
        public string confirmPin { get; set; }
    }
}
