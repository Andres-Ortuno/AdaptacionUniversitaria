using AdaptacionUniversitaria.Core.Enums;

namespace AdaptacionUniversitaria.Core.Entities
{
    public class Cita : BaseEntity
    {
        public int EstudianteId { get; set; }

        public int ConsejeroId { get; set; }

        public DateTime FechaHora { get; set; }

        public string Motivo { get; set; } = null!;

        public EstadoCita Estado { get; set; }

        public string? MotivoRechazo { get; set; }

        public DateTime CreadoEn { get; set; }

        public DateTime? RespondidoEn { get; set; }

        public virtual Usuario Estudiante { get; set; } = null!;

        public virtual Usuario Consejero { get; set; } = null!;
    }
}
