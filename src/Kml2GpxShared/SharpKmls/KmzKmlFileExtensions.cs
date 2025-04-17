using Mega.Kml2Gpx.WinForms.App.Models;
using SharpKml.Engine;

namespace Mega.Kml2Gpx.SharpKmls;

/// <summary>
/// Extension methods for KmzKmlFile
/// </summary>
public static partial class KmzKmlFileExtensions
{
    /// <summary>
    /// Get SharpKml.Dom.Folder's collection from the kml file
    /// </summary>
    /// <param name="kmlFile"></param>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static List<SharpKml.Dom.Folder> GetFolders(this KmzKmlFile kmzKmlFile)
    {
        var folders = new List<SharpKml.Dom.Folder>();
        if (kmzKmlFile.KmlFile.Root is SharpKml.Dom.Kml kml)
        {
            folders.ExtractFolders(kml.Feature);
            if (folders.Count == 0)
            {
                var clonedFeature = kml.Feature.Clone();
                var folder = new SharpKml.Dom.Folder { Name = clonedFeature.Name };
                folder.AddFeature(clonedFeature);
                folders.Add(folder);
            }
        }

        return folders;
    }

    /// <summary>
    /// Get collection of GPX placemarks from the kml file for the specified folder
    /// </summary>
    /// <param name="folder"></param>
    /// <param name="kmlFile"></param>
    /// <returns></returns>
    public static List<GpxPlacemark> GetPlacemarks(this KmzKmlFile kmzKmlFile, SharpKml.Dom.Folder folder)
    {
        var kmlPlacemarks = new List<SharpKml.Dom.Placemark>();
        kmlPlacemarks.ExtractPlacemarks(folder);
        var placemarks = new List<GpxPlacemark>();
        foreach (var kmlPlacemark in kmlPlacemarks)
        {
            if (kmlPlacemark.Geometry is SharpKml.Dom.MultipleGeometry multipleGeometry)
            {
                var marks = multipleGeometry.GetGpxPlacemarks(kmlPlacemark.Name, kmlPlacemark.Description);
                if (marks != null) placemarks.AddRange(marks);
            }
            else
            {
                var packmark = kmzKmlFile.ToGpxPlacemark(kmlPlacemark);
                if (packmark != null) placemarks.Add(packmark);
            }
        }
        return placemarks;
    }

    /// <summary>
    /// Convert a SharpKml.Dom.Placemark from the kml file to a GpxPlacemark
    /// </summary>
    /// <param name="placemark"></param>
    /// <param name="style"></param>
    /// <returns></returns>
    private static GpxPlacemark ToGpxPlacemark(this KmzKmlFile kmzKmlFile, SharpKml.Dom.Placemark placemark)
    {
        var geometry = placemark.Geometry;
        if (geometry != null)
        {
            if (geometry is SharpKml.Dom.Point)
            {
                var point = geometry as SharpKml.Dom.Point;
                if (point != null) return point.ToWaypoint(placemark.Name, placemark.Description);
            }
            else if (geometry is SharpKml.Dom.LineString)
            {
                var lineString = geometry as SharpKml.Dom.LineString;
                if (lineString != null) return lineString.ToTrack(placemark.Name);
            }
            else if (geometry is SharpKml.Dom.LinearRing)
            {
                var linearRing = geometry as SharpKml.Dom.LinearRing;
                if (linearRing != null) return linearRing.ToTrack(placemark.Name);
            }
        }
        return null;
    }

    /// <summary>
    /// Extract Placemarks from the feature
    /// </summary>
    /// <param name="placemarks"></param>
    /// <param name="feature"></param>
    public static void ExtractPlacemarks(this IList<SharpKml.Dom.Placemark> placemarks, SharpKml.Dom.Feature feature)
    {
        // Is the passed in value a Placemark?
        if (feature is SharpKml.Dom.Placemark placemark)
        {
            placemarks.Add(placemark);
        }
        else
        {
            // Is it a Container, as the Container might have a child Placemark?
            if (feature is SharpKml.Dom.Container container)
            {
                // Check each Feature to see if it's a Placemark or another Container
                foreach (SharpKml.Dom.Feature f in container.Features)
                {
                    placemarks.ExtractPlacemarks(f);
                }
            }
        }
    }

