using AdaptacionUniversitaria.Core.DTOs;
using FluentValidation;

namespace AdaptacionUniversitaria.Services.Validators
{
    public class CitaRechazarDtoValidator : AbstractValidator<CitaRechazarDto>
    {
        public CitaRechazarDtoValidator()
        {
            // RN-06: el motivo del rechazo es obligatorio
            RuleFor(x => x.MotivoRechazo)
                .NotEmpty().WithMessage("Debe indicar el motivo del rechazo.")
                .MaximumLength(500).WithMessage("El motivo del rechazo no puede exceder los 500 caracteres.");
        }
    }
}
