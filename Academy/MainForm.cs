using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//HomeWorkDONE
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
				"Directions, Groups, Students",
				"direction = direction_id AND [group] = group_id",
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
			//загружаем студентов
			dgvStudents.DataSource = connector.Select("*","Students");
			statusStripCountLabel.Text = $"Кол-во студентов: {dgvStudents.RowCount - 1}";
			dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

			//Загрузка направлений для ComboBox-ов
			DataTable directions = connector.Select("direction_id, direction_name", "Directions");
			
			// Добавляем "Все направления"
			CB_Directions.DataSource = directions;
			DataRow allDirectionsRow = directions.NewRow();
			allDirectionsRow["direction_id"] = DBNull.Value;
			allDirectionsRow["direction_name"] = "Все направления";
			directions.Rows.InsertAt(allDirectionsRow, 0);
			
			// Привязка направлений на вкладке Groups
			CB_Directions.DisplayMember = "direction_name";
			CB_Directions.ValueMember = "direction_id";
			CB_Directions.SelectedIndexChanged += CB_Directions_SelectedIndexChanged;


			// Привязка направлений на вкладке Students
			CB_StudentDirections.DataSource = directions;
			CB_StudentDirections.DisplayMember = "direction_name";
			CB_StudentDirections.ValueMember = "direction_id";
			CB_StudentDirections.SelectedIndex = 0;
		}

		private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i=tabControl.SelectedIndex;
			//Query query = queries[i];
			// Загружаем данные для выбранной вкладки
			tables[i].DataSource = connector.Select(queries[i].Columns, queries[i].Tables, queries[i].Condition, queries[i].GroupBy);
			// Отображаем кол-во записей
			statusStripCountLabel.Text = $"{status_messages[i]} {tables[i].RowCount-1}";
			if (i == 1) // вкладка Groups
			{
				CB_Directions.SelectedIndex = 0; // выбрать "Все направления"
				CB_Directions_SelectedIndexChanged(null, null);
			}
		}

		private void CB_Directions_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (CB_Directions.SelectedValue == null || CB_Directions.SelectedValue is DBNull)
			{
				// Показываем все группы
				dgvGroups.DataSource = connector.Select(
					"group_id, group_name,COUNT(stud_id) AS StudentCount, direction_name", 
					"Groups, Directions, Students",
					"direction = direction_id AND [group] = group_id",
					"group_id, group_name, direction_name"
				);
				statusStripCountLabel.Text = $"Все группы: {dgvGroups.RowCount - 1}";
			}
			else
			{
				// Показываем только группы по выбранному направлению
				string selectedDirectionId = CB_Directions.SelectedValue.ToString();
				dgvGroups.DataSource = connector.Select(
					"group_id, group_name,COUNT(stud_id) AS StudentCount, direction_name",
					"Groups, Directions, Students",
					$"direction = direction_id AND [group] = group_id AND direction = {selectedDirectionId}",
					"group_id, group_name, direction_name"
				);
				statusStripCountLabel.Text = $"Групп по направлению: {dgvGroups.RowCount - 1}";
			}

			dgvGroups.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


		}

		private void CB_StudentDirections_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!int.TryParse(CB_StudentDirections.SelectedValue?.ToString(), out int directionId))
			{
				// Все направления
				DataTable students = connector.Select("*", "Students");
				dgvStudents.DataSource = students;
				int count = students?.Rows.Count ?? 0;
				statusStripCountLabel.Text = $"Все студенты: {count}";

				// Все группы
				DataTable allGroups = connector.Select("group_id, group_name", "Groups");
				if (allGroups != null)
				{
					DataRow allGroup = allGroups.NewRow();
					allGroup["group_id"] = DBNull.Value;
					allGroup["group_name"] = "Все группы";
					allGroups.Rows.InsertAt(allGroup, 0);

					CB_StudentGroups.DisplayMember = "group_name";
					CB_StudentGroups.ValueMember = "group_id";
					CB_StudentGroups.DataSource = allGroups;
					CB_StudentGroups.SelectedIndex = 0;
				}

				return;
			}

			// Фильтрация студентов по направлению
			DataTable studentsByDirection = connector.Select(
				"*",
				"Students, Groups",
				$"[group] = group_id AND direction = {directionId}"
			);
			dgvStudents.DataSource = studentsByDirection;
			int filteredCount = studentsByDirection?.Rows.Count ?? 0;
			statusStripCountLabel.Text = $"Студентов по направлению: {filteredCount}";

			// Группы по направлению
			DataTable groups = connector.Select("group_id, group_name", "Groups", $"direction = {directionId}");
			if (groups != null)
			{
				DataRow allGroup = groups.NewRow();
				allGroup["group_id"] = DBNull.Value;
				allGroup["group_name"] = "Все группы";
				groups.Rows.InsertAt(allGroup, 0);

				CB_StudentGroups.DisplayMember = "group_name";
				CB_StudentGroups.ValueMember = "group_id";
				CB_StudentGroups.DataSource = groups;
				CB_StudentGroups.SelectedIndex = 0;
			}
		}

		private void CB_StudentGroups_SelectedIndexChanged(object sender, EventArgs e)
		{
			//if (CB_StudentDirections.SelectedValue == null || connector == null || dgvStudents == null)
			//	return;
			if (CB_StudentGroups.SelectedValue == null || CB_StudentGroups.SelectedValue is DBNull)
			{
				// Если выбрали "все группы", фильтруем только по направлению
				CB_StudentDirections_SelectedIndexChanged(null, null);
			}
			else
			{
				string groupId = CB_StudentGroups.SelectedValue.ToString();
				dgvStudents.DataSource = connector.Select(
					"*",
					"Students",
					$"[group] = {groupId}"
				);
				statusStripCountLabel.Text = $"Кол-во студентов: {dgvStudents.RowCount - 1}";
			}
		}

		private void checkBoxShowEmptyDirections_CheckedChanged(object sender, EventArgs e)
		{
			string condition = "";
			if (!checkBoxShowEmptyDirections.Checked)
				condition = "EXISTS (SELECT * FROM Groups WHERE Groups.direction = Directions.direction_id)";

			dgvDirections.DataSource = connector.Select(
				"direction_id, direction_name, COUNT(DISTINCT group_id) AS GroupCount, COUNT(stud_id) AS StudentCount",
				"Directions LEFT JOIN Groups ON Groups.direction = Directions.direction_id LEFT JOIN Students ON Students.[group] = Groups.group_id",
				condition,
				"direction_id, direction_name"
			);
		}

		private void checkBoxShowEmptyGroups_CheckedChanged(object sender, EventArgs e)
		{
			string condition = "direction = direction_id";
			if (!checkBoxShowEmptyGroups.Checked)
				condition += " AND EXISTS (SELECT * FROM Students WHERE [group] = group_id)";

			dgvGroups.DataSource = connector.Select(
				"group_id, group_name, COUNT(stud_id) AS StudentCount, direction_name",
				"Groups LEFT JOIN Directions ON Groups.direction = Directions.direction_id LEFT JOIN Students ON Students.[group] = Groups.group_id",
				condition,
				"group_id, group_name, direction_name"
			);
		}
	}
	
}
