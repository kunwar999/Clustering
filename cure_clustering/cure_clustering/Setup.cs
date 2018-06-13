using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cure_clustering
{
    public partial class Setup : Form
    {
        Welcome welcome = null;
        DataMgr dataMgr = new DataMgr();
        public String selectedTableA = "";
        public String selectedTableB = "";
        public String selectedColumnA = "";
        public String selectedColumnB = "";
        public Boolean isJoinRquired = false;
        public Setup(Welcome welcome)
        {
            InitializeComponent();
            this.welcome = welcome;
            comboBox5.DataSource = dataMgr.getAllTableNames();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                comboBox2.Enabled = true;
                comboBox3.Enabled = true;
                comboBox4.Enabled = true;
            }
            else
            {
                comboBox2.Enabled = false;
                comboBox3.Enabled = false;
                comboBox4.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.isJoinRquired = checkBox1.Checked;
            this.selectedTableA = comboBox5.Text;
            this.selectedColumnA = comboBox2.Text;
            this.selectedTableB = comboBox3.Text;
            this.selectedColumnB = comboBox4.Text;
            CureProperties cureProperties = new CureProperties(this,dataMgr);
            cureProperties.Show();
            this.Hide();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            String tableName = (comboBox5.Text.Contains(""))?comboBox5.Text.Substring(comboBox5.Text.IndexOf('.') + 1):comboBox5.Text;
            comboBox2.DataSource = dataMgr.getAllForeignKeyColumn(tableName);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            String tableName = (comboBox5.Text.Contains("")) ? comboBox5.Text.Substring(comboBox5.Text.IndexOf('.') + 1) : comboBox5.Text;
            comboBox3.DataSource = dataMgr.getAllForeignKeyTableNames(tableName,comboBox2.Text);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            String tableName = (comboBox5.Text.Contains("")) ? comboBox5.Text.Substring(comboBox5.Text.IndexOf('.') + 1) : comboBox5.Text;
            comboBox4.DataSource = dataMgr.getAllForeignKeyTableJoinColumnNames(tableName, comboBox2.Text, comboBox3.Text.Substring(comboBox3.Text.IndexOf('.') + 1));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.welcome.Show();
            this.Hide();

        }
        ~Setup()
        {
            this.Dispose(false);
        }

        private void Setup_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
