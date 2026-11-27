namespace AdaptacionUniversitaria.Core.QueryFilters
{
    public class BitacoraQueryFilters
    {
        /// <summary>Solo lo usa el consejero para elegir al estudiante (RN-10).</summary>
        public int? EstudianteId { get; set; }

        public DateOnly? FechaDesde { get; set; }

        public DateOnly? FechaHasta { get; set; }
    }
}
