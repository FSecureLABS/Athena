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

namespace Athena
{
    /// <summary>
    /// This is the main window which the user interacts with an the entry point of the program
    /// </summary>
    public partial class MainWindow : Form
    {
        //The main object holding all our IOCs. This a custom built representation of a small subset
        //STIX objects and concepts - but made generic enough to export to other formats
        public ObservableCollection ObsCollection;

        //Used to delay updates to the STIXCollection until the user finishes typing etc
        System.Windows.Forms.Timer GuiUpdateTimer;

        //Used to signal when GUI updates should be suppressed (e.g. we are programatically updating values
        // and dont wan't the updates to fire)
        private bool SuppressGUIEvents;

        //Used as a handle to dynamically created list showing relationships
        ListBox Relationshipslistbox;

        //Holds the path of the currently open file - allows to supress prompting the user for the file
        // to save to.
        string savepath;

        //Used to show when there are changed which have been made which have not been saved to a file
        public bool UnsavedChanges { get; set; }

        //Show the about information when the user clicks help -> about
        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Version 0.5\r\nLast Updated: 31 Aug 2017", "About Athena", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //A Panel used to hide the controls on the form when there isnt a collection open
        Panel lockpannel;

        /// <summary>
        /// Default contructor for our main form
        /// </summary>
        public MainWindow()
        {
            //Do the initial form setup
            InitializeComponent();
            UnsavedChanges = false;

            //This pannel is used to hide the UI from the user until they choose File-> "Open" or "New"
            //Here we set it to cover the entire form and add it to the window. It is removed/added in the "LockForm()" method.
            lockpannel = new Panel();
            lockpannel.Width = 900;
            lockpannel.Height = 900;
            lockpannel.Location = new Point(0, menuStrip1.Height);
            this.Controls.Add(lockpannel);
            lockpannel.BringToFront();

            //Lock the form until new/open
            LockForm(true);

            //Set up a timer which is started by input to the GUI fields. If the timer ticks to zero, the update
            //of the IOCs is performed. This delays updates until the user has stopped typing etc.
            GuiUpdateTimer = new System.Windows.Forms.Timer();
            GuiUpdateTimer.Interval = 300;
            GuiUpdateTimer.Tick += new EventHandler(timer_tick);

            //A bool flag used to supress updates to the IOC data if we are programatically updating the form
            SuppressGUIEvents = false;

            //Init this variable - it is the path where the current IOC data is stored, if blank we show the save as diaglogue when saving
            savepath = "";

            //Set the tool tip values for some of the more important buttons
            ToolTip tt1 = new ToolTip();
            tt1.ToolTipTitle = "Add New Observable";
            tt1.SetToolTip(AddIndicatorButton, "Add a new single observable to the collection");

            ToolTip tt2 = new ToolTip();
            tt2.ToolTipTitle = "Import Observable from CSV";
            tt2.SetToolTip(CSVLoadButton, "Mass import a CSV of Observable");

            ToolTip tt3 = new ToolTip();
            tt3.ToolTipTitle = "Delete Observable";
            tt3.SetToolTip(DeleteIndicatorButton, "Delete the selected Observable");

            ToolTip tt4 = new ToolTip();
            tt4.ToolTipTitle = "Edit Observable";
            tt4.SetToolTip(EditIndicatorButton, "Edit the selected Observable");

            ToolTip tt5 = new ToolTip();
            tt5.ToolTipTitle = "Link Observable";
            tt5.SetToolTip(LinkIndicatorButton, "Create a relationship between one or more Observable");

            ToolTip tt6 = new ToolTip();
            tt6.ToolTipTitle = "Visualise Observable";
            tt6.SetToolTip(VisualiseButton, "Show, edit and save a relationship diagram of the Observable");


            IncidentDiscoveredDatePicker.Checked = false;
            IncidentResolvedDatePicker.Checked = false;
            IncidentReportedDatePicker.Checked = false;
            InitialCompromiseDatePicker.Checked = false;


        }

        /// <summary>
        /// Fires when the timer completes - ie. the user has finished typing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_tick(object sender, EventArgs e)
        {
            UpdateIncidentDetails();
        }

