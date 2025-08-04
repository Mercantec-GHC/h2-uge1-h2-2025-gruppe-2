namespace BlazorWASM.Services;

using System.Text.Json;
using System.Text.Json.Serialization;

public class Miles
{
    [JsonConverter(typeof(StringToDoubleConverter))]
    public double Price { get; set; }

    public string Date { get; set; } = string.Empty;
}

public class StringToDoubleConverter : JsonConverter<double>
{
    public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            if (double.TryParse(reader.GetString(), out var value))
            {
                return value;
            }

            throw new JsonException($"Unable to convert \"{reader.GetString()}\" to double.");
        }
        else if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetDouble();
        }

        throw new JsonException($"Unexpected token {reader.TokenType} when parsing double.");
    }

    public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }
}