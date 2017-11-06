using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Athena;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Serialization;


namespace Stix1ExportPlugin
{
    public class Stix1ExportPlugin : AthenaPluginInterfaces.AthenaExportPlugin
    {
        public ObservableCollection col;
        public string outputpath;
        public List<string> Errors;

        public string Name { get { return "Export Stix 1.1.1"; } }
        public string Version { get { return "V0.01"; } }
        public string DisplayName { get { return "Stix 1.1.1"; } } //The text that is shown in the Export-> Menu

        public string PluginSelected()
        {
            Errors = new List<string>();
            outputpath = "";
            col = new ObservableCollection();            

            return "Stix XML|*.xml";


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
            //Before exporting anything, make sure the ID values are correct
            col.UpdateIDs();

            XmlWriterSettings x = new XmlWriterSettings();
            x.Indent = true;
            using (XmlWriter writer = XmlWriter.Create(outputpath, x))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("stix", "STIX_Package", "http://stix.mitre.org/stix-1");
                writer.WriteAttributeString("id", col.ID);


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
                tmp += "   " + "http://stix.mitre.org/stix-1";
                tmp += "   " + "http://stix.mitre.org/XMLSchema/core/1.1.1/stix_core.xsd";
                tmp += "   " + "http://stix.mitre.org/default_vocabularies-1";
                tmp += "   " + "http://stix.mitre.org/XMLSchema/default_vocabularies/1.1.1/stix_default_vocabularies.xsd";
                tmp += "   " + "http://cybox.mitre.org/cybox-2";
                tmp += "   " + "http://cybox.mitre.org/XMLSchema/core/2.1/cybox_core.xsd";
                tmp += "   " + "http://cybox.mitre.org/common-2";
                tmp += "   " + "http://cybox.mitre.org/XMLSchema/common/2.1/cybox_common.xsd";
                tmp += "   " + "http://cybox.mitre.org/default_vocabularies-2";
                tmp += "   " + "http://cybox.mitre.org/XMLSchema/default_vocabularies/2.1/cybox_default_vocabularies.xsd";
                tmp += "   " + "http://cybox.mitre.org/objects#WinRegistryKeyObject-2";
                tmp += "   " + "http://cybox.mitre.org/XMLSchema/objects/Win_Registry_Key/2.0/Win_Registry_Key_Object.xsd";
                tmp += "   " + "http://cybox.mitre.org/objects#AddressObject-2";
                tmp += "   " + "http://cybox.mitre.org/XMLSchema/objects/Address/2.1/Address_Object.xsd";
                tmp += "   " + "http://cybox.mitre.org/objects#DomainNameObject-1";
                tmp += "   " + "http://cybox.mitre.org/XMLSchema/objects/Domain_Name/1.0/Domain_Name_Object.xsd";
                tmp += "   " + "http://cybox.mitre.org/objects#FileObject-2";
                tmp += "   " + "http://cybox.mitre.org/XMLSchema/objects/File/2.1/File_Object.xsd";
                tmp += "   " + "http://cybox.mitre.org/XMLSchema/objects/Address/2.1/Address_Object.xsd";
                tmp += "   " + "http://cybox.mitre.org/objects#URIObject-2";
                tmp += "   " + "http://cybox.mitre.org/XMLSchema/objects/URI/2.1/URI_Object.xsd";
                tmp += "   " + "http://stix.mitre.org/Incident-1";
                tmp += "   " + "http://stix.mitre.org/XMLSchema/incident/1.1.1/incident.xsd";
                tmp += "   " + "http://stix.mitre.org/TTP-1";
                tmp += "   " + "http://stix.mitre.org/XMLSchema/ttp/1.1.1/ttp.xsd";
                tmp += "   " + "http://stix.mitre.org/common-1";
                tmp += "   " + "http://stix.mitre.org/XMLSchema/common/1.1.1/stix_common.xsd";
                tmp += "   " + "http://stix.mitre.org/default_vocabularies-1";
                tmp += "   " + "http://stix.mitre.org/XMLSchema/default_vocabularies/1.1.1.0/stix_default_vocabularies.xsd";
                writer.WriteAttributeString("xsi", "schemaLocation", null, tmp);



                writer.WriteAttributeString("version", "1.1.1");
                #endregion


                #region Writecol.Observables
                if (col.Observables.Count > 0)
                {
                    writer.WriteStartElement("stix", "col.Observables", null);
                    writer.WriteAttributeString("cybox_major_version", "2");
                    writer.WriteAttributeString("cybox_minor_version", "1");
                    writer.WriteAttributeString("cybox_update_version", "0");
                }

                foreach (ObservableObject obs in col.Observables)
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

                        XML_WriteRelatedObjects(writer, obs.ID);

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
                                writer.WriteAttributeString("id", obs.ID + "_obj");
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
                        writer.WriteAttributeString("id", obs.ID + "_obj");
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

                //Write the closing tag for the group of col.Observables if needed
                if (col.Observables.Count > 0) writer.WriteEndElement();

                #endregion


                #region WriteIncidentDetails
                writer.WriteStartElement("stix", "Incidents", null);
                writer.WriteStartElement("stix", "Incident", null);
                writer.WriteAttributeString("id", col.IncidentID);
                if (col.InitialCompromise != null) writer.WriteAttributeString("timestamp", ((DateTime)col.InitialCompromise).ToString("yyyy-MM-ddTHH:mm:ss") + "+00:00");
                writer.WriteAttributeString("xsi", "type", null, "incident:IncidentType");

                writer.WriteStartElement("incident", "Title", null);
                writer.WriteValue(col.IncidentTitle);
                writer.WriteEndElement();

                writer.WriteStartElement("incident", "Time", null);

                if (col.InitialCompromise != null)
                {
                    writer.WriteStartElement("incident", "Initial_Compromise", null);
                    writer.WriteAttributeString("precision", "second");
                    writer.WriteValue(((DateTime)col.InitialCompromise).ToString("yyyy-MM-ddTHH:mm:ss"));
                    writer.WriteEndElement();//A single time
                }

                if (col.IncidentDiscovered != null)
                {
                    writer.WriteStartElement("incident", "Incident_Discovery", null);
                    writer.WriteAttributeString("precision", "second");
                    writer.WriteValue(((DateTime)col.IncidentDiscovered).ToString("yyyy-MM-ddTHH:mm:ss"));
                    writer.WriteEndElement();//A single time
                }

                if (col.IncidentResolved != null)
                {
                    writer.WriteStartElement("incident", "Restoration_Achieved", null);
                    writer.WriteAttributeString("precision", "second");
                    writer.WriteValue(((DateTime)col.IncidentResolved).ToString("yyyy-MM-ddTHH:mm:ss"));
                    writer.WriteEndElement();//A single time
                }

                if (col.IncidentReported != null)
                {
                    writer.WriteStartElement("incident", "Incident_Reported", null);
                    writer.WriteAttributeString("precision", "second");
                    writer.WriteValue(((DateTime)col.IncidentReported).ToString("yyyy-MM-ddTHH:mm:ss"));
                    writer.WriteEndElement();//A single time          
                }

                writer.WriteEndElement();//The time group

                writer.WriteStartElement("incident", "Description", null);
                writer.WriteValue(col.IncidentDescription);
                writer.WriteEndElement();

                writer.WriteStartElement("incident", "Reporter", null);
                writer.WriteStartElement("stixCommon", "Description", null);
                writer.WriteValue("The person who reported the incident");
                writer.WriteEndElement();//Description
                writer.WriteStartElement("stixCommon", "Identity", null);
                writer.WriteAttributeString("id", HelperClass.GenerateID(col.ReportingOrganisation, "x", "Identity"));
                writer.WriteStartElement("stixCommon", "Name", null);
                writer.WriteValue(col.ReportedBy);
                writer.WriteEndElement();//Name
                writer.WriteEndElement();//Identity
                writer.WriteEndElement();//Reporter

                writer.WriteStartElement("incident", "Responder", null);
                writer.WriteStartElement("stixCommon", "Description", null);
                writer.WriteValue("The person who responded to the incident");
                writer.WriteEndElement();//Description
                writer.WriteStartElement("stixCommon", "Identity", null);
                writer.WriteAttributeString("id", HelperClass.GenerateID(col.ReportingOrganisation, "x", "Identity"));
                writer.WriteStartElement("stixCommon", "Name", null);
                writer.WriteValue(col.Responder);
                writer.WriteEndElement();//Name
                writer.WriteEndElement();//Identity
                writer.WriteEndElement();//Responder


                writer.WriteStartElement("incident", "Impact_Assessment", null);
                writer.WriteStartElement("incident", "Effects", null);
                writer.WriteStartElement("incident", "Effect", null);
                writer.WriteAttributeString("xsi", "type", null, "stixVocabs:IncidentEffectVocab-1.0");
                writer.WriteValue(col.IncidentEffect);
                writer.WriteEndElement();//Effect
                writer.WriteEndElement();//Effects
                writer.WriteEndElement();//Impact Assessment


                //If we have col.Observables, link them to the incident here
                if (col.Observables.Count > 0)
                {
                    writer.WriteStartElement("incident", "Related_col.Observables", null);

                }
                foreach (ObservableObject obs in col.Observables)
                {

                    writer.WriteStartElement("incident", "Related_Observable", null);
                    writer.WriteStartElement("stixCommon", "Observable", null);
                    writer.WriteAttributeString("idref", obs.ID);
                    writer.WriteEndElement(); //Observable
                    writer.WriteEndElement(); //RelatedObservable

                }
                if (col.Observables.Count > 0) writer.WriteEndElement(); //Relatedcol.Observables

                writer.WriteStartElement("incident", "Confidence", null);
                writer.WriteAttributeString("timestamp", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + ".0000+00:00");
                writer.WriteStartElement("stixCommon", "Value", null);
                writer.WriteAttributeString("xsi", "type", null, "stixVocabs:HighMediumLowVocab-1.0");
                writer.WriteValue(col.Confidence);
                writer.WriteEndElement();//Value
                writer.WriteEndElement();//Confidence

                writer.WriteEndElement();//Incident
                writer.WriteEndElement();//Incidents
                #endregion
                
                //End the document
                writer.WriteEndElement(); //STIX_Package
                writer.WriteEndDocument();

            }//Close the writer

            
        }

