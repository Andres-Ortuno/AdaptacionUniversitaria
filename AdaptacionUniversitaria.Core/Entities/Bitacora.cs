namespace AdaptacionUniversitaria.Core.Entities
{
    public class Bitacora : BaseEntity
    {
        public int EstudianteId { get; set; }

        public DateOnly Fecha { get; set; }

        // RN-02: escala de 1 (muy bajo) a 5 (muy bueno)
        public int EstadoAnimo { get; set; }

        public string? Sintomas { get; set; }

        public string? Nota { get; set; }

        public DateTime CreadoEn { get; set; }

        public virtual Usuario Estudiante { get; set; } = null!;
    }
}
