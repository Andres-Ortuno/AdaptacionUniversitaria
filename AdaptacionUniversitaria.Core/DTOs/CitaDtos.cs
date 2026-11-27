namespace AdaptacionUniversitaria.Core.DTOs
{
    public class ConsejeroDto
    {
        public int Id { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class CitaDto
    {
        public int Id { get; set; }
        public int EstudianteId { get; set; }
        public string? EstudianteNombres { get; set; }
        public string? EstudianteApellidos { get; set; }
        public int ConsejeroId { get; set; }
        public string? ConsejeroNombres { get; set; }
        public string? ConsejeroApellidos { get; set; }
        public DateTime FechaHora { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string? MotivoRechazo { get; set; }
        public DateTime CreadoEn { get; set; }
        public DateTime? RespondidoEn { get; set; }
    }

    public class CitaCrearDto
    {
        public int ConsejeroId { get; set; }
        public DateTime FechaHora { get; set; }
        public string Motivo { get; set; } = string.Empty;
    }

    public class CitaRechazarDto
    {
        public string MotivoRechazo { get; set; } = string.Empty;
    }
}
