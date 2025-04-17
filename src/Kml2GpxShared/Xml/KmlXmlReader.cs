using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Xml;

namespace Mega.Kml2Gpx.Xml;

/// <summary>
/// Class for reading KML file by use XmlDocument
/// </summary>
internal static class KmlXmlReader
{
    /// <summary>
    /// Read KML file and return list of KmlFolders
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public static List<KmlFolder> ReadFile(string filename)
    {
        var kmlFolders = new List<KmlFolder>();
        var xmlDoc = GetXmlDocument(filename);
        if (xmlDoc == null) return kmlFolders;

        XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
        nsmgr.AddNamespace("kml", "http://www.opengis.net/kml/2.2");
        var list = xmlDoc.SelectNodes("//kml:Folder", nsmgr);
        foreach (XmlNode node in list)
        {
            var kmlFolder = ProcessFolder(node, nsmgr, "Untitled");
            kmlFolders.Add(kmlFolder);
        }
        return kmlFolders;
    }

    /// <summary>
    /// Get XmlDocument from file
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    private static XmlDocument GetXmlDocument(string filePath)
    {
        var ext = Path.GetExtension(filePath);
        XmlDocument xmlDoc = new XmlDocument();
        if (ext == ".kmz")
        {
            var zip = ZipFile.OpenRead(filePath);
            var kml = zip.GetEntry("doc.kml");
            if (kml == null)
            {
                Console.WriteLine("KML not found in " + filePath);
                return null;
            }
            var dstFile = System.IO.Path.GetTempFileName();
            try
            {
                File.Delete(dstFile);
                kml.ExtractToFile(dstFile);
                xmlDoc.Load(dstFile);
            }
            finally
            {
                File.Delete(dstFile);
            }
        }
        else
        {
            xmlDoc.Load(filePath);
        }
        return xmlDoc;
    }

    /// <summary>
    /// Process KML folder
    /// </summary>
    /// <param name="XmlNode"></param>
    /// <param name="nsManager"></param>
    /// <param name="defaultName"></param>
    /// <returns></returns>
    private static KmlFolder ProcessFolder(XmlNode XmlNode, XmlNamespaceManager nsManager, string defaultName)
    {
        var folderName = XmlNode.SelectSingleNode("kml:name", nsManager)?.InnerText ?? defaultName;
        var folder = new KmlFolder { Name = folderName };
        foreach (XmlNode kmlPlacemark in XmlNode.SelectNodes("kml:Placemark", nsManager))
        {
            var placemarkName = kmlPlacemark.SelectSingleNode("kml:name", nsManager).InnerText;
            var placemarkDescription = kmlPlacemark.SelectSingleNode("kml:description", nsManager)?.InnerText;
            if (!TryAddPoint(folder, kmlPlacemark, placemarkName, placemarkDescription, nsManager))
                TryAddPath(folder, kmlPlacemark, placemarkName, placemarkDescription, nsManager);
        }
        return folder;
    }

    /// <summary>
    /// Try to convert a XmlNode to Waypoint and add it into the folder.
    /// </summary>
    /// <param name="folder">The KmlFolder instance.</param>
    /// <param name="element">The instance of XmlNode could represent a Waypoint.</param>
    /// <param name="name">The name of the waypoint.</param>
    /// <param name="description">The description of the wypoint.</param>
    /// <param name="nsManager">The instance of XmlNamespaceManager.</param>
    /// <returns></returns>
    private static bool TryAddPoint(KmlFolder folder, XmlNode element, string name, string description, XmlNamespaceManager nsManager)
    {
        var placemarkPoint = element.SelectSingleNode("kml:Point", nsManager);
        if (placemarkPoint == null)
            return false;

        var color = GetColor(nsManager, element);
        var coordinatesNode = placemarkPoint.SelectSingleNode("kml:coordinates", nsManager);
        if (coordinatesNode == null)
            return false;
        var coordinates = coordinatesNode.InnerText.Split(',');
        folder.Placemarks.Add(new Waypoint
        {
            Name = name,
            Latitude = coordinates[1].Trim(),
            Longitude = coordinates[0].Trim(),
            Elevation = coordinates.Length > 2 ? coordinates[2].Trim() : "0",
            Description = description,
        });
        return true;
    }

    /// <summary>
    /// Try to convert a XmlNode to a Gpx Track and add it into the folder.
    /// </summary>
    /// <param name="folder">The KmlFolder instance.</param>
    /// <param name="element">The instance of XmlNode could represent a track.</param>
    /// <param name="name">The name of the track.</param>
    /// <param name="description">The description of the track.</param>
    /// <param name="nsManager">The instance of XmlNamespaceManager.</param>
    /// <returns></returns>
    private static bool TryAddPath(KmlFolder folder, XmlNode element, string name, string description, XmlNamespaceManager nsManager)
    {
        var lineStringCoordinates = element.SelectSingleNode("kml:LineString/kml:coordinates", nsManager)
            ?? element.SelectSingleNode("kml:LinearRing/kml:coordinates", nsManager);
        if (lineStringCoordinates == null)
            return false;
        var pathColor = GetColor(nsManager, element);
        var track = new Track { Name = folder.Name + " - " + (string.IsNullOrEmpty(name) ? description : name) };
        var lines = lineStringCoordinates.InnerText.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        track.Points = GetGpsPoints(lines);
        folder.Placemarks.Add(track);
        return true;
    }

    /// <summary>
    /// Get GpxPoint collection from string array represent lines.
    /// </summary>
    /// <param name="lines">String array represent lines</param>
    /// <returns></returns>
    private static List<GpxPoint> GetGpsPoints(string[] lines)
    {
        var points = new List<GpxPoint>();

        foreach (var line in lines)
        {
            var coordinates = line.Split(',');
            if (coordinates.Length < 2) continue;

            points.Add(new GpxPoint
            {
                Longitude = coordinates[0],
                Latitude = coordinates[1],
                Elevation = coordinates[2],
            });
        }
        return points;
    }

    /// <summary>
    /// Get color from KML placemark represent by a instance of XmlNode.
    /// </summary>
    /// <param name="nsManager"></param>
    /// <param name="kmlPlacemark"></param>
    /// <returns></returns>
    private static string GetColor(XmlNamespaceManager nsManager, XmlNode kmlPlacemark)
    {
        // google
        var styleUrl = kmlPlacemark.SelectSingleNode("kml:styleUrl", nsManager)?.Value;
        if (styleUrl != null)
        {
            var match = Regex.Match(styleUrl, @"(?:\-)([ABCDEF\d]{6})(?:\-|$)");
            if (match.Success)
                return match.Groups[1].Value;
        }
        // yandex
        var rgb = kmlPlacemark.SelectSingleNode("kml:Style/kml:IconStyle/kml:color", nsManager)?.Value;
        if (rgb != null)
        {
            if (rgb.Length > 6)
                rgb = rgb.Substring(rgb.Length - 6, 6);
            if (rgb.Length == 6)
                rgb = rgb.Substring(4, 2) + rgb.Substring(2, 2) + rgb.Substring(0, 2);
            return rgb;
        }
        return null;
    }
}
