using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using AdaptacionUniversitaria.Core.DTOs;

namespace AdaptacionUniversitaria.Services.Validators.Bitacora
{
    public class CrearBitacoraDtoValidator : AbstractValidator<BitacoraDto>
    {
        public CrearBitacoraDtoValidator()
        {
            // Para actualización, el ID es obligatorio
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID del post es obligatorio y debe ser mayor que cero.");

            // Validación para UserId
            RuleFor(x => x.EstudianteId)
                .GreaterThan(0).WithMessage("El ID de usuario es obligatorio y debe ser mayor que cero.")
                .NotEmpty().WithMessage("El ID de usuario no puede estar vacío.");

            // Validación para Date
            RuleFor(x => x.Fecha)
                .NotEmpty().WithMessage("La fecha es obligatoria.")
                .Must(BeAValidDate).WithMessage("La fecha debe tener un formato válido (dd-MM-yyyy o similar).");
            RuleFor(x => x.CreadoEn)
                .NotEmpty().WithMessage("La fecha es obligatoria.")
                .Must(BeAValidDate).WithMessage("La fecha debe tener un formato válido (dd-MM-yyyy o similar).");

            // Validación para Description
            RuleFor(x => x.Comentario)
                .NotEmpty().WithMessage("La descripción es obligatoria.")
                .MinimumLength(5).WithMessage("La descripción debe tener al menos 5 caracteres.")
                .MaximumLength(500).WithMessage("La descripción no puede exceder los 500 caracteres.");

            // Falta validacion para las demás y corregir gramatica
        }

        private bool BeAValidDate(string date) => DateTime.TryParse(date, out _);

        private bool BeAValidUrl(string? url)
        {
            if (string.IsNullOrEmpty(url))
                return true;

            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
