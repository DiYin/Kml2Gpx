using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Mega.Kml2Gpx.WinUI3;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // Set the DataContext on the root element of your XAML, not on the Window itself.
        if (this.Content is FrameworkElement rootElement)
        {
            rootElement.DataContext = new MainViewModel();
        }

        // Set the desired width and height using the AppWindow property
        this.AppWindow.Resize(new Windows.Graphics.SizeInt32(650, 420));
    }
}

