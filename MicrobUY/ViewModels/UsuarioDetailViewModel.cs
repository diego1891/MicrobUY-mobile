using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MicrobUY.Models.Backend.Login;
using MicrobUY.Services;

namespace MicrobUY.ViewModels;

public partial class UsuarioDetailViewModel : ViewModelGlobal, IQueryAttributable
{
    [ObservableProperty]
    UsuarioResponse usuario;

    private readonly UsuarioService _usuarioService;
    private readonly INavegacionService _navegacionService;

    public UsuarioDetailViewModel(UsuarioService usuarioService, INavegacionService navegacionService)
    {
        _usuarioService = usuarioService;
        _navegacionService = navegacionService;
    }

    public async Task LoadDataAsync(string Id)
    {
        if (IsBusy)
            return;
        try
        {
            IsBusy = true;
            Usuario = await _usuarioService.GetUserById(Id);
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

}
