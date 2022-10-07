using Microsoft.AspNetCore.Identity;

namespace MensaApp.Persistence.CustomValidation.AdminValidate.UpdateAdmin
{
    public class UpdateAdminValidation : IdentityErrorDescriber
    {
        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError()
            {
                Code = "InvalidUserName",
                Description = $"Bu {userName} kullanıcı adı geçersizdir"
            };
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError()
            {
                Code = "DuplicateUserName",
                Description = $"Bu {userName} kullanıcı adı zaten kullanılmaktadır."
            };
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError()
            {
                Code = "DuplicateEmail",
                Description = $"Bu {email} adresi zaten kullanılmaktadır"
            };
        }

        public override IdentityError InvalidEmail(string email)
        {
            return new IdentityError()
            {
                Code = "InvalidEmail",
                Description = $"Bu {email} adresi geçersizdir."
            };
        }
    }
}
