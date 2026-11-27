using AdaptacionUniversitaria.Core.Entities;

namespace AdaptacionUniversitaria.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> GetUsuariosAsync(int usuarioId);
        Task<Usuario> CrearAsync(int usuarioId, Usuario nuevo, string password);
        Task<Usuario> ActualizarAsync(int usuarioId, int id, Usuario cambios, string? password);
    }
}
