using System.ComponentModel.DataAnnotations;

namespace BankingAPI.Model
{
    public class AuthenticateModel
    {
        [Required]
        [RegularExpression(@"^\d{10}$")]
        public string AccountNumber { get; set; }

        [Required]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Pin must not be must than 4 digit")]
        public string Pin { get; set; }
    }
}
