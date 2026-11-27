namespace AdaptacionUniversitaria.Core.Entities
{
    public class Usuario : BaseEntity
    {
        public int RolId { get; set; }

        public string Nombres { get; set; } = null!;

        public string Apellidos { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        // RN-08: los usuarios no se eliminan, se desactivan
        public bool Activo { get; set; }

        public DateTime CreadoEn { get; set; }

        public virtual Rol Rol { get; set; } = null!;

        public virtual ICollection<Bitacora> Bitacoras { get; set; } = new List<Bitacora>();

        public virtual ICollection<Cita> CitasComoEstudiante { get; set; } = new List<Cita>();

        public virtual ICollection<Cita> CitasComoConsejero { get; set; } = new List<Cita>();
    }
}
