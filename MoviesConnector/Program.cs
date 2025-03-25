using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;

namespace MoviesConnector
{
	//internal class Program
	//{
	//	static void Main(string[] args)
	//	{

	//	}
	//}
	class Connector
	{
		readonly string CONNECTION_STRING;
		readonly SqlConnection connection;
		public Connector(string connection_string)
		{
			this.CONNECTION_STRING = connection_string;
			this.connection = new SqlConnection(connection_string);
            Console.WriteLine(CONNECTION_STRING);
        }
		public void Select(string cmd)
		{
			
			const int PADDING = 34;
			
			//1) Создаем подключение к Базу:
			SqlConnection connection = new SqlConnection(CONNECTION_STRING);
			//2) Создаем команду, которую ходит выполнить на сервере:
			//string cmd = "SELECT * FROM Directors";
			string cmd = "SELECT * FROM Movies";
			SqlCommand command = new SqlCommand(cmd, connection);
			//3) Получаем результаты запросы с сервера:
			connection.Open();
			SqlDataReader reader = command.ExecuteReader();
			//4) Обрабатываем результаты запроса:
			Console.WriteLine();
			if (reader.HasRows)
			{
				for (int i = 0; i < reader.FieldCount; i++)
				{
					Console.Write(reader.GetName(i).ToString().PadRight(PADDING));
				}
				Console.WriteLine();
				while (reader.Read())
				{
					//Console.WriteLine($"{reader[0]}\t\t{reader[1]}\t\t{reader[2]}\t");
					for (int i = 0; i < reader.FieldCount; i++)
					{
						Console.Write(reader[i].ToString().PadRight(PADDING));
					}
					Console.WriteLine();
				}

			}
			//5) Закрываем поток и соединение с сервером:
			reader.Close();
			connection.Close();
		}
	}
}
