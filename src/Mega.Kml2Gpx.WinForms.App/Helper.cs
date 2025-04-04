namespace Mega.Kml2Gpx.WinForms.App;

/// <summary>
/// Helper class
/// </summary>
internal class Helper
{
    /// <summary>
    /// Get available filename
    /// </summary>
    /// <param name="origFileName"></param>
    /// <param name="newExtension"></param>
    /// <returns></returns>
    public static string GetAvailableFilename(string origFileName, string newExtension)
    {
        var baseFileName = Path.GetFullPath(origFileName);

        var origExtensionTrimmed = Path.GetExtension(baseFileName).TrimStart(new[] { '.' });
        baseFileName = baseFileName.Substring(0, baseFileName.Length - origExtensionTrimmed.Length - 1);

        string newFile;
        var count = 0;
        do
        {
            newFile = baseFileName + (count == 0 ? string.Empty : " (" + count + ")") + "." + newExtension.TrimStart(new[] { '.' });
            count++;
        }
        while (File.Exists(newFile));

        return newFile;
    }

    /// <summary>
    /// Get available file path   
    /// </summary>
    /// <param name="path"></param>
    /// <param name="name"></param>
    /// <param name="extension"></param>
    /// <returns></returns>
    public static string GetAvailableFilePath(string path, string name, string extension)
    {
        var folder = path;
        if (!File.GetAttributes(path).HasFlag(FileAttributes.Directory))
        {
            folder = Path.GetDirectoryName(path);
            folder = folder + @"\" + Path.GetFileNameWithoutExtension(path);
        }

        if (!Directory.Exists(folder))
        {
            // Create the folder
            Directory.CreateDirectory(folder);
        }

        var fileName = Path.Combine(folder, GetValidFileName(name));

        string newFile;
        var count = 0;
        do
        {
            newFile = fileName + (count == 0 ? string.Empty : " (" + count + ")") + "." + extension.TrimStart(new[] { '.' });
            count++;
        }
        while (File.Exists(newFile));

        return newFile;
    }

    /// <summary>
    /// Get available folder path
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string GetAvailableFolderPath(string filePath, string name)
    {
        var folder = filePath;
        if (!File.GetAttributes(filePath).HasFlag(FileAttributes.Directory))
        {
            folder = Path.GetDirectoryName(filePath);
            folder = folder + @"\" + Path.GetFileNameWithoutExtension(filePath);
        }

        folder = Path.Combine(folder, GetValidFileName(name));

        if (!Directory.Exists(folder))
        {
            // Create the folder
            Directory.CreateDirectory(folder);
        }
        return folder;
    }

    /// <summary>
    /// Get valid file name
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string GetValidFileName(string fileName)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            fileName = fileName.Replace(c, '-');
        }
        return fileName;
    }

    /// <summary>
    /// Check if the file name is valid
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static bool IsValidFileName(string fileName)
    {
        char[] invalidChars = Path.GetInvalidFileNameChars();
        return !fileName.Any(ch => invalidChars.Contains(ch));
    }
}