        public bool ExportSuccessful()
        {
            return Errors.Count == 0 ? true : false;
        }

        public List<string> ReportErrors()
        {
            return Errors;
        }

      
        /// <summary>
        /// Sub funtion to help with writing the XML
        /// </summary>
        /// <param name="writer">The xml writer being used</param>
        /// <param name="id">The ID of the object to find related objects for</param>
        private void XML_WriteRelatedObjects(XmlWriter writer, string id)
        {
            bool headerwritten = false;
            foreach (ObservableRelationship r in col.Relationships)
            {
                if (r.From == id)
                {
                    if (!headerwritten)
                    {
                        headerwritten = true;
                        writer.WriteStartElement("cybox", "Related_Objects", null);
                    }

                    writer.WriteStartElement("cybox", "Related_Object", null);
                    writer.WriteAttributeString("idref", r.To + "_obj");
                    writer.WriteStartElement("cybox", "Relationship", null);
                    writer.WriteAttributeString("xsi", "type", null, "cyboxVocabs:ObjectRelationshipVocab-1.1");
                    writer.WriteValue(Enum.GetName(typeof(ObservableRelationshipType), r.RelationshipType));
                    writer.WriteEndElement(); //Relationship
                    writer.WriteEndElement(); //Related_Object
                }
            }
            if (headerwritten) writer.WriteEndElement();//Related_Objects
        }


    }
}

