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
            _lblFileName.Location = new Point(22, 33);
            _lblFileName.Margin = new Padding(2, 0, 2, 0);
            _lblFileName.Name = "_lblFileName";
            _lblFileName.Size = new Size(85, 15);
            _lblFileName.TabIndex = 1;
            _lblFileName.Text = "KMZ/KML File:";
            // 
            // _txtFileName
            // 
            _txtFileName.BackColor = SystemColors.Window;
            _txtFileName.BorderStyle = BorderStyle.FixedSingle;
            _txtFileName.Location = new Point(113, 30);
            _txtFileName.Margin = new Padding(0, 3, 0, 3);
            _txtFileName.Name = "_txtFileName";
            _txtFileName.ReadOnly = true;
            _txtFileName.Size = new Size(470, 23);
            _txtFileName.TabIndex = 2;
            // 
            // _txtOutputFolder
            // 
            _txtOutputFolder.BackColor = SystemColors.Window;
            _txtOutputFolder.BorderStyle = BorderStyle.FixedSingle;
            _txtOutputFolder.Location = new Point(113, 81);
            _txtOutputFolder.Name = "_txtOutputFolder";
            _txtOutputFolder.ReadOnly = true;
            _txtOutputFolder.Size = new Size(472, 23);
            _txtOutputFolder.TabIndex = 5;
            // 
            // _lblOutputFolder
            // 
            _lblOutputFolder.AutoSize = true;
            _lblOutputFolder.Location = new Point(22, 84);
            _lblOutputFolder.Name = "_lblOutputFolder";
            _lblOutputFolder.Padding = new Padding(2, 0, 2, 0);
            _lblOutputFolder.Size = new Size(88, 15);
            _lblOutputFolder.TabIndex = 4;
            _lblOutputFolder.Text = "Output Folder:";
            // 
            // _btnExit
            // 
            _btnExit.BackColor = SystemColors.ButtonFace;
            _btnExit.FlatAppearance.BorderColor = Color.Black;
            _btnExit.FlatAppearance.BorderSize = 3;
            _btnExit.Location = new Point(361, 279);
            _btnExit.Name = "_btnExit";
            _btnExit.Size = new Size(224, 30);
            _btnExit.TabIndex = 12;
            _btnExit.Text = "Exit";
            _btnExit.UseVisualStyleBackColor = false;
            _btnExit.Click += ButtonExit_Click;
            // 
            // _btnConvert
            // 
            _btnConvert.BackColor = SystemColors.ButtonFace;
            _btnConvert.FlatAppearance.BorderColor = Color.Black;
            _btnConvert.FlatAppearance.BorderSize = 3;
            _btnConvert.Location = new Point(113, 279);
            _btnConvert.Name = "_btnConvert";
            _btnConvert.Size = new Size(229, 30);
            _btnConvert.TabIndex = 11;
            _btnConvert.Text = "Convert";
            _btnConvert.UseVisualStyleBackColor = false;
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
            _btnBrowseFile.BackColor = SystemColors.ButtonFace;
            _btnBrowseFile.FlatAppearance.BorderColor = Color.Black;
            _btnBrowseFile.FlatAppearance.BorderSize = 3;
            _btnBrowseFile.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            _btnBrowseFile.Location = new Point(604, 30);
            _btnBrowseFile.Name = "_btnBrowseFile";
            _btnBrowseFile.Size = new Size(30, 23);
            _btnBrowseFile.TabIndex = 3;
            _btnBrowseFile.Text = "...";
            _btnBrowseFile.UseVisualStyleBackColor = false;
            _btnBrowseFile.Click += ButtonBrowseFile_Click;
            // 
            // _btnBrowseFolder
            // 
            _btnBrowseFolder.BackColor = SystemColors.ButtonFace;
            _btnBrowseFolder.FlatAppearance.BorderColor = Color.Black;
            _btnBrowseFolder.FlatAppearance.BorderSize = 3;
            _btnBrowseFolder.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            _btnBrowseFolder.Location = new Point(604, 81);
            _btnBrowseFolder.Name = "_btnBrowseFolder";
            _btnBrowseFolder.Size = new Size(30, 23);
            _btnBrowseFolder.TabIndex = 6;
            _btnBrowseFolder.Text = "...";
            _btnBrowseFolder.UseVisualStyleBackColor = false;
            _btnBrowseFolder.Click += ButtonBrowseFolder_Click;
            // 
            // _btnInfo
            // 
            _btnInfo.BackColor = SystemColors.ButtonFace;
            _btnInfo.FlatAppearance.BorderColor = Color.Black;
            _btnInfo.FlatAppearance.BorderSize = 3;
            _btnInfo.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            _btnInfo.Location = new Point(604, 279);
            _btnInfo.Name = "_btnInfo";
            _btnInfo.Size = new Size(30, 30);
            _btnInfo.TabIndex = 13;
            _btnInfo.Text = "?";
            _btnInfo.UseVisualStyleBackColor = false;
            _btnInfo.Click += ButtonInfo_Click;
            // 
            // _checkedListBoxConverters
            // 
            _checkedListBoxConverters.CheckOnClick = true;
            _checkedListBoxConverters.FormattingEnabled = true;
            _checkedListBoxConverters.Items.AddRange(new object[] { "Sharp Kml", "XDocument", "XmlDocument", "XPathDocument" });
            _checkedListBoxConverters.Location = new Point(113, 153);
            _checkedListBoxConverters.Margin = new Padding(4);
            _checkedListBoxConverters.Name = "_checkedListBoxConverters";
            _checkedListBoxConverters.Size = new Size(229, 76);
            _checkedListBoxConverters.TabIndex = 8;
            _checkedListBoxConverters.ItemCheck += CheckedListBoxConverters_ItemCheck;
            // 
            // _lblOutputOptions
            // 
            _lblOutputOptions.Location = new Point(361, 129);
            _lblOutputOptions.Margin = new Padding(2, 0, 2, 0);
            _lblOutputOptions.Name = "_lblOutputOptions";
            _lblOutputOptions.Size = new Size(163, 21);
            _lblOutputOptions.TabIndex = 9;
            _lblOutputOptions.Text = "Output File Options";
            // 
            // _checkedListBoxOutputOptions
            // 
            _checkedListBoxOutputOptions.CheckOnClick = true;
            _checkedListBoxOutputOptions.FormattingEnabled = true;
            _checkedListBoxOutputOptions.Items.AddRange(new object[] { "One File", "One File for Each Folder", "One File for Each Track" });
            _checkedListBoxOutputOptions.Location = new Point(359, 153);
            _checkedListBoxOutputOptions.Margin = new Padding(2);
            _checkedListBoxOutputOptions.Name = "_checkedListBoxOutputOptions";
            _checkedListBoxOutputOptions.Size = new Size(224, 76);
            _checkedListBoxOutputOptions.TabIndex = 10;
            _checkedListBoxOutputOptions.ItemCheck += CheckedListBoxOutputOptions_ItemCheck;
            // 
            // _lblConverters
            // 
            _lblConverters.Location = new Point(109, 128);
            _lblConverters.Margin = new Padding(2, 0, 2, 0);
            _lblConverters.Name = "_lblConverters";
            _lblConverters.Size = new Size(163, 21);
            _lblConverters.TabIndex = 7;
            _lblConverters.Text = "Converters";
            // 
            // Kml2GpxForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(664, 361);
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
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(2);
            MaximizeBox = false;
            Name = "Kml2GpxForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "KML to GPX Converter (Win Form)";
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