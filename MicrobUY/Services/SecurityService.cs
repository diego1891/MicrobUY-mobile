using MicrobUY.Models.Backend.Login;
using MicrobUY.Models.Config;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MicrobUY.Services;

public class SecurityService
{
    private HttpClient client;
    private Settings settings;

    public SecurityService(HttpClient client, IConfiguration configuration)
    {
        this.client = client;
        settings = configuration.GetRequiredSection(nameof(Settings)).Get<Settings>();
    }

    public async Task<bool> Login(string email, string password)
    {
        var url = $"{settings.UrlBase}/DanielitaInstancia/Autenticacion";
        var loginRequest = new LoginRequest { Email = email, Password = password };

        var json = JsonConvert.SerializeObject(loginRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(url, content);

        if (!response.IsSuccessStatusCode)
        {
            return false;
        }
        var jsonResultado = await response.Content.ReadAsStringAsync();
        var resultado = JsonConvert.DeserializeObject<UsuarioResponse>(jsonResultado);

        Preferences.Set("accesstoken", resultado.Token);
        Preferences.Set("nombre", $" {resultado.Nombre}, {resultado.Apellido}");
        Preferences.Set("username", resultado.UserName);
        Preferences.Set("email", resultado.Email);
        Preferences.Set("biografia", resultado.Biografia);
        Preferences.Set("ocupacion", resultado.Ocupacion);
        Preferences.Set("ocupacion", resultado.Ocupacion);
        Preferences.Set("fechanacimiento", resultado.FechaNacimiento);
        Preferences.Set("ubicacion", resultado.Ubicacion);
        return true;
    }
    public async Task<bool> Logout()
    {
        try {
            var url = $"{settings.UrlBase}/DanielitaInstancia/Autenticacion/logout";

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accesstoken", string.Empty));

            var response = await client.PostAsync(url, null);

            Preferences.Remove("accesstoken");
            Preferences.Remove("nombre");
            Preferences.Remove("username");
            Preferences.Remove("email");
            Preferences.Remove("biografia");
            Preferences.Remove("ocupacion");
            Preferences.Remove("fechanacimiento");
            Preferences.Remove("ubicacion");


            return response.IsSuccessStatusCode;
        }
        catch(Exception ex)
        {
            return false;
        }
        
    }

}

