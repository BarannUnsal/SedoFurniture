using FluentValidation;
using MensaApp.Domain.Entities;

namespace MensaApp.Infrastructure.Validations.SubCategoryValidate
{
    public class SubCategoryValidation : AbstractValidator<SubCategory>
    {
        public SubCategoryValidation()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Alt kategori boş olamaz").MinimumLength(2);
        }
    }
}
