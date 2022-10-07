using FluentValidation;
using MensaApp.Domain.Entities;

namespace MensaApp.Infrastructure.Validations.CategoryValidate.CreateCategory
{
    public class CreateCategoryValidation : AbstractValidator<Category>
    {
        public CreateCategoryValidation()
        {
            RuleFor(x => x.CategoryName).NotEmpty().WithMessage("Lütfen kategori ismi giriniz.").MinimumLength(3).WithMessage("Kategori ismi en az 3 karakter olmalıdır.");
        }
    }
}
