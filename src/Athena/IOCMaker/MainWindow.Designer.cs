namespace Athena
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sTIXToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jSONToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.IncidentNameTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.IncidentDescriptionTextBox = new System.Windows.Forms.TextBox();
            this.OverviewGroupBox = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.IncidentEffectDropdown = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.ReportCompanyTextBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.IncidentTitleTextBox = new System.Windows.Forms.TextBox();
            this.ConfidenceList = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.ResponderTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.ReportedByTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.IncidentResolvedDatePicker = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.IncidentReportedDatePicker = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.IncidentDiscoveredDatePicker = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.InitialCompromiseDatePicker = new System.Windows.Forms.DateTimePicker();
            this.IndicatorsGroupBox = new System.Windows.Forms.GroupBox();
            this.VisualiseButton = new System.Windows.Forms.Button();
            this.CSVLoadButton = new System.Windows.Forms.Button();
            this.IndicatorDetailsPanel = new System.Windows.Forms.Panel();
            this.ObservablesListBox = new System.Windows.Forms.ListBox();
            this.DeleteIndicatorButton = new System.Windows.Forms.Button();
            this.LinkIndicatorButton = new System.Windows.Forms.Button();
            this.EditIndicatorButton = new System.Windows.Forms.Button();
            this.AddIndicatorButton = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.OverviewGroupBox.SuspendLayout();
            this.IndicatorsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(788, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sTIXToolStripMenuItem,
            this.jSONToolStripMenuItem});
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.exportToolStripMenuItem.Text = "Export";
            // 
            // sTIXToolStripMenuItem
            // 
            this.sTIXToolStripMenuItem.Name = "sTIXToolStripMenuItem";
            this.sTIXToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.sTIXToolStripMenuItem.Text = "STIX 1.1 XML";
            this.sTIXToolStripMenuItem.Click += new System.EventHandler(this.sTIXToolStripMenuItem_Click);
            // 
            // jSONToolStripMenuItem
            // 
            this.jSONToolStripMenuItem.Name = "jSONToolStripMenuItem";
            this.jSONToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.jSONToolStripMenuItem.Text = "MISP JSON";
            this.jSONToolStripMenuItem.Click += new System.EventHandler(this.jSONToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem1});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.aboutToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem1
            // 
            this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem1.Text = "About";
            this.aboutToolStripMenuItem1.Click += new System.EventHandler(this.aboutToolStripMenuItem1_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Incident Name:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // IncidentNameTextBox
            // 
            this.IncidentNameTextBox.Location = new System.Drawing.Point(125, 51);
            this.IncidentNameTextBox.Name = "IncidentNameTextBox";
            this.IncidentNameTextBox.Size = new System.Drawing.Size(140, 20);
            this.IncidentNameTextBox.TabIndex = 3;
            this.IncidentNameTextBox.Text = "e.g. ProjectName";
            this.IncidentNameTextBox.TextChanged += new System.EventHandler(this.IncidentNameTextBox_TextChanged);
            this.IncidentNameTextBox.Enter += new System.EventHandler(this.IncidentNameTextBox_Enter);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(505, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Incident Description:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // IncidentDescriptionTextBox
            // 
            this.IncidentDescriptionTextBox.Location = new System.Drawing.Point(508, 61);
            this.IncidentDescriptionTextBox.Multiline = true;
            this.IncidentDescriptionTextBox.Name = "IncidentDescriptionTextBox";
            this.IncidentDescriptionTextBox.Size = new System.Drawing.Size(250, 87);
            this.IncidentDescriptionTextBox.TabIndex = 7;
            this.IncidentDescriptionTextBox.Text = "e.g. Summary of the incident and the observables found";
            this.IncidentDescriptionTextBox.TextChanged += new System.EventHandler(this.IncidentDescriptionTextBox_TextChanged);
            this.IncidentDescriptionTextBox.Enter += new System.EventHandler(this.IncidentDescriptionTextBox_Enter);
            // 
            // OverviewGroupBox
            // 
            this.OverviewGroupBox.Controls.Add(this.label13);
            this.OverviewGroupBox.Controls.Add(this.IncidentEffectDropdown);
            this.OverviewGroupBox.Controls.Add(this.label12);
            this.OverviewGroupBox.Controls.Add(this.ReportCompanyTextBox);
            this.OverviewGroupBox.Controls.Add(this.label11);
            this.OverviewGroupBox.Controls.Add(this.IncidentTitleTextBox);
            this.OverviewGroupBox.Controls.Add(this.ConfidenceList);
            this.OverviewGroupBox.Controls.Add(this.label10);
            this.OverviewGroupBox.Controls.Add(this.ResponderTextBox);
            this.OverviewGroupBox.Controls.Add(this.label8);
            this.OverviewGroupBox.Controls.Add(this.label7);
            this.OverviewGroupBox.Controls.Add(this.ReportedByTextBox);
            this.OverviewGroupBox.Controls.Add(this.label6);
            this.OverviewGroupBox.Controls.Add(this.IncidentResolvedDatePicker);
            this.OverviewGroupBox.Controls.Add(this.label5);
            this.OverviewGroupBox.Controls.Add(this.IncidentReportedDatePicker);
            this.OverviewGroupBox.Controls.Add(this.label4);
            this.OverviewGroupBox.Controls.Add(this.IncidentDiscoveredDatePicker);
            this.OverviewGroupBox.Controls.Add(this.label3);
            this.OverviewGroupBox.Controls.Add(this.InitialCompromiseDatePicker);
            this.OverviewGroupBox.Controls.Add(this.IncidentDescriptionTextBox);
            this.OverviewGroupBox.Controls.Add(this.label1);
            this.OverviewGroupBox.Controls.Add(this.label2);
            this.OverviewGroupBox.Controls.Add(this.IncidentNameTextBox);
            this.OverviewGroupBox.Location = new System.Drawing.Point(12, 44);
            this.OverviewGroupBox.Name = "OverviewGroupBox";
            this.OverviewGroupBox.Size = new System.Drawing.Size(764, 189);
            this.OverviewGroupBox.TabIndex = 4;
            this.OverviewGroupBox.TabStop = false;
            this.OverviewGroupBox.Text = "Incident Overview";
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(277, 155);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(79, 13);
            this.label13.TabIndex = 24;
            this.label13.Text = "Incident Effect:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // IncidentEffectDropdown
            // 
            this.IncidentEffectDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.IncidentEffectDropdown.FormattingEnabled = true;
            this.IncidentEffectDropdown.Items.AddRange(new object[] {
            "Data Breach or Compromise",
            "Disruption of Service / Operations",
            "User Data Loss",
            "Brand or Image Degradation",
            "Degradation of Service",
            "Loss of Competitive Advantage",
            "Loss of Competitive Advantage - Economic",
            "Loss of Competitive Advantage - Military",
            "Loss of Competitive Advantage - Political",
            "Destruction",
            "Financial Loss",
            "Loss of Confidential / Proprietary Information or Intellectual Property",
            "Regulatory, Compliance or Legal Impact",
            "Unintended Access"});
            this.IncidentEffectDropdown.Location = new System.Drawing.Point(362, 152);
            this.IncidentEffectDropdown.Name = "IncidentEffectDropdown";
            this.IncidentEffectDropdown.Size = new System.Drawing.Size(396, 21);
            this.IncidentEffectDropdown.TabIndex = 23;
            this.IncidentEffectDropdown.SelectedIndexChanged += new System.EventHandler(this.IncidentEffectDropdown_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(1, 25);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(118, 13);
            this.label12.TabIndex = 22;
            this.label12.Text = "Reporting Organisation:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReportCompanyTextBox
            // 
            this.ReportCompanyTextBox.Location = new System.Drawing.Point(125, 22);
            this.ReportCompanyTextBox.Name = "ReportCompanyTextBox";
            this.ReportCompanyTextBox.Size = new System.Drawing.Size(140, 20);
            this.ReportCompanyTextBox.TabIndex = 1;
            this.ReportCompanyTextBox.Text = "MWRInfoSecurity";
            this.ReportCompanyTextBox.TextChanged += new System.EventHandler(this.ReportCompanyTextBox_TextChanged);
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(285, 25);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(71, 13);
            this.label11.TabIndex = 20;
            this.label11.Text = "Incident Title:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // IncidentTitleTextBox
            // 
            this.IncidentTitleTextBox.Location = new System.Drawing.Point(362, 22);
            this.IncidentTitleTextBox.Name = "IncidentTitleTextBox";
            this.IncidentTitleTextBox.Size = new System.Drawing.Size(396, 20);
            this.IncidentTitleTextBox.TabIndex = 3;
            this.IncidentTitleTextBox.Text = "e.g. Project X Malware Infection #132";
            this.IncidentTitleTextBox.TextChanged += new System.EventHandler(this.IncidentTitleTextBox_TextChanged);
            this.IncidentTitleTextBox.Enter += new System.EventHandler(this.IncidentTitleTextBox_Enter);
            // 
            // ConfidenceList
            // 
            this.ConfidenceList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ConfidenceList.FormattingEnabled = true;
            this.ConfidenceList.Items.AddRange(new object[] {
            "High",
            "Medium",
            "Low"});
            this.ConfidenceList.Location = new System.Drawing.Point(362, 125);
            this.ConfidenceList.Name = "ConfidenceList";
            this.ConfidenceList.Size = new System.Drawing.Size(128, 21);
            this.ConfidenceList.TabIndex = 11;
            this.ConfidenceList.SelectedIndexChanged += new System.EventHandler(this.ConfidenceList_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(292, 128);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(64, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "Confidence:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ResponderTextBox
            // 
            this.ResponderTextBox.Location = new System.Drawing.Point(362, 73);
            this.ResponderTextBox.Name = "ResponderTextBox";
            this.ResponderTextBox.Size = new System.Drawing.Size(128, 20);
            this.ResponderTextBox.TabIndex = 6;
            this.ResponderTextBox.Text = "e.g. Joe Bloggs";
            this.ResponderTextBox.TextChanged += new System.EventHandler(this.ResponderTextBox_TextChanged);
            this.ResponderTextBox.Enter += new System.EventHandler(this.ResponderTextBox_Enter);
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(294, 76);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(62, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Responder:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(287, 50);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Reported By:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReportedByTextBox
            // 
            this.ReportedByTextBox.Location = new System.Drawing.Point(362, 47);
            this.ReportedByTextBox.Name = "ReportedByTextBox";
            this.ReportedByTextBox.Size = new System.Drawing.Size(128, 20);
            this.ReportedByTextBox.TabIndex = 4;
            this.ReportedByTextBox.Text = "e.g. \"Countercept\"";
            this.ReportedByTextBox.TextChanged += new System.EventHandler(this.ReportedByTextBox_TextChanged);
            this.ReportedByTextBox.Enter += new System.EventHandler(this.ReportedByTextBox_Enter);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 158);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Incident Resolved:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // IncidentResolvedDatePicker
            // 
            this.IncidentResolvedDatePicker.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.IncidentResolvedDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.IncidentResolvedDatePicker.Location = new System.Drawing.Point(125, 154);
            this.IncidentResolvedDatePicker.Name = "IncidentResolvedDatePicker";
            this.IncidentResolvedDatePicker.ShowCheckBox = true;
            this.IncidentResolvedDatePicker.Size = new System.Drawing.Size(149, 20);
            this.IncidentResolvedDatePicker.TabIndex = 12;
            this.IncidentResolvedDatePicker.Value = new System.DateTime(2017, 1, 1, 0, 0, 0, 0);
            this.IncidentResolvedDatePicker.ValueChanged += new System.EventHandler(this.IncidentResolvedDatePicker_ValueChanged);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 132);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Incident Reported:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // IncidentReportedDatePicker
            // 
            this.IncidentReportedDatePicker.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.IncidentReportedDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.IncidentReportedDatePicker.Location = new System.Drawing.Point(125, 128);
            this.IncidentReportedDatePicker.Name = "IncidentReportedDatePicker";
            this.IncidentReportedDatePicker.ShowCheckBox = true;
            this.IncidentReportedDatePicker.Size = new System.Drawing.Size(149, 20);
            this.IncidentReportedDatePicker.TabIndex = 10;
            this.IncidentReportedDatePicker.Value = new System.DateTime(2017, 1, 1, 0, 0, 0, 0);
            this.IncidentReportedDatePicker.ValueChanged += new System.EventHandler(this.IncidentReportedDatePicker_ValueChanged);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Incident Discovered:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // IncidentDiscoveredDatePicker
            // 
            this.IncidentDiscoveredDatePicker.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.IncidentDiscoveredDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.IncidentDiscoveredDatePicker.Location = new System.Drawing.Point(125, 102);
            this.IncidentDiscoveredDatePicker.Name = "IncidentDiscoveredDatePicker";
            this.IncidentDiscoveredDatePicker.ShowCheckBox = true;
            this.IncidentDiscoveredDatePicker.Size = new System.Drawing.Size(149, 20);
            this.IncidentDiscoveredDatePicker.TabIndex = 8;
            this.IncidentDiscoveredDatePicker.Value = new System.DateTime(2017, 1, 1, 0, 0, 0, 0);
            this.IncidentDiscoveredDatePicker.ValueChanged += new System.EventHandler(this.IncidentDiscoveredDatePicker_ValueChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Initial Compromise:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // InitialCompromiseDatePicker
            // 
            this.InitialCompromiseDatePicker.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.InitialCompromiseDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.InitialCompromiseDatePicker.Location = new System.Drawing.Point(125, 77);
            this.InitialCompromiseDatePicker.Name = "InitialCompromiseDatePicker";
            this.InitialCompromiseDatePicker.ShowCheckBox = true;
            this.InitialCompromiseDatePicker.Size = new System.Drawing.Size(149, 20);
            this.InitialCompromiseDatePicker.TabIndex = 5;
            this.InitialCompromiseDatePicker.Value = new System.DateTime(2017, 1, 1, 0, 0, 0, 0);
            this.InitialCompromiseDatePicker.ValueChanged += new System.EventHandler(this.InitialCompromiseDatePicker_ValueChanged);
            // 
            // IndicatorsGroupBox
            // 
            this.IndicatorsGroupBox.Controls.Add(this.VisualiseButton);
            this.IndicatorsGroupBox.Controls.Add(this.CSVLoadButton);
            this.IndicatorsGroupBox.Controls.Add(this.IndicatorDetailsPanel);
            this.IndicatorsGroupBox.Controls.Add(this.ObservablesListBox);
            this.IndicatorsGroupBox.Controls.Add(this.DeleteIndicatorButton);
            this.IndicatorsGroupBox.Controls.Add(this.LinkIndicatorButton);
            this.IndicatorsGroupBox.Controls.Add(this.EditIndicatorButton);
            this.IndicatorsGroupBox.Controls.Add(this.AddIndicatorButton);
            this.IndicatorsGroupBox.Location = new System.Drawing.Point(12, 239);
            this.IndicatorsGroupBox.Name = "IndicatorsGroupBox";
            this.IndicatorsGroupBox.Size = new System.Drawing.Size(764, 389);
            this.IndicatorsGroupBox.TabIndex = 5;
            this.IndicatorsGroupBox.TabStop = false;
            this.IndicatorsGroupBox.Text = "Observables";
            // 
            // VisualiseButton
            // 
            this.VisualiseButton.BackColor = System.Drawing.SystemColors.Window;
            this.VisualiseButton.BackgroundImage = global::Athena.Properties.Resources.Tree_Structure_25;
            this.VisualiseButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VisualiseButton.FlatAppearance.BorderSize = 0;
            this.VisualiseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VisualiseButton.Location = new System.Drawing.Point(437, 19);
            this.VisualiseButton.Name = "VisualiseButton";
            this.VisualiseButton.Size = new System.Drawing.Size(25, 25);
            this.VisualiseButton.TabIndex = 20;
            this.VisualiseButton.UseVisualStyleBackColor = false;
            this.VisualiseButton.Click += new System.EventHandler(this.VisualiseButton_Click);
            // 
            // CSVLoadButton
            // 
            this.CSVLoadButton.BackColor = System.Drawing.SystemColors.Window;
            this.CSVLoadButton.BackgroundImage = global::Athena.Properties.Resources.CSV_25;
            this.CSVLoadButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CSVLoadButton.FlatAppearance.BorderSize = 0;
            this.CSVLoadButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CSVLoadButton.Location = new System.Drawing.Point(310, 19);
            this.CSVLoadButton.Name = "CSVLoadButton";
            this.CSVLoadButton.Size = new System.Drawing.Size(25, 25);
            this.CSVLoadButton.TabIndex = 19;
            this.CSVLoadButton.UseVisualStyleBackColor = false;
            this.CSVLoadButton.Click += new System.EventHandler(this.CSVLoadButton_Click);
            // 
            // IndicatorDetailsPanel
            // 
            this.IndicatorDetailsPanel.AutoScroll = true;
            this.IndicatorDetailsPanel.Location = new System.Drawing.Point(278, 54);
            this.IndicatorDetailsPanel.Name = "IndicatorDetailsPanel";
            this.IndicatorDetailsPanel.Size = new System.Drawing.Size(479, 320);
            this.IndicatorDetailsPanel.TabIndex = 18;
            // 
            // ObservablesListBox
            // 
            this.ObservablesListBox.FormattingEnabled = true;
            this.ObservablesListBox.Location = new System.Drawing.Point(15, 19);
            this.ObservablesListBox.Name = "ObservablesListBox";
            this.ObservablesListBox.Size = new System.Drawing.Size(248, 355);
            this.ObservablesListBox.TabIndex = 13;
            this.ObservablesListBox.SelectedIndexChanged += new System.EventHandler(this.ObservablesListBox_SelectedIndexChanged);
            // 
            // DeleteIndicatorButton
            // 
            this.DeleteIndicatorButton.BackColor = System.Drawing.SystemColors.Window;
            this.DeleteIndicatorButton.BackgroundImage = global::Athena.Properties.Resources.Delete_25;
            this.DeleteIndicatorButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.DeleteIndicatorButton.FlatAppearance.BorderSize = 0;
            this.DeleteIndicatorButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DeleteIndicatorButton.Location = new System.Drawing.Point(342, 19);
            this.DeleteIndicatorButton.Name = "DeleteIndicatorButton";
            this.DeleteIndicatorButton.Size = new System.Drawing.Size(25, 25);
            this.DeleteIndicatorButton.TabIndex = 15;
            this.DeleteIndicatorButton.UseVisualStyleBackColor = false;
            this.DeleteIndicatorButton.Click += new System.EventHandler(this.DeleteIndicatorButton_Click);
            // 
            // LinkIndicatorButton
            // 
            this.LinkIndicatorButton.BackColor = System.Drawing.SystemColors.Window;
            this.LinkIndicatorButton.BackgroundImage = global::Athena.Properties.Resources.Link_25;
            this.LinkIndicatorButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.LinkIndicatorButton.FlatAppearance.BorderSize = 0;
            this.LinkIndicatorButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LinkIndicatorButton.Location = new System.Drawing.Point(406, 19);
            this.LinkIndicatorButton.Name = "LinkIndicatorButton";
            this.LinkIndicatorButton.Size = new System.Drawing.Size(25, 25);
            this.LinkIndicatorButton.TabIndex = 17;
            this.LinkIndicatorButton.UseVisualStyleBackColor = false;
            this.LinkIndicatorButton.Click += new System.EventHandler(this.LinkIndicatorButton_Click);
            // 
            // EditIndicatorButton
            // 
            this.EditIndicatorButton.BackColor = System.Drawing.SystemColors.Window;
            this.EditIndicatorButton.BackgroundImage = global::Athena.Properties.Resources.Edit_Property_25;
            this.EditIndicatorButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.EditIndicatorButton.FlatAppearance.BorderSize = 0;
            this.EditIndicatorButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.EditIndicatorButton.Location = new System.Drawing.Point(374, 19);
            this.EditIndicatorButton.Name = "EditIndicatorButton";
            this.EditIndicatorButton.Size = new System.Drawing.Size(25, 25);
            this.EditIndicatorButton.TabIndex = 16;
            this.EditIndicatorButton.UseVisualStyleBackColor = false;
            this.EditIndicatorButton.Click += new System.EventHandler(this.EditIndicatorButton_Click);
            // 
            // AddIndicatorButton
            // 
            this.AddIndicatorButton.BackColor = System.Drawing.SystemColors.Window;
            this.AddIndicatorButton.BackgroundImage = global::Athena.Properties.Resources.Add_251;
            this.AddIndicatorButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.AddIndicatorButton.FlatAppearance.BorderSize = 0;
            this.AddIndicatorButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddIndicatorButton.Location = new System.Drawing.Point(278, 19);
            this.AddIndicatorButton.Name = "AddIndicatorButton";
            this.AddIndicatorButton.Size = new System.Drawing.Size(25, 25);
            this.AddIndicatorButton.TabIndex = 14;
            this.AddIndicatorButton.UseVisualStyleBackColor = false;
            this.AddIndicatorButton.Click += new System.EventHandler(this.AddIndicatorButton_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(788, 640);
            this.Controls.Add(this.IndicatorsGroupBox);
            this.Controls.Add(this.OverviewGroupBox);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Athena";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.OverviewGroupBox.ResumeLayout(false);
            this.OverviewGroupBox.PerformLayout();
            this.IndicatorsGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sTIXToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jSONToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox IncidentNameTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox IncidentDescriptionTextBox;
        private System.Windows.Forms.GroupBox OverviewGroupBox;
        private System.Windows.Forms.GroupBox IndicatorsGroupBox;
        private System.Windows.Forms.Button AddIndicatorButton;
        private System.Windows.Forms.Button EditIndicatorButton;
        private System.Windows.Forms.Button LinkIndicatorButton;
        private System.Windows.Forms.Button DeleteIndicatorButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker InitialCompromiseDatePicker;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox ReportedByTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker IncidentResolvedDatePicker;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker IncidentReportedDatePicker;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker IncidentDiscoveredDatePicker;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox IncidentTitleTextBox;
        private System.Windows.Forms.ComboBox ConfidenceList;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox ResponderTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ListBox ObservablesListBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox ReportCompanyTextBox;
        private System.Windows.Forms.Panel IndicatorDetailsPanel;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox IncidentEffectDropdown;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.Button CSVLoadButton;
        private System.Windows.Forms.Button VisualiseButton;
    }
}

