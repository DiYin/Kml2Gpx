using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace Mega.Kml2Gpx.Wpf.App;



public class CheckedListBox : ListBox
{
    // Define a dependency property for binding the items
    public ObservableCollection<ItemViewModel> BoundItems
    {
        get { return (ObservableCollection<ItemViewModel>)GetValue(BoundItemsProperty); }
        set { SetValue(BoundItemsProperty, value); }
    }

    public static readonly DependencyProperty BoundItemsProperty =
        DependencyProperty.Register("BoundItems", typeof(ObservableCollection<ItemViewModel>), typeof(CheckedListBox),
            new PropertyMetadata(null));
}


public class ItemViewModel
{
    public string Name { get; set; }
    public bool IsChecked { get; set; }
}
