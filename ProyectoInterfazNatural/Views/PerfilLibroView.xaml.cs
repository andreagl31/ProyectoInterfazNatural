using ProyectoInterfazNatural.Model;
using ProyectoInterfazNatural.ViewModels;

namespace ProyectoInterfazNatural.Views;

public partial class PerfilLibroView : ContentPage
{
	PerfilLibroViewModel vm;
    User User;
	public PerfilLibroView(User user, Book book)
	{
		vm= new PerfilLibroViewModel(user,book);
        User= user;
        InitializeComponent();
		BindingContext = vm;
	}
    private async void OnBusquedaTapped(object sender, EventArgs e)
    {
        // Navega a la nueva página
        await Navigation.PushAsync(new Buscar(User));
    }
    private async void OnCasaTapped(object sender, EventArgs e)
    {
        // Navega a la nueva página
        await Navigation.PushAsync(new PaginaPrincipalView(User));
    }
}