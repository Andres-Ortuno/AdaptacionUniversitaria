using AdaptacionUniversitaria.Core.Entities;
using AdaptacionUniversitaria.Core.Enums;
using AdaptacionUniversitaria.Core.Exceptions;
using AdaptacionUniversitaria.Core.Helpers;
using AdaptacionUniversitaria.Core.Interfaces;
using AdaptacionUniversitaria.Services.Interfaces;
using System.Net;

namespace AdaptacionUniversitaria.Services.Services
{
    public class UsuarioService : ServiceBase, IUsuarioService
    {
        public UsuarioService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public async Task<IEnumerable<Usuario>> GetUsuariosAsync(int usuarioId)
        {
            await ObtenerUsuarioActualAsync(usuarioId, RolNombre.Administrador);
            return await _unitOfWork.UsuarioRepository.GetAllConRolAsync();
        }

        public async Task<Usuario> CrearAsync(int usuarioId, Usuario nuevo, string password)
        {
            await ObtenerUsuarioActualAsync(usuarioId, RolNombre.Administrador);

            await ValidarRolAsync(nuevo.RolId);
            nuevo.Email = nuevo.Email.Trim().ToLowerInvariant();
            await ValidarEmailUnicoAsync(nuevo.Email, null);

            nuevo.Id = 0;
            nuevo.Nombres = nuevo.Nombres.Trim();
            nuevo.Apellidos = nuevo.Apellidos.Trim();
            nuevo.PasswordHash = PasswordHasher.Hash(password);
            nuevo.Activo = true;
            nuevo.CreadoEn = Procesos.AhoraBolivia();

            await _unitOfWork.UsuarioRepository.Add(nuevo);
            await _unitOfWork.SaveChangesAsync();

            // Se vuelve a leer para devolver el usuario con el nombre del rol
            return (await _unitOfWork.UsuarioRepository.GetByIdConRolAsync(nuevo.Id))!;
        }

        public async Task<Usuario> ActualizarAsync(int usuarioId, int id, Usuario cambios, string? password)
        {
            await ObtenerUsuarioActualAsync(usuarioId, RolNombre.Administrador);

            var usuario = await _unitOfWork.UsuarioRepository.GetByIdConRolAsync(id);
            if (usuario == null)
                throw new BusinessException("Usuario no encontrado.", HttpStatusCode.NotFound);

            await ValidarRolAsync(cambios.RolId);
            var email = cambios.Email.Trim().ToLowerInvariant();
            await ValidarEmailUnicoAsync(email, usuario.Id);

            usuario.RolId = cambios.RolId;
            usuario.Nombres = cambios.Nombres.Trim();
            usuario.Apellidos = cambios.Apellidos.Trim();
            usuario.Email = email;
            usuario.Activo = cambios.Activo; // RN-08: se desactiva, no se elimina

            if (!string.IsNullOrWhiteSpace(password))
                usuario.PasswordHash = PasswordHasher.Hash(password);

            await _unitOfWork.UsuarioRepository.Update(usuario);
            await _unitOfWork.SaveChangesAsync();

            return (await _unitOfWork.UsuarioRepository.GetByIdConRolAsync(usuario.Id))!;
        }

        private async Task ValidarRolAsync(int rolId)
        {
            var rol = await _unitOfWork.RolRepository.GetById(rolId);
            if (rol == null)
                throw new BusinessException("El rol indicado no existe.");
        }

        // RN-09: el correo debe ser único
        private async Task ValidarEmailUnicoAsync(string email, int? idActual)
        {
            var existente = await _unitOfWork.UsuarioRepository.GetByEmailAsync(email);
            if (existente != null && existente.Id != idActual)
                throw new BusinessException("Ya existe un usuario con ese correo.", HttpStatusCode.Conflict);
        }
    }
}
