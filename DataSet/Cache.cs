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

		public void AddTable(string table, string columns)
		{
			Data.Tables.Add(table);
			string[] a_columns = columns.Split(',');
			for (int i = 0; i < a_columns.Length; i++)
			{
				Data.Tables[table].Columns.Add(a_columns[i]);
			}
			DataColumn[] pk = new DataColumn[1];
			pk[0] = Data.Tables[table].Columns[0];
			Data.Tables[table].PrimaryKey = pk;

			string[] sqlColumnArray = new string[a_columns.Length];
			for (int i = 0; i < a_columns.Length; i++)
			{
				sqlColumnArray[i] = "[" + a_columns[i] + "]";
			}
			string sqlColumns = string.Join(",", sqlColumnArray);
			string cmd = "SELECT " + sqlColumns + " FROM " + table;
			SqlDataAdapter adapter = new SqlDataAdapter(cmd, connection);
			adapter.Fill(Data.Tables[table]);
			Print(table);
		}


		public void AddRelation(string relationName, string child, string parent)
		{
			string[] parentParts = parent.Split(',');
			string[] childParts = child.Split(',');
			Data.Relations.Add(
				relationName,
				Data.Tables[parentParts[0]].Columns[parentParts[1]],
				Data.Tables[childParts[0]].Columns[childParts[1]]
				);
		}

		public void Print1(string table)
		{
			Console.WriteLine($"\n========== TABLE: {table} ==========\n"); // загаловок

			if (!Data.Tables.Contains(table))                  // проверка названия в DataSet
			{
				Console.WriteLine($"Таблица '{table}' не найдена.");        // соощение ошибки
				return;                                                     // выход таблицы нет
			}

			DataTable dt = Data.Tables[table];                 // получаем сылку таблицы из DataSet

			const int colWidth = 15;
			Console.WriteLine(">> Main row:");                              // основная таблица

			for (int i = 0; i < dt.Columns.Count; i++)                          // Перебираем все столбцы таблицы
				Console.Write(dt.Columns[i].ColumnName.PadRight(colWidth));           // Печатаем название столбца 
			Console.WriteLine();
			Console.WriteLine(new string('-', colWidth * dt.Columns.Count));// разделиттель

			for (int i = 0; i < dt.Rows.Count; i++)                                // Проходим по всем строкам таблицы
			{
				for (int j = 0; j < dt.Columns.Count; j++)                  // Проходим по каждой ячейке строки
				{
					string value = dt.Rows[i][j]?.ToString() ?? "";      // Выводим значение
					Console.Write(value.PadRight(colWidth));
				}
				Console.WriteLine();

				if (dt.ParentRelations.Count > 0)                           // Если у таблицы есть родительские связи
				{
					for (int r = 0; r < dt.ParentRelations.Count; r++)   // Перебираем все связи
					{
						DataRelation relation = dt.ParentRelations[r];
						DataRow parentRow = dt.Rows[i].GetParentRow(relation);     // Получаем родительскую строку для текущей строки через эту связь
						Console.WriteLine($"\t-> Parent [{relation.RelationName}] " + // Перебираем все столбцы родительской таблицы
							$"({relation.ParentTable.TableName}):");

						if (parentRow != null)                              // Если родительская строка найдена				
						{
							for (int c = 0; c < relation.ParentTable.Columns.Count; c++)// Перебираем все столбцы родительской таблицы
							{
								string columnName = relation.ParentTable.Columns[c].ColumnName; // Печатаем имя столбца и соответствующее значение из родительской строки
								Console.WriteLine($"\t   {columnName}: {parentRow[columnName]}");
							}
						}
						else
						{
							Console.WriteLine("\t   null");                 // сообщение что родитеской строки нет
						}
					}
				}
			}
		}

		public void Print(string table)
		{
			if (!Data.Tables.Contains(table))
			{
				Console.WriteLine($"Таблица '{table}' не найдена.");
				return;
			}
			Console.WriteLine("\n---------------------------------------");
			for (int i = 0; i < Data.Tables[table].Columns.Count; i++)
			{
				Console.Write(Data.Tables[table].Columns[i].Caption + "\t");
			}
			Console.WriteLine("\n---------------------------------------");
			for (int i = 0; i < Data.Tables[table].Rows.Count; i++)
			{
				// Для отладки: показываем сам объект строки
				Console.Write(Data.Tables[table].Rows[i] + ":\t");

				for (int j = 0; j < Data.Tables[table].Columns.Count; j++)
				{
					Console.Write(Data.Tables[table].Rows[i][j] + "\t");
				}
				Console.WriteLine();
			}

			Console.WriteLine("\n---------------------------------------");
		}
	}
}
