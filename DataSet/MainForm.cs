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

using AcademyCache;
namespace AcademyDataSet
{
	public partial class MainForm : Form
	{
		readonly string CONNECTION_STRING="";
		SqlConnection connection=null;
		Cache cache;
		public MainForm()
		{
			InitializeComponent();
			CONNECTION_STRING = ConfigurationManager.ConnectionStrings["VPD_311_Import"].ConnectionString;
			connection = new SqlConnection(CONNECTION_STRING);
			AllocConsole();
            Console.WriteLine(CONNECTION_STRING);
			//1) Создаем Dataset:
			cache = new Cache(connection);											// создаём Cache, передаём подключение
			cache.AddTable("Directions", "direction_id,direction_name");
			cache.AddTable("Groups", "group_id,group_name,direction");
			cache.AddRelation("GroupsDirections", "Groups,direction", "Directions,direction_id");
			
			cache.AddTable("Students", "stud_id,last_name,first_name,middle_name,birth_date,group");
			cache.AddRelation("StudentsGroups", "Students,group", "Groups,group_id");
			

			cache.Print1("Students");
			
			cache.Print1("Groups");

		}
		
	[DllImport("kernel32.dll")]
	public static extern bool AllocConsole();

	[DllImport("kernel32.dll")]
	public static extern bool FreeConsole();
	}
}
