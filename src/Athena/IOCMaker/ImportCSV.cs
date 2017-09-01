using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using GenericParsing;

namespace Athena
{

    /// <summary>
    /// This form is the GUI for the importing of data from a CSV into the native IOC format
    /// used by this application
    /// </summary>
    public partial class ImportCSV : Form
    {
        ObservableCollection Collection; //Holds a reference to the collecton of the observables in the main window
        List<ObservableObject> ObservableTypes; //Holds a template of the observable types - domain, url etc
        List<ObservableObject> Observables; //Holds the observables imported from CSV before they are "saved" to the main window
        List<string> Errors; //Holds a list of errors encountered when loading a CSV

        public ImportCSV(ref ObservableCollection collection)
        {
            InitializeComponent();
            Collection = collection; //Save the reference
            ObservableTypes = new List<ObservableObject>();
            Observables = new List<ObservableObject>();
            Errors = new List<string>();

            //Get a dummy object for each type of obserable
            foreach (ObservableObject.ObservableType type in Enum.GetValues(typeof(ObservableObject.ObservableType)))
            {
                ObservableTypes.Add(new ObservableObject(type, ref Collection));
            }
        }

        /// <summary>
        /// Shows the file selection diag to allow user to select the csv to import
        /// </summary>
        private void BrowseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            diag.Filter = "CSV Files (*.csv)|*.csv";
            if (diag.ShowDialog() == DialogResult.OK)
            {
                FilePathTextBox.Text = diag.FileName;             
            }
        }

        //When the "load/validate" button is clicked
        private void LoadButton_Click(object sender, EventArgs e)
        {
            ParseCSV(FilePathTextBox.Text); //Parse the CSV (this function generates errors as needed)
        }

        /// <summary>
        /// Adds an error to the list of errors, if the advisory only is set to true, the error will only be added to the console
        /// and wont count towards the error count
        /// </summary>
        /// <param name="error">The message for the error</param>
        /// <param name="advisoryonly">Is this only a warning?</param>
        private void AddError(string error,bool advisoryonly=false)
        {
            if(!advisoryonly) Errors.Add(error);
            ProgressTextBox.AppendText(error+"\r\n");
           
        }


        /// <summary>
        /// Parses a CSV file and loads the observables to the temporary Observables list
        /// </summary>
        /// <param name="filepath">The path to the CSV</param>
        private void ParseCSV(string filepath)
        {
            //If the path is empty, invalid or the file doesn't exist, stop
            if (string.IsNullOrWhiteSpace(filepath) || !File.Exists(filepath)) return;

            //Master try to catch any other uncaught issues
            try
            {
                //GenericParser written by AndrewRissing - https://github.com/AndrewRissing/GenericParsing
                using (GenericParser parser = new GenericParser())
                {
                    //Set the params of the parser
                    parser.SetDataSource(filepath);
                    parser.ColumnDelimiter = ',';
                    parser.FirstRowHasHeader = true;
                    parser.TextQualifier = '\"';

                    //Start looping through the file line by line
                    int line = 0;
                    while (parser.Read())
                    {
                        line++;
                        try //Try per line
                        {
                            //Add the parsed columns to our dictionary of name/value pairs
                            //Column order is expected to be as per strict template order
                            //TODO: Make more flexible and allow different column orders
                            Dictionary<string, string> values = new Dictionary<string, string>();
                            for (int i = 0; i < parser.ColumnCount; i++)
                            {
                                values.Add(parser.GetColumnName(i), parser[i]);
                            }
                          

                            //Create a new observable object from the values read in this line
                            ObservableObject newobs = null;
                            string errors = GenerateObservable(values, out newobs);
                            
                            //If generating the observable gave any errors, add them to the error list
                            if (errors != null) AddError(errors);
                            else
                            {
                                //Otherwise add this new observable to our storage list
                                Observables.Add(newobs);
                            }
                        }
                        catch (Exception exep)
                        {
                            //This will likely occur if columns are missing or have been renamed etc in the template
                            MessageBox.Show("There was an error parsing the CSV at line " + line + ".\r\n\r\n Please make sure it is in correct format - check the template provided.");
                            return;
                        }
                    }
                }
            }
            catch (Exception exep)
            {
                //Should only occur due to unforseen IO errors like the file being in use etc
                MessageBox.Show("There was an error reading the CSV:\r\n"+ exep.Message);
                return;
            }

            //Try to add the observables to the GUI
            try
            {
                //Clear the box for now and set the displaymember to the right property
                ObservablesListBox.Items.Clear();
                ObservablesListBox.DisplayMember = "DisplayTitle";
                
                //Add each observable to the listbox
                foreach (ObservableObject obs in Observables) ObservablesListBox.Items.Add(obs);
                
                //Refresh the listbox
                ObservablesListBox.Refresh();
            }
            catch(Exception exep)
            {
                //Throw an exception here if there is an error updating the GUI, shouldn't happen under normal conditions
                MessageBox.Show("There was an error reading the CSV:\r\n"+ exep.Message);
                return;
            }            
        }



