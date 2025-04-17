namespace Mega.Kml2Gpx.Models;

/// <summary>
/// Class represents a folder in KML file.   
/// </summary>
public class KmlFolder
{
    public KmlFolder()
    {
        Placemarks = new List<GpxPlacemark>();
    }

    public string Name;
    public List<GpxPlacemark> Placemarks;
    public string Color;
}