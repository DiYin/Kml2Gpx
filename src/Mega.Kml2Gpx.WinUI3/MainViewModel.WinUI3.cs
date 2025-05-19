using Mega.Kml2Gpx.Models;
using Mega.Kml2Gpx.WinUI3;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace Mega.Kml2Gpx;

public partial class MainViewModel 
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MainViewModel"/> class.
    /// </summary>
    public MainViewModel()
    {
        BrowseFileCommand = new RelayCommand(async _ => await BrowseFile());
        SelectFolderCommand = new RelayCommand(async _ => await SelectFolder());
        ConvertCommand = new RelayCommand(async _ => await Convert());
        ExitCommand = new RelayCommand(_ => Exit());
        HelpCommand = new RelayCommand(async _ => await ShowHelpAsync());
        InitilizeOptions();
    }

    /// <summary>
    /// Opens a file dialog to select a Kml/Kmz file.
    /// </summary>
    private async Task BrowseFile()
    {
        var filePicker = new FileOpenPicker();
        filePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

        // Add file type filters
        filePicker.FileTypeFilter.Add(".kml");
        filePicker.FileTypeFilter.Add(".kmz");

        // Initialize the picker with the current window's HWND
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
        WinRT.Interop.InitializeWithWindow.Initialize(filePicker, hwnd);

        var file = await filePicker.PickSingleFileAsync();
        if (file != null)
        {
            SelectedFile = file.Path;
            SelectedFolder = Helper.GetAvailableFolderPath(SelectedFile);
        }
    }

    /// <summary>
    /// Opens a folder dialog to select a folder for converted Gpx file(s).
    /// </summary>
    private async Task SelectFolder()
    {
        var folderPicker = new FolderPicker();
        folderPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

        // Initialize the picker with the current window's HWND  
        var hwnd = WindowNative.GetWindowHandle(App.MainWindow);
        InitializeWithWindow.Initialize(folderPicker, hwnd);

        var folder = await folderPicker.PickSingleFolderAsync();
        if (folder != null)
        {
            SelectedFolder = folder.Path;
        }
    }

    /// <summary>
    /// Executes the conversion process.
    /// </summary>
    private async Task Convert()
    {
        var converterType = (ConvertType)SelectedConvertTypeItem.Index;
        var outputMode = (OutputOption)SelectedOutputModeItem.Index;
        var message = await Kml2GpxConvert.Convert(SelectedFile, SelectedFolder, converterType, outputMode);
        await ShowCustomDialog(message, "Conversion Result");
    }

    /// <summary>
    /// Exits the application.
    /// </summary>
    private void Exit()
    {
        App.Current.Exit();
    }

    /// <summary>
    /// Displays help information.
    /// </summary>
    private async Task ShowHelpAsync()
    {
        await ShowCustomDialog("Help Command Executed!", "Kml2Gpx");
    }

    /// <summary>
    /// Displays a custom dialog.
    /// </summary>
    private async Task ShowCustomDialog(string message, string title)
    {
        var dialog = new ContentDialog
        {
            Title = title,
            Content = message,
            PrimaryButtonText = "OK",
            XamlRoot = App.MainWindow.Content.XamlRoot // Replace with the appropriate XamlRoot
        };

        // Handle the result of the dialog
        var result = await dialog.ShowAsync();
    }
}
