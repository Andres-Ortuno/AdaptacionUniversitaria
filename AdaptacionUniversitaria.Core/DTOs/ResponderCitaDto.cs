namespace AdaptacionUniversitaria.Core.DTOs
{
    // Cuerpo para aprobar o rechazar una cita (CU-04 y CU-05).
    // Sin inicio de sesión, el consejero se identifica enviando su ID.
    public class ResponderCitaDto
    {
        public int ConsejeroId { get; set; }

        // Solo se usa al rechazar (RN-06)
        public string? MotivoRechazo { get; set; }
    }
}
