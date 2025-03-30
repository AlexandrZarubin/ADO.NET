using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;

namespace MoviesForms
{
	public static class Theme
	{
		public static bool IsDarkTheme()
		{
			var theme = Registry.GetValue(
				@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize",
				"AppsUseLightTheme", null);

			return theme is int value && value == 0;
		}

		public static void ApplyDarkTheme(Form form)
		{
			Color backColor = Color.FromArgb(28, 28, 28);
			Color btnColor = Color.FromArgb(45, 45, 48);
			Color gridColor = Color.FromArgb(36, 36, 36);
			Color textColor = Color.White;

			form.BackColor = backColor;
			form.ForeColor = textColor;

			foreach (Control control in form.Controls)
			{
				if (control is Button btn)
				{
					btn.BackColor = btnColor;
					btn.ForeColor = textColor;
					btn.FlatStyle = FlatStyle.Flat;
					btn.FlatAppearance.BorderColor = Color.Gray;
				}

				if (control is DataGridView grid)
				{
					grid.BackgroundColor = gridColor;
					grid.DefaultCellStyle.BackColor = gridColor;
					grid.DefaultCellStyle.ForeColor = textColor;
					grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(70, 70, 70);
					grid.DefaultCellStyle.SelectionForeColor = Color.White;

					grid.ColumnHeadersDefaultCellStyle.BackColor = btnColor;
					grid.ColumnHeadersDefaultCellStyle.ForeColor = textColor;
					grid.RowHeadersDefaultCellStyle.BackColor = btnColor;
					grid.RowHeadersDefaultCellStyle.ForeColor = textColor;

					grid.EnableHeadersVisualStyles = false;
					grid.GridColor = Color.FromArgb(64, 64, 64);
					grid.BorderStyle = BorderStyle.None;
				}
			}
		}
	}
}
