using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega.Kml2Gpx.Models;

/// <summary>
/// Represents a checked item in a list.
/// </summary>
public class CheckedItem : INotifyPropertyChanged
{
    private bool _isChecked;

    public int Index { get; set; }
    public string DisplayText { get; set; }

    public bool IsChecked
    {
        get => _isChecked;
        set
        {
            if (_isChecked != value)
            {
                _isChecked = value;
                OnPropertyChanged(nameof(IsChecked));
                IsCheckedChanged?.Invoke(this, EventArgs.Empty); // Notify ViewModel
            }
        }
    }

    public event EventHandler IsCheckedChanged; // Event to notify the ViewModel

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