        /// <summary>
        /// This function is called from a variey of GUI events. It restarts the timer each time an event is noticed
        /// on the GUI, preventing the updates from happening until the user has finished interecting
        /// </summary>
        void GUIUpdateNoticed()
        {
            GuiUpdateTimer.Stop();
            if (!SuppressGUIEvents)
            {
                GuiUpdateTimer.Start();
            }

        }

        /// <summary>
        /// Takes the values on the form and save them to our "ObsCollection" object
        /// Used when the user updates a value on the form and the timeout has expired
        /// </summary>
        private void UpdateIncidentDetails()
        {
            //Grab the values from the form
            ObsCollection.ReportingOrganisation = ReportCompanyTextBox.Text;
            ObsCollection.IncidentName = IncidentNameTextBox.Text;

            if (InitialCompromiseDatePicker.Checked) ObsCollection.InitialCompromise = InitialCompromiseDatePicker.Value;
            else ObsCollection.InitialCompromise = null;

            if (IncidentDiscoveredDatePicker.Checked) ObsCollection.IncidentDiscovered = IncidentDiscoveredDatePicker.Value;
            else ObsCollection.IncidentDiscovered = null;

            if (IncidentReportedDatePicker.Checked) ObsCollection.IncidentReported = IncidentReportedDatePicker.Value;
            else ObsCollection.IncidentReported = null;

            if (IncidentResolvedDatePicker.Checked) ObsCollection.IncidentResolved = IncidentResolvedDatePicker.Value;
            else ObsCollection.IncidentResolved = null;
            

            ObsCollection.IncidentTitle = IncidentTitleTextBox.Text;
            ObsCollection.ReportedBy = ReportedByTextBox.Text;
            ObsCollection.Responder = ResponderTextBox.Text;
            ObsCollection.IncidentDescription = IncidentDescriptionTextBox.Text;
            //Try and grab the values in drop down boxes - catch if nothing was selected
            try
            {
                ObsCollection.Confidence = ConfidenceList.SelectedItem.ToString();
            }
            catch { }
            try
            {
                ObsCollection.IncidentEffect = IncidentEffectDropdown.SelectedItem.ToString();
            }
            catch { }

            //Flag we just updated the info in memory but it hasn't been saved
            UnsavedChanges = true;
        }

        /// <summary>
        /// Take the values from the "ObsCollection" object and load them to the form
        /// Used when we load from a file
        /// </summary>
        private void LoadIncidentDetails()
        {
            ReportCompanyTextBox.Text = ObsCollection.ReportingOrganisation;
            IncidentNameTextBox.Text = ObsCollection.IncidentName;

            if (ObsCollection.InitialCompromise != null)
            {
                InitialCompromiseDatePicker.Checked = true;
                InitialCompromiseDatePicker.Value = (DateTime) ObsCollection.InitialCompromise;
            }
            else
            {
                InitialCompromiseDatePicker.Checked = false;
               // InitialCompromiseDatePicker.Value = DateTime.Now;
            }

            if (ObsCollection.IncidentDiscovered != null)
            {
                IncidentDiscoveredDatePicker.Checked = true;
                IncidentDiscoveredDatePicker.Value = (DateTime)ObsCollection.IncidentDiscovered;
            }
            else
            {
                IncidentDiscoveredDatePicker.Checked = false;
                //IncidentDiscoveredDatePicker.Value = DateTime.Now;
            }

            if (ObsCollection.IncidentReported != null)
            {
                IncidentReportedDatePicker.Checked = true;
                IncidentReportedDatePicker.Value = (DateTime)ObsCollection.IncidentReported;
            }
            else
            {
                IncidentReportedDatePicker.Checked = false;
               // IncidentReportedDatePicker.Value = DateTime.Now;
            }

            if (ObsCollection.IncidentResolved != null)
            {
                IncidentResolvedDatePicker.Checked = true;
                IncidentResolvedDatePicker.Value = (DateTime)ObsCollection.IncidentResolved;
            }
            else
            {
                IncidentResolvedDatePicker.Checked = false;
               // IncidentResolvedDatePicker.Value = DateTime.Now;
            }

            IncidentTitleTextBox.Text = ObsCollection.IncidentTitle;
            ReportedByTextBox.Text = ObsCollection.ReportedBy;
            ResponderTextBox.Text = ObsCollection.Responder;
            IncidentDescriptionTextBox.Text = ObsCollection.IncidentDescription;
            try
            {
                ConfidenceList.SelectedItem = ObsCollection.Confidence.ToString();
            }
            catch { }
            try
            {
                IncidentEffectDropdown.SelectedItem = ObsCollection.IncidentEffect.ToString();
            }
            catch { }
        }



