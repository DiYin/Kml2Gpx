using System.Xml;
using System.Xml.XPath;

namespace Mega.Kml2Gpx.Xml;

/// <summary>
/// Class for reading KML file by use XPathDocument
/// </summary>
internal class KmlXPathReader
{
    /// <summary>
    /// Read KML file and return list of KmlFolders
    /// </summary>
    /// <param name="filePath">The kml file path</param>
    /// <returns></returns>
    public static List<KmlFolder> ReadFile(string filePath)
    {
        var xPathDoc = new XPathDocument(filePath);
        var navigator = xPathDoc.CreateNavigator();
        XmlNamespaceManager nsmgr = new XmlNamespaceManager(navigator.NameTable);
        nsmgr.AddNamespace("kml", "http://www.opengis.net/kml/2.2");
        //				<kml xmlns="http://earth.google.com/kml/2.0">
        //				<Document xmlns:xlink="http://www.w3/org/1999/xlink">
        //				<name>Grand Canyon</name>
        var docIterator = navigator.Select("/kml/Document");

        var kmlFolders = new List<KmlFolder>();
        var namespaceUri = "http://www.opengis.net/kml/2.2";

        // Loop thorough the placemarks but output only waypoints
        var folderIterator = navigator.SelectDescendants("Folder", namespaceUri, false);
        while (folderIterator.MoveNext()) // Through the Folder (waypoints)
        {
            var folderNavigator = folderIterator.Current.Clone();
            folderNavigator.MoveToFirstChild();
            var kmlFolder = ProcessFolder(folderNavigator, namespaceUri);
            folderNavigator = folderIterator.Current.Clone();
            var placeIterator = folderNavigator.SelectChildren(XPathNodeType.Element);
            while (placeIterator.MoveNext()) // Through the Placemark
            {
                var placemarkNav = placeIterator.Current.Clone();
                var isWayPoint = placemarkNav.SelectSingleNode("kml:Point", nsmgr) != null;
                placemarkNav = placeIterator.Current.Clone();
                var childIterator = placemarkNav.SelectChildren(XPathNodeType.Element);
                if (isWayPoint)
                {
                    var waypoint = new Waypoint();
                    while (childIterator.MoveNext())
                    {
                        var childNav = childIterator.Current.Clone();
                        ProcessWaypoint(waypoint, childNav, namespaceUri);
                    }
                    if (!string.IsNullOrEmpty(waypoint.Name) && !string.IsNullOrEmpty(waypoint.Longitude) && !string.IsNullOrEmpty(waypoint.Latitude))
                    {
                        kmlFolder.Placemarks.Add(waypoint);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Invalid waypoint");
                    }
                }
                else
                {
                    var track = new Track();
                    while (childIterator.MoveNext())
                    {
                        var childNav = childIterator.Current.Clone();
                        ProcessTrack(track, childNav, namespaceUri);
                    }
                    if (track.Points != null && track.Points.Count > 0)
                    {
                        kmlFolder.Placemarks.Add(track);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Invalid track");
                    }
                }
            }
            if (kmlFolder != null) kmlFolders.Add(kmlFolder);
        }
        return kmlFolders;
    }

    /// <summary>
    /// Converts a XPathNavigator to KmlFolder.
    /// </summary>
    /// <param name="navigator">The instance of XPathNavigator</param>
    /// <param name="namespaceUri">Xmll name space Url</param>
    /// <returns></returns>
    private static KmlFolder ProcessFolder(XPathNavigator navigator, string namespaceUri)
    {
        var kmlFolder = new KmlFolder();
        if (navigator.Name == "name")
        {
            kmlFolder.Name = navigator.Value;
            kmlFolder.Placemarks = new List<GpxPlacemark>();
        }
        return kmlFolder;
    }

    /// <summary>
    /// Converts a XPathNavigator to a waypoint for gpx file
    /// </summary>
    /// <param name="waypoint"></param>
    /// <param name="nav"></param>
    private static void ProcessWaypoint(Waypoint waypoint, XPathNavigator nav, string namespaceUri)
    {
        switch (nav.Name)
        {
            case "name":
                var sName = nav.Value;
                sName = sName.Replace(@"&", @"&amp;");
                sName = sName.Replace(@"<", @"&lt;");
                sName = sName.Replace(@">", @"&gt;");
                sName = sName.Replace(@"'", @"&apos;");
                sName = sName.Replace(@"""", @"&quot;");
                waypoint.Name = sName;
                break;
            case "description":
                waypoint.Description = nav.Value;
                break;
            case "Point":
                //				<wpt lat="33.540270000" lon="-112.022800000">
                //				<ele>33.832800</ele>
                //				<name>PIESTEWA P</name>
                //				<sym>Trail Head</sym>
                //				</wpt>

                // Select the coordinates
                XPathNodeIterator ni = nav.SelectDescendants("coordinates", namespaceUri, false);
                if (ni.MoveNext())
                {
                    XPathNavigator nav2 = ni.Current.Clone();
                    nav2.MoveToFirstChild();
                    var points = nav2.Value.Split(new[] { ' ', '\n', '\r', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (points.Length > 1)
                    {
                        waypoint.Latitude = points[1];
                        waypoint.Longitude = points[0];
                    }
                    if (points.Length > 2)
                    {
                        waypoint.Elevation = points[2];
                    }
                }
                break;
        }
    }

    /// <summary>
    /// Converts a XPathNavigator to a gpx track
    /// </summary>
    /// <param name="gpxTrack"></param>
    /// <param name="nav"></param>
    private static void ProcessTrack(Track gpxTrack, XPathNavigator nav, string namespaceUri)
    {
        switch (nav.Name)
        {
            case "name":
                var sName = nav.Value;
                sName = sName.Replace(@"&", @"&amp;");
                sName = sName.Replace(@"<", @"&lt;");
                sName = sName.Replace(@">", @"&gt;");
                sName = sName.Replace(@"'", @"&apos;");
                sName = sName.Replace(@"""", @"&quot;");
                gpxTrack.Name = sName;
                break;
            case "MultiGeometry":
            case "LineString":
                //				<trk>
                //				<name>Piestewa Peak Circumference</name>
                //				<number>1</number>
                //				<trkseg>
                //				<trkpt lat="33.543250000" lon="-112.025140000">
                //					<ele>131.06399755026854</ele>
                //					<sym>Waypoint</sym>
                //				</trkpt>
                //				<trkpt lat="33.543440000" lon="-112.025950000"></trkpt>
                //				</trkseg>
                //				</trk>				
                // Select the coordinates collection		

                XPathNodeIterator xni = nav.SelectDescendants("coordinates", namespaceUri, false);
                xni.MoveNext();
                XPathNavigator xnav2 = xni.Current;

                // Display the content of each element node.
                XPathNodeIterator xni2 = xnav2.SelectDescendants(XPathNodeType.Text, false);
                while (xni2.MoveNext())
                {
                    // Get the points array
                    string[] aPoints = xni2.Current.Value.Trim().Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                    if (gpxTrack is Track track && aPoints.Length > 0)
                    {
                        var points = new List<GpxPoint>();
                        // Process the points
                        foreach (string sPoint in aPoints)
                        {
                            points.Add(new GpxPoint
                            {
                                Latitude = sPoint.Split(",".ToCharArray())[1],
                                Longitude = sPoint.Split(",".ToCharArray())[0],
                                Elevation = sPoint.Split(",".ToCharArray())[2],
                            });
                        }
                        track.Points = points;
                    }
                }
                break;
        }
    }
}
