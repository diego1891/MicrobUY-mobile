using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MicrobUY.Models.Backend.Instancia;
using MicrobUY.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrobUY.ViewModels;

public partial class LoginViewModel : ViewModelGlobal, IQueryAttributable
{
    private readonly IConnectivity _connectivity;

    [ObservableProperty]
    private string email;
    [ObservableProperty]
    private string password;
    [ObservableProperty]
    private bool tenantSeleccionado;

    [ObservableProperty]
    ObservableCollection<InstanciaResponse> instancias;

    [ObservableProperty]
    InstanciaResponse instanciaSeleccionada;

    private readonly SecurityService _securityService;

    public LoginViewModel(IConnectivity connectivity, SecurityService securityService)
    {
        _connectivity = connectivity;
        _securityService = securityService;
        _connectivity.ConnectivityChanged += _connectivity_ConnectivityChanged;
        PropertyChanged += LoginViewModel_PropertyChanged;
        LoadDataAsync();
    }

    private void LoginViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        TenantSeleccionado = true;
    }
    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        await LoadDataAsync();
    }

    private void _connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
    {
        LoginMethodCommand.NotifyCanExecuteChanged();
    }

    public async Task LoadDataAsync()
    {
        if (IsBusy)
            return;
        try
        {
            IsBusy = true;
            var listInstancias = await _securityService.getInstancias();
            Instancias = new ObservableCollection<InstanciaResponse>(listInstancias.Where(instancia => !string.IsNullOrEmpty(instancia.Nombre)));

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

    [RelayCommand(CanExecute = nameof(StatusConnection))]
    private async Task LoginMethod()
    {
        if(TenantSeleccionado){
            var resultado = await _securityService.Login(Email, Password, InstanciaSeleccionada.Nombre);
            if (resultado)
            {
                Application.Current.MainPage = new AppShell();
            }
            else
            {
                await Shell.Current.DisplayAlert("Mensaje", "Ingreso credenciales incorrectas", "Aceptar");
            }
        }
        else
        {
            await Shell.Current.DisplayAlert("Mensaje", "Debe seleccionar una instancia", "Aceptar");
        }
        
        
    }

    [RelayCommand]
    private async Task Register()
    {
       // await _securityService.Googlelogin(Email, Password);
    }

    private bool StatusConnection()
    {
        return _connectivity.NetworkAccess == NetworkAccess.Internet ? true : false;
    }
}
