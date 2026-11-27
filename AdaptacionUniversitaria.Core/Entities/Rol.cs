namespace AdaptacionUniversitaria.Core.Entities
{
    public class Rol : BaseEntity
    {
        public string Nombre { get; set; } = null!;

        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
