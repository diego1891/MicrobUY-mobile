
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MicrobUY.Models.Backend.Posts;
using MicrobUY.Models.Backend.Search;
using MicrobUY.Services;
using MicrobUY.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MicrobUY.ViewModels;

public partial class HomeViewModel : ViewModelGlobal, IQueryAttributable
{
    [ObservableProperty]
    string nombreUsuario;

    [ObservableProperty]
    SearchResponse search;

    [ObservableProperty]
    PostResponse postSeleccionado;

    [ObservableProperty]
    private string searchText;

    [ObservableProperty]
    bool isVisible;

    [ObservableProperty]
    ObservableCollection<PostResponse> posts;

    [ObservableProperty]
    ObservableCollection<SearchResponse> busquedas;

    public Command GetDataCommand { get; }

    private readonly PostsService _postsService;

    private readonly BusquedaService _busquedaService;

    private readonly SecurityService _securityService;

    private readonly INavegacionService _navegacionService;

    public HomeViewModel(PostsService postsService, INavegacionService navegacionService, BusquedaService busquedaService, SecurityService securityService)
    {
        _postsService = postsService;
        NombreUsuario = Preferences.Get("nombre", string.Empty);
        _navegacionService = navegacionService;
        _securityService = securityService;
        _busquedaService = busquedaService;
        PropertyChanged += HomeViewModel_PropertyChanged;
        LoadDataAsync();
    }

    private async void HomeViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(PostSeleccionado))
        {
            var uri = $"{nameof(PostDetailPage)}?id={PostSeleccionado.Id}";
            await _navegacionService.GoToAsync(uri);
        }
    }

    public async void OnAppearing()
    {
        await LoadDataAsync();
    }

    public async Task LoadDataAsync()
    {
        if (IsBusy)
            return;
        try
        {
            IsBusy = true;
            var listPosts = await _postsService.GetPosts();
            Posts = new ObservableCollection<PostResponse>(listPosts.Where(post => !string.IsNullOrEmpty(post.Contenido)));

        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
        }
        finally
        {
            IsBusy = false;
        }
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        await LoadDataAsync();
    }

    [RelayCommand]
    async Task Like()
    {
        if (IsBusy)
            return;
        try
        {
            IsBusy = true;
            //            await _postsService.LikePost(Id);

        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
        }
        finally
        {
            IsBusy = false;
        }
    }
    [RelayCommand]
    async Task CrearPost()
    {
        var uri = $"{nameof(EscribirPostPage)}";
        await _navegacionService.GoToAsync(uri);
    }

    [RelayCommand]
    async Task Logout()
    {
        var resultado = await _securityService.Logout();
        if (resultado != false)
        {
            await _navegacionService.GoToAsync(nameof(LoginPage));

        }
    }

    public ICommand EjecutarBusqueda => new Command(async () =>
    {
        var busqueda = await _busquedaService.EjecutarBusqueda(SearchText);
        Busquedas = new ObservableCollection<SearchResponse>(busqueda);
        if (Busquedas != null) {
            IsVisible = true;
        }
       
    });

    [RelayCommand]
    async Task Buscar()
    {
        var uri = $"{nameof(BusquedaPage)}";
        await _navegacionService.GoToAsync(uri);
    }

}
