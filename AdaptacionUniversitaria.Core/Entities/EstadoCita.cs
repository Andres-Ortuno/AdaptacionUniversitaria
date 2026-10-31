namespace AdaptacionUniversitaria.Core.Entities
{
    // Estados posibles de una cita (columna citas.estado)
    public static class EstadoCita
    {
        public const string Pendiente = "Pendiente";
        public const string Aprobada = "Aprobada";
        public const string Rechazada = "Rechazada";
        public const string Cancelada = "Cancelada";
    }
}
