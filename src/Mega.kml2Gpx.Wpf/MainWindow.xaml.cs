using System.Diagnostics;
using System.Windows;

namespace Mega.Kml2Gpx.Wpf;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        var messageBoxService = new MessageBoxService(this);
        DataContext = new MainViewModel(messageBoxService);
    }
}