    /// <summary>
    /// Extract Folders from the feature
    /// </summary>
    /// <param name="folders"></param>
    /// <param name="feature"></param>
    public static void ExtractFolders(this IList<SharpKml.Dom.Folder> folders, SharpKml.Dom.Feature feature)
    {
        // Is the passed in value a Placemark?
        if (feature is SharpKml.Dom.Folder folder)
        {
            folders.Add(folder);
        }
        else
        {
            // Is it a Container, as the Container might have a child Placemark?
            if (feature is SharpKml.Dom.Container container)
            {
                // Check each Feature to see if it's a Placemark or another Container
                foreach (SharpKml.Dom.Feature f in container.Features)
                {
                    folders.ExtractFolders(f);
                }
            }
        }
    }

    /// <summary>
    /// Convert a SharpKml.Dom.Point from the kml file to a Waypoint
    /// </summary>
    /// <param name="point"></param>
    /// <param name="name"></param>
    /// <param name="style"></param>
    /// <returns></returns>
    private static Waypoint ToWaypoint(this SharpKml.Dom.Point point, string name, SharpKml.Dom.Description description)
    {
        var jsonStyle = string.Empty;
        var waypoint = new Waypoint
        {
            Longitude = point.Coordinate.Longitude.ToString(),
            Latitude = point.Coordinate.Latitude.ToString(),
            Name = name,
            Description = description?.Text,
        };
        return waypoint;
    }

    /// <summary>
    /// Convert a SharpKml.Dom.LineString from the kml file to a Track
    /// </summary>
    /// <param name="lineString"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    private static Track ToTrack(this SharpKml.Dom.LineString lineString, string name)
    {
        return new Track
        {
            Points = lineString.Coordinates.Select(c => new GpxPoint
            {
                Longitude = c.Longitude.ToString(),
                Latitude = c.Latitude.ToString(),
            }).ToList(),
            Name = name,
        };
    }

    /// <summary>
    /// Convert a SharpKml.Dom.LinearRing from the kml file to a Track
    /// </summary>
    /// <param name="linearRing"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    private static Track ToTrack(this SharpKml.Dom.LinearRing linearRing, string name)
    {
        return new Track
        {
            Points = linearRing.Coordinates.Select(c => new GpxPoint
            {
                Longitude = c.Longitude.ToString(),
                Latitude = c.Latitude.ToString(),
            }).ToList(),
            Name = name,
        };
    }

    /// <summary>
    /// Gets the GPX placemarks from the multiple geometry
    /// </summary>
    /// <param name="multipleGeometry"></param>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    private static List<GpxPlacemark> GetGpxPlacemarks(this SharpKml.Dom.MultipleGeometry multipleGeometry, string name, SharpKml.Dom.Description description)
    {
        var gpxPlacemarks = new List<GpxPlacemark>();
        foreach (var geometry in multipleGeometry.Geometry)
        {
            if (geometry is SharpKml.Dom.Point point)
            {
                var waypoint = point.ToWaypoint(name, description);
                if (waypoint != null) gpxPlacemarks.Add(waypoint);
            }
            else if (geometry is SharpKml.Dom.LinearRing lineString)
            {
                var track = lineString.ToTrack(name);
                if (track != null) gpxPlacemarks.Add(track);
            }
            else if (geometry is SharpKml.Dom.LinearRing linearRing)
            {
                var track = linearRing.ToTrack(name);
                if (track != null) gpxPlacemarks.Add(track);
            }
            else if (geometry is SharpKml.Dom.MultipleGeometry subMultipleGeometry)
            {
                var subPlacemarks = subMultipleGeometry.GetGpxPlacemarks(name, description);
                if (subPlacemarks != null) gpxPlacemarks.AddRange(subPlacemarks);
            }
        }
        return gpxPlacemarks;
    }
}
