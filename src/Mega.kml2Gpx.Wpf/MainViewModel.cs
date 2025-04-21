using System.Collections.ObjectModel;
using System.ComponentModel;
using System;
using System.Windows;
using System.Windows.Input;
using Mega.Kml2Gpx.Models;
using System.IO;
using System.Windows.Interop;

namespace Mega.Kml2Gpx.Wpf;

/// <summary>
/// ViewModel for the Main Window.
/// </summary>
public class MainViewModel : INotifyPropertyChanged
{
    private readonly IMessageBoxService _messageBoxService;
    private string _selectedFile;
    private string _selectedFolder;
    private CheckedItem _selectedConvertTypeItem;
    private CheckedItem _selectedOutputOptionItem;

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

        // Initialize collections for convert types
        ConvertTypeItems = new ObservableCollection<CheckedItem>
        {
            new CheckedItem { Index = 0, DisplayText = "Sharp Kml", IsChecked = true },
            new CheckedItem { Index = 1, DisplayText= "XDocument", IsChecked = false },
            new CheckedItem { Index = 2, DisplayText = "XmlDocument", IsChecked = false },
            new CheckedItem { Index = 3, DisplayText = "XPathDocument", IsChecked = false }
        };
        SelectedConvertTypeItem = ConvertTypeItems[0]; // Default selection

        // Initialize collections for output modes
        OutputOptionItems = new ObservableCollection<CheckedItem>
        {
            new CheckedItem { Index = 0, DisplayText = "One File for the Kml/Kmz", IsChecked = false },
            new CheckedItem { Index = 1, DisplayText = "One File for Each Folder", IsChecked = false },
            new CheckedItem { Index = 2, DisplayText = "One File for Each Track", IsChecked = true }
        };
        SelectedOutputModeItem = OutputOptionItems[2]; // Default selection

        // Subscribe to IsCheckedChanged events
        foreach (var item in ConvertTypeItems)
        {
            item.IsCheckedChanged += (s, e) =>
            {
                if (((CheckedItem)s).IsChecked)
                {
                    SelectedConvertTypeItem = (CheckedItem)s;
                }
            };
        }

        foreach (var item in OutputOptionItems)
        {
            item.IsCheckedChanged += (s, e) =>
            {
                if (((CheckedItem)s).IsChecked)
                {
                    SelectedOutputModeItem = (CheckedItem)s;
                }
            };
        }
    }

    /// <summary>
    /// Gets or sets the selected file path.
    /// </summary>
    public string SelectedFile
    {
        get => _selectedFile;
        set
        {
            _selectedFile = value;
            OnPropertyChanged(nameof(SelectedFile));
        }
    }

    /// <summary>
    /// Gets or sets the selected folder path.
    /// </summary>
    public string SelectedFolder 
    { 
        get => _selectedFolder; 
        set
        { 
            _selectedFolder = value;
            OnPropertyChanged(nameof(SelectedFolder));
        } 
    }

    /// <summary>
    /// Command to browse for a Kml/Kmz file for the convert.
    /// </summary>
    public ICommand BrowseFileCommand { get; }

    /// <summary>
    /// Command to select a folder for the converted Gpx file(s).
    /// </summary>
    public ICommand SelectFolderCommand { get; }

    /// <summary>
    /// Command to convert the selected Kml/Kmz file to Gpx.
    /// </summary>
    public ICommand ConvertCommand { get; }

    /// <summary>
    /// Command to exit the application.
    /// </summary>
    public ICommand ExitCommand { get; }

    /// <summary>
    /// Command to show help information.
    /// </summary>
    public ICommand HelpCommand { get; }

    /// <summary>
    /// Collection of checked items for converter types.
    /// </summary>
    public ObservableCollection<CheckedItem> ConvertTypeItems { get; set; }

    /// <summary>
    /// Collection of checked items for output file options.
    /// </summary>
    public ObservableCollection<CheckedItem> OutputOptionItems { get; set; }

    /// <summary>
    /// Gets or sets the selected converter item.
    /// </summary>
    public CheckedItem SelectedConvertTypeItem
    {
        get => _selectedConvertTypeItem;
        set
        {
            _selectedConvertTypeItem = value;
            OnPropertyChanged(nameof(SelectedConvertTypeItem));
            UpdateCheckedState(ConvertTypeItems, _selectedConvertTypeItem);
        }
    }

    /// <summary>
    /// Gets or sets the selected output file item.
    /// </summary>
    public CheckedItem SelectedOutputModeItem
    {
        get => _selectedOutputOptionItem;
        set
        {
            _selectedOutputOptionItem = value;
            OnPropertyChanged(nameof(SelectedOutputModeItem));
            UpdateCheckedState(OutputOptionItems, _selectedOutputOptionItem);
        }
    }

    /// <summary>
    /// Event raised when a property changes.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Updates the checked state of the items in the collection based on the selected item.
    /// </summary>
    /// <param name="items"></param>
    /// <param name="selectedItem"></param>
    private void UpdateCheckedState(ObservableCollection<CheckedItem> items, CheckedItem selectedItem)
    {
        foreach (var item in items)
        {
            item.IsChecked = item == selectedItem;
        }
    }

    /// <summary>
    /// Raises the PropertyChanged event for the specified property.
    /// </summary>
    /// <param name="propertyName"></param>
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
            if (string.IsNullOrEmpty(SelectedFolder))
            {
                SelectedFolder = Path.GetDirectoryName(SelectedFile);
            }
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
        var converterType = (ConvertType) SelectedConvertTypeItem.Index;
        var outputMode = (OutputOption) SelectedOutputModeItem.Index;
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
