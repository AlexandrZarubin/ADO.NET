//DZHome
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
		private Dictionary<string, int> d_directions;
		private Dictionary<string, int> d_groups;

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

			Dictionary<string, int> tmpDirections = connector.GetDictionary("Directions");
			d_directions = tmpDirections != null ? tmpDirections : new Dictionary<string, int>();

			Dictionary<string, int> tmpGroups = connector.GetDictionary("Groups");
			d_groups = tmpGroups != null ? tmpGroups : new Dictionary<string, int>();

			cbStudentsDirection.Items.Add("Все направления");
			cbStudentsDirection.Items.AddRange(d_directions.Select(d => d.Key.ToString()).ToArray());
			cbStudentsDirection.SelectedIndex = 0;

			cbStudentsGroup.Items.AddRange(d_groups.Select(g=>g.Key.ToString()).ToArray());
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

		private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!(sender is ComboBox comboBox) || comboBox.SelectedItem == null) return;
			int i = tabControl.SelectedIndex;							//индекс вкладки						
			Query query = new Query(queries[i]);						//объект запроса
			Console.WriteLine(query.Condition);
			string tab_name = (sender as ComboBox).Name;                //имя ComboBox
			
			string field_name = tab_name.Substring(						//извлекаем название поля
				Array.
				FindLastIndex<char>
				(tab_name.ToCharArray(),
				Char.IsUpper));

			Console.WriteLine(field_name);
			string member_name = $"d_{field_name.ToLower()}s";          //формируем имя поля d_<field>s
			Console.WriteLine(member_name == nameof(d_directions));

			FieldInfo fieldInfo = this.GetType().           // Получаем словарь через рефлексию по имени переменной
				GetField(member_name, BindingFlags.NonPublic | BindingFlags.Instance);


			if (fieldInfo == null) return;                  // Если поле не найдено — выходим
			
			Dictionary<string, int> source =                // Получаем сам словарь из поля, приведя его к нужному типу
				fieldInfo?.GetValue(this) as Dictionary<string, int>;

			// Проверяем, что словарь существует и содержит выбранный элемент из ComboBox
			if (source == null || !source.ContainsKey(comboBox.SelectedItem.ToString()))
			{
				//если выбран элемент по умолчанию в ComboBox направлений
				if (comboBox == cbStudentsDirection && comboBox.SelectedIndex == 0)
				{
					// Очищаем группы и добавляем все доступные группы
					cbStudentsGroup.Items.Clear();
					cbStudentsGroup.Items.AddRange(d_groups.Select(g => g.Key).ToArray());
					cbStudentsGroup.SelectedIndex = -1;
					LoadTab();
				}
				return;												// Выходим, т.к. нужного значения в словаре нет
			}
		
			if (source == null) return;								// Если словарь есть, продолжаем добавлять условие в запрос
			if (query.Condition != "") query.Condition += " AND";   // Если уже есть условия, добавляем AND
			
			query.Condition +=                                      // Добавляем новое условие по выбранному элементу ComboBox
				$" [{field_name.ToLower()}] = {source[(sender as ComboBox).SelectedItem.ToString()]}";
			
			
			LoadTab(query);

			if ((sender as ComboBox) == cbStudentsDirection)
			{
				string selectedDirection =                          // Получаем выбранное направление
					cbStudentsDirection.SelectedItem.ToString(); 

				int directionId = d_directions[selectedDirection];  // Получаем его ID из словаря направлений

				DataTable filteredGroups =                          // Выполняем SQL-запрос для получения групп, связанных с направлением
					connector.Select("group_name, group_id", "Groups", $"direction = {directionId}");

				cbStudentsGroup.Items.Clear();						// Очищаем ComboBox групп

				// Если группы найдены — добавляем их в ComboBox
				if (filteredGroups != null && filteredGroups.Rows.Count > 0)
				{
					cbStudentsGroup.Items.AddRange(filteredGroups.Rows.Cast<DataRow>().Select(row => row["group_name"].ToString()).ToArray());
					cbStudentsGroup.SelectedIndex = -1;
				}
				else                                                 // Если групп нет — выводим сообщение
				{
					cbStudentsGroup.Items.Add("Нет групп по направлению");
					cbStudentsGroup.SelectedIndex = 0;
				}
			}
			Console.WriteLine((sender as ComboBox).Name);
			Console.WriteLine(e);


		}

		
	}
	
}
