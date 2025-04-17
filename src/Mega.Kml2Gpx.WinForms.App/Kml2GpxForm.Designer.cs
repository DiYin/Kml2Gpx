using System.Windows.Forms;

namespace Mega.Kml2Gpx.WinForms.App
{
    partial class Kml2GpxForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            _lblFileName = new Label();
            _txtFileName = new TextBox();
            _txtOutputFolder = new TextBox();
            _lblOutputFolder = new Label();
            _btnExit = new Button();
            _btnConvert = new Button();
            _openFileDialog = new OpenFileDialog();
            _folderBrowserDialog = new FolderBrowserDialog();
            _btnBrowseFile = new Button();
            _btnBrowseFolder = new Button();
            _btnInfo = new Button();
            _checkedListBoxConverters = new CheckedListBox();
            _lblOutputOptions = new Label();
            _checkedListBoxOutputOptions = new CheckedListBox();
            _lblConverters = new Label();
            SuspendLayout();
            // 
            // _lblFileName
            // 
            _lblFileName.AutoSize = true;
            _lblFileName.Location = new Point(14, 38);
            _lblFileName.Name = "_lblFileName";
            _lblFileName.Size = new Size(168, 25);
            _lblFileName.TabIndex = 1;
            _lblFileName.Text = "KMZ/KML Filename:";
            // 
            // _txtFileName
            // 
            _txtFileName.Location = new Point(178, 39);
            _txtFileName.Name = "_txtFileName";
            _txtFileName.Size = new Size(538, 31);
            _txtFileName.TabIndex = 2;
            // 
            // _txtOutputFolder
            // 
            _txtOutputFolder.Location = new Point(178, 101);
            _txtOutputFolder.Name = "_txtOutputFolder";
            _txtOutputFolder.Size = new Size(536, 31);
            _txtOutputFolder.TabIndex = 5;
            // 
            // _lblOutputFolder
            // 
            _lblOutputFolder.Location = new Point(14, 101);
            _lblOutputFolder.Name = "_lblOutputFolder";
            _lblOutputFolder.Size = new Size(145, 35);
            _lblOutputFolder.TabIndex = 4;
            _lblOutputFolder.Text = "Output Folder:";
            // 
            // _btnExit
            // 
            _btnExit.Location = new Point(466, 356);
            _btnExit.Name = "_btnExit";
            _btnExit.Size = new Size(248, 43);
            _btnExit.TabIndex = 12;
            _btnExit.Text = "Exit";
            _btnExit.Click += ButtonExit_Click;
            // 
            // _btnConvert
            // 
            _btnConvert.Location = new Point(178, 356);
            _btnConvert.Name = "_btnConvert";
            _btnConvert.Size = new Size(248, 43);
            _btnConvert.TabIndex = 11;
            _btnConvert.Text = "Convert";
            _btnConvert.Click += ButtonConvert_Click;
            // 
            // _openFileDialog
            // 
            _openFileDialog.DefaultExt = "kml";
            _openFileDialog.Filter = "KMZ|*.kmz|KML|*.kml";
            _openFileDialog.Title = "Select KML file to convert";
            // 
            // _btnBrowseFile
            // 
            _btnBrowseFile.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            _btnBrowseFile.Location = new Point(741, 39);
            _btnBrowseFile.Name = "_btnBrowseFile";
            _btnBrowseFile.Size = new Size(43, 33);
            _btnBrowseFile.TabIndex = 3;
            _btnBrowseFile.Text = "...";
            _btnBrowseFile.Click += ButtonBrowseFile_Click;
            // 
            // _btnBrowseFolder
            // 
            _btnBrowseFolder.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            _btnBrowseFolder.Location = new Point(741, 99);
            _btnBrowseFolder.Name = "_btnBrowseFolder";
            _btnBrowseFolder.Size = new Size(43, 33);
            _btnBrowseFolder.TabIndex = 6;
            _btnBrowseFolder.Text = "...";
            _btnBrowseFolder.Click += ButtonBrowseFolder_Click;
            // 
            // _btnInfo
            // 
            _btnInfo.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            _btnInfo.Location = new Point(741, 358);
            _btnInfo.Name = "_btnInfo";
            _btnInfo.Size = new Size(43, 43);
            _btnInfo.TabIndex = 13;
            _btnInfo.Text = "?";
            _btnInfo.Click += ButtonInfo_Click;
            // 
            // _checkedListBoxConverters
            // 
            _checkedListBoxConverters.CheckOnClick = true;
            _checkedListBoxConverters.FormattingEnabled = true;
            _checkedListBoxConverters.Items.AddRange(new object[] { "Sharp Kml", "XDocument", "XmlDocument", "XPathDocument" });
            _checkedListBoxConverters.Location = new Point(178, 210);
            _checkedListBoxConverters.Name = "_checkedListBoxConverters";
            _checkedListBoxConverters.Size = new Size(248, 116);
            _checkedListBoxConverters.TabIndex = 8;
            _checkedListBoxConverters.ItemCheck += CheckedListBoxConverters_ItemCheck;
            // 
            // _lblOutputOptions
            // 
            _lblOutputOptions.Location = new Point(466, 172);
            _lblOutputOptions.Name = "_lblOutputOptions";
            _lblOutputOptions.Size = new Size(145, 35);
            _lblOutputOptions.TabIndex = 9;
            _lblOutputOptions.Text = "Output File Options";
            // 
            // _checkedListBoxOutputOptions
            // 
            _checkedListBoxOutputOptions.CheckOnClick = true;
            _checkedListBoxOutputOptions.FormattingEnabled = true;
            _checkedListBoxOutputOptions.Items.AddRange(new object[] { "One File", "One File for Each Folder", "One File for Each Track" });
            _checkedListBoxOutputOptions.Location = new Point(466, 210);
            _checkedListBoxOutputOptions.Name = "_checkedListBoxOutputOptions";
            _checkedListBoxOutputOptions.Size = new Size(248, 116);
            _checkedListBoxOutputOptions.TabIndex = 10;
            _checkedListBoxOutputOptions.ItemCheck += CheckedListBoxOutputOptions_ItemCheck;
            // 
            // _lblConverters
            // 
            _lblConverters.Location = new Point(178, 172);
            _lblConverters.Name = "_lblConverters";
            _lblConverters.Size = new Size(145, 35);
            _lblConverters.TabIndex = 7;
            _lblConverters.Text = "Converters";
            // 
            // Kml2GpxForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(806, 445);
            Controls.Add(_lblConverters);
            Controls.Add(_checkedListBoxOutputOptions);
            Controls.Add(_lblOutputOptions);
            Controls.Add(_checkedListBoxConverters);
            Controls.Add(_btnInfo);
            Controls.Add(_btnBrowseFolder);
            Controls.Add(_btnBrowseFile);
            Controls.Add(_btnConvert);
            Controls.Add(_btnExit);
            Controls.Add(_txtOutputFolder);
            Controls.Add(_lblOutputFolder);
            Controls.Add(_txtFileName);
            Controls.Add(_lblFileName);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Kml2GpxForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "KML to GPX Converter";
            Load += Kml2GpxForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        private Label _lblFileName;
        private TextBox _txtFileName;
        private Label _lblOutputFolder;
        private Button _btnExit;
        private TextBox _txtOutputFolder;
        private Button _btnConvert;
        private OpenFileDialog _openFileDialog;
        private FolderBrowserDialog _folderBrowserDialog;
        private Button _btnBrowseFile;
        private Button _btnBrowseFolder;
        private Button _btnInfo;
        private CheckedListBox _checkedListBoxConverters;
        private Label _lblOutputOptions;
        private CheckedListBox _checkedListBoxOutputOptions;
        private Label _lblConverters;
    }

    #endregion
}