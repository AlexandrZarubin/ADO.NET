using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MoviesForms;
using Microsoft.Win32;

using System.Windows.Forms;
using System.Data.SqlClient;

namespace MoviesForms
{
	public partial class MoviesForm : Form
	{
		Connector connector;
		public MoviesForm()
		{
			InitializeComponent();
			connector = new Connector();
		}
		private void MoviesForm_Load(object sender, EventArgs e)
		{
			//if (Theme.IsDarkTheme())
			//	Theme.ApplyDarkTheme(this);
			LoadMovies();
		}
		private void LoadMovies()
		{
			dataGridMovies.DataSource = Select
				(
					"titel,release_date,FORMATMESSAGE(N'%s %s', first_name, last_name) AS N'Режиссер'",
					"Movies,Directors",
					"director=director_id"
				);
			dataGridMovies.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
		}
		public DataTable Select(string fields, string tables, string condition = "")
		{
			string query = $"SELECT {fields} FROM {tables}";
			if (!string.IsNullOrWhiteSpace(condition))
				query += $" WHERE {condition}";

			DataTable dataTable = new DataTable();

			SqlCommand command = new SqlCommand(query, connector.connection);
			connector.connection.Open();
			SqlDataReader reader = command.ExecuteReader();
			dataTable.Load(reader);
			connector.connection.Close();
			
			return dataTable;
		}
		private void btnShowAll_Click(object sender, EventArgs e)
		{
			LoadMovies();
		}
		private void btnAdd_Click(object sender, EventArgs e)
		{
			FormAdd addForm = new FormAdd();
			addForm.ShowDialog();
			//LoadMovies(); // обновим таблицу после добавления
		}
		private void btnSearch_Click(object sender, EventArgs e)
		{
			
		}
	}
}
