using System.Windows;
using System.Windows.Interop;

namespace Mega.Kml2Gpx.Wpf;

/// <summary>
/// Interface for message box service.
/// </summary>
public interface IMessageBoxService
{
    void Show(string message, string caption = "Information");
}

/// <summary>
/// Implementation of message box service.
/// </summary>
public class MessageBoxService : IMessageBoxService
{
    private readonly Window _owner;

    public MessageBoxService(Window owner)
    {
        _owner = owner;
    }

    public void Show(string message, string caption = "Information")
    {
        var customMessageBox = new CustomMessageBox(message, caption)
        {
            Owner = _owner // Set the owner to center the message box relative to the main window
        };
        customMessageBox.ShowDialog();
    }
}
