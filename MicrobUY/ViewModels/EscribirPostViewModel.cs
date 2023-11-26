

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MicrobUY.Models.Backend.Posts;
using MicrobUY.Services;

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
        Post = new PostResponse { Contenido = contenido };  
        await _postService.CrearPost(Post);
        await _navegacionService.GoToAsync("..");
    }
}
