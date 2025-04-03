using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using System.Runtime.InteropServices;

namespace Academy
{
	internal class Connector
	{
		readonly string CONNECTION_STRING;
		SqlConnection connection;
		public Connector(string connection_string)
		{
			CONNECTION_STRING = connection_string;
			connection = new SqlConnection(CONNECTION_STRING);
			AllocConsole();
            Console.WriteLine(CONNECTION_STRING);
        }
		public DataTable Select(string colums,string tables,string condition="",string group_by="")
		{
			DataTable table=null;
			string cmd = $"SELECT {colums} FROM {tables}";
			if (condition != "") cmd += $" WHERE {condition}";
			if (group_by != "") cmd += $" GROUP BY {group_by}";
			SqlCommand command = new SqlCommand(cmd, connection);
			connection.Open();

			SqlDataReader reader = command.ExecuteReader();
			if(reader.HasRows)
			{
				table = new DataTable();
				for(int i =0;i<reader.FieldCount;i++)
				{
					table.Columns.Add(reader.GetName(i));
				}
				while(reader.Read())
				{
					DataRow row = table.NewRow();
					for (int i = 0; i < reader.FieldCount; i++)
						row[i] = reader[i];
					table.Rows.Add(row);
				}
			}
			reader.Close();
			connection.Close();
			return table;
		}

		[DllImport("kernel32.dll")]
		public static extern bool AllocConsole();
		[DllImport("kernel32.dll")]
		public static extern bool FreeConsole();

	}
}
