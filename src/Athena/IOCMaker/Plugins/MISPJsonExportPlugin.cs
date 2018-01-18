using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Athena;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;


namespace MISPJsonExportPlugin
{
    public class MISPJsonExportPlugin : AthenaPluginInterfaces.AthenaExportPlugin
    {
        public ObservableCollection col;
        public string outputpath;
        public List<string> Errors;

        public string Name { get { return "Export MISP Json"; } }
        public string Version { get { return "V0.01"; } }
        public string DisplayName { get { return "MISP JSON"; } } //The text that is shown in the Export-> Menu

        public string PluginSelected()
        {
            Errors = new List<string>();
            outputpath = "";
            col = new ObservableCollection();

            // MessageBox.Show("Hello from the plug in");

            return "JSON|*.json";


        }

        public void OutputFileSelected(string filepath, ObservableCollection collection)
        {
            outputpath = filepath;
            col = collection;
        }

        public void DoPreSaveOptions()
        {


        }

        public void DoSave()
        {
            MISPEvent evt = new MISPEvent();

            evt.info = col.IncidentTitle;
            evt.publishtimestamp = HelperClass.GetUnixEpochTime(DateTime.Now);
            if (col.InitialCompromise != null) evt.timestamp = HelperClass.GetUnixEpochTime((DateTime)col.InitialCompromise);
            else evt.timestamp = "";
            evt.analysis = "2"; //"Complete"
            evt.published = true;
            if (col.IncidentDiscovered != null) evt.date = ((DateTime)col.IncidentDiscovered).ToString("yyyy-MM-dd");
            else evt.date = "";

            MISPOrgc org = new MISPOrgc();
            org.name = col.ReportingOrganisation;
            evt.orgc = org;

            evt.threat_level_id = "2"; //"Medium" - TODO - make this take value from Low/Mid/High by adding a new field to gui


            //Add the col.Observables as attributes
            #region col.ObservablesToAttributes
            foreach (ObservableObject o in col.Observables)
            {
                MISPAttribute A = new MISPAttribute();

                A.comment = o.Description;
                if (col.InitialCompromise != null) A.timestamp = HelperClass.GetUnixEpochTime((DateTime)col.InitialCompromise);
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
                    if (!string.IsNullOrEmpty(ip4) && string.IsNullOrEmpty(ip6))
                    {
                        A.value = ip4;
                    }
                    else if (string.IsNullOrEmpty(ip4) && !string.IsNullOrEmpty(ip6))
                    {
                        A.value = ip6;
                    }
                    else if (!string.IsNullOrEmpty(ip4) && !string.IsNullOrEmpty(ip6))
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


            using (StreamWriter w = new StreamWriter(outputpath))
            {
                w.WriteLine("{\"Event\":" + JsonConvert.SerializeObject(evt, Newtonsoft.Json.Formatting.Indented) + "}");
            }            

            return;
            
        }

        public bool ExportSuccessful()
        {
            if (Errors.Count == 0) return true;
            else return false;
        }

        public List<string> ReportErrors()
        {

            return Errors;
        }
    }

    class MISPAttribute
    {
        public string category;
        public string comment;
        public string uuid;
        public string timestamp;
        public bool to_ids;
        public string value;
        public string type;

        public MISPAttribute()
        {
            uuid = Guid.NewGuid().ToString();
        }

    }

    class MISPTag
    {
        public string colour;
        public bool exportable;
        public string name;
    }

    class MISPOrgc
    {
        public string name;
        public string uuid;

        public MISPOrgc()
        {
            uuid = Guid.NewGuid().ToString();
        }
    }

    class MISPEvent
    {
        public string info;
        public string publishtimestamp;
        public string timestamp;
        public string analysis;
        public List<MISPAttribute> Attribute;
        public List<MISPTag> Tag;
        public bool published;
        public string date;
        public MISPOrgc orgc;
        public string threat_level_id;
        public string uuid;

        public MISPEvent()
        {
            uuid = Guid.NewGuid().ToString();
        }

        public void AddAttribute(MISPAttribute att)
        {
            if (Attribute == null) Attribute = new List<MISPAttribute>();
            Attribute.Add(att);
        }

        public void AddTag(MISPTag tag)
        {
            if (Tag == null) Tag = new List<MISPTag>();
            Tag.Add(tag);
        }

    }
}
