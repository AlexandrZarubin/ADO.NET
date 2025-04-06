using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Configuration;
using System.Reflection;

namespace Academy
{
	public partial class MainForm : Form
	{
		Connector connector;
		Query[] queries = new Query[]
		{
			new Query("*","Students"),
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
			"direction_name"),

			new Query("*","Disciplines"),
			new Query("*","Teachers")

		};
		DataGridView[] tables;
		string[] status_messages = new string[]
		{
			"Кол-во студентов: ",
			"Кол-во групп: ",
			"Кол-во направлений: ",
			"Кол-во дисциплин: ",
			"Кол-во учителей: ",
		};

		/////////////////////////////////////////////
		public Dictionary<string, int> d_directions;
		public Dictionary<string, int> d_groups;
		public MainForm()
		{
			InitializeComponent();
			tables = new DataGridView[]
			{
				dgvStudents,
				dgvGroups,
				dgvDirections,
				dgvDisciplines,
				dgvTeachers
			};
			connector=new Connector(ConfigurationManager.ConnectionStrings["VPD_311_Import"].ConnectionString);
			dgvStudents.DataSource = connector.Select("*","Students");
			statusStripCountLabel.Text = $"Кол-во студентов: {dgvStudents.RowCount - 1}";
			dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

			d_directions = connector.GetDictionary("Directions");
			d_groups = connector.GetDictionary("Groups");

			cbStudentsGroup.Items.AddRange(d_groups.Select(g=>g.Key.ToString()).ToArray());
			cbStudentsDirection.Items.AddRange(d_directions.Select(d => d.Key.ToString()).ToArray());
			cbGroupsDirection.Items.AddRange(d_directions.Select(d => d.Key.ToString()).ToArray());
		}
		void LoadTab(Query query = null)
		{
			int i=tabControl.SelectedIndex;
			if (query == null) query = queries[i];
			tables[i].DataSource = connector.Select(query.Columns, query.Tables, query.Condition, query.GroupBy);
			statusStripCountLabel.Text = $"{status_messages[i]} {tables[i].RowCount-1}";

		}

		private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i=tabControl.SelectedIndex;
			//Query query = queries[i];
			LoadTab();
		}

		private void cbGroupsDirection_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i = tabControl.SelectedIndex;
			Query query = new Query(queries[i]);
			Console.WriteLine(query.Condition);
			string tab_name = (sender as ComboBox).Name;
			string field_name = tab_name.Substring(Array.FindLastIndex<char>(tab_name.ToCharArray(), Char.IsUpper));
			Console.WriteLine(field_name);
			string member_name = $"d_{field_name.ToLower()}s";
			Console.WriteLine(member_name == nameof(d_directions));
			Dictionary<string, int> source = this.GetType().GetField(member_name).GetValue(this) as Dictionary<string, int>;
			//Console.WriteLine(this.GetType().GetField(member_name).GetValue(this));
			//Console.WriteLine(this.GetType());
			query.Condition += $" AND {field_name.ToLower()} = {source[(sender as ComboBox).SelectedItem.ToString()]}";
			LoadTab(query);
			Console.WriteLine((sender as ComboBox).Name);
			Console.WriteLine(e);
		}
	}
	
}
