using System;
using System.Collections.Generic;
using System.Text
using Mega.Kml2Gpx.Models;

namespace Mega.Kml2Gpx;

public class Kml2GpxConvert
{
    public async void Convert(string selectedFilePath, string selectedFolder, ConverterType converterType, OutputMode outputMode)
    {
        string sOutFilePath = selectedFilePath.Substring(selectedFilePath.LastIndexOf(@"\") + 1, selectedFilePath.Length - selectedFilePath.LastIndexOf(@"\") - 1);
        string sOutFileName = sOutFilePath.Substring(0, sOutFilePath.LastIndexOf("."));
        sOutFilePath = selectedFolder + @"\" + sOutFileName + ".gpx";

        if (CheckPaths(selectedFilePath, selectedFolder))
        {
            var processResult = false;
            var onePerKml = false;
            var onePerFolder = false;
            if (outputIndex == 0)
                onePerKml = true;
            else if (outputIndex == 1)
                onePerFolder = true;

            var kmlFolders = new List<KmlFolder>();
            switch (converterIndex)
            {
                case 0:
                    processResult = await SharpKmls.SharpKml2Gpx.ProcessKml2Gpx(sFileName, _txtOutputFolder.Text, onePerKml, onePerFolder);
                    break;
                case 1:
                    kmlFolders = KmlXDocReader.ReadFile(sFileName);
                    processResult = GpxWriter.WriteGpx(sFileName, kmlFolders, outputFolder, onePerKml, onePerFolder);
                    break;
                case 2:
                    kmlFolders = KmlXmlReader.ReadFile(sFileName);
                    processResult = GpxWriter.WriteGpx(sFileName, kmlFolders, outputFolder, onePerKml, onePerFolder);
                    break;
                case 3:
                    kmlFolders = KmlXPathReader.ReadFile(sFileName);
                    processResult = GpxWriter.WriteGpx(sFileName, kmlFolders, outputFolder, onePerKml, onePerFolder);
                    break;
            }

            if (processResult)
            {
                MessageBox.Show("Conversion Completed Successfully.");
            }
            else
            {
                MessageBox.Show("Conversion Failed.");
            }
        }
    }

}