        /// <summary>
        /// Handles the click on a "new" button in the toolbar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //If there are no unsaved changes, or the user agrees to discard them
            if (!UnsavedChanges || WarnAboutChanges())
            {
                //Create a new collection - drop the old one
                InitiateObservableCollection(true);
                UnsavedChanges = true;
                ClearForm();
                LockForm(false);
                updateObservables();
                UpdateIncidentDetails();
            }
        }

        /// <summary>
        /// Clears the default data from the form.
        /// TODO: Still uses hardcoded Values - CHANGE THIS TO BE BETTER - e.g. Save default incident details to a template etc.
        /// </summary>
        private void ClearForm()
        {
            // Note - Functions elsewhere remove any text starting "e.g." from a textbox when a user clicks into that box
            ReportCompanyTextBox.Text = "MWRInfoSecurity";
            IncidentNameTextBox.Text = "e.g. ProjectName";

            IncidentTitleTextBox.Text = "e.g. Project X Malware Infection #132";
            ReportedByTextBox.Text = "e.g. \"Countercept\"";
            ResponderTextBox.Text = "e.g. Joe Bloggs";

            InitialCompromiseDatePicker.Value = new DateTime(2017, 01, 01, 0, 0, 0);
            IncidentDiscoveredDatePicker.Value = new DateTime(2017, 01, 01, 0, 0, 0);
            IncidentReportedDatePicker.Value = new DateTime(2017, 01, 01, 0, 0, 0);
            IncidentResolvedDatePicker.Value = new DateTime(2017, 01, 01, 0, 0, 0);

            IncidentDescriptionTextBox.Text = "e.g. Summary of the incident and the indicators found";
            try
            {
                ConfidenceList.SelectedIndex = 0;
            }
            catch { }
            try
            {
                IncidentEffectDropdown.SelectedIndex = 0;
            }
            catch { }
        }


        /// <summary>
        /// Central function for locking/unlocking the form functions in different modes
        /// </summary>
        /// <param name="lockform">If true, lock the form, if false, unlock it</param>
        /// <param name="ShowLoadingSymbolOnLock">If true, show a loading animation gif</param>
        /// <param name="lockmessage">The message to show on the form while it is locked. E.g. "Loading..."</param>
        private void LockForm(bool lockform = true, bool ShowLoadingSymbolOnLock = false, string lockmessage = "")
        {
            //Disable or enable the relevent controls based on value of the lockform bool
            lockpannel.Visible = lockform;
            OverviewGroupBox.Enabled = !lockform;
            IndicatorsGroupBox.Enabled = !lockform;
            exportToolStripMenuItem.Enabled = !lockform;
            saveToolStripMenuItem.Enabled = !lockform;
            saveAsToolStripMenuItem.Enabled = !lockform;

            //If we are going to show our loading gif..
            if (ShowLoadingSymbolOnLock)
            {
                Label msg = new Label() { Text = lockmessage };
                msg.Location = new System.Drawing.Point(50, 30);
                lockpannel.Controls.Add(msg);

                //Show the embeded loading gif
                PictureBox loadingicon = new PictureBox();
                loadingicon.BackColor = System.Drawing.SystemColors.Window;
                loadingicon.Image = global::Athena.Properties.Resources.loading;
                loadingicon.Location = new System.Drawing.Point(50, 60);
                loadingicon.Name = "LoadingImage";
                loadingicon.Size = new System.Drawing.Size(56, 56);
                lockpannel.Controls.Add(loadingicon);
            }
            else lockpannel.Controls.Clear(); //Clear the loading gif and message it was was there
        }


        /// <summary>
        /// Initiate the Observable collection object in the case of creating a new one
        /// </summary>
        private void InitiateObservableCollection(bool wipesettings = false)
        {
            if (ObsCollection == null || wipesettings)
            {
                ObsCollection = new ObservableCollection();

                //Update the gui with our new (blank) values
                updateObservables();
            }
        }

