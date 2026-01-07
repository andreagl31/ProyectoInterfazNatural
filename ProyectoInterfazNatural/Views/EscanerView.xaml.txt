using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Camera.MAUI;
using Camera.MAUI.ZXing;
using Camera.MAUI.ZXingHelper;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Dispatching;
using ProyectoInterfazNatural.Model;

namespace ProyectoInterfazNatural.Views;

public partial class EscanerView : ContentPage
{
    private readonly User _user;
    private readonly List<Book> _books;
    private int _detectedFlag = 0;

    public EscanerView(User user, List<Book> books)
    {
        InitializeComponent();
        _user = user;
        _books = books;

        cameraView.CamerasLoaded += CameraView_CamerasLoaded;
        cameraView.BarcodeDetected += CameraView_BarcodeDetected;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var status = await Permissions.RequestAsync<Permissions.Camera>();
        if (status != PermissionStatus.Granted)
        {
            await DisplayAlert("Permiso", "Se necesita permiso de cámara", "OK");
            return;
        }

        try
        {
            if (cameraView.NumCamerasDetected > 0)
            {
                cameraView.Camera = cameraView.Cameras.FirstOrDefault(c => c.Position == CameraPosition.Back);

                // Configuración ZXing
                cameraView.BarCodeDecoder = new ZXingBarcodeDecoder();
                cameraView.BarCodeOptions = new BarcodeDecodeOptions
                {
                    AutoRotate = true,
                    TryHarder = true,
                    ReadMultipleCodes = false,
                    PossibleFormats =
                    {
                        Camera.MAUI.BarcodeFormat.EAN_13,
                        Camera.MAUI.BarcodeFormat.EAN_8,
                        Camera.MAUI.BarcodeFormat.CODE_128
                    }
                };
                cameraView.BarCodeDetectionFrameRate = 1;
                cameraView.BarCodeDetectionMaxThreads = 2;
                cameraView.ControlBarcodeResultDuplicate = true;
                cameraView.BarCodeDetectionEnabled = true;

                // Torch y zoom
                if (cameraView.Camera.HasFlashUnit) cameraView.TorchEnabled = true;
                if (cameraView.MaxZoomFactor >= 2.0f) cameraView.ZoomFactor = 1.5f;

                await Task.Delay(200);
                var result = await cameraView.StartCameraAsync(new Size(1280, 720));
                Debug.WriteLine($"StartCameraAsync: {result}");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error OnAppearing: {ex}");
        }
    }

    protected override async void OnDisappearing()
    {
        try
        {
            cameraView.BarcodeDetected -= CameraView_BarcodeDetected;
            cameraView.CamerasLoaded -= CameraView_CamerasLoaded;

            _ = Task.Run(async () =>
            {
                try { await cameraView.StopCameraAsync(); }
                catch (Exception ex) { Debug.WriteLine($"StopCameraAsync fallo: {ex}"); }
            });
        }
        catch { }

        base.OnDisappearing();
    }

    private async void CameraView_BarcodeDetected(object sender, BarcodeEventArgs args)
    {
        if (Interlocked.Exchange(ref _detectedFlag, 1) == 1) return;

        try
        {
            if (args?.Result == null || args.Result.Length == 0)
            {
                Interlocked.Exchange(ref _detectedFlag, 0);
                return;
            }

            string isbn = args.Result[0].Text?.Trim();
            if (string.IsNullOrWhiteSpace(isbn))
            {
                Interlocked.Exchange(ref _detectedFlag, 0);
                return;
            }

            Debug.WriteLine($"ISBN detectado: {isbn}");

            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                var libro = _books.FirstOrDefault(b =>
                    string.Equals(b.IBSN?.Trim(), isbn, StringComparison.OrdinalIgnoreCase));

                // Detener temporalmente la detección
                cameraView.BarCodeDetectionEnabled = false;
                cameraView.BarcodeDetected -= CameraView_BarcodeDetected;

                if (libro != null && Navigation != null)
                {
                    await Navigation.PushAsync(new PerfilLibroView(_user, libro));
                }
                else
                {
                    await DisplayAlert("No encontrado", $"ISBN detectado: {isbn}", "OK");
                    cameraView.BarCodeDetectionEnabled = true;
                    cameraView.BarcodeDetected += CameraView_BarcodeDetected;
                    Interlocked.Exchange(ref _detectedFlag, 0);
                }
            });
        }
        catch
        {
            Interlocked.Exchange(ref _detectedFlag, 0);
        }
    }

    private void CameraView_CamerasLoaded(object sender, EventArgs e)
    {
        Debug.WriteLine($"Cámaras cargadas: {cameraView.NumCamerasDetected}");
    }
}
