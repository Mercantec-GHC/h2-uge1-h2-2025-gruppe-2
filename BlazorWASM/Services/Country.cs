using System.Text.Json.Serialization;

namespace BlazorWASM.Services;

public class Country
{
    public string Id { get; set; } = "";
    public CountryName Name { get; set; } = new();
    public Dictionary<string, CurrencyInfo>? Currencies { get; set; }
    public Flag Flags { get; set; } = new();
}

public class CountryName
{
    public string Common { get; set; } = "";
    public string Official { get; set; } = "";
}

public class CurrencyInfo
{
    public string Name { get; set; } = "";
    public string Symbol { get; set; } = "";
}

public class Flag
{
    public string png { get; set; } = "";
    public string svg { get; set; } = "";
}