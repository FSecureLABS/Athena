using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Athena;
using System.Windows.Forms;
using System.IO;


namespace YaraRulesExportPlugin
{
    public class YaraRulesExportPlugin : AthenaPluginInterfaces.AthenaExportPlugin
    {
        public ObservableCollection col;
        public string outputpath;
        public List<string> Errors;

        public string Name { get { return "Export Yara Rule"; } }
        public string Version { get { return "V0.01"; } }
        public string DisplayName { get { return "Yara Rule"; } } //The text that is shown in the Export-> Menu

        public string PluginSelected()
        {
            Errors = new List<string>();
            outputpath = "";
            col = new ObservableCollection();

            // MessageBox.Show("Hello from the plug in");

            return "Yara|*.yar";


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
            YaraRule yara = new YaraRule(col.IncidentName, col.IncidentTitle);

            List<string> fieldnamestomap = new List<string> { "Domain", "IPv4", "IPv6", "Hashes.MD5", "Hashes.SHA1", "File_Name", "File_Path", "Key", "Hive", "Value" };

            foreach (ObservableObject o in col.Observables)
            {
                foreach (ObservableObjectField f in o.Fields)
                {
                    if (fieldnamestomap.Contains(f.FieldName) && f.Value != "")
                    {
                        yara.AddString(f.ID.Substring(f.ID.Length - 12), YaraVariableType.String, f.Value);
                    }
                }
            }

            try
            {
                using (StreamWriter w = new StreamWriter(outputpath))
                {
                    w.WriteLine(yara.GetRuleText());
                }
            }
            catch
            {
                Errors.Add("Error writing to file - is it locked, or was the path invalid?");
            }
        }

        public bool ExportSuccessful()
        {
            return Errors.Count == 0 ? true:false;
        }

        public List<string> ReportErrors()
        {
             return Errors;
        }
    }


    enum YaraVariableType { String, Hex }

    class YaraRule
    {
        public string RuleName;
        public string RuleReference;
        public Dictionary<string, KeyValuePair<string, YaraVariableType>> Strings;

        public YaraRule(string rulename, string rulereference = "")
        {
            Strings = new Dictionary<string, KeyValuePair<string, YaraVariableType>>();
            RuleName = rulename;
            RuleReference = rulereference;
        }

        public void AddString(string id, YaraVariableType vartype, string value)
        {
            Strings[id] = new KeyValuePair<string, YaraVariableType>(value, vartype);
        }

        public string GetRuleText()
        {
            StringBuilder main_sb = new StringBuilder();

            main_sb.AppendLine("Rule " + RuleName);
            main_sb.AppendLine("{");


            main_sb.AppendLine("\tmeta:");
            if (RuleReference != "") main_sb.AppendLine("\t\tref = \"" + RuleReference + "\"");
            main_sb.AppendLine("\t\tgenerated = \"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\"");
            main_sb.AppendLine("\t\tgeneratedby = \"Athena\"");
            main_sb.AppendLine();


            main_sb.AppendLine("\tstrings:");

            StringBuilder conditions_sb = new StringBuilder();
            foreach (KeyValuePair<string, KeyValuePair<string, YaraVariableType>> str in Strings)
            {
                if (str.Value.Value == YaraVariableType.Hex) main_sb.AppendLine("\t\t$" + str.Key + " = {" + str.Value.Key + "}");
                else main_sb.AppendLine("\t\t$" + str.Key + " = \"" + str.Value.Key + "\" nocase");

                conditions_sb.Append(" or $" + str.Key);
            }

            main_sb.AppendLine("");
            main_sb.AppendLine("\tcondition:");
            // main_sb.AppendLine("\t\t"+conditions_sb.ToString().Substring(4));
            main_sb.AppendLine("\t\tany of them");
            main_sb.AppendLine("}");
            return main_sb.ToString();

        }

    }
}
