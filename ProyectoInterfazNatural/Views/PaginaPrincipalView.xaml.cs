using ProyectoInterfazNatural.Model;
using ProyectoInterfazNatural.ViewModels;

namespace ProyectoInterfazNatural.Views;

public partial class PaginaPrincipalView : ContentPage
{
	PaginaPrincipalViewModel vm;
    User User;

    // Deshabilitar el botón de atrás   

    // Prevenir que el usuario use el botón de atrás del sistema (Android), para q no haya lios con la navegación
    protected override bool OnBackButtonPressed()
    {
        // Retornar true para consumir el evento y evitar navegación hacia atrás
        return true;
    }
    public PaginaPrincipalView(User user)
	{
		InitializeComponent();
        User= user;
        vm = new PaginaPrincipalViewModel(User,Navigation);
		BindingContext = vm;
        NavigationPage.SetHasBackButton(this, false);

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