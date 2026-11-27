using System;
using System.Collections.Generic;
using System.Text;

namespace AdaptacionUniversitaria.Core.DTOs.CustomEntitiesDto
{
    public class PromedioAnimoEstudianteDto
    {
        public string Estudiante { get; set; } = string.Empty;
        public float Promedio_animo { get; set; }
        public int Total_bitacoras { get; set; }
        public DateTime Ultima_bitacora { get; set; }
    }
}