        /// <summary>
        /// Update the list of observable objects shown on the GUI by readng from the ObservableCollection
        /// </summary>
        public void updateObservables()
        {
            //Remember which observable we had selected in our selection box so that we can set it again later
            int selected = ObservablesListBox.SelectedIndex;

            //Reset the observables box by loading from our in memory collection
            ObservablesListBox.DataSource = null;
            ObservablesListBox.DataSource = ObsCollection.Observables;
            ObservablesListBox.DisplayMember = "DisplayTitle";

            //Try to set the selected object back to what it was (may fail if items have been removed, in which case handle it)
            try
            {
                ObservablesListBox.SelectedIndex = selected;
            }
            catch
            { //deliberatly do nothing here. If our item was removed or many were, just let it select nothing
            }
        }


        /// <summary>
        /// Warn the user that the have unsaved changes and are about to discard them
        /// </summary>
        /// <returns>True if user wants to proceed, false if not</returns>
        bool WarnAboutChanges()
        {
            if (DialogResult.OK == MessageBox.Show("You have unsaved changes and this action will discard them. Continue?", "Unsaved Changes", MessageBoxButtons.OKCancel))
            {
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Handle the user clicking File->Open
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //Let the user choose a file, filter to our filetype
                OpenFileDialog diag = new OpenFileDialog();
                diag.Filter = "MWR IR IOC Files|*.iirioc|All Files|*.*";
                if (diag.ShowDialog() == DialogResult.OK)
                {
                    //Do the load
                    if (!OpenFile(diag.FileName)) MessageBox.Show("Error opening file. Was it valid iirioc file?");
                    else
                    {
                        //Update the Gui and memory collections after the load
                        updateObservables();
                        LoadIncidentDetails();
                        LockForm(false);
                        savepath = diag.FileName;
                    }
                }
            }
            catch
            {
                //If there is an rror opening the file, show error
                MessageBox.Show("Error opening the file. Was it valid iirioc file?");
            }
        }

