using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Security.Cryptography;


namespace Athena
{
    public partial class AddIndicatorForm : Form
    {
        public bool openagain;
        public bool clearform;
        bool editmode;
        List<ObservableObject> observabletypes;

        //The indicator that we created
        public ObservableObject CreatedObservable;

        public AddIndicatorForm(ref ObservableCollection stixcollection,ObservableObject editobservable)
        {
            InitializeComponent();

            editmode = true;
          //  SaveAndCopy.Enabled = false;
            SaveAndNewButton.Enabled = false;

            observabletypes = new List<ObservableObject>();
            observabletypes.Add(editobservable);
            IndicatorTypeDropDownBox.Items.Add(editobservable.FriendlyTypeName);
            IndicatorTypeDropDownBox.SelectedIndex = 0;
            IndicatorTypeDropDownBox.Enabled = false;
            GenerateHashesButton.Visible = false;
            GenerateHashesProgressBar.Visible = false;

        }

        public AddIndicatorForm(ref ObservableCollection stixcollection)
        {
            InitializeComponent();
            editmode = false;
            openagain = false;
            clearform = true;
            observabletypes = new List<ObservableObject>();
            GenerateHashesButton.Visible = false;
            GenerateHashesProgressBar.Visible = false;
            //Get a dummy object for each type
            foreach (ObservableObject.ObservableType type in Enum.GetValues(typeof(ObservableObject.ObservableType)))
            {
                observabletypes.Add(new ObservableObject(type,ref stixcollection));
                IndicatorTypeDropDownBox.Items.Add(observabletypes.Last().FriendlyTypeName);
            }
        }

     

        private void IndicatorTypeDropDownBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            IndicatorFieldsPanel.Controls.Clear();
            Point location = new Point(0,30);
            int xspacer = 10;
            int yspacer = 10;
            int maxlabelwidth = 0;

            //Display the "generate hashes" button if we are adding a malware sample
            if (observabletypes.Find(x => x.FriendlyTypeName == IndicatorTypeDropDownBox.SelectedItem.ToString()).FriendlyTypeName == "Malware Sample")
            {
                GenerateHashesButton.Visible = true;
                GenerateHashesButton.Enabled = true;
                //GenerateHashesProgressBar.Visible = false;
            }
            else
            {
                GenerateHashesButton.Visible = false;
                GenerateHashesButton.Enabled = false;
                GenerateHashesProgressBar.Visible = false;
            }


            //Add the ID of this observable to the screen
            Label label = new Label();           
            label.Text = observabletypes.Find(x => x.FriendlyTypeName == IndicatorTypeDropDownBox.SelectedItem.ToString()).ID;
            label.Name = "IDLabel";
            label.Width = labelwidth(label);
            IndicatorFieldsPanel.Controls.Add(label);
            


            //Get the maximum label width, to keep controls in line
            foreach (ObservableObjectField F in observabletypes.Find(x => x.FriendlyTypeName == IndicatorTypeDropDownBox.SelectedItem.ToString()).Fields)
            {
                Label lbl = new Label();
                lbl.Text = F.FieldName;
                lbl.Width = labelwidth(lbl);
                if (maxlabelwidth < lbl.Width) maxlabelwidth = lbl.Width;
            }

            //Special line to add a "GetDetailsFromSampleFile" button
            if (observabletypes.Find(x => x.FriendlyTypeName == IndicatorTypeDropDownBox.SelectedItem.ToString()).FriendlyTypeName == "Malware Sample")
            {
                GenerateHashesButton.Visible = true;
            }
            else
            {
                GenerateHashesButton.Visible = false;
            }


            foreach (ObservableObjectField F in observabletypes.Find(x => x.FriendlyTypeName == IndicatorTypeDropDownBox.SelectedItem.ToString()).Fields)
            {
                Label lbl = new Label();
                lbl.Text = F.FieldName + ":";
                lbl.Name = F.FieldName.Replace(" ","") + "_Label";
                lbl.Width = labelwidth(lbl);
                IndicatorFieldsPanel.Controls.Add(lbl);

                //Set the location of the label
                lbl.Location = location;

                if(F.FieldType == ObservableObjectField.GUIFieldType.TextBox || F.FieldType == ObservableObjectField.GUIFieldType.LongTextBox)
                {
                    TextBox txt = new TextBox();
                    txt.Name = F.FieldName.Replace(" ", "");
                    if (F.FieldType == ObservableObjectField.GUIFieldType.LongTextBox)
                    {
                        txt.Multiline = true;
                        txt.Width = 400;
                        txt.Height = 75;
                    }
                    else
                    {
                        txt.Width = 400;
                       // txt.Height = 50;
                    }
                    location.X = maxlabelwidth + xspacer;
                    txt.Location = location;

                    if (editmode) txt.Text = F.Value;


                    IndicatorFieldsPanel.Controls.Add(txt);
                    location.X = 0;
                    location.Y += txt.Height + yspacer;
                }
                else if(F.FieldType == ObservableObjectField.GUIFieldType.DropDown)
                {
                    ComboBox txt = new ComboBox();
                    txt.DropDownStyle = ComboBoxStyle.DropDownList;
                    txt.Name = F.FieldName.Replace(" ", "");
                    txt.Width = 400;

                    foreach (string s in F.DropDownOptions) txt.Items.Add(s);

                    if (editmode) txt.SelectedItem = F.Value;


                    location.X = maxlabelwidth + xspacer;
                    txt.Location = location;                   
                    IndicatorFieldsPanel.Controls.Add(txt);
                    location.X = 0;
                    location.Y += txt.Height + yspacer;
                }

                
            }
        }

