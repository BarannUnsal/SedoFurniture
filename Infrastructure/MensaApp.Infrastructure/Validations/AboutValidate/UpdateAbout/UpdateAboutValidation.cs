﻿using FluentValidation;
using MensaApp.Domain.Entities;

namespace MensaApp.Infrastructure.Validations.AboutValidate.UpdateAbout
{
    public class UpdateAboutValidation : AbstractValidator<About>
    {
        public UpdateAboutValidation()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Lütfen başlık giriniz").Length(2, 75).WithMessage("Başlık 2 - 75 karakter arasında olmalıdır.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Lütfen açıklama giriniz").MinimumLength(2).WithMessage("Başlık  en az 2 karakter olmalıdır.");
        }
    }
}
