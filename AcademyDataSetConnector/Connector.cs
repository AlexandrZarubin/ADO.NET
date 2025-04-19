using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AcademyDataSetConnector
{
    public class Connector
    {
		public readonly string CONNECTION_STRING;
		public SqlConnection Connection { get; private set; }

		public Connector() : this("Data Source=USER-PC\\SQLEXPRESS;" +
			"Initial Catalog=VPD_311_Import;Integrated Security=True;" +
			"Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;" +
			"ApplicationIntent=ReadWrite;MultiSubnetFailover=False")
		{
		}
		public Connector(string connectionString)
		{
			CONNECTION_STRING = connectionString;
			Connection = new SqlConnection(CONNECTION_STRING);

			AllocConsole();
			Console.WriteLine("Подключение:");
			Console.WriteLine(CONNECTION_STRING);
		}

		[DllImport("kernel32.dll")]
		public static extern bool AllocConsole();
	}
}
