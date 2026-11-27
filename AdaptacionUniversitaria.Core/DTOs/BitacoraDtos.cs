namespace AdaptacionUniversitaria.Core.DTOs
{
    /// <summary>Respuesta: una entrada de la bitácora emocional.</summary>
    public class BitacoraDto
    {
        public int Id { get; set; }
        public int EstudianteId { get; set; }
        public DateOnly Fecha { get; set; }
        public int EstadoAnimo { get; set; }
        public string? Sintomas { get; set; }
        public string? Nota { get; set; }
        public DateTime CreadoEn { get; set; }
    }

    /// <summary>Entrada: sirve para registrar (POST) y para editar (PUT) la bitácora del día.</summary>
    public class BitacoraGuardarDto
    {
        public int EstadoAnimo { get; set; }
        public string? Sintomas { get; set; }
        public string? Nota { get; set; }
    }
}
