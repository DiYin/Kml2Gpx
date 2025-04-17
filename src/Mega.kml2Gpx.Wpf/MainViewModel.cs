using System.Collections.ObjectModel;
using System.ComponentModel;
using System;
using System.Windows;
using System.Windows.Input;
using Mega.Kml2Gpx.Models;

namespace Mega.Kml2Gpx.Wpf;
public class MainViewModel : INotifyPropertyChanged
{
    private string _selectedFile;
    private string _selectedFolder;
    private CheckedItem _selectedConverterItem;
    private CheckedItem _selectedOutputFileItem;

    public MainViewModel()
    {
        BrowseFileCommand = new RelayCommand(_ => BrowseFile());
        SelectFolderCommand = new RelayCommand(_ => SelectFolder());
        ConvertCommand = new RelayCommand(_ => Convert());
        ExitCommand = new RelayCommand(_ => Exit());
        HelpCommand = new RelayCommand(_ => ShowHelp());

        // Initialize collections with sample data
        ConverterCheckedItems = new ObservableCollection<CheckedItem>
        {
            new CheckedItem { Index = 0, DisplayText = "Sharp Kml", IsChecked = true },
            new CheckedItem { Index = 1, DisplayText= "XDocument", IsChecked = false },
            new CheckedItem { Index = 2, DisplayText = "XmlDocument", IsChecked = false },
            new CheckedItem { Index = 3, DisplayText = "XPathDocument", IsChecked = false }
        };

        OutputFileCheckedItems = new ObservableCollection<CheckedItem>
        {
            new CheckedItem { Index = 0, DisplayText = "One File for the Kml/Kmz", IsChecked = false },
            new CheckedItem { Index = 1, DisplayText = "One File for Each Folder", IsChecked = false },
            new CheckedItem { Index = 2, DisplayText = "One File for Each Track", IsChecked = true },
        };
    }
    public string SelectedFile
    {
        get => _selectedFile;
        set
        {
            _selectedFile = value;
            OnPropertyChanged(nameof(SelectedFile));
        }
    }

    public string SelectedFolder 
    { 
        get => _selectedFolder; 
        set
        { 
            _selectedFolder = value;
            OnPropertyChanged(nameof(SelectedFolder));
        } 
    }
    public ConverterType ConverterType { get; set; } = ConverterType.SharpKml;
    public OutputMode OutputMode { get; set; } = OutputMode.OneFilePerTrack;
    public ICommand BrowseFileCommand { get; }
    public ICommand SelectFolderCommand { get; }
    public ICommand ConvertCommand { get; }
    public ICommand ExitCommand { get; }
    public ICommand HelpCommand { get; }
    public ObservableCollection<CheckedItem> ConverterCheckedItems { get; set; }
    public ObservableCollection<CheckedItem> OutputFileCheckedItems { get; set; }

    public CheckedItem SelectedConverterItem
    {
        get => _selectedConverterItem;
        set
        {
            _selectedConverterItem = value;
            OnPropertyChanged(nameof(SelectedConverterItem));
            OnConverterItemSelected(); // Handle selection logic
        }
    }

    public CheckedItem SelectedOutputFileItem
    {
        get => _selectedOutputFileItem;
        set
        {
            _selectedOutputFileItem = value;
            OnPropertyChanged(nameof(SelectedOutputFileItem));
            OnOutputFileItemSelected(); // Handle selection logic
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
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
        }
    }

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

    private void OnConverterItemSelected()
    {
        if (SelectedConverterItem != null)
        {
            ConverterType = (ConverterType)SelectedConverterItem.Index;
        }
    }

    private void OnOutputFileItemSelected()
    {
        if (SelectedOutputFileItem != null)
        {
            OutputMode = (OutputMode)SelectedConverterItem.Index;
        }
    }

    private async void Convert()
    {
        var message = await Kml2GpxConvert.Convert(SelectedFile, SelectedFolder, ConverterType, OutputMode);
        MessageBox.Show(message);
    }

    private void Exit()
    {
        Application.Current.Shutdown();
    }

    private void ShowHelp()
    {
        MessageBox.Show("Help Command Executed!");
    }
}

public class CheckedItem
{
    public int Index { get; set; }
    public string DisplayText { get; set; }
    public bool IsChecked { get; set; }
}


