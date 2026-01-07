using ProyectoInterfazNatural.Model;
using ProyectoInterfazNatural.ViewModels;

namespace ProyectoInterfazNatural.Views;

public partial class Buscar : ContentPage
{
	BuscarViewModel vm;
    User User;
    public Buscar(User user)
	{
		InitializeComponent();
		vm= new BuscarViewModel(user);
        User= user;
        BindingContext = vm;

		// Deshabilitar el botón de atrás
		NavigationPage.SetHasBackButton(this, false);
	}

	// Prevenir que el usuario use el botón de atrás del sistema (Android), esto es para que no haya lios con la navegación
	protected override bool OnBackButtonPressed()
	{
		// Retornar true para consumir el evento y evitar navegación hacia atrás
		return true;
	}

    private async void OnCasaTapped(object sender, EventArgs e)
    {
        // Navega a la nueva página
        await Navigation.PushAsync(new PaginaPrincipalView(User));
    }
}