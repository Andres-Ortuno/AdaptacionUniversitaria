using System;
using System.Collections.Generic;
using System.Text;

namespace AdaptacionUniversitaria.Infrastructure.Queries
{
    public static class BitacorasQueries
    {
        public static string historialMySql = @"
            SELECT id            AS Id,
                   estudiante_id AS EstudianteId,
                   fecha         AS Fecha,
                   estado_animo  AS EstadoAnimo,
                   sintomas      AS Sintomas,
                   nota          AS Nota,
                   creado_en     AS CreadoEn
            FROM bitacoras
            WHERE estudiante_id = @EstudianteId
              AND (@Desde IS NULL OR fecha >= @Desde)
              AND (@Hasta IS NULL OR fecha <= @Hasta)
            ORDER BY fecha DESC;";
    }
}
