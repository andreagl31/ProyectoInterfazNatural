using ProyectoInterfazNatural.Services;
using ProyectoInterfazNatural.ViewModels;

namespace ProyectoInterfazNatural.Views;

public partial class RegistroView : ContentPage
{
	public RegistroView(InicioSesionViewModel vm)
	{
		InitializeComponent();
		var deviceService = Application.Current?.Handler?.MauiContext?.Services.GetService<IDeviceService>();
        BindingContext = new RegistroViewModel(vm,deviceService);
	}
}