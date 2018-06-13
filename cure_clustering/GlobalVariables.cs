using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cure_clustering
{
    class GlobalVariables
    {
        public static String tempTableName = "Temp";
        public static String connection_string= @"Data Source=(local);Initial Catalog=AdventureWorks;User ID=KUNWAR-PC;Password=a";
        public static String foriegn_key_query =   " SELECT  obj.name AS FK_NAME,sch.name AS [schema_name],tab1.name AS " +
                         " [table],col1.name AS [column],tab2.name AS [referenced_table],col2.name AS " +
                         " [referenced_column] FROM sys.foreign_key_columns fkc INNER JOIN sys.objects obj " +
                         " ON obj.object_id = fkc.constraint_object_id INNER JOIN sys.tables tab1 ON " +
                         " tab1.object_id = fkc.parent_object_id INNER JOIN sys.schemas sch " +
                         " ON tab1.schema_id = sch.schema_id INNER JOIN sys.columns col1 " +
                         " ON col1.column_id = parent_column_id AND col1.object_id = tab1.object_id " +
                         " INNER JOIN sys.tables tab2  ON tab2.object_id = fkc.referenced_object_id " +
                         " INNER JOIN sys.columns col2 ON col2.column_id = referenced_column_id AND " +
                         " col2.object_id = tab2.object_id";
        public static String column_query= "SELECT * from INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME=N'{0}'";
        public static String drop_table_query = "IF OBJECT_ID('"+tempTableName+"', 'U') IS NOT NULL DROP TABLE " + tempTableName;
    }
}
