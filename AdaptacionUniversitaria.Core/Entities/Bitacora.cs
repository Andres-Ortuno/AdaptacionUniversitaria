using System;
using System.Collections.Generic;

namespace AdaptacionUniversitaria.Core.Entities;

public partial class Bitacora
{
    public int Id { get; set; }

    public int EstudianteId { get; set; }

    public DateTime Fecha { get; set; }

    public sbyte EstadoAnimo { get; set; }

    public string? Sintomas { get; set; }

    public string? Nota { get; set; }

    public DateTime CreadoEn { get; set; }

    public virtual Usuario? Estudiante { get; set; } = null!;
}
