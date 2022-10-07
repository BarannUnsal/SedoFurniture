using FluentValidation;
using MensaApp.Domain.Entities;

namespace MensaApp.Infrastructure.Validations.ContactValidate.CreateContact
{
    public class CreateContactValidator : AbstractValidator<Contact>
    {
        public CreateContactValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Lütfen isminizi giriniz.").Length(2, 75).WithMessage("İsim 2 - 75 karakter arasında olmalıdır.");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Lütfen geçerli bir email adresi giriniz.").NotEmpty().WithMessage("Lütfen email adresinizi giriniz.").MinimumLength(2);
            RuleFor(x => x.Subject).NotEmpty().WithMessage("Lütfen mesaj konusunu giriniz.").Length(2, 75).WithMessage("Mesaj konusu 2 - 75 karakter arasında olmalıdır.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Lütfen mesajınızı giriniz.").MinimumLength(2);
        }
    }
}
