using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace MoviesForms
{
	internal class Connector
	{
		public readonly SqlConnection connection;
		readonly string CONNECTION_STRING;
		public Connector():this(ConfigurationManager.ConnectionStrings["Movies_VPD_311"].ConnectionString)
		{
			//string connString = ConfigurationManager.ConnectionStrings["Movies_VPD_311"].ConnectionString;
			//connection = new SqlConnection(connString);
		}
		public Connector(string connection_string)
		{
			this.CONNECTION_STRING = connection_string;
			this.connection = new SqlConnection(connection_string);
			Console.WriteLine(CONNECTION_STRING);
		}
		public DataTable ExecuteSelect(string query)
		{
			SqlCommand cmd = new SqlCommand(query, connection);
			DataTable table = new DataTable();
			connection.Open();
			SqlDataReader reader = cmd.ExecuteReader();
			table.Load(reader);
			reader.Close();
			connection.Close();
			return table;
		}
	}
}
