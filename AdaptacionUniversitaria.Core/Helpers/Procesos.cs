namespace AdaptacionUniversitaria.Core.Helpers
{
    public static class Procesos
    {
        // Bolivia está en UTC-4 todo el año (no usa horario de verano).
        // El servidor en Azure corre en UTC, por eso se calcula la hora local a mano.
        public static DateTime AhoraBolivia() => DateTime.UtcNow.AddHours(-4);

        public static DateOnly HoyBolivia() => DateOnly.FromDateTime(AhoraBolivia());
    }
}
