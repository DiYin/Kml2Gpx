using Mega.Kml2Gpx.Models;
using Mega.Kml2Gpx.Xml;
using SharpKml.Engine;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

namespace Mega.Kml2Gpx.SharpKmls;

/// <summary>
/// Class to convert KML to GPX
/// </summary>
internal class SharpKml2Gpx
{
    /// <summary>
    /// Process to convert Kml to GPX
    /// </summary>
    /// <param name="filePath">The file path of the kml file.</param>
    /// <param name="outputFolder">The folder of the gpx files to store.</param>
    /// <param name="onePerKml">Flag to indicate if store all gpx file in one folder for a kml file. Default value is false.</param>
    /// <returns></returns>
    public static async Task<bool> ProcessKml2Gpx(string filePath, string outputFolder, OutputMode outputMode = OutputMode.OneFilePerTrack)
    {
        var kmlFile = await LoadKmzKmlFile(filePath);

        var folders = kmlFile.GetFolders();

        var gpxPlacemarks = new List<GpxPlacemark>();
        foreach (var folder in folders)
        {
            var placemarks = kmlFile.GetPlacemarks(folder);
            switch (outputMode)
            {
                case OutputMode.OneFile:
                    gpxPlacemarks.AddRange(placemarks);
                    break;
                case OutputMode.OneFilePerFolder:
                    var outputFile = Helper.GetAvailableFilePath(outputFolder, folder.Name, GpxWriter.FileExtension);
                    GpxWriter.WriteFile(outputFile, placemarks, true);
                    break;
                case OutputMode.OneFilePerTrack:
                    var folderPath = Helper.GetAvailableFolderPath(outputFolder, folder.Name);
                    GpxWriter.WriteFile(outputFolder, placemarks);
                    break;
            }
        }

        if (outputMode == OutputMode.OneFile)
        {
            var name = Path.GetFileNameWithoutExtension(filePath);
            var outFileName = Helper.GetAvailableFilePath(outputFolder, name, GpxWriter.FileExtension);
            GpxWriter.WriteFile(outFileName, gpxPlacemarks, true);
        }
        return folders.Count > 0;
    }

    /// <summary>
    /// Load Kml or Kmz file for a given file path.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static async Task<KmzKmlFile> LoadKmzKmlFile(string filePath)
    {
        var kmzKmlFile = new KmzKmlFile();
#if WINUI || UWP
                using (var stream = await file.OpenStreamForReadAsync())
#else
        using (FileStream stream = File.Open(filePath, FileMode.Open))
#endif
        {
            kmzKmlFile.FilePath = filePath;
            if (filePath.EndsWith(".kml", StringComparison.InvariantCultureIgnoreCase))
                kmzKmlFile.OpenKml(stream);
            else
                kmzKmlFile.OpenKmz(stream);
        }
        await Task.Delay(1);
        return kmzKmlFile;
    }

    /// <summary>
    /// Load Kml file for a given file path.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static async Task<KmlFile> LoadKmlFile(string filePath)
    {
#if WINUI || UWP
        using (var stream = await file.OpenStreamForReadAsync())
#else
        using (FileStream stream = File.Open(filePath, FileMode.Open))
#endif
        {
            var kmlFile = KmlFile.Load(stream);
            await Task.Delay(1);
            return kmlFile;
        }
    }

}
