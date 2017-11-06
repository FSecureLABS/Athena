using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using Newtonsoft.Json;
using System.Globalization;

namespace Athena
{
    /// <summary>
    /// Contains helper functions used by multiple parts of code for generating ID values etc
    /// </summary>
    [Serializable]
    public static class HelperClass
    {
        public const string IOC_COLLECTION_VERSION_NUMBER = "V0.03";


        /// <summary>
        /// Removes any non alphanumeric characters from the supplied string and returns a new occurance of that string.
        /// </summary>
        /// <param name="str">The string to remove non-alphanumeric characters from</param>
        /// <returns>New string with only AlphaNumeric characters</returns>
        public static string RemoveNonAlphaNumeric(string str)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9]");
            return rgx.Replace(str, "");
        }

        //Generates a new ID for an observable based on the values supplied
        public static string GenerateID(string ReportingOrganisation, string ProjectName, string Type = "Object")
        {
            return HelperClass.RemoveNonAlphaNumeric(ReportingOrganisation).ToLower() + ":" + Type + "-" + HelperClass.RemoveNonAlphaNumeric(ProjectName).ToLower() + "-" + Guid.NewGuid().ToString().Substring(24);
        }

        /// <summary>
        /// Translates a .NET DateTime object into a string representation of a Unix Epoch time
        /// </summary>
        /// <param name="time">The datetime to change</param>
        /// <returns>A string containing a nueric unix epoch value</returns>
        public static string GetUnixEpochTime(DateTime time)
        {
            TimeSpan t = time - new DateTime(1970, 1, 1);
            int secondsSinceEpoch = (int)t.TotalSeconds;
            return secondsSinceEpoch.ToString();
        }
       
    }

    /// <summary>
    /// Defines the actual data(properties) within an object, and the ascociated data, as well
    /// as how to display this data in a GUI
    /// </summary>
    [Serializable]
    public class ObservableObjectField
    {
        public string ID;
        /// <summary>
        /// This defines how this field should be displayed in a GUI, if it is.
        /// </summary>
        public enum GUIFieldType
        {
            TextBox, //A standard one-line text box
            DropDown, //A drop down list - populate DropDownOptions with values
            LongTextBox //A multiline textbox
        };

        public List<string> DropDownOptions;

        public string FieldName;

        public GUIFieldType FieldType;

        public string Value; //The value of the field

        //Empty construtor to allow serialisation/loading from binary file. Should not be used programatically
        public ObservableObjectField()
        {

        }

        //Main constructor. Initialises the ID values based on the collection values
        public ObservableObjectField(ref ObservableCollection collection)
        {
            ID = HelperClass.GenerateID(collection.ReportingOrganisation, collection.IncidentName);
            DropDownOptions = new List<string>();
        }

    }



    [Serializable]
    public class ObservableObject
    {
        //Empty construtor to allow serialisation/loading from binary file. Should not be used programatically
        public ObservableObject()
        {

        }

        public string ID;

        //Generates a display title which is a compination of the observable type and the "title" entered by the user
        public string DisplayTitle
        {
            get { return Enum.GetName(typeof(ObservableType), Type).ToString() + ": " + Fields.Find(x => x.FieldName == "Title").Value; }
            set
            {
                DisplayTitle = Enum.GetName(typeof(ObservableType), Type).ToString() + ": " + Fields.Find(x => x.FieldName == "Title").Value;
            }
        }
        
        //Links the title to the field called title and exposes the field directly
        public string Title
        {
            get { return Fields.Find(x => x.FieldName == "Title").Value; }
            set
            {
                Fields.Find(x => x.FieldName == "Title").Value = value;
            }
        }

        //Links the description to the field called description and exposes the field directly
        public string Description
        {
            get { return Fields.Find(x => x.FieldName == "Description").Value; }
            set
            {
                Fields.Find(x => x.FieldName == "Description").Value = value;
            }
        }
        
        //The observable types allowed
        public enum ObservableType
        {
            Domain, IPAddress, Sample, Registry
        }

        //The observable type of this object
        public ObservableType Type;

        //A friendly type name used in the GUI
        public string FriendlyTypeName;

        //A list of the fields for this observable
        public List<ObservableObjectField> Fields;


        /// <summary>
        /// This constructor is the key for generating the fields used for each different type of indicator.
        /// The creation of new fields in this constructor (along with the GUIFieldType property) on each one, determines which
        /// fields show up on the gui when one adds a new indicatior. The GUI for adding indicators is all driven from this code below
        ///
        /// 
        /// If what you are adding adds a new type of field not already included, you will need to write the corresponding code for that
        /// field in the "toXML" and/or other export functions
        /// 
        /// </summary>
        /// <param name="type">The type of this observable: Domain, Sample etc.</param>
        public ObservableObject(ObservableType type, ref ObservableCollection collection)
        {
            ID = HelperClass.GenerateID(collection.ReportingOrganisation, collection.IncidentName, "Observable");
            Type = type;
            Fields = new List<ObservableObjectField>();
            Fields.Add(new ObservableObjectField(ref collection) { FieldName = "Title", FieldType = ObservableObjectField.GUIFieldType.TextBox });
            Fields.Add(new ObservableObjectField(ref collection) { FieldName = "Description", FieldType = ObservableObjectField.GUIFieldType.LongTextBox });

            if (Type == ObservableObject.ObservableType.Domain)
            {
                FriendlyTypeName = "Domain";
                Fields.Add(new ObservableObjectField(ref collection) { FieldName = "Domain", FieldType = ObservableObjectField.GUIFieldType.TextBox });
            }
            else if (Type == ObservableObject.ObservableType.IPAddress)
            {
                FriendlyTypeName = "IP Address";
                Fields.Add(new ObservableObjectField(ref collection) { FieldName = "IPv4", FieldType = ObservableObjectField.GUIFieldType.TextBox });
                Fields.Add(new ObservableObjectField(ref collection) { FieldName = "IPv6", FieldType = ObservableObjectField.GUIFieldType.TextBox });
                //Fields.Add(new STIXField(ref collection) { FieldName = "TestDropDown", FieldType = STIXField.GUIFieldType.DropDown, DropDownOptions = new List<string>() { "Item1", "Item2" } });
            }
            else if (Type == ObservableObject.ObservableType.Sample)
            {
                FriendlyTypeName = "Malware Sample";
                Fields.Add(new ObservableObjectField(ref collection) { FieldName = "Hashes.MD5", FieldType = ObservableObjectField.GUIFieldType.TextBox });
                Fields.Add(new ObservableObjectField(ref collection) { FieldName = "Hashes.SHA1", FieldType = ObservableObjectField.GUIFieldType.TextBox });
                Fields.Add(new ObservableObjectField(ref collection) { FieldName = "File_Name", FieldType = ObservableObjectField.GUIFieldType.TextBox });
                Fields.Add(new ObservableObjectField(ref collection) { FieldName = "File_Path", FieldType = ObservableObjectField.GUIFieldType.TextBox });
            }
            else if (Type == ObservableObject.ObservableType.Registry)
            {
                FriendlyTypeName = "Registry";
                Fields.Add(new ObservableObjectField(ref collection) { FieldName = "Key", FieldType = ObservableObjectField.GUIFieldType.TextBox });
                Fields.Add(new ObservableObjectField(ref collection) { FieldName = "Hive", FieldType = ObservableObjectField.GUIFieldType.TextBox });
                Fields.Add(new ObservableObjectField(ref collection) { FieldName = "Value", FieldType = ObservableObjectField.GUIFieldType.TextBox });
            }
        }

        //Generates a new ID based on this observable, if needed when loading/saving etc
        internal void NewID(ref ObservableCollection collection)
        {
            ID = HelperClass.GenerateID(collection.ReportingOrganisation, collection.IncidentName, "Observable");
        }
        
    }
        
    //List of valid relationship types according to STIX 1.1.1
    public enum ObservableRelationshipType
    {
        Allocated,Allocated_By,
        Bound,Bound_By,Characterized_By,Characterizes,Child_Of,Compressed,Compressed_By,Compressed_From,
        Compressed_Into,Connected_From,Connected_To,Contained_Within,Contains,Copied,Copied_By,Copied_From,
        Copied_To,Created_By,Decoded,Decoded_By,Decompressed,Decompressed_By,Decrypted,Decrypted_By,Deleted_By,Deleted_From,
        Downloaded,Downloaded_By,Downloaded_From,Downloaded_To,Dropped,Dropped_By,Encoded,Encoded_By,Encrypted,Encrypted_By,Encrypted_From,
        Encrypted_To,Extracted_From,FQDN_Of,Freed,Freed_By,Hooked,Hooked_By,Initialized_By,Initialized_To,Injected,Injected_As,
        Injected_By,Injected_Into,Installed_By,Joined,Joined_By,Killed,Killed_By,Listened_On,Listened_On_By,Loaded_From,Loaded_Into,
        Locked,Locked_By,Mapped_By,Mapped_Into,Merged,Merged_By,Modified_Properties_Of,Monitored,Monitored_By,Moved,
        Moved_By,Moved_From,Moved_To,Packed,Packed_By,Packed_From,Packed_Into,Parent_Of,Previously_Contained,
        Properties_Modified_By,Properties_Queried,Properties_Queried_By,Read_From,Read_From_By,Received,Received_By,
        Received_From,Received_Via_Upload,Redirects_To,Related_To,Renamed,Renamed_By,Renamed_From,Renamed_To,Resolved_To,
        Root_Domain_Of,Searched_For,Searched_For_By,Sent,Sent_By,Sent_To,Sent_Via_Upload,Set_From,Set_To,
        Suspended_By,Unhooked,Unhooked_By,Unlocked,Unlocked_By,Unpacked,Unpacked_By,Uploaded_By,Uploaded_From,Uploaded_To,Used_By,Values_Enumerated,Values_Enumerated_By,Written_To_By,Wrote_To
    }

    /// <summary>
    /// A class representing a link between two observables
    /// </summary>
    [Serializable]
    public class ObservableRelationship
    {
        //A unique ID for this relationship used to reference it.
        private string hiddenRelationshipID;
        public string RelationshipID
        {
            get
            {
                if (hiddenRelationshipID == null)
                {
                    hiddenRelationshipID = Guid.NewGuid().ToString();
                    return hiddenRelationshipID;
                }
                else return hiddenRelationshipID;
            }
            set
            {
                hiddenRelationshipID = value;
            }
        }
        
        //Strings to hold the ID of the observables this relationship operates on
        public string From;
        public string To;
        
        //The type of the relationship
        public ObservableRelationshipType RelationshipType;
        
        public ObservableRelationship()
        {
            hiddenRelationshipID = Guid.NewGuid().ToString();
        }

        public ObservableRelationship(string to, string from, ObservableRelationshipType relationshiptype)
        {
            From = from;
            To = to;
            RelationshipType = relationshiptype;
        }
    }

    [Serializable]
    public class ObservableCollection
    {
        //Generic Details
        public string ID;
        public string IncidentID;

        //Incident Detail Fields
        public string ReportingOrganisation;
        public string IncidentName;
        public DateTime? InitialCompromise;
        public DateTime? IncidentDiscovered;
        public DateTime? IncidentReported;
        public DateTime? IncidentResolved;
        public string IncidentTitle;
        public string ReportedBy;
        public string Responder;
        public string IncidentEffect;
        public string IncidentDescription;
        public string Confidence;

        public List<ObservableObject> Observables;
        public List<ObservableRelationship> Relationships;

        //Generic bare-bones constructor
        public ObservableCollection()
        {
            ReportingOrganisation = "";
            IncidentName = "";
            IncidentTitle = "";
            ReportedBy = "";
            Responder = "";
            IncidentEffect = "";
            IncidentDescription = "";
            Confidence = "";


            ID = HelperClass.GenerateID(ReportingOrganisation, IncidentName, "Package");
            IncidentID = HelperClass.GenerateID(ReportingOrganisation, IncidentName, "Incident");

            Observables = new List<ObservableObject>();
            Relationships = new List<ObservableRelationship>();
        }

        //Update the IDs with new values
        public void UpdateIDs()
        {
             
            if(ID==null || !ID.Contains(ReportingOrganisation) || !ID.Contains(IncidentName))
            {
                ID = HelperClass.GenerateID(ReportingOrganisation, IncidentName, "Package");
            }
            if (IncidentID==null||!IncidentID.Contains(ReportingOrganisation) || !IncidentID.Contains(IncidentName))
            {
                IncidentID = HelperClass.GenerateID(ReportingOrganisation, IncidentName, "Incident");
            }
        }

        //Get the title of a child observable by supplying its ID value
        public string GetObservableTitleFromID(string id)
        {
            foreach (ObservableObject o in Observables)
            {
                if (o.ID == id) return o.DisplayTitle;
            }
            return null;
        }
    }


    /// <summary>
    /// Class to handle the save and load of the collection to the local binary file
    /// </summary>
    public static class SaveLoadLocalObservableCollection
    {
        /// <summary>
        /// Saves the STIX Collection to a binary file 
        /// </summary>
        /// <param name="filepath">The file to save to</param>
        /// <param name="col">The stix collection to save</param>
        /// <returns>True if save worked, false if not</returns>
        public static bool SaveToBasicFile(string filepath,ObservableCollection col)
        {
            //Update the IDs before saving
            col.UpdateIDs();
            //Write to the file            
            WriteToFile(filepath, col);
            return true;
        }

        /// <summary>
        /// Write the collection date out to a file
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static bool WriteToFile(string filepath, ObservableCollection col)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                //using (StreamWriter w = new StreamWriter(filepath))
                // using (StreamWriter w = new StreamWriter(()
                // {
                sb.AppendLine("Athena-IOC-Collection-File-" + HelperClass.IOC_COLLECTION_VERSION_NUMBER);
                sb.AppendLine("|||" + col.ID);
                sb.AppendLine("|||" + col.IncidentID);
                sb.AppendLine("|||" + col.ReportingOrganisation);
                sb.AppendLine("|||" + col.IncidentName);
                sb.AppendLine(col.InitialCompromise != null ? "|||" + ((DateTime)col.InitialCompromise).ToString("yyyy-MM-dd HH:mm:ss") : "|||");
                sb.AppendLine(col.IncidentDiscovered != null ? "|||" + ((DateTime)col.IncidentDiscovered).ToString("yyyy-MM-dd HH:mm:ss") : "|||");
                sb.AppendLine(col.IncidentReported != null ? "|||" + ((DateTime)col.IncidentReported).ToString("yyyy-MM-dd HH:mm:ss") : "|||");
                sb.AppendLine(col.IncidentResolved != null ? "|||" + ((DateTime)col.IncidentResolved).ToString("yyyy-MM-dd HH:mm:ss") : "|||");
                sb.AppendLine("|||" + col.IncidentTitle);
                sb.AppendLine("|||" + col.ReportedBy);
                sb.AppendLine("|||" + col.Responder);
                sb.AppendLine("|||" + col.IncidentEffect);
                sb.AppendLine("|||" + col.IncidentDescription.Replace("\r", "\\r").Replace("\n", "\\n"));
                sb.AppendLine("|||" + col.Confidence);

                foreach (ObservableObject o in col.Observables)
                {
                    sb.AppendLine("|AthenaObservable|");
                    sb.AppendLine("|||" + o.ID);
                    sb.AppendLine("|||" + o.Title);
                    sb.AppendLine("|||" + o.Description.Replace("\r", "\\r").Replace("\n", "\\n"));
                    sb.AppendLine("|||" + Enum.GetName(typeof(ObservableObject.ObservableType), o.Type));
                    //  sb.AppendLine("|||" + o.FriendlyTypeName);

                    foreach (ObservableObjectField f in o.Fields)
                    {
                        sb.AppendLine("|AthenaObservableField|:" + f.ID + "|||" + f.FieldName + "|||" + f.Value);
                    }
                }
                foreach (ObservableRelationship r in col.Relationships)
                {
                    sb.AppendLine("|AthenaRelationship|");
                    sb.AppendLine("|||" + r.RelationshipID);
                    sb.AppendLine("|||" + r.To);
                    sb.AppendLine("|||" + r.From);
                    sb.AppendLine("|||" + Enum.GetName(typeof(ObservableRelationshipType), r.RelationshipType));
                }
                // }


                //Save to BASE64 file
                File.WriteAllText(filepath,System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(sb.ToString())));

            }
            catch (Exception exep)
            {

            }
            return true;
        }
        

        /// <summary>
        /// Loads the STIX collection from a textual Base64 encoded IIRIOC file.
        /// Try/catches mean any issues with the load or the file will cause the method to return a null collection
        /// and the gui will process
        /// </summary>
        /// <param name="filepath">The filepath to load</param>
        /// <returns>A stix collection or null if errors</returns>
        public static ObservableCollection LoadFromBasicFile(string filepath)
        {
            //return ReadFromBinaryFile<ObservableCollection>(filepath);
            ObservableCollection col = new ObservableCollection();

            //Used to extract the file version. Will be used in furture to load differently if we change the way the file is structured etc.
            String FileVersion = "";

            //Load the Base64 into memory and convert it - present a stream to read
            string filecontents = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(File.ReadAllText(filepath)));
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(filecontents);
            writer.Flush();
            stream.Position = 0;

            try
            {
                //using (StreamReader r = new StreamReader(filepath))
                using (StreamReader r = new StreamReader(stream))
                {
                    //Read the header and get the version
                    string line = r.ReadLine();
                    if (!line.StartsWith("Athena-IOC-Collection-File-")) return null;
                    FileVersion = line.Substring("Athena-IOC-Collection-File-".Length);

                    //Get the ID of the whole collection
                    line = r.ReadLine();
                    if (line.StartsWith("|||")) col.ID = line.Substring(3);
                    else return null;

                    //Get the incident ID
                    line = r.ReadLine();
                    if (line.StartsWith("|||")) col.IncidentID = line.Substring(3);
                    else return null;

                    //Get the reporting org
                    line = r.ReadLine();
                    if (line.StartsWith("|||")) col.ReportingOrganisation = line.Substring(3);
                    else return null;

                    //Get the incident name
                    line = r.ReadLine();
                    if (line.StartsWith("|||")) col.IncidentName = line.Substring(3);
                    else return null;

                    //Get the initial compromise date
                    line = r.ReadLine();
                    if (line.StartsWith("|||"))
                    {
                        line = line.Substring(3);
                        if (line.Length > 1) col.InitialCompromise = DateTime.ParseExact(line, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    }
                    else return null;

                    //Get the Incident Discovered date
                    line = r.ReadLine();
                    if (line.StartsWith("|||"))
                    {
                        line = line.Substring(3);
                        if (line.Length > 1) col.IncidentDiscovered = DateTime.ParseExact(line, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    }
                    else return null;

                    //Get the Incident Reported data
                    line = r.ReadLine();
                    if (line.StartsWith("|||"))
                    {
                        line = line.Substring(3);
                        if (line.Length > 1) col.IncidentReported = DateTime.ParseExact(line, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    }
                    else return null;

                    //Get the incident resolved date
                    line = r.ReadLine();
                    if (line.StartsWith("|||"))
                    {
                        line = line.Substring(3);
                        if (line.Length > 1) col.IncidentResolved = DateTime.ParseExact(line, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    }
                    else return null;

                    //Get the incident title
                    line = r.ReadLine();
                    if (line.StartsWith("|||")) col.IncidentTitle = line.Substring(3);
                    else return null;

                    //Get the reported by
                    line = r.ReadLine();
                    if (line.StartsWith("|||")) col.ReportedBy = line.Substring(3);
                    else return null;

                    //Get the Responder name
                    line = r.ReadLine();
                    if (line.StartsWith("|||")) col.Responder = line.Substring(3);
                    else return null;

                    //Get the incident effect name
                    line = r.ReadLine();
                    if (line.StartsWith("|||")) col.IncidentEffect = line.Substring(3);
                    else return null;

                    //Get the incident description
                    line = r.ReadLine();
                    if (line.StartsWith("|||")) col.IncidentDescription = line.Substring(3).Replace("\\r", "\r").Replace("\\n", "\n");
                    else return null;

                    //Get the incident confidence
                    line = r.ReadLine();
                    if (line.StartsWith("|||")) col.Confidence = line.Substring(3);
                    else return null;

                    //Load each of the observables
                    line = r.ReadLine();
                    while(line== "|AthenaObservable|")
                    {
                        string oid, otitle, odesc;

                        ObservableObject.ObservableType otype;

                        //Get the observable ID
                        line = r.ReadLine();
                        if (line.StartsWith("|||")) oid = line.Substring(3);
                        else return null;

                        //Get the observable title
                        line = r.ReadLine();
                        if (line.StartsWith("|||")) otitle = line.Substring(3);
                        else return null;

                        //Get the observable description
                        line = r.ReadLine();
                        if (line.StartsWith("|||")) odesc = line.Substring(3).Replace("\\r", "\r").Replace("\\n", "\n");
                        else return null;

                        //Get the observable type
                        //If it doesnt match a defined type the whole load will fail
                        line = r.ReadLine();
                        if (line.StartsWith("|||"))
                        {
                            if (!Enum.TryParse(line.Substring(3), out otype)) return null;
                        }
                        else return null;

                        //Create a new object and set the values we just loaded - done this way to make sure the constructor runs with the right values
                        ObservableObject ox = new ObservableObject(otype, ref col);
                        ox.Description = odesc;
                        ox.ID = oid;
                        ox.Title = otitle;

                        //Get all the fields for the current observable
                        line = r.ReadLine();
                        while (line.StartsWith("|AthenaObservableField|:")) 
                        {
                            string[] split = line.Substring("| AthenaObservableField |:".Length).Split(new string[] { "|||" },StringSplitOptions.None);

                            ox.Fields.Find(x => x.FieldName == split[1]).ID = split[0];
                            ox.Fields.Find(x => x.FieldName == split[1]).Value = split[2];
                            line = r.ReadLine();
                        }
                        col.Observables.Add(ox);
                    }
                    
                    //Get all the relationships
                    while (line == "|AthenaRelationship|")
                    {
                        string rid, rto, rfrom;
                        ObservableRelationshipType rtype;

                        //Get the relationship ID
                        line = r.ReadLine();
                        if (line.StartsWith("|||")) rid = line.Substring(3);
                        else return null;

                        //Get the "to" ID
                        line = r.ReadLine();
                        if (line.StartsWith("|||")) rto = line.Substring(3);
                        else return null;

                        //Get the "from" ID
                        line = r.ReadLine();
                        if (line.StartsWith("|||")) rfrom = line.Substring(3);
                        else return null;

                        //Get the type of the relationship (will fail if it isn't a valid type)
                        line = r.ReadLine();
                        if (line.StartsWith("|||"))
                        {
                            if (!Enum.TryParse(line.Substring(3), out rtype)) return null;
                        }
                        else return null;

                        ObservableRelationship ro = new ObservableRelationship(rto, rfrom, rtype);
                        ro.RelationshipID = rid;

                        //Add the relationship to the collection
                        col.Relationships.Add(ro);

                        //Advance the reader
                        line = r.ReadLine();
                    }
                }                
            }
            catch (Exception exep)
            {
                //Return a null object if any errors are encountered
                return null;
            }
            return col;
        }     
    }
}