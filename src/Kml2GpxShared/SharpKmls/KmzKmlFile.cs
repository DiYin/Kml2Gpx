using SharpKml.Engine;
#if WINUI || UWP
using Windows.Storage;
#endif

namespace Kml2Gpx.SharpKmls;

public delegate void FileLoadedEventHandler(Object sender, FileLoadedEventArgs e);

/// <summary>
/// Event arguments for when a file is loaded
/// </summary>
public class FileLoadedEventArgs : EventArgs
{
    public string FilePath { get; set; } = String.Empty;
}

/// <summary>
/// Represents a Kmz or Kml file.
/// </summary>
public class KmzKmlFile
{
    //public event FileLoadedEventHandler FileLoaded;

    public bool IsKmzFile { get; private set; }
    public string FilePath { get; set; } = string.Empty;
    public string FolderPath { get; set; } = string.Empty;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public KmlFile KmlFile { get; private set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public KmzFile KmzFile { get; private set; }

    /// <summary>
    /// Open Kmz file from stream.    
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    public KmzFile OpenKmz(Stream stream)
    {
        KmzFile = KmzFile.Open(stream);
        IsKmzFile = true;
        this.KmlFile = KmzFile.GetDefaultKmlFile();
        var fileInfo = new FileInfo(this.FilePath);
        FolderPath = fileInfo.CreateFolder();
        return this.KmzFile;
    }

    /// <summary>
    /// Open Kml file from stream.
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    public KmlFile OpenKml(Stream stream)
    {
        KmlFile = SharpKml.Engine.KmlFile.Load(stream);
        IsKmzFile = false;
        var folder = Path.GetDirectoryName(this.FilePath);
        if (!string.IsNullOrEmpty(folder)) FolderPath = folder;
        return this.KmlFile;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public byte[] GetResource(string path)
    {
        var resource = new byte[0];
        if (KmzFile != null)
        {
            resource = KmzFile.ReadFile(path);
        }
        return resource;
    }

#if WINUI || UWP
    public async Task Load(StorageFile file)
    {
        using (var stream = await file.OpenStreamForReadAsync())
        {
            var zip = new ZipArchive(stream);
            var kmlFileName = zip.GetFileNames().FirstOrDefault(x => Path.GetExtension(x)?.Equals(".kml") == true);
            if (kmlFileName == null)
            {
                throw new InvalidOperationException("Provided KMZ file is invalid. An entry for KML was not found");
            }

            var kmlContent = zip.GetFileContent(kmlFileName);
            if (kmlContent != null)
            {
                using (var reader = new StringReader(kmlContent))
                {
                    this.KmlFile = KmlFile.Load(reader);
                }
            }

            var resourceEntries = zip.GetFileNames().Where(x => !x.EndsWith(".kml")).ToList();
            foreach (var filename in resourceEntries)
            {
                var blob = zip.GetFileBytes(filename);
                Resources.Add(new KmlResource { FileName = filename, Blob = blob });
            }
        }
    }
#endif
}
