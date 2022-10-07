using System.ComponentModel.DataAnnotations;

namespace MensaApp.API.Areas.Admin.Models.Admin
{
    public class PasswordChangeAdminViewModel
    {
        [Required(ErrorMessage = "Eski şifrenizi giriniz")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Şifrenizi giriniz")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Şifrenizi doğrulayın")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor")]
        public string PasswordConfirm { get; set; }
    }
}
