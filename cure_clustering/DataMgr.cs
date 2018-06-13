using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace cure_clustering
{
    public class DataMgr
    {
         SqlConnection connection = null;
        public DataMgr()
        {
            connection = new SqlConnection(GlobalVariables.connection_string);
            connection.Open();
        }
      
        
        public  List<String> getAllTableNames()
        {
            List<String> tableNames=new List<string>();
            string cmdstr = "select SCHEMA_NAME(t.schema_id)+'.'+t.name AS Name From sys.tables As t";
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmdstr,connection);
            try
            {
                sda.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    tableNames.Add((String)row["Name"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return tableNames;
        }
        public List<String> getAllForeignKeyColumn(String tableName)
        {
            List<String> columnNames=new List<string>();
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(GlobalVariables.foriegn_key_query + "  where tab1.name='"+tableName+"'", connection);
            try
            {
                sda.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    columnNames.Add((String)row["column"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return columnNames;
        }
        public List<String> getAllForeignKeyTableNames(String tableName, String columnName)
        {
            List<String> tableNames = new List<string>();
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(GlobalVariables.foriegn_key_query + "  where tab1.name='" + tableName + "' AND col1.name='" + columnName + "'", connection);
            try
            {
                sda.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    String name = (String)row["referenced_table"];
                    String schemaQuery = GlobalVariables.foriegn_key_query + "  where tab1.name='" + name + "'";
                    SqlCommand command = new SqlCommand(schemaQuery,connection);
                    SqlDataReader reader = command.ExecuteReader();
                    String schema = "";
                    while(reader.Read()){
                        schema=reader["schema_name"].ToString();
                    }
                    if (schema != "")
                        tableNames.Add(schema + "." + name);
                    else
                        tableNames.Add(name);
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
               
            }
            return tableNames;
        }
        public List<String> getAllForeignKeyTableJoinColumnNames(String tableName,String columnName,String referenceTableName)
        {
            List<String> columnNames = new List<string>();
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(GlobalVariables.foriegn_key_query + "  where tab1.name='" + tableName + "' AND col1.name='"+columnName+"' AND tab2.name='"+referenceTableName+"'", connection);
            try
            {
                sda.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    String name = (String)row["referenced_column"];

                    columnNames.Add( name);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return columnNames;
        }
        public List<String> getAllColumns(String tableName, SqlConnection connection)
        {
            SqlDataAdapter sda = new SqlDataAdapter(GlobalVariables.column_query.Replace("{0}", tableName), connection);
            return getColumns(tableName,sda);
        }
        public List<String> getAllColumns(String tableName)
        {
            SqlDataAdapter sda = new SqlDataAdapter(GlobalVariables.column_query.Replace("{0}", tableName), GlobalVariables.connection_string);
            return getColumns(tableName, sda);
        }
        private List<String> getColumns(String tableName,SqlDataAdapter sda)
        {
           List<String> columnNames=new List<String>();
           
            DataTable dt=new DataTable();
            try
            {
                sda.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    columnNames.Add((String)row["COLUMN_NAME"]);
                }
            }
            catch (Exception ex)
            {

            }
            return columnNames;
        }
        public int countAllRows(String query)
        {
            SqlCommand command = new SqlCommand(query,this.connection);
            return command.ExecuteNonQuery();
        }
        public void dropTempTable()
        {
            SqlCommand command = new SqlCommand(GlobalVariables.drop_table_query, this.connection);
            command.ExecuteScalar();
        }
        ~DataMgr()
        {
            try
            {
                connection.Close();
            }
            catch(Exception ex){
                
            }
        }
    }
}
