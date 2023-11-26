using MicrobUY.ViewModels;

namespace MicrobUY.Views;

public partial class EscribirPostPage : ContentPage
{
	public EscribirPostPage(EscribirPostViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}