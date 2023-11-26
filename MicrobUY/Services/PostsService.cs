using MicrobUY.Models.Backend.Posts;
using MicrobUY.Models.Config;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MicrobUY.Services;

public class PostsService
{
    private readonly HttpClient client;

    private Settings settings;

    public PostsService(HttpClient client, IConfiguration configuration)
    {
        this.client = client;
        settings = configuration.GetRequiredSection(nameof(Settings)).Get<Settings>();
    }

    public async Task<PostResponse> GetPostById(string id)
    {
        var uri = $"{settings.UrlBase}/DanielitaInstancia/Post/Get/{id}";
        client.DefaultRequestHeaders.Authorization = new
           AuthenticationHeaderValue("bearer", Preferences.Get("accesstoken", string.Empty));

        var resultado = await client.GetStringAsync(uri);

        return JsonConvert.DeserializeObject<PostResponse>(resultado);
    }


    public async Task<List<PostResponse>> GetPosts()
    {
        var uri = $"{settings.UrlBase}/DanielitaInstancia/Post";
        client.DefaultRequestHeaders.Authorization = new
           AuthenticationHeaderValue("bearer", Preferences.Get("accesstoken", string.Empty));

        var resultado = await client.GetStringAsync(uri);

        return JsonConvert.DeserializeObject<List<PostResponse>>(resultado);
    }

    public async Task LikePost(string id )
    {
        var uri = $"{settings.UrlBase}/DanielitaInstancia/Post/{id}/like";
        client.DefaultRequestHeaders.Authorization = new
           AuthenticationHeaderValue("bearer", Preferences.Get("accesstoken", string.Empty));

        var content = new StringContent(id, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(uri, content);
    }

    public async Task CrearPost(PostResponse post)
    {
        var uri = $"{settings.UrlBase}/DanielitaInstancia/Post";
        client.DefaultRequestHeaders.Authorization = new
           AuthenticationHeaderValue("bearer", Preferences.Get("accesstoken", string.Empty));

        var postJson = JsonConvert.SerializeObject(post);

        var content = new StringContent(postJson, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(uri, content);
    }
}
