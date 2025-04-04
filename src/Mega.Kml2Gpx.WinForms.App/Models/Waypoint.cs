namespace Mega.Kml2Gpx.WinForms.App.Models;

/// <summary>
/// Waypoint in gpx file
/// </summary>
public class Waypoint : GpxPlacemark
{
    public Waypoint()
    {
        GpxType = GpxPlacemarkType.WayPoint;
    }

    public string Latitude { get; set; }
    public string Longitude { get; set; }
    public string Elevation { get; set; }
    public string Description { get; set; }
}
