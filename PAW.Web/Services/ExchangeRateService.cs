using PAW.Architecture.Providers;
using System.Text.Json.Serialization;

namespace PAW.Web.Services;

public interface IExchangeRateService
{
    Task<TipoCambioResponse> GetExchangeRateAsync(
        CancellationToken ct = default);
}

public class ExchangeRateService : IExchangeRateService
{
    private readonly TipoCambioClient _client;

    public ExchangeRateService(TipoCambioClient client)
    {
        _client = client;
    }

    public Task<TipoCambioResponse> GetExchangeRateAsync(
        CancellationToken ct = default)
    {
        return _client.GetTipoCambioAsync(ct);
    }
}

public class TipoCambioClient
{
    private readonly HttpClient _http;

    public TipoCambioClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<TipoCambioResponse> GetTipoCambioAsync(
        CancellationToken ct = default)
    {
        using var response =
            await _http.GetAsync("indicadores/tc", ct);

        response.EnsureSuccessStatusCode();

        var json =
            await response.Content.ReadAsStringAsync(ct);

        var result =
            JsonProvider.DeserializeSimple<TipoCambioResponse>(json);

        return result
            ?? throw new InvalidOperationException(
                "No se pudo obtener el tipo de cambio.");
    }
}


// DTOs

public class TipoCambioResponse
{
    [JsonPropertyName("dolar")]
    public Dolar Dolar { get; set; } = new();

    [JsonPropertyName("euro")]
    public Euro Euro { get; set; } = new();
}

public class Dolar
{
    [JsonPropertyName("venta")]
    public ValorFecha Venta { get; set; } = new();

    [JsonPropertyName("compra")]
    public ValorFecha Compra { get; set; } = new();
}

public class ValorFecha
{
    [JsonPropertyName("fecha")]
    public DateTime Fecha { get; set; }

    [JsonPropertyName("valor")]
    public decimal Valor { get; set; }
}

public class Euro
{
    [JsonPropertyName("fecha")]
    public DateTime Fecha { get; set; }

    [JsonPropertyName("dolares")]
    public decimal Dolares { get; set; }

    [JsonPropertyName("colones")]
    public decimal Colones { get; set; }
}
