using MicrobUY.ViewModels;

namespace MicrobUY.Views;

public partial class UsuarioDetailPage : ContentPage
{
    public UsuarioDetailPage(UsuarioDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}