using System;
using System.IO;
using AnimeX.ViewModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;

namespace AnimeX.View;

public partial class CreateAnimeView : UserControl
{
    public CreateAnimeView()
    {
        InitializeComponent();
    }
    private async void AgregarImagenButton_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            var dlg = new OpenFileDialog();
            dlg.Filters!.Add(new FileDialogFilter() { Name = "Imágenes", Extensions = { "jpg", "png", "jpeg", "gif" } });
            dlg.AllowMultiple = false;
        
            var result = await dlg.ShowAsync((Window)this.VisualRoot);
            if (result != null && result.Length > 0)
            {
                // Leer la imagen desde el archivo seleccionado
                using (var stream = File.OpenRead(result[0]))
                {
                    var bitmap = new Bitmap(stream);
                    // Asignar la imagen al Image control en la vista
                    if (DataContext is CreateAnimeViewModel viewModel)
                    {
                        // Asignar la imagen al ViewModel
                        viewModel.Imagen = bitmap;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Manejar cualquier error que pueda ocurrir durante la selección de la imagen
            Console.WriteLine($"Error al agregar la imagen: {ex.Message}");
        }
    }

}