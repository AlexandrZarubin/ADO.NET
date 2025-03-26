using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;    //ADO.NET classes
using System.Runtime.Remoting.Messaging;
/*
	ADO.NET - ActiveX Data Objects
		System.Data.SqlClient:
			-SqlConnection  - описывает сетевое соединение с Базой данных;
			-SqlCommand		- описываеет команду, отправляемую на Сервер;
			-SqlDataReader	- описывает поток данных от сервера, к клиентскому приложению,
								а так же позволяет отправлять команды на Сервер;
			-DataSet		- обеспечивает локальное хранилище данных из Базы;
			-DataAdapter	- является посредником между DataSet и источником данных;

*/
namespace ADO.NET_V2
{
	internal class Program
	{
		static void Main(string[] args)
		{
            Console.WriteLine("HELLO SASHKA");
			const int PADDING = 33;												// Количество пробелов для выравнивания вывода в консоли
			const string CONNECTION_STRING =                                    // Строка подключения к локальной базе данных SQL Server
				"Data Source=(localdb)\\MSSQLLocalDB;" +
				"Initial Catalog=Movies_VPD_311;" +
				"Integrated Security=True;" +
				"Connect Timeout=30;" +
				"Encrypt=False;" +
				"TrustServerCertificate=False;" +//без пробелов
				"ApplicationIntent=ReadWrite;" +
				"MultiSubnetFailover=False";
            Console.WriteLine(CONNECTION_STRING);

			//1) Создаем подключение к Базу:
			SqlConnection connection = new SqlConnection(CONNECTION_STRING);	// 1) Создаем объект подключения к базе данных

			//2) Создаем команду, которую ходит выполнить на сервере:
			//string cmd = "SELECT * FROM Directors";                             // 2) Создаем SQL-команду: выбрать все записи из таблицы Directors
			string cmd = "SELECT * FROM Movies";
			SqlCommand command = new SqlCommand(cmd, connection);

			//3) Получаем результаты запросы с сервера:
			connection.Open();                                                  // 3) Открываем подключение к базе данных
			SqlDataReader reader = command.ExecuteReader();                     // Выполняем команду и получаем результат в виде SqlDataReader
																				
			//4) Обрабатываем результаты запроса:
			Console.WriteLine();
			if(reader.HasRows)                                                  // Проверяем, вернулись ли строки
			{
				for(int i = 0;i<reader.FieldCount;i++)                          // Выводим заголовки столбцов
				{
                    //Console.Write(reader.GetName(i)+"\t");
					Console.Write(reader.GetName(i).ToString().PadRight(PADDING));
				}
                Console.WriteLine();
                while (reader.Read())                                           // Чтение строк из результата
				{
					//Console.WriteLine($"{reader[0]}\t{reader[1]}\t{reader[2]}\t");
					for (int i = 0; i<reader.FieldCount;i++)                    // Вывод значений каждого столбца строки
					{
						//Console.Write(reader[i]+"\t\t");
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
