using System;
using System.IO;

namespace Mega.Kml2Gpx.SharpKmls;

/// <summary>
/// Extension methods for FileInfo
/// </summary>
public static class FileInfoExtensions
{
    /// <summary>
    /// Create a folder with the same name as the file info
    /// </summary>
    /// <param name="fileInfo"></param>
    /// <returns></returns>
    public static string CreateFolder(this FileInfo fileInfo)
    {
        var folderName = Path.GetFileNameWithoutExtension(fileInfo.FullName);
        var folderPath = Path.GetDirectoryName(fileInfo.FullName);
        if (folderPath != null)
        {
            folderPath = Path.Combine(folderPath, folderName);
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            return folderPath;
        }
        else
            return String.Empty;
    }

    /// <summary>
    /// Create a subfolder with the same name as the file info
    /// </summary>
    /// <param name="fileInfo"></param>
    /// <returns></returns>
    public static string CreateSubFolder(this FileInfo fileInfo)
    {
        var folderPath = Path.GetDirectoryName(fileInfo.FullName);
        if (folderPath != null)
        {
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            return folderPath;
        }
        else
            return String.Empty;
    }
}
