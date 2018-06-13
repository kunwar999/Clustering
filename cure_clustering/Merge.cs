using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace cure_clustering
{
    public partial class Merge : Form
    {
        PartitioningResults partitioningResults = null;

        public Merge(PartitioningResults partitioningResults)
        {
            InitializeComponent();
            this.partitioningResults = partitioningResults;
            DataTable dt = new DataTable();
            DataTable[]  values=this.partitioningResults.map.Values.ToArray();
            foreach (DataTable table in values)
            {
                dt.Merge(table);
            }
            DataView view = dt.DefaultView;
            view.Sort = this.partitioningResults.selectedColumn+" ASC";
            dataGridView1.DataSource = view.ToTable();
        }

     
        private void button2_Click(object sender, EventArgs e)
        {
            this.partitioningResults.Show();
            this.Hide();
        }

      

        private void Merge_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

      
    }
}
