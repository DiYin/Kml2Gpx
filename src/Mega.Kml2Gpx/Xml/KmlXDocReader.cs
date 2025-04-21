using Mega.Kml2Gpx.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Mega.Kml2Gpx.Xml;

/// <summary>
/// Class for reading KML files by use XDocument
/// </summary>
internal static class KmlXDocReader
{
    /// <summary>
    /// Read KML file and return list of KmlFolders
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static List<KmlFolder> ReadFile(string filePath)
    {
        var document = GetXmlDocument(filePath);
        var kmlFolders = new List<KmlFolder>();
        if (document != null)
        {
            kmlFolders = GetFolders(document);
        }
        return kmlFolders;
    }

    /// <summary>
    /// Get XDocument from file
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    private static XDocument GetXmlDocument(string filePath)
    {
        var ext = Path.GetExtension(filePath);

        if (ext == ".kmz")
        {
            var zip = ZipFile.OpenRead(filePath);
            var kml = zip.GetEntry("doc.kml");
            if (kml == null)
            {
                Console.WriteLine("KML not found in " + filePath);
                return null;
            }
            return XDocument.Load(kml.Open());
        }
        else
            return XDocument.Load(filePath);

    }

    /// <summary>
    /// Get list of KmlFolders from XDocument
    /// </summary>
    /// <param name="doc"></param>
    /// <returns></returns>
    private static List<KmlFolder> GetFolders(XDocument doc)
    {
        var firstElement = doc.Root;
        var ns = firstElement.GetDefaultNamespace();
        var nsManager = new XmlNamespaceManager(new NameTable());
        nsManager.AddNamespace("kml", "http://www.opengis.net/kml/2.2");
        var kmlFolders = new List<KmlFolder>();
        var defaultName = doc.XPathSelectElement("/kml:kml/kml:Document/kml:name", nsManager)?.Value;
        foreach (var folder in doc.XPathSelectElements("//kml:Folder", nsManager))
        {
            var kmlFolder = ProcessFolder(folder, nsManager, defaultName);
            if (kmlFolder != null) kmlFolders.Add(kmlFolder);
        }
        if (kmlFolders.Count == 0)
        {
            foreach (var kmlDocument in doc.XPathSelectElements("//kml:Document", nsManager))
            {
                var kmlFolder = ProcessFolder(kmlDocument, nsManager, defaultName);
                kmlFolders.Add(kmlFolder);
            }
        }
        return kmlFolders;
    }

    /// <summary>
    /// Convert a XElement to KmlFolder.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="nsManager"></param>
    /// <param name="defaultName"></param>
    /// <returns></returns>
    private static KmlFolder ProcessFolder(XElement element, XmlNamespaceManager nsManager, string defaultName)
    {
        var name = element.XPathSelectElement("kml:name", nsManager)?.Value ?? defaultName;
        var kmlFolder = new KmlFolder { Name = name };
        foreach (var placemarkElement in element.XPathSelectElements("kml:Placemark", nsManager))
        {
            name = placemarkElement.XPathSelectElement("kml:name", nsManager).Value;
            var description = placemarkElement.XPathSelectElement("kml:description", nsManager)?.Value;
            if (!TryAddPoint(kmlFolder, placemarkElement, name, description, nsManager))
                TryAddPath(kmlFolder, placemarkElement, name, description, nsManager);
        }
        return kmlFolder;
    }

    /// <summary>
    /// Try to convert a XElement to Waypoint and add it into the folder.
    /// </summary>
    /// <param name="folder">The KmlFolder instance.</param>
    /// <param name="element">The instance of XElement could represent a Waypoint.</param>
    /// <param name="name">The name of the waypoint.</param>
    /// <param name="description">The description of the wypoint.</param>
    /// <param name="nsManager">The instance of XmlNamespaceManager.</param>
    /// <returns></returns>
    private static bool TryAddPoint(KmlFolder folder, XElement element, string name, string description, XmlNamespaceManager nsManager)
    {
        var placemarkPoint = element.XPathSelectElement("kml:Point/kml:coordinates", nsManager);
        if (placemarkPoint != null)
        {

            var coordinates = placemarkPoint.Value.Split(',');
            if (coordinates.Length > 1)
            {
                var waypoint = new Waypoint
                {
                    Name = name,
                    Latitude = coordinates[1].Trim(),
                    Longitude = coordinates[0].Trim(),
                    Description = description,
                };
                if (coordinates.Length > 2)
                {
                    waypoint.Elevation = coordinates[2].Trim();
                }
                folder.Placemarks.Add(waypoint);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Try to convert a XElement to a Gpx Track and add it into the folder.
    /// </summary>
    /// <param name="folder">The KmlFolder instance.</param>
    /// <param name="element">The instance of XElement could represent a track.</param>
    /// <param name="name">The name of the track.</param>
    /// <param name="description">The description of the track.</param>
    /// <param name="nsManager">The instance of XmlNamespaceManager.</param>
    /// <returns></returns>
    private static bool TryAddPath(KmlFolder folder, XElement element, string name, string description, XmlNamespaceManager nsManager)
    {
        var lineStringCoordinates = element.XPathSelectElement("kml:LineString/kml:coordinates", nsManager)
            ?? element.XPathSelectElement("//kml:LinearRing/kml:coordinates", nsManager);
        if (lineStringCoordinates != null)
        {
            var track = new Track
            {
                Name = name,
                Description = description,
                Points = new List<GpxPoint>()
            };
            var lines = lineStringCoordinates.Value.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length > 0)
            {
                foreach (var coordinateLine in lines)
                {
                    var coordinates = coordinateLine.Split(',');
                    track.Points.Add(new GpxPoint
                    {
                        Latitude = coordinates[1].Trim(),
                        Longitude = coordinates[0].Trim()
                    });
                }
                folder.Placemarks.Add(track);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Get color from KML placemark represent by a instance of XElement.
    /// </summary>
    /// <param name="nsManager"></param>
    /// <param name="kmlPlacemark"></param>
    /// <returns></returns>
    private static string GetColor(XmlNamespaceManager nsManager, XElement kmlPlacemark)
    {
        // google
        var styleUrl = kmlPlacemark.XPathSelectElement("kml:styleUrl", nsManager)?.Value;
        if (styleUrl != null)
        {
            var match = Regex.Match(styleUrl, @"(?:\-)([ABCDEF\d]{6})(?:\-|$)");
            if (match.Success)
                return match.Groups[1].Value;
        }
        // yandex
        var rgb = kmlPlacemark.XPathSelectElement("kml:Style/kml:IconStyle/kml:color", nsManager)?.Value;
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
