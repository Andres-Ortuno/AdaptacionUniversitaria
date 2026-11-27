using AdaptacionUniversitaria.Core.DTOs;
using FluentValidation;

namespace AdaptacionUniversitaria.Services.Validators
{
    public class BitacoraGuardarDtoValidator : AbstractValidator<BitacoraGuardarDto>
    {
        public BitacoraGuardarDtoValidator()
        {
            // RN-02: escala de 1 (muy bajo) a 5 (muy bueno)
            RuleFor(x => x.EstadoAnimo)
                .InclusiveBetween(1, 5)
                .WithMessage("El estado de ánimo debe estar entre 1 y 5.");

            RuleFor(x => x.Sintomas)
                .MaximumLength(255).WithMessage("Los síntomas no pueden exceder los 255 caracteres.");

            RuleFor(x => x.Nota)
                .MaximumLength(2000).WithMessage("La nota no puede exceder los 2000 caracteres.");
        }
    }
}
