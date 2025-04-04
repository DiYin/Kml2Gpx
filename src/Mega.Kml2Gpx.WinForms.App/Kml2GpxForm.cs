using Mega.Kml2Gpx.WinForms.App.Models;
using Mega.Kml2Gpx.WinForms.App.Xml;
using Mega.Kml2Gpx.WinForms.App.XmlDocuments;
using System.Reflection;

namespace Mega.Kml2Gpx.WinForms.App
{
    /// <summary>
    /// Form for KML to GPX conversion
    /// </summary>
    public partial class Kml2GpxForm : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Kml2GpxForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event handler for form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Kml2GpxForm_Load(object sender, EventArgs e)
        {
            _checkedListBoxConverters.SetItemChecked(0, true);
            var index = _checkedListBoxOutputOptions.Items.Count - 1;
            _checkedListBoxOutputOptions.SetItemChecked(index, true);
        }

        /// <summary>
        /// Event handler for browse file button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonBrowseFile_Click(object sender, System.EventArgs e)
        {
            _openFileDialog.ShowDialog();
            _txtFileName.Text = _openFileDialog.FileName;
        }

        /// <summary>
        /// Event handler for browse folder button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonBrowseFolder_Click(object sender, System.EventArgs e)
        {
            _folderBrowserDialog.ShowDialog();
            _txtOutputFolder.Text = _folderBrowserDialog.SelectedPath;
        }

        /// <summary>
        /// Event handler for item check in converter list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckedListBoxConverters_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked && _checkedListBoxConverters.CheckedItems.Count > 0)
            {
                _checkedListBoxConverters.ItemCheck -= CheckedListBoxConverters_ItemCheck;
                _checkedListBoxConverters.SetItemChecked(_checkedListBoxConverters.CheckedIndices[0], false);
                _checkedListBoxConverters.ItemCheck += CheckedListBoxConverters_ItemCheck;
            }
        }

        /// <summary>
        /// Event handler for item check in output options list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckedListBoxOutputOptions_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked && _checkedListBoxOutputOptions.CheckedItems.Count > 0)
            {
                _checkedListBoxOutputOptions.ItemCheck -= CheckedListBoxOutputOptions_ItemCheck;
                _checkedListBoxOutputOptions.SetItemChecked(_checkedListBoxOutputOptions.CheckedIndices[0], false);
                _checkedListBoxOutputOptions.ItemCheck += CheckedListBoxOutputOptions_ItemCheck;
            }
        }

        /// <summary>
        /// Event handler for convert button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ButtonConvert_Click(object sender, System.EventArgs e)
        {
            string sOutFilePath = _txtFileName.Text.Substring(_txtFileName.Text.LastIndexOf(@"\") + 1, _txtFileName.Text.Length - _txtFileName.Text.LastIndexOf(@"\") - 1);
            string sOutFileName = sOutFilePath.Substring(0, sOutFilePath.LastIndexOf("."));
            sOutFilePath = _txtOutputFolder.Text + @"\" + sOutFileName + ".gpx";

            if (CheckPaths(_txtFileName.Text, _txtOutputFolder.Text))
            {
                var sFileName = _txtFileName.Text;
                var outputFolder = _txtOutputFolder.Text;

                var processResult = false;
                var converterIndex = GetCheckItemIndex(_checkedListBoxConverters, 0);
                var outputIndex = GetCheckItemIndex(_checkedListBoxOutputOptions, _checkedListBoxOutputOptions.Items.Count - 1);
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

        /// <summary>
        /// Event handler for exit button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonExit_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Event handler for info button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonInfo_Click(object sender, System.EventArgs e)
        {

            System.Reflection.AssemblyName an = Assembly.GetExecutingAssembly().GetName();
            string sVer = an.Version.Major.ToString() + "." + an.Version.Minor.ToString();
            string sVer2 = an.Version.Major.ToString() + "." + an.Version.Minor.ToString() + "." + an.Version.Revision.ToString();

            MessageBox.Show("Version: " + sVer2 + "\n\nThis utility is freeware and comes as is, with no support.", "Mega Kml2Gpx " + sVer + "  By Di Yin");

        }

        /// <summary>
        /// Get the index of the checked item in the checked list box
        /// </summary>
        /// <param name="checkedListBox">The instance of CheckedListBox.</param>
        /// <param name="defaultIndex">Default index.</param>
        /// <returns></returns>
        private int GetCheckItemIndex(CheckedListBox checkedListBox, int defaultIndex)
        {
            for (int i = 0; i < checkedListBox.Items.Count; i++)
            {
                if (checkedListBox.GetItemChecked(i))
                {
                    return i;
                }
            }
            return defaultIndex;
        }

        /// <summary>
        /// Check if the input file and output folder are valid
        /// </summary>
        /// <param name="sFilename"></param>
        /// <param name="sOutputFolder"></param>
        /// <returns></returns>
        private bool CheckPaths(string sFilename, string sOutputFolder)
        {
            if (!File.Exists(sFilename))
            {
                MessageBox.Show("Input file does not exists.");
                return false;
            }
            if (!Directory.Exists(sOutputFolder))
            {
                MessageBox.Show("Output Folder does not exists.");
                return false;
            }
            return true;
        }
    }
}
