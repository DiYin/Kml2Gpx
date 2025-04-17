using System.Windows;

namespace Mega.Kml2Gpx.Wpf;

public partial class App : Application
{
    public App()
    {
    }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        var mainWindow = new MainWindow();
        mainWindow.Show();
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {

    }
}
