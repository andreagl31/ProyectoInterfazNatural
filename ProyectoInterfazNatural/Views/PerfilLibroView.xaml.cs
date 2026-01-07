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

		// Deshabilitar el botón de atrás
		NavigationPage.SetHasBackButton(this, false);
	}

    // Prevenir que el usuario use el botón de atrás del sistema (Android), para q no haya lios con la navegación
    protected override bool OnBackButtonPressed()
	{
		// Retornar true para consumir el evento y evitar navegación hacia atrás
		return true;
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