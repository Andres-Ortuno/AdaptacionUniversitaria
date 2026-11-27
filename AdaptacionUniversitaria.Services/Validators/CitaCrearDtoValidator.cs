using AdaptacionUniversitaria.Core.DTOs;
using FluentValidation;

namespace AdaptacionUniversitaria.Services.Validators
{
    public class CitaCrearDtoValidator : AbstractValidator<CitaCrearDto>
    {
        public CitaCrearDtoValidator()
        {
            RuleFor(x => x.ConsejeroId)
                .GreaterThan(0).WithMessage("Debe elegir un consejero.");

            RuleFor(x => x.FechaHora)
                .NotEmpty().WithMessage("La fecha y hora de la cita son obligatorias.");

            RuleFor(x => x.Motivo)
                .NotEmpty().WithMessage("El motivo de la cita es obligatorio.")
                .MaximumLength(500).WithMessage("El motivo no puede exceder los 500 caracteres.");
        }
    }
}
