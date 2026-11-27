using System;
using System.Collections.Generic;
using System.Text;

namespace AdaptacionUniversitaria.Core.CustomEntities.Reporteria
{
    public class EstudianteAnimo
    {
        public int EstudianteId { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int TotalRegistros { get; set; }
        public decimal PromedioAnimo { get; set; }
        public DateTime UltimoRegistro { get; set; }
    }
}