        private int labelwidth(Label lbl)
        {
            using (Graphics g = CreateGraphics())
            {
                SizeF size = g.MeasureString(lbl.Text, lbl.Font);
                return (int)size.Width+5;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == MessageBox.Show("Discard this information?", "Discard", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void SaveAndNewButton_Click(object sender, EventArgs e)
        {
            SaveandReturn(true,true);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveandReturn();
        }

        private void GenerateIDs()
        {

        }

        private void SaveandReturn(bool another = false,bool clean = false)
        {
            if (IndicatorTypeDropDownBox.SelectedItem == null) return;


            //Create a stix object and get the values from the typed data
            ObservableObject newobs = observabletypes.Find(x => x.FriendlyTypeName == IndicatorTypeDropDownBox.SelectedItem.ToString());
            for(int i=0;i<newobs.Fields.Count;i++)
            {
                string value = "";
                if (newobs.Fields[i].FieldType == ObservableObjectField.GUIFieldType.LongTextBox || newobs.Fields[i].FieldType == ObservableObjectField.GUIFieldType.TextBox)
                {
                    value = IndicatorFieldsPanel.Controls.Find(newobs.Fields[i].FieldName.Replace(" ", ""), true).First().Text;                    
                }
                if (newobs.Fields[i].FieldType == ObservableObjectField.GUIFieldType.DropDown)
                {
                    try
                    {
                        value = ((ComboBox)IndicatorFieldsPanel.Controls.Find(newobs.Fields[i].FieldName.Replace(" ", ""), true).First()).SelectedItem.ToString();
                    }
                    catch
                    {
                        value = "";
                    }
                }
                newobs.Fields[i].Value = value;
            }

            clearform = clean;
            openagain = another;
            CreatedObservable = newobs;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void SaveAndCopy_Click(object sender, EventArgs e)
        {
            SaveandReturn(true);
        }



        internal void regenerateIDs(ref ObservableCollection collection)
        {
            foreach(ObservableObject x in observabletypes)
            {
                x.NewID(ref collection);
            }
        }

        private void GenerateHashesButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            if(diag.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Thread filehashthread = new Thread(() => PopulateFileInfo(diag.FileName));
                    filehashthread.Start();
                }
                catch
                {
                    GenerateHashesProgressBar.Visible = false;
                    MessageBox.Show("Error starting to hash file");
                }
            }
        }

        private void PopulateFileInfo(string filepath)
        {
            try
            {
                IndicatorFieldsPanel.BeginInvoke((Action)delegate
                {
                    GenerateHashesProgressBar.Visible = true;
                    GenerateHashesProgressBar.Maximum = 100;
                    GenerateHashesProgressBar.Value = 10;
                });
                string md5 = getMD5(filepath);
                IndicatorFieldsPanel.BeginInvoke((Action)delegate
                {
                    GenerateHashesProgressBar.Value = 50;
                });
                string sha1 = getSHA1(filepath);
                IndicatorFieldsPanel.BeginInvoke((Action)delegate
                {
                    GenerateHashesProgressBar.Value = 90;
                });


                string filename = Path.GetFileName(filepath);


                IndicatorFieldsPanel.BeginInvoke((Action)delegate
               {
                   ((TextBox)IndicatorFieldsPanel.Controls.Find("File_Name", false).First()).Text = filename;
                   ((TextBox)IndicatorFieldsPanel.Controls.Find("Hashes.MD5", false).First()).Text = md5;
                   ((TextBox)IndicatorFieldsPanel.Controls.Find("Hashes.SHA1", false).First()).Text = sha1;
                   if( ((TextBox)IndicatorFieldsPanel.Controls.Find("Title", false).First()).Text == "")
                   {
                       ((TextBox)IndicatorFieldsPanel.Controls.Find("Title", false).First()).Text = filename;
                   }
                   GenerateHashesProgressBar.Visible = false;
               });

                IndicatorFieldsPanel.BeginInvoke((Action)delegate
                {
                    GenerateHashesProgressBar.Value = 0;
                });
            }
            catch 
            {
                MessageBox.Show("Error hashing file");
            }
        
        }


        private string getMD5(string filepath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filepath))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "‌​").ToLower();
                }
            }
        }

        private string getSHA1(string filepath)
        {
            try
            {
                using (FileStream stream = File.OpenRead(filepath))
                {
                    using (SHA1Managed sha = new SHA1Managed())
                    {
                        byte[] checksum = sha.ComputeHash(stream);
                        return BitConverter.ToString(checksum)
                            .Replace("-", string.Empty).ToLower();
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return "";
            }
        }
    }
}
