using System.ComponentModel.DataAnnotations;

namespace MensaApp.API.Models
{
    public class SignInUserViewModel
    {
        public string NameSurname { get; set; }
        public string UserName { get; set; }
        [RegularExpression(@"^(05(\d{9}))$", ErrorMessage = "Telefon numaranızı boşluk bırakmadan yazınız.")]
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
