namespace Mega.Kml2Gpx.Xml;

internal static class StringExtensions
{
    /// <summary>
    /// Trim the string to remove illegal charectors.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string TrimIllegalChars(this string path)
    {
        return string.Concat(path.Split(Path.GetInvalidFileNameChars(), StringSplitOptions.RemoveEmptyEntries));
    }

    /// <summary>
    /// Trim the string to remove line return charectors
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string Trim(this string s)
    {
        return s?.Trim('\n', '\r', ' ');
    }
}
