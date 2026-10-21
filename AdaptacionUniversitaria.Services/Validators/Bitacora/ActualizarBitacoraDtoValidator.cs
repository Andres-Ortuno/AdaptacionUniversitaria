using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using AdaptacionUniversitaria.Core.DTOs;

namespace AdaptacionUniversitaria.Services.Validators.Bitacora
{
    public class ActualizarBitacoraDtoValidator : AbstractValidator<BitacoraDto>
    {
        public ActualizarBitacoraDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("El ID de la bitácora es obligatorio y debe ser mayor que cero.");

            RuleFor(x => x.EstudianteId)
               .GreaterThan(0)
               .WithMessage("El ID del estudiante es obligatorio y debe ser mayor que cero.");

            RuleFor(x => x.Fecha)
                .NotEmpty().WithMessage("La fecha de la bitácora es obligatoria.")
                .Must(BeAValidDate).WithMessage("La fecha de la bitácora debe tener un formato válido (por ejemplo, 2026-10-17).")
                .Must(NotBeInTheFuture).WithMessage("La fecha de la bitácora no puede ser futura.");

            RuleFor(x => x.EstadoAnimo)
                .InclusiveBetween((sbyte)1, (sbyte)5)
                .WithMessage("El estado de ánimo debe estar entre 1 y 5.");

            // Opcional en la base de datos (VARCHAR(255) NULL)
            RuleFor(x => x.Sintomas)
                .MaximumLength(255)
                .WithMessage("Los síntomas no pueden exceder los 255 caracteres.");

            // Opcional, pero si se envía debe tener contenido real
            RuleFor(x => x.Comentario)
                .MinimumLength(5).WithMessage("El comentario debe tener al menos 5 caracteres.")
                .MaximumLength(500).WithMessage("El comentario no puede exceder los 500 caracteres.")
                .When(x => !string.IsNullOrWhiteSpace(x.Comentario));

            // Opcional: si no se envía, debería asignarse al guardar
            RuleFor(x => x.CreadoEn)
                .Must(BeAValidDate!)
                .WithMessage("La fecha de creación debe tener un formato válido (por ejemplo, 2026-10-17).")
                .When(x => !string.IsNullOrWhiteSpace(x.CreadoEn));
        }

        private static bool BeAValidDate(string? date) => DateTime.TryParse(date, out _);

        private static bool NotBeInTheFuture(string? date) =>
            DateTime.TryParse(date, out var parsed) && parsed.Date <= DateTime.Today;
    }
}
