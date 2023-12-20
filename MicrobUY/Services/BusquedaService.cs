using MicrobUY.Models.Backend.Search;
using MicrobUY.Models.Config;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace MicrobUY.Services;

public  class BusquedaService
{
    private readonly HttpClient client;

    private Settings settings;

    public BusquedaService(HttpClient client, IConfiguration configuration)
    {
        this.client = client;
        this.settings = configuration.GetRequiredSection(nameof(Settings)).Get<Settings>(); ;
    }

    public async Task<List<SearchResponse>> EjecutarBusqueda(string contenido)
    {
        var tenant = Preferences.Get("tenant", string.Empty);
        var uri = $"{settings.UrlBase}/{tenant}/Search?query={Uri.EscapeDataString(contenido)}";
        client.DefaultRequestHeaders.Authorization = new
           AuthenticationHeaderValue("bearer", Preferences.Get("accesstoken", string.Empty));

        var resultado = await client.GetStringAsync(uri);

        return JsonConvert.DeserializeObject<List<SearchResponse>>(resultado);
    }
}
