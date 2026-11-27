using System;
using System.Collections.Generic;
using System.Text;

namespace AdaptacionUniversitaria.Infrastructure.Queries
{
    public static class ReportesQueries
    {
        public static string estudiantesAnimoBajoMySql = @"
            SELECT u.id                              AS EstudianteId,
                   CONCAT(u.nombres, ' ', u.apellidos) AS NombreCompleto,
                   u.email                           AS Email,
                   COUNT(b.id)                       AS TotalRegistros,
                   ROUND(AVG(b.estado_animo), 2)     AS PromedioAnimo,
                   MAX(b.fecha)                      AS UltimoRegistro
            FROM usuarios u
            INNER JOIN bitacoras b ON b.estudiante_id = u.id
            WHERE b.fecha >= @Desde
              AND EXISTS (SELECT 1 FROM citas c
                          WHERE c.consejero_id  = @ConsejeroId
                            AND c.estudiante_id = u.id
                            AND c.estado        = 'Aprobada')
            GROUP BY u.id, u.nombres, u.apellidos, u.email
            HAVING AVG(b.estado_animo) <= @Umbral
            ORDER BY PromedioAnimo ASC, u.id ASC;";
    }
}
