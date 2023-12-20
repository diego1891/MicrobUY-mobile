

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase;
using Firebase.Messaging;
using MicrobUY.Models.Backend.Posts;
using MicrobUY.Services;
using Plugin.Firebase.CloudMessaging;
using Plugin.FirebasePushNotification;

namespace MicrobUY.ViewModels;

public partial class EscribirPostViewModel : ViewModelGlobal
{
    [ObservableProperty]
    PostResponse post;

    private readonly PostsService _postService;

    private readonly INavegacionService _navegacionService;

    public EscribirPostViewModel(PostsService postService, INavegacionService navegacionService)
    {
        _postService = postService;
        _navegacionService = navegacionService;
    }

    [RelayCommand]
    async Task Postear(string contenido)
    {
        CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
        var token = await CrossFirebaseCloudMessaging.Current.GetTokenAsync();
        Post = new PostResponse { Contenido = contenido };
        await _postService.CrearPost(Post);
        await _navegacionService.GoToAsync("..");

        // Enviar notificación local con Plugin.FirebasePushNotification
        var notification = new NotificationRequest
        {
            Title = "MicrobUY",
            Description = "Post realizado con éxito!",
            NotificationId = 1
        };

        NotificationCenter.Current.Show(notification)
    }



}

