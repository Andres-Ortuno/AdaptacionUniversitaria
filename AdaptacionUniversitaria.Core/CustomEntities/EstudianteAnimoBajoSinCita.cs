using System;
using System.Collections.Generic;
using System.Text;

namespace AdaptacionUniversitaria.Core.CustomEntities
{
    public class EstudianteAnimoBajoSinCita
    {
        /*SELECT 
    u.id AS estudiante_id,
    CONCAT(u.nombres, ' ', u.apellidos) AS estudiante,
    u.email,
    u.activo,
    MIN(b.estado_animo) AS peor_animo
FROM usuarios u
INNER JOIN bitacoras b ON u.id = b.estudiante_id
WHERE b.estado_animo <= 2
  AND NOT EXISTS (
      SELECT 1
      FROM citas c
      WHERE c.estudiante_id = u.id
        AND c.estado = 'Aprobada'
  )
GROUP BY u.id, u.nombres, u.apellidos, u.email, u.activo
ORDER BY peor_animo ASC, u.apellidos ASC;*/
        public int Estudiante_id { get; set; }
        public string Estudiante { get; set; } = string.Empty;
        public float Peor_animo { get; set; }

    }
}
