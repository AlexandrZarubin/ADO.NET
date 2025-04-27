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

//using ;

namespace AcademyDataSet
{
	public partial class MainForm : Form
	{
		Cache cache;
		Timer reloadTimer;
		int reloadIntervalSeconds;                                  // Интервал перезагрузки в секундах			
		int timeLeftSeconds;                                        // Сколько секунд осталось до следующего обновления
		public MainForm()
		{
			InitializeComponent();
			AllocConsole();
			InitReloadTimer();
			cache =new Cache(ConfigurationManager.ConnectionStrings["VPD_311_Import"].ConnectionString);
			#region MyRegion

			/*
			//1) Создаем Dataset:
			//GroupsRelatedData = new DataSet("GroupsRelatedData");
			cache.AddTable("Directions", "direction_id,direction_name");
			cache.AddTable("Groups", "group_id,group_name,direction");
			cache.AddRelation("GroupsDirections", "Groups,direction", "Directions,direction_id" );
			cache.Print("Groups");
            //PrintGroups();
            //LoadGroupsRelatedData();
            Console.WriteLine(cache.HasParents("Groups"));
			cbDirections.DataSource = cache.Set.Tables["Directions"];
			cbDirections.ValueMember = "direction_id";
			cbDirections.DisplayMember = "direction_name";

			cbGoups.DataSource = cache.Set.Tables["Groups"];
			cbGoups.ValueMember = "group_id";
			cbGoups.DisplayMember = "group_name";

			DataTable directionsTable = cache.Set.Tables["Directions"];
			DataRow allRow = directionsTable.NewRow();
			allRow["direction_id"] = -1; // уникальный id
			allRow["direction_name"] = "Все направления";
			directionsTable.Rows.InsertAt(allRow, 0);
			cbDirections.SelectedValue = -1;

			DataTable groupsTable = cache.Set.Tables["Groups"];
			DataRow allGroupsRow = groupsTable.NewRow();
			allGroupsRow["group_id"] = -1; // уникальный id
			allGroupsRow["group_name"] = "Все группы";
			allGroupsRow["direction"] = DBNull.Value; // можно поставить null для направления
			groupsTable.Rows.InsertAt(allGroupsRow, 0);
			cbGoups.SelectedValue = -1;

			cache.AddTable("Students", "stud_id,last_name,first_name,middle_name,birth_date,group");
			cache.AddRelation("StudentsGroups", "Students,group", "Groups,group_id");
			dgvStudents.DataSource = cache.Set.Tables["Students"];
			cbGoups.SelectedIndexChanged += cbGoups_SelectedIndexChanged;
			*/
			#endregion
			ReloadCache();                                          // Первая загрузка данных
			SetReloadInterval(1, 10);                               // Установка интервала обновления кэша
			//InitReloadTimer();
		}
		
	[DllImport("kernel32.dll")]
	public static extern bool AllocConsole();

	[DllImport("kernel32.dll")]
	public static extern bool FreeConsole();

