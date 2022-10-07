using FluentValidation;
using MensaApp.Domain.Entities;

namespace MensaApp.Infrastructure.Validations.CategoryValidate.UpdateCategory
{
    public class UpdateCategoryValidation : AbstractValidator<Category>
    {
        public UpdateCategoryValidation()
        {
            RuleFor(x => x.CategoryName).NotEmpty().WithMessage("Lütfen kategori ismi giriniz.").MinimumLength(3).WithMessage("Kategori ismi en az 3 karakter olmalıdır.");
        }
    }
}
