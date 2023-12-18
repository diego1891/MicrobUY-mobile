
using MicrobUY.ViewModels;
namespace MicrobUY.Views;

public partial class BusquedaPage : ContentPage
{
    private readonly BusquedaViewModel _viewModel;
    public BusquedaPage(BusquedaViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

}
