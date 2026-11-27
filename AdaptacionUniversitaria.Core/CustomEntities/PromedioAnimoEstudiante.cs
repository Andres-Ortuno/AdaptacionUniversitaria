using System;
using System.Collections.Generic;
using System.Text;

namespace AdaptacionUniversitaria.Core.CustomEntities
{
    public class PromedioAnimoEstudiante
    {
        /*SELECT 
            u.id AS estudiante_id,
            CONCAT(u.nombres, ' ', u.apellidos) AS estudiante,
            ROUND(AVG(b.estado_animo), 2) AS promedio_animo,
            COUNT(b.id) AS total_bitacoras,
            MAX(b.fecha) AS ultima_bitacora
        FROM usuarios u
        INNER JOIN bitacoras b ON u.id = b.estudiante_id
        GROUP BY u.id, u.nombres, u.apellidos
        HAVING COUNT(b.id) >= 2
        ORDER BY promedio_animo ASC;*/
        public int Estudiante_id { get; set; }
        public string Estudiante { get; set; } = string.Empty;
        public float Promedio_animo { get; set; }
        public int Total_bitacoras { get; set; }
        public DateTime Ultima_bitacora { get; set; }

    }
}
