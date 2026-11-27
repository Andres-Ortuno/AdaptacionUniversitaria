using System;
using System.Collections.Generic;
using System.Text;

namespace AdaptacionUniversitaria.Core.CustomEntities
{
    public class CantidadCitasEstado
    {
        public int CitaId { get; set; }
        public string Estado { get; set; } = string.Empty;
        public int TotalCitas { get; set; }
    }
}
