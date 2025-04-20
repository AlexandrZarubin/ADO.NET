using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.Configuration;

namespace AcademyDataSet
{
	public partial class MainForm : Form
	{
		readonly string CONNECTION_STRING="";
		SqlConnection connection=null;
		DataSet GroupsRelatedData=null;
		public MainForm()
		{
			InitializeComponent();
			CONNECTION_STRING = ConfigurationManager.ConnectionStrings["VPD_311_Import"].ConnectionString;
			connection = new SqlConnection(CONNECTION_STRING);
			AllocConsole();
            Console.WriteLine(CONNECTION_STRING);
			//1) Создаем Dataset:
			GroupsRelatedData = new DataSet("GroupsRelatedData");
			AddTable("Directions", "direction_id,direction_name");
			AddTable("Groups", "group_id,group_name,direction");
			AddRelation("GroupsDirections", "Groups,direction", "Directions,direction_id" );
			Print("Groups");
            //PrintGroups();
            //LoadGroupsRelatedData();
            Console.WriteLine(HasParents("Groups"));
        }
		public void AddTable(string table,string columns)
		{
			
			//2.1)добавляем таблицу в DataSet
			GroupsRelatedData.Tables.Add(table);
			//2.2)добавляемполя(столбики) в таблицу
			string[] a_coluns=columns.Split(',');
			
			for(int i=0;i<a_coluns.Length;i++)
			{
				GroupsRelatedData.Tables[table].Columns.Add(a_coluns[i]);

			}

			//GroupsRelatedData.Tables[dsTable_Directions].Columns.Add(dst_col_direction_id, typeof(byte));
			//GroupsRelatedData.Tables[dsTable_Directions].Columns.Add(dst_col_direction_name, typeof(string));
			//2.3 Определяем, какое поле будем первичным ключем
			GroupsRelatedData.Tables[table].PrimaryKey =
				new DataColumn[] { GroupsRelatedData.Tables[table].Columns[0] };
			
			string cmd = $"SELECT {columns} FROM {table}";
			SqlDataAdapter adapter = new SqlDataAdapter(cmd, connection);
			adapter.Fill(GroupsRelatedData.Tables[table]);
			//Print(table);
		}
		public void AddRelation(string relation_name, string child,string parent)
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
			GroupsRelatedData.Tables[dsTable_Directions].Columns.Add(dst_col_direction_id,typeof(byte));
			GroupsRelatedData.Tables[dsTable_Directions].Columns.Add(dst_col_direction_name,typeof(string));
			//2.3 Определяем, какое поле будем первичным ключем
			GroupsRelatedData.Tables[dsTable_Directions].PrimaryKey =
				new DataColumn[] { GroupsRelatedData.Tables[dsTable_Directions].Columns[dst_col_direction_id] };

			const string dsTable_Groups = "Groups";
			const string dst_Groups_col_group_id = "group_id";
			const string dst_Groups_col_group_name = "group_name";
			const string dst_Groups_col_group_direction = "direction";
			GroupsRelatedData.Tables.Add(dsTable_Groups);
			GroupsRelatedData.Tables[dsTable_Groups].Columns.Add(dst_Groups_col_group_id,typeof(int));
			GroupsRelatedData.Tables[dsTable_Groups].Columns.Add(dst_Groups_col_group_name,typeof(string));
			GroupsRelatedData.Tables[dsTable_Groups].Columns.Add(dst_Groups_col_group_direction,typeof(byte));
			GroupsRelatedData.Tables[dsTable_Groups].PrimaryKey =
				new DataColumn[] { GroupsRelatedData.Tables[dsTable_Groups].Columns[0] };

			//3) строим связи между таблицами
			GroupsRelatedData.Relations.Add
				(
				"GroupsDirections",
				GroupsRelatedData.Tables[dsTable_Directions].Columns[dst_col_direction_id],			//Parent field - первичный ключ
				GroupsRelatedData.Tables[dsTable_Groups].Columns[dst_Groups_col_group_direction]	//Child field  - внешний ключ
				);

			//4) Загрузка данных в DataSet
			string directionsCmd = "SELECT * FROM Directions";
			string groupsCmd = "SELECT * FROM Groups";
			SqlDataAdapter directionsAdapter=new SqlDataAdapter(directionsCmd, connection);
			SqlDataAdapter groupsAdapter=new SqlDataAdapter(groupsCmd, connection);

			directionsAdapter.Fill(GroupsRelatedData.Tables[dsTable_Directions]);
			groupsAdapter.Fill(GroupsRelatedData.Tables[dsTable_Groups]);
			
			Print("Directions");
			Print("Groups");
			
		}
		public void Print(string table)
		{
			Console.WriteLine("\n---------------------------------------");
			for (int i = 0; i < GroupsRelatedData.Tables[table].Columns.Count; i++)
				Console.Write(GroupsRelatedData.Tables[table].Columns[i].Caption + "\t");
			Console.WriteLine("\n---------------------------------------");
			int number_of_parens=GroupsRelatedData.Tables[table].ParentRelations.Count;
			for (int i = 0; i < number_of_parens; i++)
			{
				Console.WriteLine(GroupsRelatedData.Tables[table].ParentRelations[i].ToString());
            }
			Console.WriteLine(GroupsRelatedData.Tables[table].ParentRelations.Contains("GroupsDirections"));
			for (int i = 0; i < GroupsRelatedData.Tables[table].Rows.Count; i++)
			{
				//Console.Write(GroupsRelatedData.Tables[table].Rows[i]+":\t");
				
				for (int j = 0; j < GroupsRelatedData.Tables[table].Columns.Count; j++)
				{

					if (HasParents(table) &&
						GroupsRelatedData.Tables[table].ParentRelations[0].ChildColumns.Contains(GroupsRelatedData.Tables[table].Columns[j]))
					{
						string paretn_relation_name = !HasParents(table) ? "" :
							$"{GroupsRelatedData.Tables[table].TableName}{GroupsRelatedData.Tables[table].Columns[j].ColumnName}s";
						Console.WriteLine(
							//GroupsRelatedData.Tables[table].ParentRelations[0].ParentColumns[$"{GroupsRelatedData.Tables[table].Columns[j].ColumnName}_name"]
							GroupsRelatedData.Tables[table].Rows[i].GetParentRow(paretn_relation_name)[$"{GroupsRelatedData.Tables[table].Columns[j].ColumnName}_name"]
							);
					}
					else
					{
						Console.Write(GroupsRelatedData.Tables[table].Rows[i][j]+"\t\t");
					}
					
                }
            
                Console.WriteLine();
            }
            Console.WriteLine("\n---------------------------------------");

        }
		bool HasParents(string table)
		{
			return GroupsRelatedData.Tables[table].ParentRelations.Count > 0;
		}
		void PrintGroups()
		{
            Console.WriteLine("\n---------------------------------------");
			string table = "Groups";
			for (int i = 0; i < GroupsRelatedData.Tables[table].Rows.Count; i++)
			{
				for (int j = 0; j < GroupsRelatedData.Tables[table].Columns.Count;j++)
				{
					Console.Write(GroupsRelatedData.Tables[table].Rows[i][j]+"\t");
                }
				Console.WriteLine(GroupsRelatedData.Tables[table].Rows[i].GetParentRow("GroupsDirections")["direction_name"]);
				Console.WriteLine();
            }
            Console.WriteLine("\n---------------------------------------");
		}
	[DllImport("kernel32.dll")]
	public static extern bool AllocConsole();

	[DllImport("kernel32.dll")]
	public static extern bool FreeConsole();
	}
}
