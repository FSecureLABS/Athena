using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Athena
{
    class AthenaPluginInterfaces
    {
        public interface AthenaExportPlugin
        {
            string Name { get; } //The name of the plugin - not currently used for anything
            string Version { get; } //The version of the pluging - not currently used for anything

            string DisplayName { get; } //The text that is shown in the Export-> Menu

            string PluginSelected(); //Fires when the user first selects this export plugin. Should return the a string of filters for the filter property of the Save As dialoge, e.g.: "Yara Files|*.yar"

            void OutputFileSelected(string filepath, ObservableCollection collection); //Fires when the Save As box is closed with "OK" and passes the selected path to the file to save as and the collection to save

            void DoPreSaveOptions(); //Fires straight after OutputFileSelcted. Use this function to display a dialogue etc to edit the options
            
            void DoSave(); //Fires after DoPreSaveOptions and should do the processing and saving. 

            bool ExportSuccessful(); //Should return true on successful save, false on error - Method used to allow processing to check, if needed

            List<string> ReportErrors(); //Fires if DoSave() returns false, main program will handle display to user
        }
    }

    public static class AthenaPluginFormHelpers
    {
        public static void AddLabel(string text,Panel target,int posx,int posy,string name="")
        {
            Label l = new Label();
            l.Text = text;
            l.Location = new System.Drawing.Point(posx, posy);
            if (name != "") l.Name = name;

            //Set the correct width and height to ensure all text is shown
            using (Graphics g = target.CreateGraphics())
            {
                SizeF size = g.MeasureString(l.Text, l.Font);
                l.Width =  (int)size.Width + 5;
                SizeF hsize = g.MeasureString(l.Text, l.Font, l.Width);
                l.Height = (int)hsize.Height;
            }
            
            target.Controls.Add(l);
        }
        public static void AddCheckbox(string text,Panel target,int posx,int posy,bool checkedbydefault,string name = "")
        {
            Label l = new Label();
            l.Text = text;
            int width = 50;
            using (Graphics g = target.CreateGraphics())
            {
                SizeF size = g.MeasureString(l.Text, l.Font);
                width = (int)size.Width + 25;
            }

            CheckBox b = new CheckBox();
            b.Text = text;
            b.Location = new System.Drawing.Point(posx, posy);
            b.Checked = checkedbydefault;
            b.Width = width;
            if (name != "") b.Name = name;
            target.Controls.Add(b);
        }
    }
}
