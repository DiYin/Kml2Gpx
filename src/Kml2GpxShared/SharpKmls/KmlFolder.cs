using SharpKml.Dom;
using SharpKml.Engine;

namespace Mega.Kml2Gpx.SharpKmls;

/// <summary>
/// Class represents a folder in KML file.
/// </summary>
public class KmlFolder : Element
{
    public string Name { get; set; }
    public List<Placemark> Placemarks { get; }

    public bool ContainsRoute => Placemarks.Any(x => x.Geometry is LinearRing || x.Geometry is LineString);

    /// <summary>
    /// Constructor to create a new instance of KmlFolder with specified name.
    /// </summary>
    /// <param name="name"></param>
    public KmlFolder(string name) : this(name, new List<Placemark>())
    {
    }

    /// <summary>
    ///  Constructor to create a new instance of KmlFolder with specified placemarker collection.   
    /// </summary>
    /// <param name="placemarks"></param>
    public KmlFolder(IEnumerable<Placemark> placemarks) : this(null, placemarks)
    {
    }

    /// <summary>
    /// Constructor to create a new instance of KmlFolder with specified name and placemarker collection.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="placemarks"></param>
    public KmlFolder(string name, IEnumerable<Placemark> placemarks)
    {
        Name = name;
        Placemarks = placemarks.ToList();
    }

    /// <summary>
    /// Clones the folder excluding the specified elements.
    /// </summary>
    /// <param name="elementsToExclude"></param>
    /// <returns></returns>
    public KmlFolder CloneWithExcluding(Element[] elementsToExclude)
    {
        return new KmlFolder(Name,
            Placemarks
                .Where(p => !elementsToExclude.Contains(p))
                .Select(p => p.Clone()));
    }
}
