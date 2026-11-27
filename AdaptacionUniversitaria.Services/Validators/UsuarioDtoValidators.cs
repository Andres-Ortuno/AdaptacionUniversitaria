using AdaptacionUniversitaria.Core.DTOs;
using FluentValidation;

namespace AdaptacionUniversitaria.Services.Validators
{
    public class UsuarioCrearDtoValidator : AbstractValidator<UsuarioCrearDto>
    {
        public UsuarioCrearDtoValidator()
        {
            RuleFor(x => x.RolId).GreaterThan(0).WithMessage("Debe indicar el rol.");

            RuleFor(x => x.Nombres)
                .NotEmpty().WithMessage("Los nombres son obligatorios.")
                .MaximumLength(100).WithMessage("Los nombres no pueden exceder los 100 caracteres.");

            RuleFor(x => x.Apellidos)
                .NotEmpty().WithMessage("Los apellidos son obligatorios.")
                .MaximumLength(100).WithMessage("Los apellidos no pueden exceder los 100 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El correo es obligatorio.")
                .EmailAddress().WithMessage("El correo no tiene un formato válido.")
                .MaximumLength(255).WithMessage("El correo no puede exceder los 255 caracteres.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña temporal es obligatoria.")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");
        }
    }

    public class UsuarioActualizarDtoValidator : AbstractValidator<UsuarioActualizarDto>
    {
        public UsuarioActualizarDtoValidator()
        {
            RuleFor(x => x.RolId).GreaterThan(0).WithMessage("Debe indicar el rol.");

            RuleFor(x => x.Nombres)
                .NotEmpty().WithMessage("Los nombres son obligatorios.")
                .MaximumLength(100).WithMessage("Los nombres no pueden exceder los 100 caracteres.");

            RuleFor(x => x.Apellidos)
                .NotEmpty().WithMessage("Los apellidos son obligatorios.")
                .MaximumLength(100).WithMessage("Los apellidos no pueden exceder los 100 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El correo es obligatorio.")
                .EmailAddress().WithMessage("El correo no tiene un formato válido.")
                .MaximumLength(255).WithMessage("El correo no puede exceder los 255 caracteres.");

            RuleFor(x => x.Password)
                .MinimumLength(6).When(x => !string.IsNullOrEmpty(x.Password))
                .WithMessage("La contraseña debe tener al menos 6 caracteres.");
        }
    }
}
