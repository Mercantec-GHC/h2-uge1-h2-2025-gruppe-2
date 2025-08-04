namespace BlazorWASM.Services;

using System.Text.Json;
using System.Text.Json.Serialization;

public class Miles
{
    [JsonConverter(typeof(StringToDecimalConverter))]
    public decimal Price { get; set; }

    public string Date { get; set; } = string.Empty;
}

public class StringToDecimalConverter : JsonConverter<decimal>
{
    public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            if (decimal.TryParse(reader.GetString(), out var value))
            {
                return value;
            }

            throw new JsonException($"Unable to convert \"{reader.GetString()}\" to decimal.");
        }
        else if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetDecimal();
        }

        throw new JsonException($"Unexpected token {reader.TokenType} when parsing decimal.");
    }

    public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }
}