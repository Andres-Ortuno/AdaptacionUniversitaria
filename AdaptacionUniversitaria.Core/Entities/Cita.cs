using System;
using System.Collections.Generic;

namespace AdaptacionUniversitaria.Core.Entities;

public partial class Cita
{
    public int Id { get; set; }

    public int EstudianteId { get; set; }

    public int ConsejeroId { get; set; }

    public DateTime FechaHora { get; set; }

    public string Motivo { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public string? MotivoRechazo { get; set; }

    public DateTime CreadoEn { get; set; }

    public DateTime? RespondidoEn { get; set; }

    public virtual Usuario Consejero { get; set; } = null!;

    public virtual Usuario Estudiante { get; set; } = null!;
}
