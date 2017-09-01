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
        
        /// <summary>
        /// Output a STIX 1.1.1 representation of the collection to the specified file.
        /// </summary>
        /// <param name="filepath">The output path to save the XML file</param>
        /// <returns>True if output succeeded, false if it failed</returns>
        public bool ToSTIX_1_1_1_XML(string filepath)
        {
            //Before exporting anything, make sure the ID values are correct
            UpdateIDs();

            XmlWriterSettings x = new XmlWriterSettings();
            x.Indent = true;
            using (XmlWriter writer = XmlWriter.Create(filepath, x))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("stix", "STIX_Package", "http://stix.mitre.org/stix-1");
                writer.WriteAttributeString("id", this.ID);


                //Write all the namespaces
                #region namespaces
                writer.WriteAttributeString("xmlns", "AddressObj", null, "http://cybox.mitre.org/objects#AddressObject-2");
                writer.WriteAttributeString("xmlns", "FileObj", null, "http://cybox.mitre.org/objects#FileObject-2");
                writer.WriteAttributeString("xmlns", "DomainNameObj", null, "http://cybox.mitre.org/objects#DomainNameObject-1");
                writer.WriteAttributeString("xmlns", "WinRegistryKeyObj", null, "http://cybox.mitre.org/objects#WinRegistryKeyObject-2");
                writer.WriteAttributeString("xmlns", "URIObj", null, "http://cybox.mitre.org/objects#URIObject-2");
                writer.WriteAttributeString("xmlns", "cybox", null, "http://cybox.mitre.org/cybox-2");
                writer.WriteAttributeString("xmlns", "cyboxCommon", null, "http://cybox.mitre.org/common-2");
                writer.WriteAttributeString("xmlns", "cyboxVocabs", null, "http://cybox.mitre.org/default_vocabularies-2");
                writer.WriteAttributeString("xmlns", "mwrinfosecurity", null, "http://www.mwrinfosecurity.com");
                writer.WriteAttributeString("xmlns", "incident", null, "http://stix.mitre.org/Incident-1");
                writer.WriteAttributeString("xmlns", "stixCommon", null, "http://stix.mitre.org/common-1");
                writer.WriteAttributeString("xmlns", "stixVocabs", null, "http://stix.mitre.org/default_vocabularies-1");
                writer.WriteAttributeString("xmlns", "stix", null, "http://stix.mitre.org/stix-1");
                writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                writer.WriteAttributeString("xmlns", "ttp", null, "http://stix.mitre.org/TTP-1");
                //string x = @"http://stix.mitre.org/stix-1 http://stix.mitre.org/XMLSchema/core/1.1.1/stix_core.xsd    http://stix.mitre.org/default_vocabularies-1 http://stix.mitre.org/XMLSchema/default_vocabularies/1.1.1/stix_default_vocabularies.xsd	http://cybox.mitre.org/cybox-2 http://cybox.mitre.org/XMLSchema/core/2.1/cybox_core.xsd	http://cybox.mitre.org/common-2 http://cybox.mitre.org/XMLSchema/common/2.1/cybox_common.xsd     http://cybox.mitre.org/default_vocabularies-2 http://cybox.mitre.org/XMLSchema/default_vocabularies/2.1/cybox_default_vocabularies.xsd	http://cybox.mitre.org/objects#WinRegistryKeyObject-2 http://cybox.mitre.org/XMLSchema/objects/Win_Registry_Key/2.0/Win_Registry_Key_Object.xsd	http://cybox.mitre.org/objects#AddressObject-2 http://cybox.mitre.org/XMLSchema/objects/Address/2.1/Address_Object.xsd	http://cybox.mitre.org/objects#DomainNameObject-1 http://cybox.mitre.org/XMLSchema/objects/Domain_Name/1.0/Domain_Name_Object.xsd	http://cybox.mitre.org/objects#FileObject-2 http://cybox.mitre.org/XMLSchema/objects/File/2.1/File_Object.xsd	http://cybox.mitre.org/XMLSchema/objects/Address/2.1/Address_Object.xsd http://cybox.mitre.org/objects#URIObject-2 http://cybox.mitre.org/XMLSchema/objects/URI/2.1/URI_Object.xsd	http://stix.mitre.org/Incident-1 http://stix.mitre.org/XMLSchema/incident/1.1.1/incident.xsd	http://stix.mitre.org/TTP-1 http://stix.mitre.org/XMLSchema/ttp/1.1.1/ttp.xsd http://stix.mitre.org/common-1 http://stix.mitre.org/XMLSchema/common/1.1.1/stix_common.xsd	http://stix.mitre.org/default_vocabularies-1 http://stix.mitre.org/XMLSchema/default_vocabularies/1.1.1.0/stix_default_vocabularies.xsd		";
                // writer.WriteAttributeString("xsi", "schemaLocation", null, "http://stix.mitre.org/stix-1 http://stix.mitre.org/XMLSchema/core/1.1.1/stix_core.xsd    http://stix.mitre.org/default_vocabularies-1 http://stix.mitre.org/XMLSchema/default_vocabularies/1.1.1/stix_default_vocabularies.xsd	http://cybox.mitre.org/cybox-2 http://cybox.mitre.org/XMLSchema/core/2.1/cybox_core.xsd	http://cybox.mitre.org/common-2 http://cybox.mitre.org/XMLSchema/common/2.1/cybox_common.xsd     http://cybox.mitre.org/default_vocabularies-2 http://cybox.mitre.org/XMLSchema/default_vocabularies/2.1/cybox_default_vocabularies.xsd	http://cybox.mitre.org/objects#WinRegistryKeyObject-2 http://cybox.mitre.org/XMLSchema/objects/Win_Registry_Key/2.0/Win_Registry_Key_Object.xsd	http://cybox.mitre.org/objects#AddressObject-2 http://cybox.mitre.org/XMLSchema/objects/Address/2.1/Address_Object.xsd	http://cybox.mitre.org/objects#DomainNameObject-1 http://cybox.mitre.org/XMLSchema/objects/Domain_Name/1.0/Domain_Name_Object.xsd	http://cybox.mitre.org/objects#FileObject-2 http://cybox.mitre.org/XMLSchema/objects/File/2.1/File_Object.xsd	http://cybox.mitre.org/XMLSchema/objects/Address/2.1/Address_Object.xsd http://cybox.mitre.org/objects#URIObject-2 http://cybox.mitre.org/XMLSchema/objects/URI/2.1/URI_Object.xsd	http://stix.mitre.org/Incident-1 http://stix.mitre.org/XMLSchema/incident/1.1.1/incident.xsd	http://stix.mitre.org/TTP-1 http://stix.mitre.org/XMLSchema/ttp/1.1.1/ttp.xsd http://stix.mitre.org/common-1 http://stix.mitre.org/XMLSchema/common/1.1.1/stix_common.xsd	http://stix.mitre.org/default_vocabularies-1 http://stix.mitre.org/XMLSchema/default_vocabularies/1.1.1.0/stix_default_vocabularies.xsd");
                string tmp = "";
                tmp +="   "+"http://stix.mitre.org/stix-1";
                tmp +="   "+"http://stix.mitre.org/XMLSchema/core/1.1.1/stix_core.xsd";
                tmp +="   "+"http://stix.mitre.org/default_vocabularies-1";
                tmp +="   "+"http://stix.mitre.org/XMLSchema/default_vocabularies/1.1.1/stix_default_vocabularies.xsd";
                tmp +="   "+"http://cybox.mitre.org/cybox-2";
                tmp +="   "+"http://cybox.mitre.org/XMLSchema/core/2.1/cybox_core.xsd";
                tmp +="   "+"http://cybox.mitre.org/common-2";
                tmp +="   "+"http://cybox.mitre.org/XMLSchema/common/2.1/cybox_common.xsd";
                tmp +="   "+"http://cybox.mitre.org/default_vocabularies-2";
                tmp +="   "+"http://cybox.mitre.org/XMLSchema/default_vocabularies/2.1/cybox_default_vocabularies.xsd";
                tmp +="   "+"http://cybox.mitre.org/objects#WinRegistryKeyObject-2";
                tmp +="   "+"http://cybox.mitre.org/XMLSchema/objects/Win_Registry_Key/2.0/Win_Registry_Key_Object.xsd";
                tmp +="   "+"http://cybox.mitre.org/objects#AddressObject-2";
                tmp +="   "+"http://cybox.mitre.org/XMLSchema/objects/Address/2.1/Address_Object.xsd";
                tmp +="   "+"http://cybox.mitre.org/objects#DomainNameObject-1";
                tmp +="   "+"http://cybox.mitre.org/XMLSchema/objects/Domain_Name/1.0/Domain_Name_Object.xsd";
                tmp +="   "+"http://cybox.mitre.org/objects#FileObject-2";
                tmp +="   "+"http://cybox.mitre.org/XMLSchema/objects/File/2.1/File_Object.xsd";
                tmp +="   "+"http://cybox.mitre.org/XMLSchema/objects/Address/2.1/Address_Object.xsd";
                tmp +="   "+"http://cybox.mitre.org/objects#URIObject-2";
                tmp +="   "+"http://cybox.mitre.org/XMLSchema/objects/URI/2.1/URI_Object.xsd";
                tmp +="   "+"http://stix.mitre.org/Incident-1";
                tmp +="   "+"http://stix.mitre.org/XMLSchema/incident/1.1.1/incident.xsd";
                tmp +="   "+"http://stix.mitre.org/TTP-1";
                tmp +="   "+"http://stix.mitre.org/XMLSchema/ttp/1.1.1/ttp.xsd";
                tmp +="   "+"http://stix.mitre.org/common-1";
                tmp +="   "+"http://stix.mitre.org/XMLSchema/common/1.1.1/stix_common.xsd";
                tmp +="   "+"http://stix.mitre.org/default_vocabularies-1";
                tmp +="   "+"http://stix.mitre.org/XMLSchema/default_vocabularies/1.1.1.0/stix_default_vocabularies.xsd";
                writer.WriteAttributeString("xsi", "schemaLocation", null, tmp);

                    
                    
                writer.WriteAttributeString("version", "1.1.1");
                #endregion


                #region WriteObservables
                if (Observables.Count > 0)
                {
                    writer.WriteStartElement("stix", "Observables", null);
                    writer.WriteAttributeString("cybox_major_version", "2");
                    writer.WriteAttributeString("cybox_minor_version", "1");
                    writer.WriteAttributeString("cybox_update_version", "0");
                }

                foreach (ObservableObject obs in Observables)
                {
                    //Write the header for this observable
                    writer.WriteStartElement("cybox", "Observable", null);
                    writer.WriteAttributeString("id", obs.ID);

                    //Write the title
                    writer.WriteStartElement("cybox", "Title", null);
                    writer.WriteValue(obs.Title);
                    writer.WriteEndElement();

                    //Write the description
                    if (!string.IsNullOrWhiteSpace(obs.Description))
                    {
                        writer.WriteStartElement("cybox", "Description", null);
                        writer.WriteValue(obs.Description);
                        writer.WriteEndElement();
                    }

                    #region ipaddresses
                    if (obs.Type == ObservableObject.ObservableType.IPAddress)
                    {
                        writer.WriteStartElement("cybox", "Object", null);
                        writer.WriteAttributeString("id", obs.ID + "_obj");
                        foreach (ObservableObjectField f in obs.Fields)
                        {
                            //If there is an IPV4, write it
                            if (f.FieldName == "IPv4" && !string.IsNullOrWhiteSpace(f.Value))
                            {
                                writer.WriteStartElement("cybox", "Properties", null);
                                writer.WriteAttributeString("xsi", "type", null, "AddressObj:AddressObjectType");
                                writer.WriteAttributeString("category", "ipv4-addr");

                                writer.WriteStartElement("AddressObj", "Address_Value", null);
                                writer.WriteValue(f.Value);
                                writer.WriteEndElement(); //Value
                                writer.WriteEndElement(); //Properties
                            }
                            //If there is an IPV6, write it
                            if (f.FieldName == "IPv6" && !string.IsNullOrWhiteSpace(f.Value))
                            {
                                writer.WriteStartElement("cybox", "Properties", null);
                                writer.WriteAttributeString("xsi", "type", null, "AddressObj:AddressObjectType");
                                writer.WriteAttributeString("category", "ipv6-addr");

                                writer.WriteStartElement("AddressObj", "Address_Value", null);
                                writer.WriteValue(f.Value);
                                writer.WriteEndElement(); //Value
                                writer.WriteEndElement(); //Properties
                                   
                            }
                        }

                        XML_WriteRelatedObjects(writer,obs.ID);

                        writer.WriteEndElement(); //Object
                    }
                    #endregion

                    #region Domain
                    if (obs.Type == ObservableObject.ObservableType.Domain)
                    {
                        foreach (ObservableObjectField f in obs.Fields)
                        {
                            //If there is a domain, write it
                            if (f.FieldName == "Domain" && !string.IsNullOrWhiteSpace(f.Value))
                            {
                                writer.WriteStartElement("cybox", "Object", null);
                                writer.WriteAttributeString("id", obs.ID+"_obj");
                                writer.WriteStartElement("cybox", "Properties", null);
                                writer.WriteAttributeString("type", "FQDN");
                                writer.WriteAttributeString("xsi", "type", null, "DomainNameObj:DomainNameObjectType");

                                writer.WriteStartElement("DomainNameObj", "Value", null);
                                writer.WriteValue(f.Value);
                                writer.WriteEndElement(); //Value


                                writer.WriteEndElement();//Properties

                                XML_WriteRelatedObjects(writer, obs.ID);
                                writer.WriteEndElement();//Object
                            }
                        }
                    }
                    #endregion

                    #region Sample
                    if (obs.Type == ObservableObject.ObservableType.Sample)
                    {
                        writer.WriteStartElement("cybox", "Object", null);
                        writer.WriteAttributeString("id", obs.ID+"_obj");
                        writer.WriteStartElement("cybox", "Properties", null);
                        writer.WriteAttributeString("xsi", "type", null, "FileObj:FileObjectType");

                        //Write the filename, if present
                        if (obs.Fields.Find(z => z.FieldName == "File_Name").Value != "")
                        {
                            writer.WriteStartElement("FileObj", "File_Name", null);
                            writer.WriteValue(obs.Fields.Find(z => z.FieldName == "File_Name").Value);
                            writer.WriteEndElement();
                        }
                        //write the filepath if present
                        if (obs.Fields.Find(z => z.FieldName == "File_Path").Value != "")
                        {
                            writer.WriteStartElement("FileObj", "File_Path", null);
                            writer.WriteValue(obs.Fields.Find(z => z.FieldName == "File_Path").Value);
                            writer.WriteEndElement();
                        }

                        //Write the hashes, if we have at least one present
                        if (obs.Fields.Find(z => z.FieldName == "Hashes.MD5").Value != "" || obs.Fields.Find(z => z.FieldName == "Hashes.SHA1").Value != "")
                        {
                            //Write the header for the hashes
                            writer.WriteStartElement("FileObj", "Hashes", null);

                            //Write the MD5 if presnt
                            if (obs.Fields.Find(z => z.FieldName == "Hashes.MD5").Value != "")
                            {
                                writer.WriteStartElement("cyboxCommon", "Hash", null);
                                writer.WriteStartElement("cyboxCommon", "Type", null);
                                writer.WriteValue("MD5");
                                writer.WriteEndElement();//Type
                                writer.WriteStartElement("cyboxCommon", "Simple_Hash_Value", null);
                                writer.WriteValue(obs.Fields.Find(z => z.FieldName == "Hashes.MD5").Value);
                                writer.WriteEndElement();//Type
                                writer.WriteEndElement();//Hash
                            }
                            //Write the MD5 if presnt
                            if (obs.Fields.Find(z => z.FieldName == "Hashes.SHA1").Value != "")
                            {
                                writer.WriteStartElement("cyboxCommon", "Hash", null);
                                writer.WriteStartElement("cyboxCommon", "Type", null);
                                writer.WriteValue("SHA1");
                                writer.WriteEndElement();//Type
                                writer.WriteStartElement("cyboxCommon", "Simple_Hash_Value", null);
                                writer.WriteValue(obs.Fields.Find(z => z.FieldName == "Hashes.SHA1").Value);
                                writer.WriteEndElement();//Type
                                writer.WriteEndElement();//Hash
                            }
                            writer.WriteEndElement();//Hashes
                        }

                            
                        writer.WriteEndElement();//Properties

                        XML_WriteRelatedObjects(writer, obs.ID);
                        writer.WriteEndElement();//Object
                    }//End IF File sample
                    #endregion

                    #region Registry
                    if (obs.Type == ObservableObject.ObservableType.Registry)
                    {
                        writer.WriteStartElement("cybox", "Object", null);
                        writer.WriteAttributeString("id", obs.Fields.First().ID);
                        writer.WriteStartElement("cybox", "Properties", null);
                        writer.WriteAttributeString("xsi", "type", null, "WinRegistryKeyObj:WindowsRegistryKeyObjectType");

                        writer.WriteStartElement("WinRegistryKeyObj", "Key", null);
                        writer.WriteValue(obs.Fields.Find(z => z.FieldName == "Key").Value);
                        writer.WriteEndElement();

                        writer.WriteStartElement("WinRegistryKeyObj", "Hive", null);
                        writer.WriteValue(obs.Fields.Find(z => z.FieldName == "Hive").Value);
                        writer.WriteEndElement();

                        writer.WriteStartElement("WinRegistryKeyObj", "Values", null);
                        writer.WriteStartElement("WinRegistryKeyObj", "Name", null);
                        writer.WriteValue(obs.Fields.Find(z => z.FieldName == "Value").Value);
                        writer.WriteEndElement();
                        writer.WriteEndElement();

                        writer.WriteEndElement();//Properties

                        XML_WriteRelatedObjects(writer, obs.ID);

                        writer.WriteEndElement();//Object
                    }
                    #endregion

                    writer.WriteEndElement();//Observable
                } //End foreach observable

                //Write the closing tag for the group of observables if needed
                if (Observables.Count > 0) writer.WriteEndElement();

                #endregion


                #region WriteIncidentDetails
                writer.WriteStartElement("stix", "Incidents",null);
                writer.WriteStartElement("stix", "Incident",null);
                writer.WriteAttributeString("id", IncidentID);
                if(this.InitialCompromise != null) writer.WriteAttributeString("timestamp", ((DateTime)this.InitialCompromise).ToString("yyyy-MM-ddTHH:mm:ss") + "+00:00");
                writer.WriteAttributeString("xsi", "type", null, "incident:IncidentType");

                writer.WriteStartElement("incident", "Title", null);
                writer.WriteValue(this.IncidentTitle);
                writer.WriteEndElement();

                writer.WriteStartElement("incident", "Time", null);

                if (this.InitialCompromise != null)
                {
                    writer.WriteStartElement("incident", "Initial_Compromise", null);
                    writer.WriteAttributeString("precision", "second");
                    writer.WriteValue(((DateTime)this.InitialCompromise).ToString("yyyy-MM-ddTHH:mm:ss"));
                    writer.WriteEndElement();//A single time
                }

                if (this.IncidentDiscovered != null)
                {
                    writer.WriteStartElement("incident", "Incident_Discovery", null);
                    writer.WriteAttributeString("precision", "second");
                    writer.WriteValue(((DateTime)this.IncidentDiscovered).ToString("yyyy-MM-ddTHH:mm:ss"));
                    writer.WriteEndElement();//A single time
                }

                if (this.IncidentResolved != null)
                {
                    writer.WriteStartElement("incident", "Restoration_Achieved", null);
                    writer.WriteAttributeString("precision", "second");
                    writer.WriteValue(((DateTime)this.IncidentResolved).ToString("yyyy-MM-ddTHH:mm:ss"));
                    writer.WriteEndElement();//A single time
                }

                if (this.IncidentReported != null)
                {
                    writer.WriteStartElement("incident", "Incident_Reported", null);
                    writer.WriteAttributeString("precision", "second");
                    writer.WriteValue(((DateTime)this.IncidentReported).ToString("yyyy-MM-ddTHH:mm:ss"));
                    writer.WriteEndElement();//A single time          
                }
                    
                writer.WriteEndElement();//The time group

                writer.WriteStartElement("incident", "Description", null);
                writer.WriteValue(this.IncidentDescription);
                writer.WriteEndElement();

                writer.WriteStartElement("incident", "Reporter", null);
                writer.WriteStartElement("stixCommon", "Description", null);
                writer.WriteValue("The person who reported the incident");
                writer.WriteEndElement();//Description
                writer.WriteStartElement("stixCommon", "Identity", null);
                writer.WriteAttributeString("id", HelperClass.GenerateID(this.ReportingOrganisation,"x","Identity"));
                writer.WriteStartElement("stixCommon", "Name", null);
                writer.WriteValue(this.ReportedBy);
                writer.WriteEndElement();//Name
                writer.WriteEndElement();//Identity
                writer.WriteEndElement();//Reporter

                writer.WriteStartElement("incident", "Responder", null);
                writer.WriteStartElement("stixCommon", "Description", null);
                writer.WriteValue("The person who responded to the incident");
                writer.WriteEndElement();//Description
                writer.WriteStartElement("stixCommon", "Identity", null);
                writer.WriteAttributeString("id", HelperClass.GenerateID(this.ReportingOrganisation, "x", "Identity"));
                writer.WriteStartElement("stixCommon", "Name", null);
                writer.WriteValue(this.Responder);
                writer.WriteEndElement();//Name
                writer.WriteEndElement();//Identity
                writer.WriteEndElement();//Responder


                writer.WriteStartElement("incident", "Impact_Assessment", null);
                writer.WriteStartElement("incident", "Effects", null);
                writer.WriteStartElement("incident", "Effect", null);
                writer.WriteAttributeString("xsi", "type", null, "stixVocabs:IncidentEffectVocab-1.0");
                writer.WriteValue(this.IncidentEffect);
                writer.WriteEndElement();//Effect
                writer.WriteEndElement();//Effects
                writer.WriteEndElement();//Impact Assessment


                //If we have observables, link them to the incident here
                if (Observables.Count > 0)
                {
                    writer.WriteStartElement("incident", "Related_Observables", null);
                        
                }
                foreach(ObservableObject obs in Observables)
                {

                    writer.WriteStartElement("incident", "Related_Observable", null);
                    writer.WriteStartElement("stixCommon", "Observable", null);
                    writer.WriteAttributeString("idref",obs.ID);
                    writer.WriteEndElement(); //Observable
                    writer.WriteEndElement(); //RelatedObservable

                }
                if (Observables.Count > 0) writer.WriteEndElement(); //RelatedObservables

                writer.WriteStartElement("incident", "Confidence", null);
                writer.WriteAttributeString("timestamp",DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")+".0000+00:00");
                writer.WriteStartElement("stixCommon", "Value", null);
                writer.WriteAttributeString("xsi", "type", null, "stixVocabs:HighMediumLowVocab-1.0");
                writer.WriteValue(this.Confidence);
                writer.WriteEndElement();//Value
                writer.WriteEndElement();//Confidence

                writer.WriteEndElement();//Incident
                writer.WriteEndElement();//Incidents
                #endregion



                //End the document
                writer.WriteEndElement(); //STIX_Package
                writer.WriteEndDocument();

            }//Close the writer

            return true;
        }

        /// <summary>
        /// Sub funtion to help with writing the XML
        /// </summary>
        /// <param name="writer">The xml writer being used</param>
        /// <param name="id">The ID of the object to find related objects for</param>
        private void XML_WriteRelatedObjects(XmlWriter writer, string id)
        {
            bool headerwritten = false;
            foreach (ObservableRelationship r in Relationships)
            {
                if (r.From == id)
                {
                    if (!headerwritten)
                    {
                        headerwritten = true;
                        writer.WriteStartElement("cybox", "Related_Objects", null);
                    }

                    writer.WriteStartElement("cybox", "Related_Object", null);
                    writer.WriteAttributeString("idref", r.To+"_obj");
                    writer.WriteStartElement("cybox", "Relationship", null);
                    writer.WriteAttributeString("xsi", "type", null, "cyboxVocabs:ObjectRelationshipVocab-1.1");
                    writer.WriteValue(Enum.GetName(typeof(ObservableRelationshipType), r.RelationshipType));
                    writer.WriteEndElement(); //Relationship
                    writer.WriteEndElement(); //Related_Object
                }
            }
            if (headerwritten) writer.WriteEndElement();//Related_Objects
        }
        
        /// <summary>
        /// Output a MISP JSON represenation of the data in the collection
        /// </summary>
        /// <param name="filepath">The output path to save the XML file</param>
        /// <returns>True if output succeeded, false if it failed</returns>
        public  bool ToMISP_JSON(string filepath)
        {
            MISPEvent evt = new MISPEvent();

            evt.info = this.IncidentTitle;
            evt.publishtimestamp = HelperClass.GetUnixEpochTime(DateTime.Now);
            if (this.InitialCompromise != null) evt.timestamp = HelperClass.GetUnixEpochTime((DateTime)this.InitialCompromise);
            else evt.timestamp = "";
            evt.analysis = "2"; //"Complete"
            evt.published = true;
            if (this.IncidentDiscovered != null) evt.date = ((DateTime)this.IncidentDiscovered).ToString("yyyy-MM-dd");
            else evt.date = "";

            MISPOrgc org = new MISPOrgc();
            org.name = this.ReportingOrganisation;
            evt.orgc = org;

            evt.threat_level_id = "2"; //"Medium" - TODO - make this take value from Low/Mid/High by adding a new field to gui


            //Add the observables as attributes
            #region ObservablesToAttributes
            foreach (ObservableObject o in this.Observables)
            {
                MISPAttribute A = new MISPAttribute();
                
                A.comment = o.Description;
                if (this.InitialCompromise != null) A.timestamp = HelperClass.GetUnixEpochTime((DateTime)this.InitialCompromise);
                else A.timestamp = "";
                A.to_ids = false;

                //Do different stuff dependant on the type of indicator
                #region MalwareSample
                if (o.FriendlyTypeName == "Malware Sample")
                {
                    A.category = "Payload delivery";

                    if (o.Fields.FindAll(x => x.FieldName == "Hashes.MD5").Count > 0 && !String.IsNullOrWhiteSpace(o.Fields.Find(x => x.FieldName == "Hashes.MD5").Value))
                    {
                        if (!String.IsNullOrWhiteSpace(o.Fields.Find(x => x.FieldName == "File_Name").Value))
                        {
                            A.value = o.Fields.Find(x => x.FieldName == "File_Name").Value + "|" + o.Fields.Find(x => x.FieldName == "Hashes.MD5").Value;
                            A.type = "filename|md5";
                        }
                        else
                        {
                            A.value = o.Fields.Find(x => x.FieldName == "Hashes.MD5").Value;
                            A.type = "md5";
                        }                        
                    }
                    else if (o.Fields.FindAll(x => x.FieldName == "Hashes.SHA1").Count > 0 && !String.IsNullOrWhiteSpace(o.Fields.Find(x => x.FieldName == "Hashes.SHA1").Value))
                    {
                        if (!String.IsNullOrWhiteSpace(o.Fields.Find(x => x.FieldName == "File_Name").Value))
                        {
                            A.value = o.Fields.Find(x => x.FieldName == "File_Name").Value + "|" + o.Fields.Find(x => x.FieldName == "Hashes.SHA1").Value;
                            A.type = "filename|sha1";
                        }
                        else
                        {
                            A.value = o.Fields.Find(x => x.FieldName == "Hashes.SHA1").Value;
                            A.type = "sha1";
                        }
                    }
                    evt.AddAttribute(A);
                }
                #endregion
                #region Domain
                else if (o.FriendlyTypeName == "Domain")
                {
                    A.category = "Network activity";
                    A.value = o.Fields.Find(x => x.FieldName == "Domain").Value;
                    A.type = "domain";
                    evt.AddAttribute(A);
                }
                #endregion
                #region IP
                else if (o.FriendlyTypeName == "IP Address")
                {
                    A.category = "Network activity";
                    A.type = "ip-dst"; //TODO - CORRECTLY ASSIGN DIRECTION BASED ON RELATIONSHIPS

                    string ip4 = o.Fields.Find(x => x.FieldName == "IPv4").Value;
                    string ip6 = o.Fields.Find(x => x.FieldName == "IPv6").Value;
                    if(!string.IsNullOrEmpty(ip4) && string.IsNullOrEmpty(ip6))
                    {
                        A.value = ip4;
                    }
                    else if(string.IsNullOrEmpty(ip4) && !string.IsNullOrEmpty(ip6))
                    {
                        A.value = ip6;
                    }
                    else if(!string.IsNullOrEmpty(ip4) && !string.IsNullOrEmpty(ip6))
                    {
                        A.value = ip4;
                        MISPAttribute A2 = new MISPAttribute();                
                        A2.comment = A.comment;
                        A2.timestamp = A.timestamp;
                        A2.to_ids = A.to_ids;
                        A2.type = A.type;
                        A2.category = A.category;
                        A2.value = ip6;
                        evt.AddAttribute(A2);
                    }                    
                    evt.AddAttribute(A);
                }
                #endregion
                #region Registry
                else if (o.FriendlyTypeName == "Registry")
                {
                    A.category = "Persistence mechanism";

                    string hive = o.Fields.Find(x => x.FieldName == "Hive").Value;
                    string key = o.Fields.Find(x => x.FieldName == "Key").Value;
                    string rvalue = o.Fields.Find(x => x.FieldName == "Value").Value;


                    if (!string.IsNullOrEmpty(hive) && !string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(rvalue))
                    {
                        A.type = "regkey|value";
                        A.value = hive + "\\" + key + "|" + rvalue;
                    }
                    else
                    {
                        A.type = "regkey";
                        A.value = "";
                        if (!string.IsNullOrEmpty(hive)) A.value = hive + "\\";
                        A.value += key;
                    }
                    evt.AddAttribute(A);
                }
                #endregion
            }
            #endregion


            using(StreamWriter w = new StreamWriter(filepath))
            {
                w.WriteLine("{\"Event\":"+ JsonConvert.SerializeObject(evt,Newtonsoft.Json.Formatting.Indented)+"}");
            }


            return true;
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