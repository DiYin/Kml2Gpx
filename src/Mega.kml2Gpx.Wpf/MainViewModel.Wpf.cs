using Mega.Kml2Gpx.Models;
using Mega.Kml2Gpx.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Mega.Kml2Gpx;

public partial class MainViewModel 
{
    private readonly IMessageBoxService _messageBoxService;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainViewModel"/> class.
    /// </summary>
    public MainViewModel(IMessageBoxService messageBoxService)
    {
        _messageBoxService = messageBoxService;
        BrowseFileCommand = new RelayCommand(_ => BrowseFile());
        SelectFolderCommand = new RelayCommand(_ => SelectFolder());
        ConvertCommand = new RelayCommand(_ => Convert());
        ExitCommand = new RelayCommand(_ => Exit());
        HelpCommand = new RelayCommand(_ => ShowHelp());
        InitilizeOptions();
    }

    /// <summary>
    /// Opens a file dialog to select a Kml/Kmz file.
    /// </summary>
    private void BrowseFile()
    {
        var openFileDialog = new Microsoft.Win32.OpenFileDialog
        {
            Title = "Select a KML or KMZ File",
            Filter = "KML Files (*.kml)|*.kml|KMZ Files (*.kmz)|*.kmz|All Files (*.*)|*.*",
            DefaultExt = ".kml"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            SelectedFile = openFileDialog.FileName;
            SelectedFolder = Helper.GetAvailableFolderPath(SelectedFile);
        }
    }

    /// <summary>
    /// Opens a folder dialog to select a folder for converted Gpx file(s).
    /// </summary>
    private void SelectFolder()
    {
        var openFoderDialog = new Microsoft.Win32.OpenFolderDialog
        {
            Title = "Select a Folder for Converted Gpx File(s)",
            DefaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
        };

        if (openFoderDialog.ShowDialog() == true)
        {
            SelectedFolder = openFoderDialog.FolderName;
        }
    }

    /// <summary>
    /// Executes the conversion process.
    /// </summary>
    private async void Convert()
    {
        var converterType = (ConvertType)SelectedConvertTypeItem.Index;
        var outputMode = (OutputOption)SelectedOutputModeItem.Index;
        var message = await Kml2GpxConvert.Convert(SelectedFile, SelectedFolder, converterType, outputMode);
        _messageBoxService.Show(message, "Conversion Result");
    }

    /// <summary>
    /// Exits the application.
    /// </summary>
    private void Exit()
    {
        Application.Current.Shutdown();
    }

    /// <summary>
    /// Displays help information.
    /// </summary>
    private void ShowHelp()
    {
        MessageBox.Show("Help Command Executed!");
    }

}
