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
    public partial class PartitioningResults : Form
    {
        Setup setup = null;
        CureProperties cureProperties = null;
        DataMgr dataMgr =null;
        SqlConnection connection = null;
        public Dictionary<String, DataTable> map = null;
        public String selectedColumn = null;
        
        public PartitioningResults(Setup setup,CureProperties cureProperties, DataMgr datamgr)
        {
            InitializeComponent();
            this.setup = setup;
            this.connection = new SqlConnection(GlobalVariables.connection_string);
            this.connection.Open();
            this.dataMgr = datamgr;
            this.cureProperties = cureProperties;
            initliazePartitionMap();
            this.comboBox1.DataSource=this.map.Keys.ToArray();  
        }

        private void initliazePartitionMap()
        {
            map = new Dictionary<String, DataTable>();
            int count=Convert.ToInt32(this.cureProperties.totalNumberOfClusters);
            for (int i = 0; i <count ; i++)
            {
                DataTable dt = new DataTable();
                StringBuilder query = new StringBuilder();
                query.Append("SELECT * FROM ( SELECT ROW_NUMBER() OVER(ORDER BY ID) AS rowid,* FROM "+GlobalVariables.tempTableName+") C WHERE rowid % "+count+" =  "+i);
                if(this.cureProperties.selectedConditional!=null && this.cureProperties.selectedConditional!=""){
                    if (this.cureProperties.selectedConditional.Equals("EQUAL"))
                    {
                        query.Append(" AND "+this.cureProperties.selectedTempColumn+" = '"+this.cureProperties.selectedValue+"'");
                    }
                    else if (this.cureProperties.selectedConditional.Equals("LESS THAN"))
                    {
                        query.Append(" AND " + this.cureProperties.selectedTempColumn + " < '" + this.cureProperties.selectedValue + "'");
                    }
                    else if (this.cureProperties.selectedConditional.Equals("LESS THAN EQUAL"))
                    {
                        query.Append(" AND " + this.cureProperties.selectedTempColumn + " <= '" + this.cureProperties.selectedValue + "'");
                    }
                    else if (this.cureProperties.selectedConditional.Equals("GREATER THAN EQUAL"))
                    {
                        query.Append(" AND " + this.cureProperties.selectedTempColumn + " >= '" + this.cureProperties.selectedValue + "'");
                    }
                    else if (this.cureProperties.selectedConditional.Equals("GREATER THAN"))
                    {
                        query.Append(" AND " + this.cureProperties.selectedTempColumn + " > '" + this.cureProperties.selectedValue + "'");
                    }
                    else if (this.cureProperties.selectedConditional.Equals("LIKE"))
                    {
                        query.Append(" AND " + this.cureProperties.selectedTempColumn + " LIKE '" + this.cureProperties.selectedValue + "'");
                    }
                    else if (this.cureProperties.selectedConditional.Equals("NOT LIKE"))
                    {
                        query.Append(" AND " + this.cureProperties.selectedTempColumn + " NOT LIKE '" + this.cureProperties.selectedValue + "'");
                    }
                    else if (this.cureProperties.selectedConditional.Equals("NOT IN"))
                    {
                        query.Append(" AND " + this.cureProperties.selectedTempColumn + " NOT IN (" + this.cureProperties.selectedValue+")" );
                    }
                    else if (this.cureProperties.selectedConditional.Equals("IN"))
                    {
                        query.Append(" AND " + this.cureProperties.selectedTempColumn + " IN  (" + this.cureProperties.selectedValue+")" );
                    }
                    else if (this.cureProperties.selectedConditional.Equals("NULL"))
                    {
                        query.Append(" AND " + this.cureProperties.selectedTempColumn + " IS NULL");
                    }
                    else if (this.cureProperties.selectedConditional.Equals("NOT NULL"))
                    {
                        query.Append(" AND " + this.cureProperties.selectedTempColumn + " IS NOT NULL");
                    }
                      else if (this.cureProperties.selectedConditional.Equals("BETWEEN"))
                    {
                        String[] values = this.cureProperties.selectedValue.Split(',');
                        query.Append(" AND " + this.cureProperties.selectedTempColumn + " BETWEEN "+values[0]+" AND "+values[1]);
                    }
                    else if (this.cureProperties.selectedConditional.Equals("NOT BETWEEN"))
                    {
                        String[] values = this.cureProperties.selectedValue.Split(',');
                        query.Append(" AND " + this.cureProperties.selectedTempColumn + " NOT BETWEEN " + values[0] + " AND " + values[1] );
                    }
                }
                SqlDataAdapter sda = new SqlDataAdapter(query.ToString(), GlobalVariables.connection_string);
                try
                {
                    sda.Fill(dt);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                map.Add("partition" + (i + 1),dt);
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.cureProperties.Show();
            this.Hide();
        }

        private void PartitioningResults_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource=map[comboBox1.Text];

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.selectedColumn = this.cureProperties.selectedTempColumn;
            Merge merge = new Merge(this);
            merge.Show();
            this.Hide();
        }

      
    }
}
