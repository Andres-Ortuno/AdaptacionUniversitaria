using FluentValidation;
using AdaptacionUniversitaria.Core.DTOs;

namespace AdaptacionUniversitaria.Services.Validators.Cita
{
    public class CrearCitaDtoValidator : AbstractValidator<CitaDto>
    {
        public CrearCitaDtoValidator()
        {
            RuleFor(x => x.EstudianteId)
                .GreaterThan(0)
                .WithMessage("El ID del estudiante es obligatorio y debe ser mayor que cero.");

            RuleFor(x => x.ConsejeroId)
                .GreaterThan(0)
                .WithMessage("El ID del consejero es obligatorio y debe ser mayor que cero.");

            RuleFor(x => x)
                .Must(x => x.EstudianteId != x.ConsejeroId)
                .WithMessage("El estudiante y el consejero no pueden ser la misma persona.")
                .When(x => x.EstudianteId > 0 && x.ConsejeroId > 0);

            // RN-04: la cita debe ser posterior al momento de la solicitud
            RuleFor(x => x.FechaHora)
                .Must(fecha => fecha > DateTime.Now)
                .WithMessage("La fecha y hora de la cita deben ser posteriores al momento actual.");

            RuleFor(x => x.Motivo)
                .NotEmpty().WithMessage("El motivo de la cita es obligatorio.")
                .MaximumLength(500).WithMessage("El motivo no puede exceder los 500 caracteres.");
        }
    }
}
