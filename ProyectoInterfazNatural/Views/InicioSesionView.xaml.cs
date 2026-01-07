using ProyectoInterfazNatural.Model;
using ProyectoInterfazNatural.Services;
using ProyectoInterfazNatural.ViewModels;
using System.Threading.Tasks;

namespace ProyectoInterfazNatural.Views;

public partial class InicioSesionView : ContentPage
{
	InicioSesionViewModel vm;
	
	public InicioSesionView()
	{
		InitializeComponent();
		var deviceService= Application.Current?.Handler?.MauiContext?.Services.GetService<IDeviceService>();
		vm= new InicioSesionViewModel(deviceService);
		BindingContext = vm;
		
		// Deshabilitar el botón de atrás
		NavigationPage.SetHasBackButton(this, false);
	}

	// Prevenir que el usuario use el botón de atrás del sistema (Android)
	protected override bool OnBackButtonPressed()
	{
		// Retornar true para consumir el evento y evitar que haga algo
		return true;
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		// Navegar a registro de forma normal
		await Navigation.PushAsync(new RegistroView(vm));
    }
}