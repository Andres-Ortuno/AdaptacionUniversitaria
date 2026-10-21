using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using AdaptacionUniversitaria.Core.DTOs;

namespace AdaptacionUniversitaria.Services.Validators.Bitacora
{
    public class BitacoraDtoValidator : AbstractValidator<BitacoraDto>
    {
        public BitacoraDtoValidator()
        {
            RuleFor(x => x.EstudianteId)
                .GreaterThan(0).WithMessage("El ID del estudiante es obligatorio y debe ser mayor que cero.");

            RuleFor(x => x.Fecha)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("La fecha es obligatoria.")
                .Must(BeAValidDate).WithMessage("La fecha debe tener un formato válido (dd-MM-yyyy).")
                .Must(NotBeInTheFuture).WithMessage("La fecha no puede ser posterior a hoy.");

            RuleFor(x => x.EstadoAnimo)
                .InclusiveBetween((sbyte)1, (sbyte)5)
                .WithMessage("El estado de ánimo debe estar entre 1 (muy bajo) y 5 (muy bueno).");

            RuleFor(x => x.Sintomas)
                .MaximumLength(255).WithMessage("Los síntomas no pueden exceder los 255 caracteres.");

            RuleFor(x => x.Comentario)
                .MaximumLength(500).WithMessage("El comentario no puede exceder los 500 caracteres.");

            RuleFor(x => x.CreadoEn)
                .Must(BeAValidDate).WithMessage("La fecha de creación debe tener un formato válido (dd-MM-yyyy).")
                .When(x => !string.IsNullOrWhiteSpace(x.CreadoEn));
        }

        private static bool BeAValidDate(string? date) => DateTime.TryParse(date, out _);

        private static bool NotBeInTheFuture(string? date) =>
            DateTime.TryParse(date, out var parsed) && parsed.Date <= DateTime.Today;
    }
}
