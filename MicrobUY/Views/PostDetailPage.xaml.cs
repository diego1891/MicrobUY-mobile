using MicrobUY.ViewModels;

namespace MicrobUY.Views;

public partial class PostDetailPage : ContentPage
{
	public PostDetailPage(PostDetailViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}