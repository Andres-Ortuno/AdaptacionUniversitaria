using System;
using System.Collections.Generic;

namespace AdaptacionUniversitaria.Core.Entities;

public partial class Usuario
{
    public int Id { get; set; }

    public int RolId { get; set; }

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public bool? Activo { get; set; }

    public DateTime CreadoEn { get; set; }

    public virtual ICollection<Bitacora> Bitacoras { get; set; } = new List<Bitacora>();

    public virtual ICollection<Cita> CitaConsejeros { get; set; } = new List<Cita>();

    public virtual ICollection<Cita> CitaEstudiantes { get; set; } = new List<Cita>();

    public virtual Role Rol { get; set; } = null!;
}
