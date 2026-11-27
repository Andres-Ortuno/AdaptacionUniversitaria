namespace AdaptacionUniversitaria.Infrastructure.Queries
{
    public static class CitasQueries
    {
        // RN-10: el consejero solo ve registros de estudiantes con al menos una cita aprobada
        public static string existeCitaAprobadaMySql = @"
            SELECT COUNT(1)
            FROM citas
            WHERE consejero_id  = @ConsejeroId
              AND estudiante_id = @EstudianteId
              AND estado        = 'Aprobada';";
    }
}
