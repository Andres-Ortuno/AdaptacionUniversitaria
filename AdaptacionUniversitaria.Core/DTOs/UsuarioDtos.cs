namespace AdaptacionUniversitaria.Core.DTOs
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public int RolId { get; set; }
        public string? RolNombre { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool Activo { get; set; }
        public DateTime CreadoEn { get; set; }
    }

    public class UsuarioCrearDto
    {
        public int RolId { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        /// <summary>Contraseña temporal; se guarda cifrada (hash), nunca en texto plano.</summary>
        public string Password { get; set; } = string.Empty;
    }

    public class UsuarioActualizarDto
    {
        public int RolId { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        /// <summary>Opcional: si se envía, reemplaza la contraseña.</summary>
        public string? Password { get; set; }
        public bool Activo { get; set; }
    }
}
