namespace AdaptacionUniversitaria.Core.DTOs
{
    public class CitaDto
    {
        public int Id { get; set; }

        public int EstudianteId { get; set; }

        public int ConsejeroId { get; set; }

        public DateTime FechaHora { get; set; }

        public string Motivo { get; set; } = string.Empty;

        public string Estado { get; set; } = string.Empty;

        public string? MotivoRechazo { get; set; }

        public DateTime CreadoEn { get; set; }

        public DateTime? RespondidoEn { get; set; }
    }
}
