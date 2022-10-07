using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MensaApp.API.Models
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "Lütfen kullanıcı adı giriniz")]
        [Display(Name = "Kullanıcı adı")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Lütfen şifre giriniz")]
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
