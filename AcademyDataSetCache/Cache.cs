using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AcademyDataSetCache
{
    public class Cache
    {
		public DataSet Data { get; private set; }                           // публичное свойство с DataSet (только для чтения извне)
		private SqlConnection connection;                                   // приватное поле для подключения к SQL Server


		public Cache(SqlConnection connection)                              // конструктор: принимает SQL-подключение извне
		{
			this.connection = connection;                                   // сохраняем подключение в поле
			Data = new DataSet();                                           // создаём пустой DataSet
		}

		public void AddTable(string table, string columns)                  // Метод добавляет таблицу и загружает в неё данные
		{
			Data.Tables.Add(table);                                         // добавляем таблицу в DataSet
			string[] a_columns = columns.Split(',');                        // разбиваем строку с именами столбцов по запятой
			for (int i = 0; i < a_columns.Length; i++)                      // по каждому имени столбца
			{
				Data.Tables[table].Columns.Add(a_columns[i]);               // добавляем столбец в таблицу
			}
			DataColumn[] pk = new DataColumn[1];                            // создаём массив под первичный ключ
			pk[0] = Data.Tables[table].Columns[0];                          // назначаем первым столбец из списка
			Data.Tables[table].PrimaryKey = pk;                             // указываем его как первичный ключ


			// Экранируем SQL-имена столбцов, чтобы избежать проблем с зарезервированными словами
			string[] sqlColumnArray = new string[a_columns.Length];         // массив для [имя]
			for (int i = 0; i < a_columns.Length; i++)
			{
				sqlColumnArray[i] = "[" + a_columns[i] + "]";               // экранируем каждое имя
			}
			string sqlColumns = string.Join(",", sqlColumnArray);           // объединяем в строку через запятую
			string cmd = "SELECT " + sqlColumns + " FROM " + table;         // формируем SELECT-запрос
			SqlDataAdapter adapter = new SqlDataAdapter(cmd, connection);   // создаём адаптер
			adapter.Fill(Data.Tables[table]);                               // загружаем данные из БД в таблицу
																			//Print(table);
		}


		public void AddRelation(string relationName, string child, string parent)   // Метод создаёт отношение (связь) между двумя таблицами
		{
			string[] parentParts = parent.Split(',');                       // разбиваем "TableName,columnName" на части
			string[] childParts = child.Split(',');
			Data.Relations.Add(                                             // Добавляем связь: родитель - ребёнок
				relationName,                                               // имя связи
				Data.Tables[parentParts[0]].Columns[parentParts[1]],        // родительский столбец
				Data.Tables[childParts[0]].Columns[childParts[1]]           // дочерний столбец
				);
		}

		public void Print1(string table)
		{
			Console.WriteLine($"\n========== TABLE: {table} ==========\n"); // заголовок таблицы

			if (!Data.Tables.Contains(table))                               // проверка на наличие таблицы в DataSet
			{
				Console.WriteLine($"Таблица '{table}' не найдена.");        // соощение ошибки
				return;                                                     // выход, таблицы нет
			}

			DataTable dt = Data.Tables[table];                              // получаем сылку таблицы из DataSet

			const int colWidth = 15;
			Console.WriteLine(">> Main row:");                              // основная таблица

			for (int i = 0; i < dt.Columns.Count; i++)                      // Перебираем все столбцы таблицы
				Console.Write(dt.Columns[i].ColumnName.PadRight(colWidth)); // Печатаем название столбца 
			Console.WriteLine();
			Console.WriteLine(new string('-', colWidth * dt.Columns.Count));// разделиттель

			for (int i = 0; i < dt.Rows.Count; i++)                         // по каждой строке таблицы
			{
				for (int j = 0; j < dt.Columns.Count; j++)                  // Проходим по каждой ячейке строки
				{
					string value = dt.Rows[i][j]?.ToString() ?? "";         // получаем значение
					Console.Write(value.PadRight(colWidth));                // печатаем выровнено
				}
				Console.WriteLine();

				if (dt.ParentRelations.Count > 0)                           // Если у таблицы есть родительские связи
				{
					for (int r = 0; r < dt.ParentRelations.Count; r++)      // Перебираем все связи
					{
						DataRelation relation = dt.ParentRelations[r];      // текущая связь
						DataRow parentRow = dt.Rows[i].GetParentRow(relation);// родительская строка
						Console.WriteLine($"\t-> Parent [{relation.RelationName}] " + // Перебираем все столбцы родительской таблицы
							$"({relation.ParentTable.TableName}):");

						if (parentRow != null)                              // Если родительская строка найдена				
						{
							for (int c = 0; c < relation.ParentTable.Columns.Count; c++)// Перебираем все столбцы родительской таблицы
							{
								string columnName = relation.ParentTable.Columns[c].ColumnName;
								Console.WriteLine($"\t   {columnName}: {parentRow[columnName]}");// имя: значение
							}
						}
						else
						{
							Console.WriteLine("\t   null");
						}
					}
				}
			}
		}

		public void Print(string table)                                             // Простой вывод без связей — только таблица
		{
			if (!Data.Tables.Contains(table))                                       // проверка на наличие таблицы
			{
				Console.WriteLine($"Таблица '{table}' не найдена.");
				return;
			}
			Console.WriteLine("\n---------------------------------------");
			for (int i = 0; i < Data.Tables[table].Columns.Count; i++)
			{
				Console.Write(Data.Tables[table].Columns[i].Caption + "\t");        // заголовки колонок
			}
			Console.WriteLine("\n---------------------------------------");
			for (int i = 0; i < Data.Tables[table].Rows.Count; i++)
			{
				// Для отладки: показываем сам объект строки
				Console.Write(Data.Tables[table].Rows[i] + ":\t");                  // индекс строки (object)

				for (int j = 0; j < Data.Tables[table].Columns.Count; j++)
				{
					Console.Write(Data.Tables[table].Rows[i][j] + "\t");            // значения в строке
				}
				Console.WriteLine();
			}

			Console.WriteLine("\n---------------------------------------");
		}

		public DataTable Select(Query query)
		{
			string cmdText = $"SELECT {query.Columns} FROM {query.Tables}";
			if (!string.IsNullOrEmpty(query.Condition))
			{
				cmdText += " WHERE " + query.Condition;
			}
			if (!string.IsNullOrEmpty(query.GroupBy))
			{
				cmdText += " GROUP BY " + query.GroupBy;
			}

			DataTable result = new DataTable();
			SqlDataAdapter adapter = new SqlDataAdapter(cmdText, connection);
			adapter.Fill(result);
			return result;
		}
	}
}

