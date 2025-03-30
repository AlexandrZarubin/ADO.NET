namespace MoviesForms
{
	partial class FormAdd
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.chkAddDirector = new System.Windows.Forms.CheckBox();
			this.groupDirector = new System.Windows.Forms.GroupBox();
			this.lblFirstName = new System.Windows.Forms.Label();
			this.txtFirstName = new System.Windows.Forms.TextBox();
			this.lblLastName = new System.Windows.Forms.Label();
			this.txtLastName = new System.Windows.Forms.TextBox();

			this.lblTitle = new System.Windows.Forms.Label();
			this.txtTitle = new System.Windows.Forms.TextBox();
			this.lblReleaseDate = new System.Windows.Forms.Label();
			this.dateRelease = new System.Windows.Forms.DateTimePicker();
			this.lblDirectorCombo = new System.Windows.Forms.Label();
			this.comboDirectors = new System.Windows.Forms.ComboBox();
			this.btnSave = new System.Windows.Forms.Button();

			this.SuspendLayout();

			// chkAddDirector
			this.chkAddDirector.Location = new System.Drawing.Point(20, 15);
			this.chkAddDirector.Name = "chkAddDirector";
			this.chkAddDirector.Size = new System.Drawing.Size(200, 20);
			this.chkAddDirector.Text = "Добавить нового режиссёра";
			this.chkAddDirector.CheckedChanged += new System.EventHandler(this.chkAddDirector_CheckedChanged);

			// groupDirector
			this.groupDirector.Location = new System.Drawing.Point(20, 40);
			this.groupDirector.Name = "groupDirector";
			this.groupDirector.Size = new System.Drawing.Size(330, 90);
			this.groupDirector.Text = "Новый режиссёр";
			this.groupDirector.Visible = false;

			// lblFirstName
			this.lblFirstName.Location = new System.Drawing.Point(15, 25);
			this.lblFirstName.Size = new System.Drawing.Size(70, 20);
			this.lblFirstName.Text = "Имя:";

			// txtFirstName
			this.txtFirstName.Location = new System.Drawing.Point(90, 25);
			this.txtFirstName.Size = new System.Drawing.Size(210, 20);

			// lblLastName
			this.lblLastName.Location = new System.Drawing.Point(15, 55);
			this.lblLastName.Size = new System.Drawing.Size(70, 20);
			this.lblLastName.Text = "Фамилия:";

			// txtLastName
			this.txtLastName.Location = new System.Drawing.Point(90, 55);
			this.txtLastName.Size = new System.Drawing.Size(210, 20);

			// Добавляем контролы в groupBox
			this.groupDirector.Controls.Add(this.lblFirstName);
			this.groupDirector.Controls.Add(this.txtFirstName);
			this.groupDirector.Controls.Add(this.lblLastName);
			this.groupDirector.Controls.Add(this.txtLastName);

			// lblTitle
			this.lblTitle.Location = new System.Drawing.Point(30, 140);
			this.lblTitle.AutoSize = true;
			this.lblTitle.Text = "Название:";

			// txtTitle
			this.txtTitle.Location = new System.Drawing.Point(120, 140);
			this.txtTitle.Size = new System.Drawing.Size(200, 20);

			// lblReleaseDate
			this.lblReleaseDate.Location = new System.Drawing.Point(30, 170);
			this.lblReleaseDate.AutoSize = true;
			this.lblReleaseDate.Text = "Дата выхода:";

			// dateRelease
			this.dateRelease.Location = new System.Drawing.Point(120, 170);
			this.dateRelease.Size = new System.Drawing.Size(200, 20);

			// lblDirectorCombo
			this.lblDirectorCombo.Location = new System.Drawing.Point(30, 200);
			this.lblDirectorCombo.AutoSize = true;
			this.lblDirectorCombo.Text = "Режиссёр:";

			// comboDirectors
			this.comboDirectors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboDirectors.Location = new System.Drawing.Point(120, 200);
			this.comboDirectors.Size = new System.Drawing.Size(200, 21);

			// btnSave
			this.btnSave.Location = new System.Drawing.Point(30, 240);
			this.btnSave.Size = new System.Drawing.Size(290, 30);
			this.btnSave.Text = "Сохранить";
			this.btnSave.UseVisualStyleBackColor = true;

			// FormAdd
			this.ClientSize = new System.Drawing.Size(380, 300);
			this.Controls.Add(this.chkAddDirector);
			this.Controls.Add(this.groupDirector);
			this.Controls.Add(this.lblTitle);
			this.Controls.Add(this.txtTitle);
			this.Controls.Add(this.lblReleaseDate);
			this.Controls.Add(this.dateRelease);
			this.Controls.Add(this.lblDirectorCombo);
			this.Controls.Add(this.comboDirectors);
			this.Controls.Add(this.btnSave);
			this.Name = "FormAdd";
			this.Text = "Добавить фильм / режиссёра";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox chkAddDirector;
		private System.Windows.Forms.GroupBox groupDirector;

		private System.Windows.Forms.Label lblFirstName;
		private System.Windows.Forms.TextBox txtFirstName;
		private System.Windows.Forms.Label lblLastName;
		private System.Windows.Forms.TextBox txtLastName;

		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.TextBox txtTitle;
		private System.Windows.Forms.Label lblReleaseDate;
		private System.Windows.Forms.DateTimePicker dateRelease;
		private System.Windows.Forms.Label lblDirectorCombo;
		private System.Windows.Forms.ComboBox comboDirectors;
		private System.Windows.Forms.Button btnSave;
	}
}