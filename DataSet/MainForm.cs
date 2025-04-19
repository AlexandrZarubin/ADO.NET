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

//using AcademyCache;
using System.Reflection;

using AcademyDataSetCache;
using AcademyDataSetConnector;
//using Query = AcademyDataSetCache.Query;

namespace AcademyDataSet
{
	public partial class MainForm : Form
	{
		readonly string CONNECTION_STRING="";
		SqlConnection connection=null;
		Cache cache;


		Query[] queries = new Query[]
		{
			new Query("*","Students JOIN Groups ON([group]=group_id) JOIN Directions ON (direction=direction_id)"),
			//new Query("*","Groups"),
			new Query("group_id,group_name,COUNT(stud_id),direction_name",
				"Students, Groups, Directions",
				"direction=direction_id AND [group]=group_id",
				"group_id,group_name,direction_name"),
			//new Query("*","Directions"),
			//new Query(
			//	"direction_id, direction_name, COUNT(DISTINCT group_id) AS GroupCount, COUNT(stud_id) AS StudentCount",
			//	"Directions LEFT JOIN Groups ON Groups.direction = Directions.direction_id LEFT JOIN Students ON Students.[group] = Groups.group_id",
			//	"",
			//	"direction_id, direction_name"),
			new Query(
				@"direction_name,
			COUNT(DISTINCT group_id) AS N'Кол-во групп',
			COUNT(DISTINCT stud_id)	AS N'Кол-во студентов'",
				@"Students
			JOIN        Groups      ON  ([group]	=group_id)
			RIGHT JOIN  Directions  ON  (direction	= direction_id)",
			"",
			"direction_name")
		};
		DataGridView[] tables;
		string[] status_messages = new string[]
		{
			"Кол-во студентов: ",
			"Кол-во групп: ",
			"Кол-во направлений: "
		};

		private Dictionary<string, int> d_directions = new Dictionary<string, int>();
		private Dictionary<string, int> d_groups = new Dictionary<string, int>();

		public Dictionary<string, int> Directions => d_directions;
		public Dictionary<string, int> Groups => d_groups;
		public MainForm()
		{
			InitializeComponent();
			tabControl.SelectedIndexChanged += tabControl_SelectedIndexChanged;
			tables = new DataGridView[]
{
	dgvStudents,
	dgvGroups,
	dgvDirections
};
			//CONNECTION_STRING = ConfigurationManager.ConnectionStrings["VPD_311_Import"].ConnectionString;
			//connection = new SqlConnection(CONNECTION_STRING);

			//Connector connector = new Connector("Data Source=USER-PC\\SQLEXPRESS;" +
			//"Initial Catalog=VPD_311_Import;Integrated Security=True;" +
			//"Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;" +
			//"ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

			Connector connector = new Connector();
			connection = connector.Connection;

			AllocConsole();
            Console.WriteLine(CONNECTION_STRING);
			//1) Создаем Dataset:
			cache = new Cache(connection);											// создаём Cache, передаём подключение
			cache.AddTable("Directions", "direction_id,direction_name");
			cache.AddTable("Groups", "group_id,group_name,direction");
			cache.AddRelation("GroupsDirections", "Groups,direction", "Directions,direction_id");
			
			cache.AddTable("Students", "stud_id,last_name,first_name,middle_name,birth_date,group");
			cache.AddRelation("StudentsGroups", "Students,group", "Groups,group_id");

			
			InitComboBoxes();
			dgvStudents.DataSource = cache.Data.Tables["Students"];
			//cache.Print1("Students");

			//cache.Print1("Groups");
			LoadTab();
		}
		private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			LoadTab();
		}

		void LoadTab(Query query = null)
		{
			int i = tabControl.SelectedIndex;
			if (query == null) query = queries[i];
			tables[i].DataSource = cache.Select(query);
			toolStripStatusLabel1.Text = $"{status_messages[i]} {tables[i].RowCount - 1}";
		}
		private void InitComboBoxes()
		{
			DataTable directions = cache.Data.Tables["Directions"];
			DataTable groups = cache.Data.Tables["Groups"];

			// Заполняем словарь направлений
			for (int i = 0; i < directions.Rows.Count; i++)
			{
				string name = directions.Rows[i]["direction_name"].ToString();
				int id = Convert.ToInt32(directions.Rows[i]["direction_id"]);
				d_directions[name] = id;
				cbStudentsDirection.Items.Add(name);
			}
			
			// Заполняем словарь всех групп (пока без фильтрации)
			for (int i = 0; i < groups.Rows.Count; i++)
			{
				string name = groups.Rows[i]["group_name"].ToString();
				int id = Convert.ToInt32(groups.Rows[i]["group_id"]);
				d_groups[name] = id;
				cbStudentsGroup.Items.Add(name);
			}
			foreach (string directionName in d_directions.Keys)
			{
				cbGroupsDirection.Items.Add(directionName);
			}
			cbStudentsDirection.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
			cbStudentsGroup.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
			cbGroupsDirection.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
		}
		private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i = tabControl.SelectedIndex;
			Query query = new Query(queries[i]); // копируем оригинальный запрос
			string tab_name = (sender as ComboBox).Name;

			
			int lastUpper = Array.FindLastIndex(tab_name.ToCharArray(), Char.IsUpper);
			string field_name = tab_name.Substring(lastUpper);

			string member_name = $"d_{field_name.ToLower()}s"; // d_directions или d_groups
			Dictionary<string, int> source = 
				this.GetType().GetField(member_name, BindingFlags.NonPublic | BindingFlags.Instance)
				.GetValue(this) as Dictionary<string, int>;

			string selected_value = (sender as ComboBox).SelectedItem?.ToString();

			// Если ничего не выбрано — просто обновляем вкладку без фильтра
			if (string.IsNullOrEmpty(selected_value))
			{
				LoadTab(queries[i]);
				return;
			}

			// Добавляем условие
			if (!string.IsNullOrEmpty(query.Condition)) query.Condition += " AND";
			query.Condition += $" [{field_name.ToLower()}] = {source[selected_value]}";

			LoadTab(query);
		}


		[DllImport("kernel32.dll")]
	public static extern bool AllocConsole();

	[DllImport("kernel32.dll")]
	public static extern bool FreeConsole();

		
	}
}
