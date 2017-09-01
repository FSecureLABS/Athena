namespace Athena
{
    partial class AddIndicatorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddIndicatorForm));
            this.label1 = new System.Windows.Forms.Label();
            this.IndicatorTypeDropDownBox = new System.Windows.Forms.ComboBox();
            this.SaveButton = new System.Windows.Forms.Button();
            this.SaveAndNewButton = new System.Windows.Forms.Button();
            this.CancelNewButton = new System.Windows.Forms.Button();
            this.IndicatorFieldsPanel = new System.Windows.Forms.Panel();
            this.GenerateHashesButton = new System.Windows.Forms.Button();
            this.GenerateHashesProgressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Observable Type:";
            // 
            // IndicatorTypeDropDownBox
            // 
            this.IndicatorTypeDropDownBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.IndicatorTypeDropDownBox.FormattingEnabled = true;
            this.IndicatorTypeDropDownBox.Location = new System.Drawing.Point(109, 23);
            this.IndicatorTypeDropDownBox.Name = "IndicatorTypeDropDownBox";
            this.IndicatorTypeDropDownBox.Size = new System.Drawing.Size(194, 21);
            this.IndicatorTypeDropDownBox.TabIndex = 1;
            this.IndicatorTypeDropDownBox.SelectedIndexChanged += new System.EventHandler(this.IndicatorTypeDropDownBox_SelectedIndexChanged);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(375, 360);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(88, 23);
            this.SaveButton.TabIndex = 2;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // SaveAndNewButton
            // 
            this.SaveAndNewButton.Location = new System.Drawing.Point(469, 360);
            this.SaveAndNewButton.Name = "SaveAndNewButton";
            this.SaveAndNewButton.Size = new System.Drawing.Size(88, 23);
            this.SaveAndNewButton.TabIndex = 3;
            this.SaveAndNewButton.Text = "Save and New";
            this.SaveAndNewButton.UseVisualStyleBackColor = true;
            this.SaveAndNewButton.Click += new System.EventHandler(this.SaveAndNewButton_Click);
            // 
            // CancelNewButton
            // 
            this.CancelNewButton.Location = new System.Drawing.Point(15, 360);
            this.CancelNewButton.Name = "CancelNewButton";
            this.CancelNewButton.Size = new System.Drawing.Size(88, 23);
            this.CancelNewButton.TabIndex = 4;
            this.CancelNewButton.Text = "Cancel";
            this.CancelNewButton.UseVisualStyleBackColor = true;
            this.CancelNewButton.Click += new System.EventHandler(this.button3_Click);
            // 
            // IndicatorFieldsPanel
            // 
            this.IndicatorFieldsPanel.Location = new System.Drawing.Point(15, 63);
            this.IndicatorFieldsPanel.Name = "IndicatorFieldsPanel";
            this.IndicatorFieldsPanel.Size = new System.Drawing.Size(542, 291);
            this.IndicatorFieldsPanel.TabIndex = 5;
            // 
            // GenerateHashesButton
            // 
            this.GenerateHashesButton.Location = new System.Drawing.Point(392, 21);
            this.GenerateHashesButton.Name = "GenerateHashesButton";
            this.GenerateHashesButton.Size = new System.Drawing.Size(161, 23);
            this.GenerateHashesButton.TabIndex = 6;
            this.GenerateHashesButton.Text = "Generate Hashes of a File";
            this.GenerateHashesButton.UseVisualStyleBackColor = true;
            this.GenerateHashesButton.Click += new System.EventHandler(this.GenerateHashesButton_Click);
            // 
            // GenerateHashesProgressBar
            // 
            this.GenerateHashesProgressBar.Location = new System.Drawing.Point(392, 47);
            this.GenerateHashesProgressBar.Name = "GenerateHashesProgressBar";
            this.GenerateHashesProgressBar.Size = new System.Drawing.Size(161, 10);
            this.GenerateHashesProgressBar.TabIndex = 7;
            // 
            // AddIndicatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 395);
            this.Controls.Add(this.GenerateHashesProgressBar);
            this.Controls.Add(this.GenerateHashesButton);
            this.Controls.Add(this.IndicatorFieldsPanel);
            this.Controls.Add(this.CancelNewButton);
            this.Controls.Add(this.SaveAndNewButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.IndicatorTypeDropDownBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddIndicatorForm";
            this.Text = "Edit Observable";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox IndicatorTypeDropDownBox;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button SaveAndNewButton;
        private System.Windows.Forms.Button CancelNewButton;
        private System.Windows.Forms.Panel IndicatorFieldsPanel;
        private System.Windows.Forms.Button GenerateHashesButton;
        private System.Windows.Forms.ProgressBar GenerateHashesProgressBar;
    }
}