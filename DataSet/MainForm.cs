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
		public MainForm()
		{
			InitializeComponent();
			AllocConsole();
			cache=new Cache(ConfigurationManager.ConnectionStrings["VPD_311_Import"].ConnectionString);
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
        }
		
	[DllImport("kernel32.dll")]
	public static extern bool AllocConsole();

	[DllImport("kernel32.dll")]
	public static extern bool FreeConsole();

		private void cbDirections_SelectedIndexChanged(object sender, EventArgs e)
		{
			object selectedValue=(sender as ComboBox).SelectedValue;
			string filter=$"direction = {selectedValue.ToString()}";
            Console.WriteLine(filter);
			cache.Set.Tables["Groups"].DefaultView.RowFilter = filter;
            //if(selectedValue?.ToString()!=selectedValue?)
            //cbGoups.DataSource = cache.Set.Tables["Groups"].ChildRelations[0];
        }
	}
}
