using System.Windows;

namespace Mega.Kml2Gpx.Wpf;
public partial class CustomMessageBox : Window
{
    public CustomMessageBox(string message, string caption)
    {
        InitializeComponent();
        Title = caption;
        MessageTextBlock.Text = message;
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
    }
}

