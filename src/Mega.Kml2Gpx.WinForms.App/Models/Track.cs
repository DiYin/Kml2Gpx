namespace Mega.Kml2Gpx.WinForms.App.Models;

/// <summary>
/// Track in gpx file
/// </summary>
public class Track : GpxPlacemark
{
    public Track()
    {
        GpxType = GpxPlacemarkType.Track;
    }
    public List<GpxPoint> Points { get; set; }
}

/// <summary>
/// Point in gpx file
/// </summary>
public class GpxPlacemark
{
    public string Name { get; set; }
    public GpxPlacemarkType GpxType { get; set; }
}