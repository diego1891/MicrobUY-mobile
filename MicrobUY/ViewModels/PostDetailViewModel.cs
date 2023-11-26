using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MicrobUY.Models.Backend.Posts;
using MicrobUY.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrobUY.ViewModels;
public partial class PostDetailViewModel : ViewModelGlobal, IQueryAttributable
{
    [ObservableProperty]
    PostResponse post;

    private readonly PostsService _postsService;

    private readonly INavegacionService _navegacionService;

    public PostDetailViewModel(PostsService postsService, INavegacionService navegacionService)
    {
        _postsService = postsService;
        _navegacionService = navegacionService;
    }

    public async Task LoadDataAsync(string postId)
    {
        if (IsBusy)
            return;
        try
        {
            IsBusy = true;
            Post = await _postsService.GetPostById(postId);
        }
        catch (Exception e)
        {
            await Application.Current.MainPage.DisplayAlert("Error", e.Message, "Aceptar");
        }
        finally
        {
            IsBusy = false;
        }
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var id = query["id"].ToString();
        await LoadDataAsync(id);

    }


    [RelayCommand]
    async Task GetBackEvent()
    {
        await _navegacionService.GoToAsync("..");
    }


    [RelayCommand]
    async Task Like(string id)
    {
        await _postsService.LikePost(id);
    }



}
