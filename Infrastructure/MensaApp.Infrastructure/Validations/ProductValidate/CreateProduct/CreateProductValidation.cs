using FluentValidation;
using MensaApp.Domain.Entities;

namespace MensaApp.Infrastructure.Validations.ProductValidate.CreateProduct
{
    public class CreateProductValidation : AbstractValidator<Product>
    {
        public CreateProductValidation()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Lütfen ürün ismi giriniz.").Length(2, 150).WithMessage("Ürün ismi 2 - 150 karakter arasında olmalıdır.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Lütfen ürün açıklaması giriniz.").MinimumLength(2);
            RuleFor(x => x.ProductImages).NotEmpty().WithMessage("Lütfen ürün resmi yükleyin.");
        }
    }
}
