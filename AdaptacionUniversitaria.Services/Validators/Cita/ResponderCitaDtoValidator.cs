using FluentValidation;
using AdaptacionUniversitaria.Core.DTOs;

namespace AdaptacionUniversitaria.Services.Validators.Cita
{
    // Validación para aprobar una cita (CU-04)
    public class ResponderCitaDtoValidator : AbstractValidator<ResponderCitaDto>
    {
        public ResponderCitaDtoValidator()
        {
            RuleFor(x => x.ConsejeroId)
                .GreaterThan(0)
                .WithMessage("El ID del consejero es obligatorio y debe ser mayor que cero.");
        }
    }
}