        //Handle the user clicking File->Save
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveOrSaveAs(false);
        }

        /// <summary>
        /// Do the saving of the gui information to a fale
        /// </summary>
        /// <param name="saveas">Is this a "Save As" operation?</param>
        private void SaveOrSaveAs(bool saveas = false)
        {
            SaveFileDialog diag = new SaveFileDialog();
            diag.Filter = "MWR IR IOC Files|*.iirioc";

            //If this is a Save As, or we have no savepath set, prompt the user to pick a file to save as
            if (string.IsNullOrWhiteSpace(savepath) || saveas)
            {
                if (diag.ShowDialog() == DialogResult.OK)
                {
                    if (!SaveFile(diag.FileName)) MessageBox.Show("Error saving file");
                    savepath = diag.FileName;
                    UnsavedChanges = false;
                }
            }
            else //We have a savepath already so save to that path
            {
                if (!SaveFile(savepath)) MessageBox.Show("Error saving file");
                else UnsavedChanges = false;
            }
        }

        /// <summary>
        /// Handle the user choosing File->Export->Stix 1.1.1
        /// </summary>
        private void sTIXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog diag = new SaveFileDialog();
            diag.Filter = "Stix XML|*.xml";
            if (diag.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //Do the export in a different thread to keep the GUI free to display the "loading" animation and any errors
                    Thread exportthread = new Thread(() => ObsCollection.ToSTIX_1_1_1_XML(diag.FileName));
                    exportthread.Start();

                    //Create aother thread to keep track of the export
                    Thread monitorexportthread = new Thread(() => MonitorExportThread(exportthread));
                    monitorexportthread.Start();
                }
                catch (Exception exep)
                {
                    MessageBox.Show("Error saving file");
                }
            }
        }



        /// <summary>
        /// Locks the form and Waits until the export thread is done, then unlocks the form
        /// </summary>
        /// <param name="t">The thread to wait for</param>
        private void MonitorExportThread(Thread t)
        {
            //Make sure the thread hasn't already finished
            if (t.IsAlive)
            {
                ///Lock the form
                IndicatorDetailsPanel.BeginInvoke((Action)delegate { LockForm(true, true, "Exporting..."); });

                //Wait for the thread to finish
                while (t.IsAlive) Thread.Sleep(1000); ///Wait one second before checking again

                //Unlock the form
                IndicatorDetailsPanel.BeginInvoke((Action)delegate { LockForm(false); });
            }
        }


        private void jSONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog diag = new SaveFileDialog();
            diag.Filter = "JSON|*.json";
            if (diag.ShowDialog() == DialogResult.OK)
            {
                Thread exportthread = new Thread(() => ObsCollection.ToMISP_JSON(diag.FileName));
                exportthread.Start();
                Thread monitorexportthread = new Thread(() => MonitorExportThread(exportthread));
                monitorexportthread.Start();
            }
        }

        /// <summary>
        /// Save the observable collection to the specified file
        /// </summary>
        /// <param name="filename">The filename to save it to</param>
        /// <returns>True if save succeeded, false if it failed</returns>
        private bool SaveFile(string filename)
        {
            return SaveLoadLocalObservableCollection.SaveToBasicFile(filename, ObsCollection);
        }

        /// <summary>
        /// /Load a saved observable collection in iirioc format
        /// </summary>
        /// <param name="filepath">The filepath to load</param>
        /// <returns>True if open succeeded, false if it failed</returns>
        private bool OpenFile(string filepath)
        {
            ObservableCollection ob = SaveLoadLocalObservableCollection.LoadFromBasicFile(filepath);
            if (ob == null) return false;
            else
            {
                ObsCollection = ob;
           
                return true;
            }
        }

        /// <summary>
        /// Handle if user clicks File->Exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }        

        /// <summary>
        /// Intercept the form closing and prompt the user to save changes
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            if (this.UnsavedChanges)
            {
                if (MessageBox.Show(this, "You have unsaved changes, are you sure you want to close?", "Closing", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// Check if any of the "incident detail" fields have e.g. in them - to see if the user has missed any fields
        /// </summary>
        /// <returns>True if there have been fields missed, false if all is ok</returns>
        private bool checkIncidentDetails()
        {
            string value = "";

            value = ObsCollection.ReportingOrganisation;
            if (string.IsNullOrWhiteSpace(value) || value.Contains("e.g. ")) return true;

            value = ObsCollection.IncidentName;
            if (string.IsNullOrWhiteSpace(value) || value.Contains("e.g. ")) return true;

            value = ObsCollection.Responder;
            if (string.IsNullOrWhiteSpace(value) || value.Contains("e.g. ")) return true;

            value = ObsCollection.ReportingOrganisation;
            if (string.IsNullOrWhiteSpace(value) || value.Contains("e.g. ")) return true;

            value = ObsCollection.IncidentDescription;
            if (string.IsNullOrWhiteSpace(value) || value.Contains("e.g. ")) return true;

            value = ObsCollection.IncidentTitle;
            if (string.IsNullOrWhiteSpace(value) || value.Contains("e.g. ")) return true;

            return false;
        }

        //Handle the GUI Add indicator button being clicked
        private void AddIndicatorButton_Click(object sender, EventArgs e)
        {
            //Ask the user if they are sure they want to proceded if they havn't filled in the incident details
            //We generate the ID values for each observable based on the incident data, so its a good idea to fill it out first
            if (checkIncidentDetails())
            {
                if (MessageBox.Show("It looks like you haven't completed all the values relating to the incident metadata.\r\n\r\nID values for the indicators you add are derived from this data, so you may want to double check.\r\n\r\nYou can change the metadata later, but ID values for any indicators added now won't show the right values.\r\n\r\nCarry on to add a new indicator?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                {
                    return;
                }
            }

            //Create the GUI form used to input the data
            AddIndicatorForm currentAddForm = new AddIndicatorForm(ref ObsCollection);

            //Make sure the new form opens over the main window, not in some random place, which is uber annoying
            currentAddForm.StartPosition = FormStartPosition.Manual;
            currentAddForm.Location = this.Location;

            //In a loop so the user can click "Save and Add Another"
            do
            {
                //Should we clear the form between indicators?
                if (currentAddForm.clearform)
                {
                    //Clear and reset the form
                    currentAddForm = new AddIndicatorForm(ref ObsCollection);
                    currentAddForm.StartPosition = FormStartPosition.Manual;
                    currentAddForm.Location = this.Location;
                }

                //Show the form and handle the result
                if (currentAddForm.ShowDialog() == DialogResult.OK)
                {
                    //Add the created observable
                    ObsCollection.Observables.Add(currentAddForm.CreatedObservable);

                    //Update the main GUI
                    updateObservables();
                }
            }
            while (currentAddForm.openagain); //Open another form if requested by the last form
        }

        /// <summary>
        /// Handle the user clicking the GUI Edit Indicator button
        /// </summary>
        private void EditIndicatorButton_Click(object sender, EventArgs e)
        {
            if (ObservablesListBox.SelectedItem == null) return;
            else
            {
                AddIndicatorForm X = new AddIndicatorForm(ref ObsCollection, ObsCollection.Observables[ObservablesListBox.SelectedIndex]);
                X.StartPosition = FormStartPosition.Manual;
                X.Location = this.Location;
                if (X.ShowDialog() == DialogResult.OK)
                {
                    ObsCollection.Observables[ObservablesListBox.SelectedIndex] = X.CreatedObservable;
                    updateObservables();
                }
            }
        }


        /// <summary>
        /// Function used to add a pair of labels to the gui - one for the "Property" name and one for the "Value". Used
        ///  in this class to display the fields of each observable on the GUI when selected by the user
        /// </summary>
        /// <param name="propety">The name of the property. Will be displayed in bold and followed be a colon</param>
        /// <param name="Value">The value to display next to the property name</param>
        /// <param name="location">The location on the panel to display the controls</param>
        /// <returns></returns>
        private int AddPropertyValueLabel(string propety, string Value, Point location)
        {
            //Set the max width of the value field
            int maxwidth = 360;

            //Create the property name label
            Label proplbl = new Label();
            proplbl.Text = propety + ": ";
            proplbl.Location = location;
            proplbl.Font = new Font(proplbl.Font, FontStyle.Bold);
            //Determine how wide our label is
            int width = getlabelwidth(proplbl) + 10;
            proplbl.Width = width;
            proplbl.Location = location;
            IndicatorDetailsPanel.Controls.Add(proplbl);

            //Update our location tracker so we know where to place the value
            location.X += width;

            Label vallbl = new Label();
            vallbl.MaximumSize = new Size(maxwidth, 0);
            vallbl.AutoSize = true;
            vallbl.Text = Value;
            vallbl.Location = location;
            vallbl.Width = getlabelwidth(vallbl) + 10;
            vallbl.Location = location;
            IndicatorDetailsPanel.Controls.Add(vallbl);
            
            //Set the height of the label so the text wrapped is shown.
            int height = getlabelheight(vallbl, maxwidth);

            //If the value text was empty the heigh will be 0 - so lets set the height to be the same as the property name
            if (height < 2) height = getlabelheight(proplbl, maxwidth);
            return height;
        }

        /// <summary>
        /// Handle when a user changes the selected observable in the box - and show the details of the selected observable
        /// </summary>
        private void ObservablesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  Do we actually have one selected? If not, clear the displayed details
            if (ObservablesListBox.SelectedItem == null)
            {
                IndicatorDetailsPanel.Controls.Clear();
            }
            else
            {
                try
                {
                    //Get the selected observable and clear the existing details
                    ObservableObject obs = (ObservableObject)ObservablesListBox.SelectedItem;
                    IndicatorDetailsPanel.Controls.Clear();
                    
                    //Set our floating location point which will be incrimented as we place items
                    Point loc = new Point(0, 0);

                    //Show the id
                    loc.Y += AddPropertyValueLabel("ID", obs.ID, loc) + 10;
                    loc.X = 0;

                    //Show each field
                    foreach (ObservableObjectField f in obs.Fields)
                    {
                        loc.Y += AddPropertyValueLabel(f.FieldName, f.Value, loc) + 10;
                        loc.X = 0;
                    }
                    
                    //Show the relationships
                    Relationshipslistbox = new ListBox();
                    Relationshipslistbox.DisplayMember = "Value";
                    foreach (ObservableRelationship r in ObsCollection.Relationships)
                    {
                        //Phase things differently depending on if this item does something to the other one, or is done unto
                        if (r.From == obs.ID)
                        {
                            Relationshipslistbox.Items.Add(new KeyValuePair<string, string>(r.RelationshipID, "This " + Enum.GetName(typeof(ObservableObject.ObservableType), obs.Type) + " " + Enum.GetName(typeof(ObservableRelationshipType), r.RelationshipType) + " " + ObsCollection.GetObservableTitleFromID(r.To)));
                        }
                        else if (r.To == obs.ID)
                        {
                            Relationshipslistbox.Items.Add(new KeyValuePair<string, string>(r.RelationshipID, ObsCollection.GetObservableTitleFromID(r.From) + " " + Enum.GetName(typeof(ObservableRelationshipType), r.RelationshipType) + " this " + Enum.GetName(typeof(ObservableObject.ObservableType), obs.Type)));
                        }
                    }
                    loc.Y += AddPropertyValueLabel("Relationships", "", loc) + 10;
                    loc.X = 0;
                    Relationshipslistbox.Location = loc;
                    Relationshipslistbox.Width = 400;
                    IndicatorDetailsPanel.Controls.Add(Relationshipslistbox);

                    //Add the relationsips delete button
                    Button deleteRelatinshipButton = new Button();
                    deleteRelatinshipButton.Click += new EventHandler(this.DeleteRelationshipButton_Click);
                    deleteRelatinshipButton.BackColor = System.Drawing.SystemColors.Window;
                    deleteRelatinshipButton.BackgroundImage = global::Athena.Properties.Resources.Delete_25;
                    deleteRelatinshipButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                    deleteRelatinshipButton.FlatAppearance.BorderSize = 0;
                    deleteRelatinshipButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    deleteRelatinshipButton.Location = new System.Drawing.Point(621, 19);
                    deleteRelatinshipButton.Name = "AddIndicatorButton";
                    deleteRelatinshipButton.Size = new System.Drawing.Size(25, 25);
                    deleteRelatinshipButton.UseVisualStyleBackColor = false;
                    loc.X += 410;
                    deleteRelatinshipButton.Location = loc;
                    IndicatorDetailsPanel.Controls.Add(deleteRelatinshipButton);
                }
                catch (Exception exep)
                {
                    //Exceptions are generally due to weird input data. Show error and bail if it fails
                    IndicatorDetailsPanel.Controls.Clear();
                    MessageBox.Show("There was an exception when trying to display the details for this observable. Perhaps you the observable contains too much data, or invalid characters?");
                }

            }

        }
        
        /// <summary>
        /// Handle when the user clicks to delete a relationship
        /// </summary>
        void DeleteRelationshipButton_Click(object sender, EventArgs e)
        {
            if (Relationshipslistbox == null || Relationshipslistbox.SelectedItem == null) return;
            else
            {
                string selectedguid = ((KeyValuePair<string, string>)Relationshipslistbox.SelectedItem).Key;
                ObsCollection.Relationships.RemoveAll(r => r.RelationshipID == selectedguid);
                updateObservables();
            }
        }

        /// <summary>
        /// Handle when the user clicks to add a relationship
        /// </summary>
        private void LinkIndicatorButton_Click(object sender, EventArgs e)
        {
            RelationshipAddForm RelationshipForm;
            do
            {
                RelationshipForm = new RelationshipAddForm(ref ObsCollection);
                RelationshipForm.StartPosition = FormStartPosition.Manual;
                RelationshipForm.Location = this.Location;
                RelationshipForm.ShowDialog(); //We dont care about the result - Obs collection is passed by ref so changes happen to main object              
            }
            while (RelationshipForm.openagain);
            //Update the GUI once we return
            updateObservables();
        }

        //Handle when the user clicks File->Save As
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveOrSaveAs(true);
        }

        //Handle when the user clicks the load from CSV button
        private void CSVLoadButton_Click(object sender, EventArgs e)
        {
            //Show the CSV import form
            ImportCSV fm = new ImportCSV(ref ObsCollection);
            fm.StartPosition = FormStartPosition.Manual;
            fm.Location = this.Location;
            if (fm.ShowDialog() == DialogResult.OK)
            {
                updateObservables();
            }
        }

        private void DeleteIndicatorButton_Click(object sender, EventArgs e)
        {
            if (ObservablesListBox.SelectedIndex != -1)
            {
                string id = ((ObservableObject)ObservablesListBox.SelectedItem).ID;
                ObsCollection.Observables.RemoveAll(x => x.ID == id);

                for (int i = ObsCollection.Relationships.Count - 1; i >= 0; i--)
                {
                    if (ObsCollection.Relationships[i].From == id || ObsCollection.Relationships[i].To == id)
                    {
                        ObsCollection.Relationships.RemoveAt(i);
                    }
                }


                updateObservables();
            }
        }

        private void VisualiseButton_Click(object sender, EventArgs e)
        {
            //VisualiseForm fm = new VisualiseForm(ref ObsCollection);
            VisualiseDataForm fm = new VisualiseDataForm(ref ObsCollection);
            fm.StartPosition = FormStartPosition.Manual;
            fm.Location = this.Location;
            fm.ShowDialog();     
        }


        #region RemoveExampleDataOnClick
        private void IncidentNameTextBox_Enter(object sender, EventArgs e)
        {
            if (IncidentNameTextBox.Text.Contains("e.g. ")) IncidentNameTextBox.Text = "";
        }

        private void IncidentTitleTextBox_Enter(object sender, EventArgs e)
        {
            if (IncidentTitleTextBox.Text.Contains("e.g. ")) IncidentTitleTextBox.Text = "";
        }

        private void ReportedByTextBox_Enter(object sender, EventArgs e)
        {
            if (ReportedByTextBox.Text.Contains("e.g. ")) ReportedByTextBox.Text = "";
        }

        private void IncidentDescriptionTextBox_Enter(object sender, EventArgs e)
        {
            if (IncidentDescriptionTextBox.Text.Contains("e.g. ")) IncidentDescriptionTextBox.Text = "";
        }

        private void ResponderTextBox_Enter(object sender, EventArgs e)
        {
            if (ResponderTextBox.Text.Contains("e.g. ")) ResponderTextBox.Text = "";
        }
        #endregion

        #region GenericGUIInputEvents
        private void IncidentEffectDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            GUIUpdateNoticed();
        }

        private void ReportCompanyTextBox_TextChanged(object sender, EventArgs e)
        {
            GUIUpdateNoticed();
        }

        private void IncidentNameTextBox_TextChanged(object sender, EventArgs e)
        {

            GUIUpdateNoticed();
        }

        private void InitialCompromiseDatePicker_ValueChanged(object sender, EventArgs e)
        {
            GUIUpdateNoticed();
        }

        private void IncidentDiscoveredDatePicker_ValueChanged(object sender, EventArgs e)
        {
            GUIUpdateNoticed();
        }

        private void IncidentReportedDatePicker_ValueChanged(object sender, EventArgs e)
        {
            GUIUpdateNoticed();
        }

        private void IncidentResolvedDatePicker_ValueChanged(object sender, EventArgs e)
        {
            GUIUpdateNoticed();
        }

        private void IncidentTitleTextBox_TextChanged(object sender, EventArgs e)
        {
            GUIUpdateNoticed();
        }

        private void ReportedByTextBox_TextChanged(object sender, EventArgs e)
        {

            GUIUpdateNoticed();
        }

        private void ResponderTextBox_TextChanged(object sender, EventArgs e)
        {
            GUIUpdateNoticed();
        }

        private void IncidentEffectTextBox_TextChanged(object sender, EventArgs e)
        {
            GUIUpdateNoticed();
        }

        private void ConfidenceList_SelectedIndexChanged(object sender, EventArgs e)
        {
            GUIUpdateNoticed();
        }

        private void IncidentDescriptionTextBox_TextChanged(object sender, EventArgs e)
        {

            GUIUpdateNoticed();
        }
        #endregion
        
        #region GUIHelpers
        /// <summary>
        /// Gets the width in pixels of a label which is to be shown on the form, based on the font etc. Used 
        /// when programatically adding text controls
        /// </summary>
        /// <param name="lbl">The label to check the width of</param>
        /// <returns>And int representing the width of the label</returns>
        private int getlabelwidth(Label lbl)
        {
            using (Graphics g = CreateGraphics())
            {
                SizeF size = g.MeasureString(lbl.Text, lbl.Font);
                return (int)size.Width + 5;
            }
        }

        /// <summary>
        /// Gets the height of a label if it is constrained to a certain width
        /// i.e. how tall will the label be after wrapping the text?
        /// </summary>
        /// <param name="lbl">The label to check</param>
        /// <param name="maxwidth">The maxiumum width the label can be</param>
        /// <returns>An int representing the height of the label</returns>
        private int getlabelheight(Label lbl, int maxwidth)
        {
            using (Graphics g = CreateGraphics())
            {
                SizeF size = g.MeasureString(lbl.Text, lbl.Font, maxwidth);
                return (int)size.Height;
            }
        }
        #endregion
        
    }
}
