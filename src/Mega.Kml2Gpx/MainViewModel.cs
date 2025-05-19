//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System;
//using System.Windows;
//using System.Windows.Input;
//using Mega.Kml2Gpx.Models;
//using System.IO;
//using Windows.UI.Popups;
//using Windows.Storage.Pickers;
//using WinRT.Interop;
//using Microsoft.UI.Xaml.Controls;
//using System.Threading.Tasks;

//namespace Mega.Kml2Gpx.WinUI3;

///// <summary>
///// ViewModel for the Main Window.
///// </summary>
//public class MainViewModel : INotifyPropertyChanged
//{
//    private string _selectedFile;
//    private string _selectedFolder;
//    private CheckedItem _selectedConvertTypeItem;
//    private CheckedItem _selectedOutputOptionItem;

//    /// <summary>
//    /// Initializes a new instance of the <see cref="MainViewModel"/> class.
//    /// </summary>
//    public MainViewModel()
//    {
//        BrowseFileCommand = new RelayCommand(_ => BrowseFile());
//        SelectFolderCommand = new RelayCommand(_ => SelectFolder());
//        ConvertCommand = new RelayCommand(async _ => await Convert());
//        ExitCommand = new RelayCommand(_ => Exit());
//        HelpCommand = new RelayCommand(_ => ShowHelp());

//        // Initialize collections for convert types
//        ConvertTypeItems = new ObservableCollection<CheckedItem>
//        {
//            new CheckedItem { Index = 0, DisplayText = "Sharp Kml", IsChecked = true },
//            new CheckedItem { Index = 1, DisplayText= "XDocument", IsChecked = false },
//            new CheckedItem { Index = 2, DisplayText = "XmlDocument", IsChecked = false },
//            new CheckedItem { Index = 3, DisplayText = "XPathDocument", IsChecked = false }
//        };
//        SelectedConvertTypeItem = ConvertTypeItems[0]; // Default selection

//        // Initialize collections for output modes
//        OutputOptionItems = new ObservableCollection<CheckedItem>
//        {
//            new CheckedItem { Index = 0, DisplayText = "One File for the Kml/Kmz", IsChecked = false },
//            new CheckedItem { Index = 1, DisplayText = "One File for Each Folder", IsChecked = false },
//            new CheckedItem { Index = 2, DisplayText = "One File for Each Track", IsChecked = true }
//        };
//        SelectedOutputModeItem = OutputOptionItems[2]; // Default selection

//        // Subscribe to IsCheckedChanged events
//        foreach (var item in ConvertTypeItems)
//        {
//            item.IsCheckedChanged += (s, e) =>
//            {
//                if (((CheckedItem)s).IsChecked)
//                {
//                    SelectedConvertTypeItem = (CheckedItem)s;
//                }
//            };
//        }

//        foreach (var item in OutputOptionItems)
//        {
//            item.IsCheckedChanged += (s, e) =>
//            {
//                if (((CheckedItem)s).IsChecked)
//                {
//                    SelectedOutputModeItem = (CheckedItem)s;
//                }
//            };
//        }
//    }

//    /// <summary>
//    /// Gets or sets the selected file path.
//    /// </summary>
//    public string SelectedFile
//    {
//        get => _selectedFile;
//        set
//        {
//            _selectedFile = value;
//            OnPropertyChanged(nameof(SelectedFile));
//        }
//    }

//    /// <summary>
//    /// Gets or sets the selected folder path.
//    /// </summary>
//    public string SelectedFolder 
//    { 
//        get => _selectedFolder; 
//        set
//        { 
//            _selectedFolder = value;
//            OnPropertyChanged(nameof(SelectedFolder));
//        } 
//    }

//    /// <summary>
//    /// Command to browse for a Kml/Kmz file for the convert.
//    /// </summary>
//    public ICommand BrowseFileCommand { get; }

//    /// <summary>
//    /// Command to select a folder for the converted Gpx file(s).
//    /// </summary>
//    public ICommand SelectFolderCommand { get; }

//    /// <summary>
//    /// Command to convert the selected Kml/Kmz file to Gpx.
//    /// </summary>
//    public ICommand ConvertCommand { get; }

//    /// <summary>
//    /// Command to exit the application.
//    /// </summary>
//    public ICommand ExitCommand { get; }

//    /// <summary>
//    /// Command to show help information.
//    /// </summary>
//    public ICommand HelpCommand { get; }

//    /// <summary>
//    /// Collection of checked items for converter types.
//    /// </summary>
//    public ObservableCollection<CheckedItem> ConvertTypeItems { get; set; }

