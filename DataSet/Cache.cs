using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace AcademyDataSet
{
	internal class Cache
	{
		readonly string CONNECTION_STRING = "";
		SqlConnection connection = null;
		DataSet GroupsRelatedData = null;
		public DataSet Set { get => GroupsRelatedData; }
		public SqlConnection Connection { get => connection; }
		public Cache(string connectionString)
		{
			CONNECTION_STRING = ConfigurationManager.ConnectionStrings["VPD_311_Import"].ConnectionString;
			connection = new SqlConnection(CONNECTION_STRING);
			GroupsRelatedData = new DataSet();
			Console.WriteLine(CONNECTION_STRING);
		}
		public void AddTable(string table, string columns)
		{

			//2.1)добавляем таблицу в DataSet
			GroupsRelatedData.Tables.Add(table);

			//2.2)добавляемполя(столбики) в таблицу
			string[] a_coluns = columns.Split(',');

			for (int i = 0; i < a_coluns.Length; i++)
			{
				GroupsRelatedData.Tables[table].Columns.Add(a_coluns[i]);

			}

			//GroupsRelatedData.Tables[dsTable_Directions].Columns.Add(dst_col_direction_id, typeof(byte));
			//GroupsRelatedData.Tables[dsTable_Directions].Columns.Add(dst_col_direction_name, typeof(string));
			//2.3 Определяем, какое поле будем первичным ключем
			GroupsRelatedData.Tables[table].PrimaryKey =
				new DataColumn[] { GroupsRelatedData.Tables[table].Columns[0] };

			string safeColumns = string.Join(",", a_coluns.Select(col => $"[{col.Trim()}]"));
			string cmd = $"SELECT {safeColumns} FROM {table}";
			//string cmd = $"SELECT {columns} FROM {table}";
			SqlDataAdapter adapter = new SqlDataAdapter(cmd, connection);
			adapter.Fill(GroupsRelatedData.Tables[table]);
			//Print(table);
		}
		public void AddRelation(string relation_name, string child, string parent)
		{
			GroupsRelatedData.Relations.Add
				(
				relation_name,
				GroupsRelatedData.Tables[parent.Split(',')[0]].Columns[parent.Split(',')[1]],
				GroupsRelatedData.Tables[child.Split(',')[0]].Columns[child.Split(',')[1]]
				);
		}
		void LoadGroupsRelatedData()
		{

			//2) Добавляем таблицы в DataSet:
			const string dsTable_Directions = "Directions";
			const string dst_col_direction_id = "direction_id";
			const string dst_col_direction_name = "direction_name";
			//2.1)добавляем таблицу в DataSet
			GroupsRelatedData.Tables.Add(dsTable_Directions);
			//2.2)добавляемполя(столбики) в таблицу
			GroupsRelatedData.Tables[dsTable_Directions].Columns.Add(dst_col_direction_id, typeof(byte));
			GroupsRelatedData.Tables[dsTable_Directions].Columns.Add(dst_col_direction_name, typeof(string));
			//2.3 Определяем, какое поле будем первичным ключем
			GroupsRelatedData.Tables[dsTable_Directions].PrimaryKey =
				new DataColumn[] { GroupsRelatedData.Tables[dsTable_Directions].Columns[dst_col_direction_id] };

			const string dsTable_Groups = "Groups";
			const string dst_Groups_col_group_id = "group_id";
			const string dst_Groups_col_group_name = "group_name";
			const string dst_Groups_col_group_direction = "direction";
			GroupsRelatedData.Tables.Add(dsTable_Groups);
			GroupsRelatedData.Tables[dsTable_Groups].Columns.Add(dst_Groups_col_group_id, typeof(int));
			GroupsRelatedData.Tables[dsTable_Groups].Columns.Add(dst_Groups_col_group_name, typeof(string));
			GroupsRelatedData.Tables[dsTable_Groups].Columns.Add(dst_Groups_col_group_direction, typeof(byte));
			GroupsRelatedData.Tables[dsTable_Groups].PrimaryKey =
				new DataColumn[] { GroupsRelatedData.Tables[dsTable_Groups].Columns[0] };

			//3) строим связи между таблицами
			GroupsRelatedData.Relations.Add
				(
				"GroupsDirections",
				GroupsRelatedData.Tables[dsTable_Directions].Columns[dst_col_direction_id],         //Parent field - первичный ключ
				GroupsRelatedData.Tables[dsTable_Groups].Columns[dst_Groups_col_group_direction]    //Child field  - внешний ключ
				);

			//4) Загрузка данных в DataSet
			string directionsCmd = "SELECT * FROM Directions";
			string groupsCmd = "SELECT * FROM Groups";
			SqlDataAdapter directionsAdapter = new SqlDataAdapter(directionsCmd, connection);
			SqlDataAdapter groupsAdapter = new SqlDataAdapter(groupsCmd, connection);

			directionsAdapter.Fill(GroupsRelatedData.Tables[dsTable_Directions]);
			groupsAdapter.Fill(GroupsRelatedData.Tables[dsTable_Groups]);

			Print("Directions");
			Print("Groups");

		}
		public void Print(string table)
		{
			Console.WriteLine($"\n=== Таблица: {table} ===\n");

			if (!GroupsRelatedData.Tables.Contains(table))
			{
				Console.WriteLine("Таблица не найдена.");
				return;
			}

			// Заголовки
			for (int i = 0; i < GroupsRelatedData.Tables[table].Columns.Count; i++)
			{
				Console.Write(GroupsRelatedData.Tables[table].Columns[i].ColumnName + "\t\t");
			}
			Console.WriteLine("\n----------------------------------");

			// Обход строк
			for (int rowIndex = 0; rowIndex < GroupsRelatedData.Tables[table].Rows.Count; rowIndex++)
			{
				DataRow row = GroupsRelatedData.Tables[table].Rows[rowIndex];

				for (int colIndex = 0; colIndex < GroupsRelatedData.Tables[table].Columns.Count; colIndex++)
				{
					DataColumn column = GroupsRelatedData.Tables[table].Columns[colIndex];
					bool valuePrinted = false;

					// Проверяем каждую связь с родителем
					for (int relIndex = 0; relIndex < GroupsRelatedData.Tables[table].ParentRelations.Count; relIndex++)
					{
						DataRelation relation = GroupsRelatedData.Tables[table].ParentRelations[relIndex];

						// Если текущий столбец — часть этой связи (child column)
						for (int c = 0; c < relation.ChildColumns.Length; c++)
						{
							if (relation.ChildColumns[c] == column)
							{
								// Получаем родительскую строку
								DataRow parent = row.GetParentRow(relation);
								if (parent != null)
								{
									// Ищем в родителе колонку с "name"
									for (int pc = 0; pc < relation.ParentTable.Columns.Count; pc++)
									{
										DataColumn parentCol = relation.ParentTable.Columns[pc];
										if (parentCol.ColumnName.Contains("name"))
										{
											Console.Write(parent[parentCol] + "\t\t");
											valuePrinted = true;
											break;
										}
									}
								}
							}
							if (valuePrinted) break;
						}
						if (valuePrinted) break;
					}

					// Если не нашли родительскую колонку — выводим обычное значение
					if (!valuePrinted)
					{
						Console.Write(row[column] + "\t\t");
					}
				}

				Console.WriteLine();
			}

			Console.WriteLine("\n=============================\n");
		}
		public bool HasParents(string table)
		{
			return GroupsRelatedData.Tables[table].ParentRelations.Count > 0;
		}
		public void PrintGroups()
		{
			Console.WriteLine("\n---------------------------------------");
			string table = "Groups";
			for (int i = 0; i < GroupsRelatedData.Tables[table].Rows.Count; i++)
			{
				for (int j = 0; j < GroupsRelatedData.Tables[table].Columns.Count; j++)
				{
					Console.Write(GroupsRelatedData.Tables[table].Rows[i][j] + "\t");
				}
				Console.WriteLine(GroupsRelatedData.Tables[table].Rows[i].GetParentRow("GroupsDirections")["direction_name"]);
				Console.WriteLine();
			}
			Console.WriteLine("\n---------------------------------------");
		}
	}
}
