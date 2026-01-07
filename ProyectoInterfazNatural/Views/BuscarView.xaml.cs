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
        
	}
    private async void OnCasaTapped(object sender, EventArgs e)
    {
        // Navega a la nueva página
        await Navigation.PushAsync(new PaginaPrincipalView(User));
    }

}