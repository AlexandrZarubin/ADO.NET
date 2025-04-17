using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;

namespace AcademyCache
{
	internal class Cache
	{
		public DataSet Data { get; private set; }
		private SqlConnection connection;


		public Cache(SqlConnection connection)
		{
			this.connection = connection;
			Data = new DataSet();
		}
	}
}
