namespace Athena
{
    partial class ImportCSV
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportCSV));
            this.label1 = new System.Windows.Forms.Label();
            this.TemplateLink = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.FilePathTextBox = new System.Windows.Forms.TextBox();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.LoadButton = new System.Windows.Forms.Button();
            this.ProgressTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ObservablesListBox = new System.Windows.Forms.ListBox();
            this.SaveButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(482, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "You can import a bulk load of Indicators quickly from a csv. This needs to be in " +
    "a specifc format, use ";
            // 
            // TemplateLink
            // 
            this.TemplateLink.AutoSize = true;
            this.TemplateLink.Location = new System.Drawing.Point(488, 13);
            this.TemplateLink.Name = "TemplateLink";
            this.TemplateLink.Size = new System.Drawing.Size(105, 13);
            this.TemplateLink.TabIndex = 1;
            this.TemplateLink.TabStop = true;
            this.TemplateLink.Text = "this template csv file.";
            this.TemplateLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.TemplateLink_LinkClicked);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "CSV File:";
            // 
            // FilePathTextBox
            // 
            this.FilePathTextBox.Location = new System.Drawing.Point(67, 55);
            this.FilePathTextBox.Name = "FilePathTextBox";
            this.FilePathTextBox.Size = new System.Drawing.Size(418, 20);
            this.FilePathTextBox.TabIndex = 3;
            // 
            // BrowseButton
            // 
            this.BrowseButton.Location = new System.Drawing.Point(491, 53);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(103, 23);
            this.BrowseButton.TabIndex = 4;
            this.BrowseButton.Text = "Browse";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // LoadButton
            // 
            this.LoadButton.Location = new System.Drawing.Point(212, 81);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(229, 23);
            this.LoadButton.TabIndex = 5;
            this.LoadButton.Text = "Load/Validate";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // ProgressTextBox
            // 
            this.ProgressTextBox.Location = new System.Drawing.Point(12, 133);
            this.ProgressTextBox.Multiline = true;
            this.ProgressTextBox.Name = "ProgressTextBox";
            this.ProgressTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ProgressTextBox.Size = new System.Drawing.Size(343, 147);
            this.ProgressTextBox.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Progress/Errors:";
            // 
            // ObservablesListBox
            // 
            this.ObservablesListBox.FormattingEnabled = true;
            this.ObservablesListBox.Location = new System.Drawing.Point(361, 133);
            this.ObservablesListBox.Name = "ObservablesListBox";
            this.ObservablesListBox.Size = new System.Drawing.Size(233, 147);
            this.ObservablesListBox.TabIndex = 8;
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(285, 286);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(308, 23);
            this.SaveButton.TabIndex = 9;
            this.SaveButton.Text = "Save/Close";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(358, 114);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(168, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Loaded Observables: (Not Saved)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(570, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "The column order isn\'t important, but the column names and \"ObservableType\" names" +
    " must match the template exactly.";
            // 
            // ImportCSV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 317);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.ObservablesListBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ProgressTextBox);
            this.Controls.Add(this.LoadButton);
            this.Controls.Add(this.BrowseButton);
            this.Controls.Add(this.FilePathTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TemplateLink);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ImportCSV";
            this.Text = "ImportCSV";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel TemplateLink;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox FilePathTextBox;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.Button LoadButton;
        private System.Windows.Forms.TextBox ProgressTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox ObservablesListBox;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}