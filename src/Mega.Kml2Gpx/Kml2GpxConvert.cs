using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Mega.Kml2Gpx.Models;
using Mega.Kml2Gpx.SharpKmls;
using Mega.Kml2Gpx.Xml;

namespace Mega.Kml2Gpx;

public class Kml2GpxConvert
{
    public static async Task<string> Convert(string selectedFile, string selectedFolder, ConverterType converterType, OutputMode outputMode)
    {
        string sOutFilePath = selectedFile.Substring(selectedFile.LastIndexOf(@"\") + 1, selectedFile.Length - selectedFile.LastIndexOf(@"\") - 1);
        string sOutFileName = sOutFilePath.Substring(0, sOutFilePath.LastIndexOf("."));
        sOutFilePath = selectedFolder + @"\" + sOutFileName + ".gpx";

        var processResult = false;

        var kmlFolders = new List<Models.KmlFolder>();
        switch (converterType)
        {
            case ConverterType.SharpKml:
                processResult = await SharpKml2Gpx.ProcessKml2Gpx(selectedFile, selectedFolder, outputMode);
                break;
            case ConverterType.XDocument:
                kmlFolders = KmlXDocReader.ReadFile(selectedFile);
                processResult = GpxWriter.WriteGpx(selectedFile, kmlFolders, selectedFolder, outputMode);
                break;
            case ConverterType.XmlDocument:
                kmlFolders = KmlXmlReader.ReadFile(selectedFile);
                processResult = GpxWriter.WriteGpx(selectedFile, kmlFolders, selectedFolder, outputMode);
                break;
            case ConverterType.XPathDocument:
                kmlFolders = KmlXPathReader.ReadFile(selectedFile);
                processResult = GpxWriter.WriteGpx(selectedFile, kmlFolders, selectedFolder, outputMode);
                break;
        }

        return processResult ? "Conversion Completed Successfully." : "Conversion Failed.";
    }
}