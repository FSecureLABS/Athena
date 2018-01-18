using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Athena;
using System.Windows.Forms;
using System.IO;


namespace SnortRulesExportPlugin
{
    public class SnortRulesExportPlugin : AthenaPluginInterfaces.AthenaExportPlugin
    {
        public ObservableCollection col;
        public string outputpath;
        public List<string> Errors;

        Form optform;

        public List<string> SupportedProtocols;
        public List<SnortRule> Rules;

        public string Name { get { return "Export Snort Rules"; } }
        public string Version { get { return "V0.01"; } }
        public string DisplayName { get { return "Snort Rules..."; } } //The text that is shown in the Export-> Menu

        public string PluginSelected()
        {
            Errors = new List<string>();
            outputpath = "";
            col = new ObservableCollection();

            SupportedProtocols = new List<string>(){"TCP", "UDP", "ICMP","IP"};

           // MessageBox.Show("Hello from the plug in");

            return "Snort Rules|*.rules";

            
        }

        public void OutputFileSelected(string filepath, ObservableCollection collection)
        {
            outputpath = filepath;
            col = collection;
        }

        public void DoPreSaveOptions()
        {
            Rules = new List<SnortRule>();

            optform = new Form();
            optform.Width = 500;
            optform.Height = 500;
            Panel optpanel = new Panel();
            optform.Controls.Add(optpanel);
            optpanel.Width = 470;
            optpanel.Height = 420;
            optpanel.AutoScroll = true;

            //Add the explanation label
            AthenaPluginFormHelpers.AddLabel("Please select the options below to customise your Snort Rules output:", optpanel, 5, 5);

            int maxtitlelength = 60;
            int x = 10;
            int y = 30;
            int yinc = 20;
            int cnt = 0;
            Dictionary<string,int> ID_Form_Mapping = new Dictionary< string,int> ();
            foreach(ObservableObject o in col.Observables)
            {

                cnt++;
                ID_Form_Mapping[o.ID] = cnt;

                string title = o.DisplayTitle.Length < maxtitlelength ? o.DisplayTitle : o.DisplayTitle.Substring(0, maxtitlelength);


                if (o.Type == ObservableObject.ObservableType.Domain)
                {
                    AthenaPluginFormHelpers.AddCheckbox("Any mention in TCP/UDP packets of " + title, optpanel, x, y, true, "obs_" + cnt);
                    y += yinc;
                }

                if (o.Type == ObservableObject.ObservableType.IPAddress)
                { 
                    AthenaPluginFormHelpers.AddCheckbox("Any traffic, in any protocol, to or from " + title, optpanel, x, y, true, "obs_" + cnt);
                    y += yinc;
                }
            //if(o.Type == ObservableObject.ObservableType.Registry)
            //    AthenaPluginFormHelpers.AddCheckbox("Any mention in TCP/UDP packets of the key/value under " + title, optpanel, x, y, true, "obs_" + cnt);
            //if (o.Type == ObservableObject.ObservableType.Sample)
            //    AthenaPluginFormHelpers.AddCheckbox("Any mention in TCP/UDP packets of filename for " + title, optpanel, x, y, true, "obs_" + cnt);


        }

            Button closebut = new Button();
            closebut.Text = "Save";
            closebut.Click += Closebut_Click;
            closebut.Location = new System.Drawing.Point(15, 430);
            optform.Controls.Add(closebut);

            optform.ShowDialog();

            SnortRule r;
            foreach (ObservableObject o in col.Observables)
            {
                if ( (o.Type == ObservableObject.ObservableType.Domain || o.Type == ObservableObject.ObservableType.IPAddress) &&  ((CheckBox)optpanel.Controls["obs_" + ID_Form_Mapping[o.ID]]).Checked)
                {
                    if (o.Type == ObservableObject.ObservableType.Domain)
                    {
                        r = new SnortRule();
                        r.id = o.ID;
                        r.protocol = "TCP";
                        r.sourceip = "any";
                        r.sourceport = "any";
                        r.destip = "any";
                        r.destport = "any";
                        r.contentstring = o.Fields.First(fld => fld.FieldName == "Domain").Value;
                        r.msg = "ATHENA KNOWN IOC: Mention of Domain which is present in an IOC Collection";

                        Rules.Add(r);
                        //Add a rule for UDP, too
                        r = r.Clone();
                        r.protocol = "UDP";
                        Rules.Add(r);
                    }
                       
                    if (o.Type == ObservableObject.ObservableType.IPAddress)
                    {
                        //Temp list to hold the placeholder rules
                        List<SnortRule> temprules = new List<SnortRule>();


                        //First let add a "TO" rule for [blank] protocol
                        r = new SnortRule();
                        r.id = o.ID;
                        r.protocol = "";
                        r.sourceip = "any";
                        r.sourceport = "any";
                        r.destip = "any";
                        r.destport = "any";
                        r.contentstring = "";
                        r.msg = "ATHENA KNOWN IOC: [PROTO] Traffic to an IP Address which is present in an IOC Collection";
                        //Set the IP Address and Deal with cases where the IPV4 and IPV6 addresses are populated for the same IOC (add two rules, one for each)
                        if (o.Fields.First(fld => fld.FieldName == "IPv4").Value.Length > 0 && o.Fields.First(fld => fld.FieldName == "IPv6").Value.Length > 0)
                        {
                            r.sourceip = o.Fields.First(fld => fld.FieldName == "IPv4").Value;
                            temprules.Add(r);
                            r = r.Clone();
                            r.sourceip = o.Fields.First(fld => fld.FieldName == "IPv6").Value;
                            temprules.Add(r);
                        }
                        else
                        {
                            //r = r.Clone();
                            r.sourceip = o.Fields.First(fld => fld.FieldName == "IPv4").Value.Length > 0 ? o.Fields.First(fld => fld.FieldName == "IPv4").Value : o.Fields.First(fld => fld.FieldName == "IPv6").Value;
                            temprules.Add(r);
                        }

                        //Now we have our "TO" rule(s), lets duplicate it/them to also be "from" rules
                        SnortRule newrule;
                        int trc = temprules.Count;
                        for (int c = 0;c<trc;c++)
                        {
                            
                            newrule = temprules[c].Clone();
                            newrule.destip = temprules[c].sourceip;
                            newrule.sourceip = "any";
                            newrule.msg = "ATHENA KNOWN IOC: [PROTO] Traffic from an IP Address which is present in an IOC Collection";
                            temprules.Add(newrule);
                        }

                        //Now lets duplicate all of these for all the protocols
                        trc = temprules.Count;
                        for (int c = 0; c < trc; c++)
                        {                            
                            foreach (string s in SupportedProtocols)
                            {
                                newrule = temprules[c].Clone();
                                newrule.protocol = s;
                                newrule.msg = newrule.msg.Replace("[PROTO]", s);
                                temprules.Add(newrule);
                            }
                        }

                        //Now lets remove the template rules and add the rest to the main list
                        foreach(SnortRule rls in temprules)
                        {
                            if (rls.protocol != "") Rules.Add(rls);
                        }
                           
                    }
                    //if (o.Type == ObservableObject.ObservableType.Registry)
                    //{
                    //    SnortRule r = new SnortRule();
                    //    r.id = o.ID;
                    //}
                    //if (o.Type == ObservableObject.ObservableType.Sample)
                    //{
                    //    SnortRule r = new SnortRule();
                    //    r.id = o.ID;
                    //}

                    }
            }

        }

