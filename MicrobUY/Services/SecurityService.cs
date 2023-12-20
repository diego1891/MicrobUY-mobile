using Firebase.Auth;
using MicrobUY.Models.Backend.Instancia;
using MicrobUY.Models.Backend.Login;
using MicrobUY.Models.Config;
using MicrobUY.Views;
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

    public async Task<List<InstanciaResponse>> getInstancias()
    {
        var uri = $"{settings.UrlBase}/Instancia";

        var resultado = await client.GetStringAsync(uri);

        if (resultado.StartsWith("{")) 
        {
            // Si es un objeto, crea una lista con un solo elemento
            var jsonCompleto = JsonConvert.DeserializeAnonymousType(resultado, new { listaInstancias = new List<InstanciaResponse>() });

            // Extraer la propiedad "listaInstancias"
            var listaInstancias = jsonCompleto.listaInstancias;

            return listaInstancias;
        }
        else 
        {
            // Si es un array, realiza la deserialización normalmente
        return JsonConvert.DeserializeObject<List<InstanciaResponse>>(resultado);
    }
    }

    public async Task<bool> Login(string email, string password, string tenantName)
    {
        var url = $"{settings.UrlBase}/{tenantName}/Autenticacion";
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
        Preferences.Set("tenant", tenantName);
        return true;
    }
    public async Task<bool> Logout()
    {
        var tenant = Preferences.Get("tenant", string.Empty);
        try {
            var url = $"{settings.UrlBase}/{tenant}/Autenticacion/logout";

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
            Preferences.Remove("tenant");


            return response.IsSuccessStatusCode;
        }
        catch(Exception ex)
        {
            return false;
        }
        
    }

    public async Task<string> MyRedirectMethod(string uri)
    {
        // Implement your redirect logic here
        // For example, you might start an external browser process or handle the URI in some other way
        // Then return a string, possibly the result of the redirect operation
        return  $"{nameof(HomePage)}";
    }




/*    public async Task<UserCredential> Googlelogin(string email, string password)
    {
        FirebaseProviderType firebaseProviderType = FirebaseProviderType.Google;
        //var authCredential = new { ProviderType: firebaseProviderType };

        //SignInRedirectDelegate redirectDelegate = MyRedirectMethod; // Assign your delegate method here
        var userCredential = await cliente.SignInWithRedirectAsync(firebaseProviderType, uri =>
        {
            Console.WriteLine($"Go to \n{uri}\n and paste here the redirect uri after you finish signing in");
            return Task.FromResult(uri);
        });
        //var a = await cliente.SignInWithCredentialAsync(FirebaseProviderType.Google);
        //var userCredentials = await cliente.SignInWithEmailAndPasswordAsync(email,password);
        //var a = await cliente.SignInWithEmailAndPasswordAsync(email, password);  

        return userCredential;
    }
*/
}

