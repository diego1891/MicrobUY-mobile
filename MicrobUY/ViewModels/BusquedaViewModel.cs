using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MicrobUY.Models.Backend.Search;
using MicrobUY.Services;
using MicrobUY.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MicrobUY.ViewModels;

public partial class BusquedaViewModel : ViewModelGlobal
{

    [ObservableProperty]
    ObservableCollection<SearchResponse> busquedas;

    [ObservableProperty]
    SearchResponse busquedaSeleccionada;

    [ObservableProperty]
    private string searchText;

    [ObservableProperty]
    bool isVisible;


    private readonly BusquedaService _busquedaService;
    private readonly INavegacionService _navegacionService;

    public BusquedaViewModel(INavegacionService navegacionService, BusquedaService busquedaService)
    {
        _busquedaService = busquedaService;
        _navegacionService = navegacionService;
        PropertyChanged += BusquedaViewModel_PropertyChanged;
    }

    private async void BusquedaViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(BusquedaSeleccionada))
        {
            var uri = "";
            switch (BusquedaSeleccionada.Type)
            {

                case "Post":
                    uri = $"{nameof(PostDetailPage)}?id={BusquedaSeleccionada.Id}";
                    await _navegacionService.GoToAsync(uri);
                    break;
                case "Usuario":
                    uri = $"{nameof(UsuarioDetailPage)}?id={BusquedaSeleccionada.Id}";
                    await _navegacionService.GoToAsync(uri);
                    break;

            }

        }
    }

    [RelayCommand]
    async Task GetBackEvent()
    {
        await _navegacionService.GoToAsync("..");
    }

    public ICommand EjecutarBusqueda => new Command(async () =>
    {
        var busqueda = await _busquedaService.EjecutarBusqueda(SearchText);
        Busquedas = new ObservableCollection<SearchResponse>(busqueda);
        IsVisible = true;
    });
}