//    /// <summary>
//    /// Collection of checked items for output file options.
//    /// </summary>
//    public ObservableCollection<CheckedItem> OutputOptionItems { get; set; }

//    /// <summary>
//    /// Gets or sets the selected converter item.
//    /// </summary>
//    public CheckedItem SelectedConvertTypeItem
//    {
//        get => _selectedConvertTypeItem;
//        set
//        {
//            _selectedConvertTypeItem = value;
//            OnPropertyChanged(nameof(SelectedConvertTypeItem));
//            UpdateCheckedState(ConvertTypeItems, _selectedConvertTypeItem);
//        }
//    }

//    /// <summary>
//    /// Gets or sets the selected output file item.
//    /// </summary>
//    public CheckedItem SelectedOutputModeItem
//    {
//        get => _selectedOutputOptionItem;
//        set
//        {
//            _selectedOutputOptionItem = value;
//            OnPropertyChanged(nameof(SelectedOutputModeItem));
//            UpdateCheckedState(OutputOptionItems, _selectedOutputOptionItem);
//        }
//    }

//    /// <summary>
//    /// Event raised when a property changes.
//    /// </summary>
//    public event PropertyChangedEventHandler PropertyChanged;

//    /// <summary>
//    /// Updates the checked state of the items in the collection based on the selected item.
//    /// </summary>
//    /// <param name="items"></param>
//    /// <param name="selectedItem"></param>
//    private void UpdateCheckedState(ObservableCollection<CheckedItem> items, CheckedItem selectedItem)
//    {
//        foreach (var item in items)
//        {
//            item.IsChecked = item == selectedItem;
//        }
//    }

//    /// <summary>
//    /// Raises the PropertyChanged event for the specified property.
//    /// </summary>
//    /// <param name="propertyName"></param>
//    protected virtual void OnPropertyChanged(string propertyName)
//    {
//        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//    }

//    /// <summary>
//    /// Opens a file picker to select a Kml/Kmz file.
//    /// </summary>
//    private async void BrowseFile()
//    {
//        var filePicker = new FileOpenPicker();
//        filePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

//        // Add file type filters
//        filePicker.FileTypeFilter.Add(".kml");
//        filePicker.FileTypeFilter.Add(".kmz");

//        // Initialize the picker with the current window's HWND
//        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
//        WinRT.Interop.InitializeWithWindow.Initialize(filePicker, hwnd);

//        var file = await filePicker.PickSingleFileAsync();
//        if (file != null)
//        {
//            SelectedFile = file.Path;
//            SelectedFolder = Helper.GetAvailableFolderPath(file.Path);
//        }
//    }

//    /// <summary>
//    /// Opens a folder picker to select a folder for converted Gpx file(s).
//    /// </summary>
//    private async void SelectFolder()
//    {
//        var folderPicker = new FolderPicker();
//        folderPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

//        // Initialize the picker with the current window's HWND  
//        var hwnd = WindowNative.GetWindowHandle(App.MainWindow);
//        InitializeWithWindow.Initialize(folderPicker, hwnd);

//        var folder = await folderPicker.PickSingleFolderAsync();
//        if (folder != null)
//        {
//            SelectedFolder = folder.Path;
//        }
//    }

//    /// <summary>
//    /// Executes the conversion process.
//    /// </summary>
//    private async Task Convert()
//    {
//        var converterType = (ConvertType) SelectedConvertTypeItem.Index;
//        var outputMode = (OutputOption) SelectedOutputModeItem.Index;
//        var message = await Kml2GpxConvert.Convert(SelectedFile, SelectedFolder, converterType, outputMode);
//        await ShowCustomDialog(message, "Conversion Result");
//    }

//    /// <summary>
//    /// Exits the application.
//    /// </summary>
//    private void Exit()
//    {
//        App.Current.Exit();
//    }

//    /// <summary>
//    /// Displays help information.
//    /// </summary>
//    private async void ShowHelp()
//    {
//        await ShowCustomDialog("Help Command Executed!", "Kml2Gpx");
//    }

//    /// <summary>
//    /// Displays a custom dialog.
//    /// </summary>
//    private async Task ShowCustomDialog(string message, string title)
//    {
//        var dialog = new ContentDialog
//        {
//            Title = title,
//            Content = message,
//            PrimaryButtonText = "OK",
//            XamlRoot = App.MainWindow.Content.XamlRoot // Replace with the appropriate XamlRoot
//        };

//        // Handle the result of the dialog
//        var result = await dialog.ShowAsync();
//    }
//}
