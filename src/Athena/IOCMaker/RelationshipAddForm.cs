using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Athena
{
    public partial class RelationshipAddForm : Form
    {
        public bool openagain;
        ObservableCollection collection;

        public RelationshipAddForm(ref ObservableCollection c)
        {
            collection = c;
            InitializeComponent();
            openagain = false;

            foreach(ObservableObject o in collection.Observables)
            {
                FromListBox.Items.Add(o);
                ToListBox.Items.Add(o);
            }
            ToListBox.DisplayMember = "DisplayTitle";
            FromListBox.DisplayMember = "DisplayTitle";


            FromListBox.SelectionMode = SelectionMode.MultiExtended;
            ToListBox.SelectionMode = SelectionMode.MultiExtended;

            foreach(string s in Enum.GetNames(typeof(ObservableRelationshipType)))
            {
                RelationshipTypeDropDown.Items.Add(s);
            }
        }

        private void saveandclose()
        {
            if (ToListBox.SelectedItems == null || ToListBox.SelectedItems.Count == 0 || FromListBox.SelectedItems == null || FromListBox.SelectedItems.Count == 0)
            {
                MessageBox.Show("You didn't select either a to or a from observable to connect via the relationship", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (RelationshipTypeDropDown.SelectedItem == null)
            {
                MessageBox.Show("Select a relationship type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                foreach (ObservableObject to in ToListBox.SelectedItems)
                {
                    foreach (ObservableObject from in FromListBox.SelectedItems)
                    {
                        ObservableRelationship r = new ObservableRelationship(to.ID, from.ID, (ObservableRelationshipType)Enum.Parse(typeof(ObservableRelationshipType), RelationshipTypeDropDown.SelectedItem.ToString(), true));
                        collection.Relationships.Add(r);
                    }
                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            saveandclose();
        }

        private void SaveandNewButton_Click(object sender, EventArgs e)
        {
            openagain = true;
            saveandclose();
        }
    }
}
