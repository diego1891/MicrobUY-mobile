using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MicrobUY.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrobUY.ViewModels;

public partial class LoginViewModel : ViewModelGlobal
{
    private readonly IConnectivity _connectivity;

    [ObservableProperty]
    private string email;
    [ObservableProperty]
    private string password;

    private readonly SecurityService _securityService;

    public LoginViewModel(IConnectivity connectivity, SecurityService securityService)
    {
        _connectivity = connectivity;
        _securityService = securityService;
        _connectivity.ConnectivityChanged += _connectivity_ConnectivityChanged;
    }

    private void _connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
    {
        LoginMethodCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand(CanExecute = nameof(StatusConnection))]
    private async Task LoginMethod()
    {
        var resultado = await _securityService.Login(Email, Password);
        if (resultado)
        {
            Application.Current.MainPage = new AppShell();
        }
        else
        {
            await Shell.Current.DisplayAlert("Mensaje", "Ingreso credenciales incorrectas", "Aceptar");
        }
    }

    [RelayCommand]
    private async Task RegisterCommand()
    {

    }

    private bool StatusConnection()
    {
        return _connectivity.NetworkAccess == NetworkAccess.Internet ? true : false;
    }
}