        /// <summary>
        /// Takes <name,value> pairs and converts them into observable objects. Names must match the propeties of that observable type
        /// </summary>
        /// <param name="data"></param>
        /// <param name="obs"></param>
        /// <returns></returns>
        public string GenerateObservable(Dictionary<string,string> data,out ObservableObject obs)
        {
            obs = null;
            //Check if this is a valid observable type
            if (!data.ContainsKey("ObservableType") || string.IsNullOrWhiteSpace(data["ObservableType"])) return "Row did not contain an \"Observable Type\" column or ObservableType was empty";
            else
            {
                //Check again
                ObservableObject xx = ObservableTypes.Find(x => x.FriendlyTypeName == data["ObservableType"]);
                if (xx == null) return data["ObservableType"] + " is not a valid ObservableType";
                else
                {
                    obs = new ObservableObject(xx.Type, ref Collection);
                    try
                    {
                        //Translate the name/value pairs into the fields on the object
                        foreach (ObservableObjectField f in obs.Fields)
                        {
                            if (data.ContainsKey(f.FieldName))
                            {
                                obs.Fields.Find(x => x.FieldName == f.FieldName).Value = data[f.FieldName];
                            }
                        }
                    }
                    catch(Exception exep)
                    {
                        //If we get an error here, then there is likely an issue with the CSV column naming
                        return "Error processing fields, import may not be complete for this line";
                    }

                }
            }
            return null;
        }

        /// <summary>
        /// Dynamically Generates and opens a CSV template based on the STIXObservable types available
        /// </summary>
        private void TemplateLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            StringBuilder headers = new StringBuilder();
            StringBuilder rows = new StringBuilder();
            
            //Get Unique Fields for each observable type
            Dictionary<string, string> fields = new Dictionary<string, string>();
            foreach(ObservableObject o in ObservableTypes)
            {               
                foreach (ObservableObjectField f in o.Fields) fields[f.FieldName] = "";
            }

            //Print the fields as column headers, remove last comma, add to the output text
            int columncounter = 1;
            headers.Append("ObservableType" + ",");
            foreach (string s in fields.Keys)
            {
                headers.Append(s + ",");
                columncounter++;
            }
            headers.Remove(headers.Length - 1, 1);
            rows.AppendLine();


            //Add three examples for each observable type to the template
             foreach(ObservableObject o in ObservableTypes)
             {
                 rows.AppendLine(o.FriendlyTypeName+ returncommas(columncounter));
                 rows.AppendLine(o.FriendlyTypeName + returncommas(columncounter));
                 rows.AppendLine(o.FriendlyTypeName + returncommas(columncounter));
             }
            
            //Get the output path - current directory and hardcoded filename
            string path = Path.Combine(Directory.GetCurrentDirectory(),"IOCCSVTemplate.csv");
            
            //Write the file
            using(StreamWriter w = new StreamWriter(path))
            {
                w.Write(headers.ToString() + rows.ToString());
            }

            //Open the file
            System.Diagnostics.Process.Start(path);
        }


        /// <summary>
        /// Returns a number of empty columns to help with building a csv
        /// </summary>
        /// <param name="number">The number of empty columns (commas) to return</param>
        /// <returns>S string representing the CSV representation of empty commans</returns>
        private string returncommas(int number)
        {
            string rtn ="";
            while (number > 0) 
            {
                rtn += ",";
                number--;
            }
            return rtn;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            foreach(ObservableObject o in Observables)
            {
                Collection.Observables.Add(o);
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
