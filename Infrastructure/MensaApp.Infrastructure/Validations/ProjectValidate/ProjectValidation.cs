using FluentValidation;
using MensaApp.Domain.Entities;

namespace MensaApp.Infrastructure.Validations.ProjectValidate
{
    public class ProjectValidation : AbstractValidator<Project>
    {
        public ProjectValidation()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("İsim boş olamaz.").Length(2, 150).WithMessage("İsim uzunluğu 2 - 150 karakter arasında olmalıdır");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama boş olamaz").MinimumLength(2);
            RuleFor(x => x.ProjectImages).NotEmpty().WithMessage("Görsel yükleyin");
        }
    }
}