        private void Closebut_Click(object sender, EventArgs e)
        {
            optform.DialogResult = DialogResult.OK;
            optform.Close();
        }

        public void DoSave()
        {
            using (StreamWriter w = new StreamWriter(outputpath))
            {
                foreach (SnortRule r in Rules)
                {
                    w.WriteLine(r.GetAlert());
                }
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

    public class SnortRule
    {
        public string id;
        public string protocol;
        public string sourceip;
        public string sourceport;
        public string destip;
        public string destport;
        public string contentstring;
        public string msg;

        public SnortRule()
        {
            id = "";
            protocol = "";
            sourceip = "";
            sourceport = "";
            destip = "";
            destport = "";
            msg = "";
            contentstring = "";
        }

        public string GetAlert()
        {
            string al = "Alert " + protocol + " " + sourceip + " " + sourceport + " -> " + destip + " " + destport + " (msg:\"" + msg + "\";";
            if (contentstring != "") al += "content:\"" + contentstring + "\";";
            al += ")";
            return al;
        }

        public SnortRule Clone()
        {
            SnortRule cloned = new SnortRule();
            cloned.id = id;
            cloned.protocol = protocol;
            cloned.sourceip = sourceip;
            cloned.sourceport = sourceport;
            cloned.destip = destip;
            cloned.destport = destport;
            cloned.msg = msg;
            cloned.contentstring = contentstring;

            return cloned;
        }
    }
}
