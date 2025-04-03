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
			new Query(
				"direction_id, direction_name, COUNT(DISTINCT group_id) AS GroupCount, COUNT(stud_id) AS StudentCount",
				"Directions LEFT JOIN Groups ON Groups.direction = Directions.direction_id LEFT JOIN Students ON Students.[group] = Groups.group_id",
				"",
				"direction_id, direction_name"),
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
		}

		private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i=tabControl.SelectedIndex;
			//Query query = queries[i];
			tables[i].DataSource = connector.Select(queries[i].Columns, queries[i].Tables, queries[i].Condition, queries[i].GroupBy);
			statusStripCountLabel.Text = $"{status_messages[i]} {tables[i].RowCount-1}";
		}
	}
	
}
