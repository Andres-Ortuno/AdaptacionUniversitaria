namespace AdaptacionUniversitaria.Api.Controllers
{
    public static class Cabeceras
    {
        /// <summary>
        /// El sistema no tiene inicio de sesión: cada request indica quién es el usuario
        /// con este header (por ejemplo X-Usuario-Id: 1).
        /// </summary>
        public const string UsuarioId = "X-Usuario-Id";
    }
}