		void cbDirections_SelectedIndexChanged(object sender, EventArgs e)
		{
			/*
			object selectedValue=(sender as ComboBox).SelectedValue;
			string filter=$"direction = {selectedValue.ToString()}";
            Console.WriteLine(filter);
			cache.Set.Tables["Groups"].DefaultView.RowFilter = filter;
            //if(selectedValue?.ToString()!=selectedValue?)
            //cbGoups.DataSource = cache.Set.Tables["Groups"].ChildRelations[0];*/

			ComboBox comboBox = sender as ComboBox;
			if (comboBox != null && comboBox.SelectedItem != null)
			{
				DataRowView rowView = comboBox.SelectedItem as DataRowView;
				if (rowView != null)
				{
					string selectedValue = rowView["direction_id"].ToString();
					DataTable groupsTable = cache.Set.Tables["Groups"];

					if (selectedValue == "-1")
					{
						groupsTable.DefaultView.RowFilter = "";				// показываем все группы
					}
					else
					{
						string filter = "(direction = " + selectedValue + ")";
						groupsTable.DefaultView.RowFilter = filter;         // Фильтруем группы по выбранному направлению
					}

					if (cbGoups.Items.Count > 0)
					{
						cbGoups.SelectedIndex = 0;                          // Выбираем первую группу после фильтрации
						cbGoups_SelectedIndexChanged(cbGoups, EventArgs.Empty);
					}
				}
			}
		}
		void cbGoups_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cache.Set.Tables.Contains("Students"))
			{
				ComboBox comboBox = sender as ComboBox;
				if (comboBox != null && comboBox.SelectedItem != null)
				{
					DataRowView rowView = comboBox.SelectedItem as DataRowView;
					if (rowView != null)
					{
						string selectedGroupId = rowView["group_id"].ToString();

						if (selectedGroupId == "-1")
						{
							cache.Set.Tables["Students"].DefaultView.RowFilter = ""; // показываем всех студентов
						}
						else
						{
							string filter = "group = " + selectedGroupId;
							cache.Set.Tables["Students"].DefaultView.RowFilter = filter;
						}
					}
				}
			}
		}
		void SetReloadInterval(int minutes, int seconds = 0)                    // задания времени
		{
			reloadIntervalSeconds = (minutes * 60) + seconds;
			timeLeftSeconds = reloadIntervalSeconds;							// Установка начального значения таймера
		}
		void ReloadTimer_Tick(object sender, EventArgs e)
		{
			timeLeftSeconds--;                                                  // Уменьшаем оставшееся время
			if (timeLeftSeconds <= 0)
			{
				ReloadCache();                                                  // Обновляем кэш
				timeLeftSeconds = reloadIntervalSeconds;                        // Сброс таймера
			}
			UpdateLabelTimer();                                                 // Обновляем отображение таймера
		}
		void UpdateLabelTimer()                                                 // Метод для обновления текста таймера на форме
		{
			int minutes = timeLeftSeconds / 60;
			int seconds = timeLeftSeconds % 60;
			lblTimer.Text = $"Обновление через: {minutes:D2}:{seconds:D2}";
		}
		void InitReloadTimer()
		{
			reloadTimer = new Timer();
			reloadTimer.Interval = 1000;										// тикает каждую секунду
			reloadTimer.Tick += ReloadTimer_Tick;
			reloadTimer.Start();                                                // Запуск таймера
		}
		
		void ClearDataSet(DataSet set)
		{
			// Удаляем все отношения между таблицами
			for (int i = set.Relations.Count - 1; i >= 0; i--)
			{
				set.Relations.RemoveAt(i);                                      // Удаляем все связи между таблицами
			}

			// Удаляем сначала ForeignKeyConstraints вручную
			for (int t = 0; t < set.Tables.Count; t++)
			{
				DataTable table = set.Tables[t];
				for (int i = table.Constraints.Count - 1; i >= 0; i--)
				{
					Constraint constraint = table.Constraints[i];
					if (constraint is ForeignKeyConstraint)
					{
						table.Constraints.RemoveAt(i);                          // Удаляем внешние ключи
					}
				}
			}

			// Удаляем остальные ограничения
			for (int t = 0; t < set.Tables.Count; t++)
			{
				DataTable table = set.Tables[t];
				table.Constraints.Clear();
			}

			// Теперь можно удалить таблицы
			for (int i = set.Tables.Count - 1; i >= 0; i--)
			{
				set.Tables.RemoveAt(i);                                         // Удаляем все таблицы
			}
		}

		public void ReloadCache()
		{
			// Сохраняем текущее выбранное направление и группу
			object savedDirection = cbDirections.SelectedValue;                 // Сохраняем выбранное направление
			object savedGroup = cbGoups.SelectedValue;                          // Сохраняем выбранную группу

			// 1) Убираем обработчик перед очисткой
			cbGoups.SelectedIndexChanged -= cbGoups_SelectedIndexChanged;       // Отключаем обработчик на время

			// Очищаем старые данные
			ClearDataSet(cache.Set);                                            // Очищаем старые данные

			// 2) Загружаем заново таблицы и связи
			cache.AddTable("Directions", "direction_id,direction_name");
			cache.AddTable("Groups", "group_id,group_name,direction");
			cache.AddRelation("GroupsDirections", "Groups,direction", "Directions,direction_id");

			cache.AddTable("Students", "stud_id,last_name,first_name,middle_name,birth_date,group");
			cache.AddRelation("StudentsGroups", "Students,group", "Groups,group_id");

			// 3) Привязываем данные к ComboBox и DataGridView
			cbDirections.DataSource = cache.Set.Tables["Directions"];
			cbDirections.ValueMember = "direction_id";
			cbDirections.DisplayMember = "direction_name";

			cbGoups.DataSource = cache.Set.Tables["Groups"];
			cbGoups.ValueMember = "group_id";
			cbGoups.DisplayMember = "group_name";

			dgvStudents.DataSource = cache.Set.Tables["Students"];

			// Добавляем обратно "Все направления" и "Все группы"
			DataTable directionsTable = cache.Set.Tables["Directions"];
			if (!directionsTable.Rows.Cast<DataRow>().Any(r => r["direction_id"].ToString() == "-1"))
			{
				DataRow allRow = directionsTable.NewRow();
				allRow["direction_id"] = -1;
				allRow["direction_name"] = "Все направления";
				directionsTable.Rows.InsertAt(allRow, 0);
			}

			DataTable groupsTable = cache.Set.Tables["Groups"];
			if (!groupsTable.Rows.Cast<DataRow>().Any(r => r["group_id"].ToString() == "-1"))
			{
				DataRow allGroupsRow = groupsTable.NewRow();
				allGroupsRow["group_id"] = -1;
				allGroupsRow["group_name"] = "Все группы";
				allGroupsRow["direction"] = DBNull.Value;
				groupsTable.Rows.InsertAt(allGroupsRow, 0);
			}

			// Восстанавливаем выбранные значения
			if (savedDirection != null && directionsTable.AsEnumerable().Any(r => r["direction_id"].ToString() == savedDirection.ToString()))
				cbDirections.SelectedValue = savedDirection;
			else
				cbDirections.SelectedValue = -1;

			if (savedGroup != null && groupsTable.AsEnumerable().Any(r => r["group_id"].ToString() == savedGroup.ToString()))
				cbGoups.SelectedValue = savedGroup;
			else
				cbGoups.SelectedValue = -1;

			// 4) Снова вешаем обработчик
			cbGoups.SelectedIndexChanged += cbGoups_SelectedIndexChanged;

			// 5) И вручную вызываем фильтрацию студентов
			cbGoups_SelectedIndexChanged(cbGoups, EventArgs.Empty);

			// 6) Печать в консоль
			Console.WriteLine(new string('-', 50));
			Console.WriteLine($"Кэш обновился в {DateTime.Now:HH:mm:ss}");
			Console.WriteLine();
			foreach (DataTable table in cache.Set.Tables)
			{
				Console.WriteLine($"Таблица {table.TableName}: {table.Rows.Count} записей");
			}
		}
		void timerCache_Tick(object sender, EventArgs e)
		{
			ReloadCache();
		}
	}
}
