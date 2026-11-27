using AdaptacionUniversitaria.Core.Entities;
using AdaptacionUniversitaria.Core.Exceptions;
using AdaptacionUniversitaria.Core.Interfaces;
using System.Net;

namespace AdaptacionUniversitaria.Services.Services
{
    public abstract class ServiceBase
    {
        protected readonly IUnitOfWork _unitOfWork;

        protected ServiceBase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// El sistema no tiene inicio de sesión: la persona se identifica con su id
        /// (header X-Usuario-Id). Aquí se valida que exista, esté activa y tenga el rol permitido.
        /// </summary>
        protected async Task<Usuario> ObtenerUsuarioActualAsync(int usuarioId, params string[] rolesPermitidos)
        {
            if (usuarioId <= 0)
                throw new BusinessException("Debe indicar su identificador en el header X-Usuario-Id.",
                    HttpStatusCode.Unauthorized);

            var usuario = await _unitOfWork.UsuarioRepository.GetByIdConRolAsync(usuarioId);
            if (usuario == null)
                throw new BusinessException("El usuario no existe.", HttpStatusCode.Unauthorized);

            if (!usuario.Activo)
                throw new BusinessException("La cuenta está desactivada.", HttpStatusCode.Forbidden);

            if (rolesPermitidos.Length > 0 && !rolesPermitidos.Contains(usuario.Rol.Nombre))
                throw new BusinessException("Su rol no tiene permiso para esta operación.", HttpStatusCode.Forbidden);

            return usuario;
        }
    }
}
