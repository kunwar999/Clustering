using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace cure_clustering
{
    public partial class CureProperties : Form
    {
        Setup setup = null;
        DataMgr dataMgr =null;
        public String totalNumberOfRows = "";
        public String totalNumberOfClusters = "";
        public String selectedTempColumn = "";
        public String selectedConditional = "";
        public String selectedValue = "";
        public CureProperties(Setup setup,DataMgr datamgr)
        {
            InitializeComponent();
            this.setup = setup;
            this.dataMgr = datamgr;
            this.dataMgr.dropTempTable();
            textBox1.Text = this.dataMgr.countAllRows(getQuery())+"";
            this.comboBox1.DataSource = this.dataMgr.getAllColumns(GlobalVariables.tempTableName);
        }
        private String getQuery()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * INTO " + GlobalVariables.tempTableName + " FROM " + this.setup.selectedTableA);
            StringBuilder columnSelect = new StringBuilder();
            columnSelect.Append(" IDENTITY(int, 1,1) AS ID ,");
            String tableAName = this.setup.selectedTableA.Substring(this.setup.selectedTableA.IndexOf('.') + 1);
            String tableBName = this.setup.selectedTableB.Substring(this.setup.selectedTableB.IndexOf('.') + 1);
            List<String> tableAColumns = this.dataMgr.getAllColumns(tableAName);
            foreach (String column in tableAColumns)
            {
                columnSelect.Append("a." + column + " AS " + tableAName + column + " ,");
            }
            if (this.setup.isJoinRquired)
            {
                List<String> tableBColumns = this.dataMgr.getAllColumns(tableBName);
                foreach (String column in tableBColumns)
                {
                    columnSelect.Append("b." + column + " AS " + tableBName +column+",");
                }
              }
            columnSelect.Remove(columnSelect.Length - 1, 1);
            query.Replace("*", columnSelect.ToString());
            query.Append(" a INNER JOIN " + this.setup.selectedTableB + " b ON a." + this.setup.selectedColumnA + "=b." + this.setup.selectedColumnB);
            
                return query.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.setup.Show();
            this.Hide();
        }

        private void CureProperties_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.totalNumberOfRows = textBox1.Text;
            this.totalNumberOfClusters = textBox2.Text;
            this.selectedTempColumn = comboBox1.Text;
            this.selectedConditional = comboBox2.Text;
            this.selectedValue = textBox3.Text;
            PartitioningResults partitioningResults = new PartitioningResults(this.setup,this,this.dataMgr);
            partitioningResults.Show();
            this.Hide();
        }
    }
}
