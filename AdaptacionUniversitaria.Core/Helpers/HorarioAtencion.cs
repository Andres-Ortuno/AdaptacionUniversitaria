using System;
using System.Collections.Generic;
using System.Text;

namespace AdaptacionUniversitaria.Core.Helpers
{    public static class HorarioAtencion
    {
        public static readonly DayOfWeek[] DiasAtencion =
        {
            DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday,
            DayOfWeek.Thursday, DayOfWeek.Friday
        };

        public static readonly TimeOnly Apertura = new(8, 0);
        public static readonly TimeOnly Cierre = new(18, 0);

        public static string Descripcion =>
            $"lunes a viernes de {Apertura:HH\\:mm} a {Cierre:HH\\:mm}";

        public static bool EsDiaFuturo(DateTime fechaHora, DateTime ahora) =>
            DateOnly.FromDateTime(fechaHora) > DateOnly.FromDateTime(ahora);

        public static bool EsDiaDeAtencion(DateTime fechaHora) =>
            DiasAtencion.Contains(fechaHora.DayOfWeek);

        public static bool EstaEnHorario(DateTime fechaHora)
        {
            var hora = TimeOnly.FromDateTime(fechaHora);
            return hora >= Apertura && hora < Cierre;
        }

        public static string NombreDia(DayOfWeek dia) => dia switch
        {
            DayOfWeek.Monday => "lunes",
            DayOfWeek.Tuesday => "martes",
            DayOfWeek.Wednesday => "miercoles",
            DayOfWeek.Thursday => "jueves",
            DayOfWeek.Friday => "viernes",
            DayOfWeek.Saturday => "sabado",
            DayOfWeek.Sunday=> "domingo"
        };
    }
}