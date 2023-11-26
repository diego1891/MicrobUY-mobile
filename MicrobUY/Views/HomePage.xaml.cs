using MicrobUY.ViewModels;

namespace MicrobUY.Views;

public partial class HomePage : ContentPage
{

    private readonly HomeViewModel _viewModel;

    public HomePage(HomeViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        var accessToken = Preferences.Get("accesstoken", string.Empty);
        if (string.IsNullOrEmpty(accessToken))
        {
            await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
        }
        else
        {
            _viewModel.OnAppearing();
        }

    }
}