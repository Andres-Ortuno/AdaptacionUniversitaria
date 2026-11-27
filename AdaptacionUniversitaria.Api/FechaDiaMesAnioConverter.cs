using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AdaptacionUniversitaria.Api.Converters
{
    /// <summary>Lee y escribe fechas como dd-MM-yyyy HH:mm:ss (día, mes, año).</summary>
    public class FechaDiaMesAnioConverter : JsonConverter<DateTime>
    {
        private static readonly string[] Formatos =
        {
            "dd-MM-yyyy HH:mm:ss", "dd-MM-yyyy HH:mm", "dd-MM-yyyy",
            "dd/MM/yyyy HH:mm:ss", "dd/MM/yyyy HH:mm", "dd/MM/yyyy",
            "yyyy-MM-dd'T'HH:mm:ss", "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd"
        };

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var texto = reader.GetString();

            if (DateTime.TryParseExact(texto, Formatos, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out var fecha))
                return fecha;

            throw new JsonException("Fecha inválida. Use el formato dd-MM-yyyy HH:mm:ss (ejemplo: 10-12-2026 15:00:00).");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture));
        }
    }
}