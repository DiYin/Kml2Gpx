using Mega.Kml2Gpx.WinForms.App.Models;
using Mega.Kml2Gpx.WinForms.App;
using System.Xml;

namespace Mega.Kml2Gpx.WinForms.App.Xml;

/// <summary>
/// Class to write gpx file
/// </summary>
internal static class GpxWriter
{
    public const string FileExtension = "gpx";

    /// <summary>
    /// Write gpx file from kml folders.
    /// </summary>
    /// <param name="filePath">The original kml file path.</param>
    /// <param name="kmlFolders">The collection of all KmlFolder hold the GPX files</param>
    /// <param name="outputFolder">The Gpx output folder.</param>
    /// <param name="onePerKml">Flag to indicate weither only create one folder for Kml file.</param>
    /// <param name="onePerFolder">Flag to indicate weither create sub folder for each Kml Folder.</param>
    /// <returns></returns>
    public static bool WriteGpx(string filePath, List<KmlFolder> kmlFolders, string outputFolder, bool onePerKml, bool onePerFolder)
    {
        var gpxPlacemarks = new List<GpxPlacemark>();
        foreach (var folder in kmlFolders)
        {
            var placemarks = folder.Placemarks;
            if (onePerKml)
            {
                gpxPlacemarks.AddRange(placemarks);
            }
            else if (onePerFolder)
            {
                var outputFile = Helper.GetAvailableFilePath(outputFolder, folder.Name, FileExtension);
                WriteFile(outputFile, placemarks, onePerFolder);
            }
            else
            {
                var folderPath = Helper.GetAvailableFolderPath(outputFolder, folder.Name);
                WriteFile(folderPath, placemarks);
            }
        }

        if (onePerKml)
        {
            var name = Path.GetFileNameWithoutExtension(filePath);
            var outFileName = Helper.GetAvailableFilePath(outputFolder, name, FileExtension);
            WriteFile(outFileName, gpxPlacemarks, true);
        }
        return kmlFolders.Count > 0;
    }

    /// <summary>
    /// Write a collection of GPXPlaceMark in to a gpx file.
    /// </summary>
    /// <param name="fileName">The file name of the Gpx fiel to write.</param>
    /// <param name="placemarks">The collection of GpxPlacemark to save.</param>
    /// <param name="onefile">Flag to indicate weither save the collection in one file.</param>
    public static void WriteFile(string fileName, IEnumerable<GpxPlacemark> placemarks, bool onefile = false)
    {
        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Indent = true;
        settings.IndentChars = "\t";
        settings.NewLineHandling = NewLineHandling.Replace;

        placemarks = placemarks.Where(x => x != null).ToList();
        var waypoints = placemarks.Where(x => x.GpxType == GpxPlacemarkType.WayPoint).Select(x => x as Waypoint).ToList();
        var tracks = placemarks.Where(x => x.GpxType == GpxPlacemarkType.Track).Select(x => x as Track).ToList();
        if (onefile)
        {
            using (var xmlWriter = XmlWriter.Create(fileName, settings))
            {
                xmlWriter.WriteHead();
                xmlWriter.WriteWaypoints(waypoints);
                xmlWriter.WriteTracks(tracks);
                xmlWriter.WriteEndDocument();
            }
        }
        else
        {
            var index = 0;
            if (tracks.Count == 0)
            {
                var waypointFile = Helper.GetAvailableFilePath(fileName, "Waypoints", FileExtension);
                using (var xmlWriter = XmlWriter.Create(waypointFile, settings))
                {
                    xmlWriter.WriteHead();
                    xmlWriter.WriteWaypoints(waypoints);
                    xmlWriter.WriteEndDocument();
                }
            }
            else
            {
                foreach (var track in tracks)
                {
                    var trackFile = Helper.GetAvailableFilePath(fileName, track.Name, FileExtension);
                    using (var xmlWriter = XmlWriter.Create(trackFile, settings))
                    {
                        xmlWriter.WriteHead();
                        if (index == 0)
                        {
                            xmlWriter.WriteWaypoints(waypoints);
                            xmlWriter.WriteTrack(track);
                            xmlWriter.WriteEndDocument();
                        }
                        else
                        {
                            xmlWriter.WriteTrack(track);
                            xmlWriter.WriteEndDocument();
                        }
                    }
                    index++;
                }
            }
        }
    }

    /// <summary>
    /// Write the head of the gpx file.
    /// </summary>
    /// <param name="xmlWriter"></param>
    private static void WriteHead(this XmlWriter xmlWriter)
    {
        xmlWriter.WriteStartDocument();
        xmlWriter.WriteStartElement("gpx");
        xmlWriter.WriteAttributeString("xmlns", "gx", null, "http://www.google.com/kml/ext/2.2");
        xmlWriter.WriteAttributeString("xmlns", "kml", null, "http://www.opengis.net/kml/2.2");
        xmlWriter.WriteAttributeString("creator", "Di Yin");
        xmlWriter.WriteElementString("time", DateTimeOffset.UtcNow.ToString());
    }

    /// <summary>
    /// Write the collection of Waypoint into the gpx file.
    /// </summary>
    /// <param name="xmlWriter"></param>
    /// <param name="waypoints"></param>
    private static void WriteWaypoints(this XmlWriter xmlWriter, List<Waypoint> waypoints)
    {
        foreach (var waypoint in waypoints)
        {
            xmlWriter.WriteStartElement("wpt");
            xmlWriter.WriteAttributeString("lat", waypoint.Latitude);
            xmlWriter.WriteAttributeString("lon", waypoint.Longitude);
            xmlWriter.WriteElementString("name", waypoint.Name);
            xmlWriter.WriteElementString("desc", waypoint.Description);
            xmlWriter.WriteEndElement(); // </wpt>
        }
    }

    /// <summary>
    /// Write a collection of track into the gpx file.
    /// </summary>
    /// <param name="xmlWriter"></param>
    /// <param name="tracks"></param>
    private static void WriteTracks(this XmlWriter xmlWriter, List<Track> tracks)
    {
        foreach (var track in tracks)
        {
            xmlWriter.WriteTrack(track);
        }
    }

    /// <summary>
    /// Write a Gpx track into the gpx file.
    /// </summary>
    /// <param name="xmlWriter"></param>
    /// <param name="track"></param>
    private static void WriteTrack(this XmlWriter xmlWriter, Track track)
    {
        xmlWriter.WriteStartElement("trk");
        xmlWriter.WriteElementString("name", track.Name);
        xmlWriter.WriteStartElement("trkseg");
        foreach (var point in track.Points)
        {
            xmlWriter.WriteStartElement("trkpt");
            xmlWriter.WriteAttributeString("lat", point.Latitude);
            xmlWriter.WriteAttributeString("lon", point.Longitude);

            if (!string.IsNullOrWhiteSpace(point.Elevation))
            {
                xmlWriter.WriteElementString("ele", point.Elevation);
            }
            xmlWriter.WriteEndElement(); // </trkpt>
        }
        xmlWriter.WriteEndElement(); // </trkseg>
        xmlWriter.WriteEndElement(); // </trk>
    }
}
