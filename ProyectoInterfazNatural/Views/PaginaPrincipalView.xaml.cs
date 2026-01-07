using ProyectoInterfazNatural.Model;
using ProyectoInterfazNatural.ViewModels;

namespace ProyectoInterfazNatural.Views;

public partial class PaginaPrincipalView : ContentPage
{
	PaginaPrincipalViewModel vm;
    User User;
    public PaginaPrincipalView(User user)
	{
		InitializeComponent();
        User= user;
        vm = new PaginaPrincipalViewModel(User,Navigation);
		BindingContext = vm;

    }
    private async void OnBusquedaTapped(object sender, EventArgs e)
    {
        // Navega a la nueva página
        await Navigation.PushAsync(new Buscar(User));
    }

    /*Anteriormente estabamos intentando hacer un escaner, por si en un futuro 
     queremos añadir el escaner*/
    private async void Button_Clicked(object sender, EventArgs e)
    {
        var books = new BuscarViewModel(User).Books.ToList();
        await Navigation.PushAsync(new EscanerView(User, books));
    }
}