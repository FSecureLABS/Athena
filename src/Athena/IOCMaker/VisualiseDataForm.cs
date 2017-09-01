using System;
using System.Windows.Forms;
using Microsoft.Msagl;

namespace Athena
{
    public partial class VisualiseDataForm : Form
    {
        public VisualiseDataForm(ref ObservableCollection Collection)
        {
            InitializeComponent();

            //create a form 
            System.Windows.Forms.Form form = this;
            //create a viewer object 
            Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            //create a graph object 
            Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");

            //Add all the nodes
            foreach(ObservableObject o in Collection.Observables)
            {
                graph.AddNode(o.DisplayTitle);
            }

            //create the links
            foreach (ObservableRelationship r in Collection.Relationships)
            {
                Microsoft.Msagl.Drawing.Edge ed = graph.AddEdge(Collection.Observables.Find(x => x.ID == r.From).DisplayTitle, Enum.GetName(typeof(ObservableRelationshipType), r.RelationshipType), Collection.Observables.Find(x => x.ID == r.To).DisplayTitle);

                //Set the link text to grey
                Microsoft.Msagl.Drawing.Label l = ed.Label;
                l.FontColor = Microsoft.Msagl.Drawing.Color.DarkGray;
            }

            //Get rid of GUI controls we dont want the user to have access to
            viewer.LayoutAlgorithmSettingsButtonVisible = false;
            viewer.EdgeInsertButtonVisible = false;
            viewer.SaveAsMsaglEnabled = false; //This disables save and load of the actual graph data.

            //Give some extra space
            graph.LayoutAlgorithmSettings.NodeSeparation = 10;

            //bind the graph to the viewer 
            viewer.Graph = graph;
            //associate the viewer with the form 
            form.SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            form.Controls.Add(viewer);
            form.ResumeLayout();
        }
    }
}
