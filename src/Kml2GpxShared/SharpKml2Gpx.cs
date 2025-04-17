using Kml2Gpx.SharpKmls;
using Mega.Kml2Gpx.WinForms.App.Models;
using Mega.Kml2Gpx.WinForms.App.Xml;
using SharpKml.Engine;

namespace Mega.Kml2Gpx.WinForms.App.SharpKmls;

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
    public static async Task<bool> ProcessKml2Gpx(string filePath, string outputFolder, bool onePerKml = false, bool onePerFolder = false)
    {
        var kmlFile = await LoadKmzKmlFile(filePath);

        var folders = kmlFile.GetFolders();

        var gpxPlacemarks = new List<GpxPlacemark>();
        foreach (var folder in folders)
        {
            var placemarks = kmlFile.GetPlacemarks(folder);
            if (onePerKml)
            {
                gpxPlacemarks.AddRange(placemarks);
            }
            else if (onePerFolder)
            {
                var outputFile = Helper.GetAvailableFilePath(outputFolder, folder.Name, GpxWriter.FileExtension);
                GpxWriter.WriteFile(outputFile, placemarks, onePerFolder);
            }
            else
            {
                var folderPath = Helper.GetAvailableFolderPath(outputFolder, folder.Name);
                GpxWriter.WriteFile(folderPath, placemarks);
            }
        }

        if (onePerKml)
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
