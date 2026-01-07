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
		
		
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
		 Navigation.PushAsync(new RegistroView(vm));
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
		var user= await vm.LoginWithPassword();
        if (user != null)
            await Navigation.PushAsync(new PaginaPrincipalView(user));
    }

    private async void Button_Clicked_2(object sender, EventArgs e)
    {
		var user= await vm.LoginWithBiometric();
		if (user != null)
			await Navigation.PushAsync(new PaginaPrincipalView(user));

    }
}