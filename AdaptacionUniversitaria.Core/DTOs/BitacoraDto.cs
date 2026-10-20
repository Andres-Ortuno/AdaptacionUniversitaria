using AdaptacionUniversitaria.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdaptacionUniversitaria.Core.DTOs
{
    public class BitacoraDto
    {
        public int Id { get; set; }

        public int EstudianteId { get; set; }

        public string? Fecha { get; set; } = null;

        public sbyte EstadoAnimo { get; set; }

        public string? Sintomas { get; set; }

        public string? Comentario { get; set; }

        public string? CreadoEn { get; set; } = null;
    }
}
