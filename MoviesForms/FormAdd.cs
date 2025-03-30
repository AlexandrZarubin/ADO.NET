using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoviesForms
{
	public partial class FormAdd : Form
	{
		public FormAdd()
		{
			//if (Theme.IsDarkTheme())
			//	Theme.ApplyDarkTheme(this);
			InitializeComponent();
		}
		private void chkAddDirector_CheckedChanged(object sender, EventArgs e)
		{
			bool addingDirector = chkAddDirector.Checked;

			// Включаем/отключаем поля для нового режиссёра
			bool adding = chkAddDirector.Checked;
			groupDirector.Visible = adding;
			comboDirectors.Enabled = !adding;
		}
	}
}
