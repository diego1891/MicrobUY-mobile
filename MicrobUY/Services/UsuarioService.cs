using Microsoft.Extensions.Configuration;
using MicrobUY.Models.Config;
using MicrobUY.Models.Backend.Login;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace MicrobUY.Services;

public class UsuarioService
{
    private readonly HttpClient client;

    private Settings settings;

    public UsuarioService(HttpClient client, IConfiguration configuration)
    {
        this.client = client;
        settings = configuration.GetRequiredSection(nameof(Settings)).Get<Settings>();
    }

    public async Task<UsuarioResponse> GetUserById(string id)
    {
        var uri = $"{settings.UrlBase}/DanielitaInstancia/Usuario/consultar/{id}";
        client.DefaultRequestHeaders.Authorization = new
           AuthenticationHeaderValue("bearer", Preferences.Get("accesstoken", string.Empty));

        var resultado = await client.GetStringAsync(uri);

        return JsonConvert.DeserializeObject<UsuarioResponse>(resultado);
    }



}
