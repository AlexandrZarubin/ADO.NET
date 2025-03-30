using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace MoviesConnector
{
	internal class Connector
	{
		static readonly int PADDING = 33;
		readonly string CONNECTION_STRING;
		readonly SqlConnection connection;
		public Connector() : this(ConfigurationManager.ConnectionStrings["Movies_VPD_311"].ConnectionString)
		{
			//CONNECTION_STRING =
			//	ConfigurationManager.ConnectionStrings["Movies_VPD_311"].ConnectionString;
			//this.connection = new SqlConnection(CONNECTION_STRING);
		}
		public Connector(string connection_string)
		{
			this.CONNECTION_STRING = connection_string;
			this.connection = new SqlConnection(connection_string);
			Console.WriteLine(CONNECTION_STRING);
		}
		public void Select(string fields,string tables,string condition="")
		{

			//1) Создаем подключение к Базу:
			//SqlConnection connection = new SqlConnection(CONNECTION_STRING);    // 1) Создаем объект подключения к базе данных

			//2) Создаем команду, которую ходит выполнить на сервере:
			//string cmd = "SELECT * FROM Directors";                             // 2) Создаем SQL-команду: выбрать все записи из таблицы Directors
			string cmd = "SELECT * FROM Movies";
			SqlCommand command = new SqlCommand(cmd, connection);

			//3) Получаем результаты запросы с сервера:
			connection.Open();                                                  // 3) Открываем подключение к базе данных
			SqlDataReader reader = command.ExecuteReader();                     // Выполняем команду и получаем результат в виде SqlDataReader

			//4) Обрабатываем результаты запроса:
			Console.WriteLine();
			if (reader.HasRows)                                                  // Проверяем, вернулись ли строки
			{

				Border(reader.FieldCount);
				for (int i = 0; i < reader.FieldCount; i++)                          // Выводим заголовки столбцов
				{
					//Console.Write(reader.GetName(i)+"\t");
					Console.Write(reader.GetName(i).ToString().PadRight(PADDING));
				}
				Console.WriteLine();
				Border(reader.FieldCount);
				while (reader.Read())                                           // Чтение строк из результата
				{
					//Console.WriteLine($"{reader[0]}\t{reader[1]}\t{reader[2]}\t");
					for (int i = 0; i < reader.FieldCount; i++)                    // Вывод значений каждого столбца строки
					{
						//Console.Write(reader[i]+"\t\t");
						Console.Write(reader[i].ToString().PadRight(PADDING));
					}
					Console.WriteLine();
				}

			}
			//5) Закрываем поток и соединение с сервером:
			Border(reader.FieldCount, "=");
			reader.Close();
			connection.Close();
		}
		void Border(int fields_count, string symbol = "-")
		{
			for (int i = 0; i < fields_count; i++)
			{
				for (int j = 0; j < PADDING / 1.2; j++)
				{
					Console.Write(symbol);
				}
			}
			Console.WriteLine();
		}
	}
}
