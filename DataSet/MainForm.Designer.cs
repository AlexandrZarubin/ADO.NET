namespace AcademyDataSet
{
	partial class MainForm
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
			this.components = new System.ComponentModel.Container();
			this.cbGoups = new System.Windows.Forms.ComboBox();
			this.cbDirections = new System.Windows.Forms.ComboBox();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabPageStudents = new System.Windows.Forms.TabPage();
			this.dgvStudents = new System.Windows.Forms.DataGridView();
			this.tabPageGroups = new System.Windows.Forms.TabPage();
			this.cbGroupsDirection = new System.Windows.Forms.ComboBox();
			this.dgvGroups = new System.Windows.Forms.DataGridView();
			this.timerCache = new System.Windows.Forms.Timer(this.components);
			this.lblTimer = new System.Windows.Forms.Label();
			this.tabControl.SuspendLayout();
			this.tabPageStudents.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).BeginInit();
			this.tabPageGroups.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvGroups)).BeginInit();
			this.SuspendLayout();
			// 
			// cbGoups
			// 
			this.cbGoups.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbGoups.FormattingEnabled = true;
			this.cbGoups.Location = new System.Drawing.Point(27, 17);
			this.cbGoups.Name = "cbGoups";
			this.cbGoups.Size = new System.Drawing.Size(198, 21);
			this.cbGoups.TabIndex = 0;
			// 
			// cbDirections
			// 
			this.cbDirections.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbDirections.FormattingEnabled = true;
			this.cbDirections.Location = new System.Drawing.Point(465, 17);
			this.cbDirections.Name = "cbDirections";
			this.cbDirections.Size = new System.Drawing.Size(284, 21);
			this.cbDirections.TabIndex = 1;
			this.cbDirections.SelectedIndexChanged += new System.EventHandler(this.cbDirections_SelectedIndexChanged);
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.tabPageStudents);
			this.tabControl.Controls.Add(this.tabPageGroups);
			this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl.Location = new System.Drawing.Point(0, 0);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(800, 450);
			this.tabControl.TabIndex = 2;
			// 
			// tabPageStudents
			// 
			this.tabPageStudents.Controls.Add(this.lblTimer);
			this.tabPageStudents.Controls.Add(this.dgvStudents);
			this.tabPageStudents.Controls.Add(this.cbGoups);
			this.tabPageStudents.Controls.Add(this.cbDirections);
			this.tabPageStudents.Location = new System.Drawing.Point(4, 22);
			this.tabPageStudents.Name = "tabPageStudents";
			this.tabPageStudents.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageStudents.Size = new System.Drawing.Size(792, 424);
			this.tabPageStudents.TabIndex = 0;
			this.tabPageStudents.Text = "Students";
			this.tabPageStudents.UseVisualStyleBackColor = true;
			// 
			// dgvStudents
			// 
			this.dgvStudents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvStudents.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.dgvStudents.Location = new System.Drawing.Point(3, 53);
			this.dgvStudents.Name = "dgvStudents";
			this.dgvStudents.Size = new System.Drawing.Size(786, 368);
			this.dgvStudents.TabIndex = 2;
			// 
			// tabPageGroups
			// 
			this.tabPageGroups.Controls.Add(this.cbGroupsDirection);
			this.tabPageGroups.Controls.Add(this.dgvGroups);
			this.tabPageGroups.Location = new System.Drawing.Point(4, 22);
			this.tabPageGroups.Name = "tabPageGroups";
			this.tabPageGroups.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageGroups.Size = new System.Drawing.Size(792, 424);
			this.tabPageGroups.TabIndex = 1;
			this.tabPageGroups.Text = "Groups";
			this.tabPageGroups.UseVisualStyleBackColor = true;
			// 
			// cbGroupsDirection
			// 
			this.cbGroupsDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbGroupsDirection.FormattingEnabled = true;
			this.cbGroupsDirection.Location = new System.Drawing.Point(66, 16);
			this.cbGroupsDirection.Name = "cbGroupsDirection";
			this.cbGroupsDirection.Size = new System.Drawing.Size(283, 21);
			this.cbGroupsDirection.TabIndex = 1;
			// 
			// dgvGroups
			// 
			this.dgvGroups.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvGroups.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.dgvGroups.Location = new System.Drawing.Point(3, 52);
			this.dgvGroups.Name = "dgvGroups";
			this.dgvGroups.Size = new System.Drawing.Size(786, 369);
			this.dgvGroups.TabIndex = 0;
			// 
			// timerCache
			// 
			this.timerCache.Tick += new System.EventHandler(this.timerCache_Tick);
			// 
			// lblTimer
			// 
			this.lblTimer.AutoSize = true;
			this.lblTimer.Location = new System.Drawing.Point(283, 20);
			this.lblTimer.Name = "lblTimer";
			this.lblTimer.Size = new System.Drawing.Size(26, 13);
			this.lblTimer.TabIndex = 3;
			this.lblTimer.Text = "time";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.tabControl);
			this.Name = "MainForm";
			this.Text = "Form1";
			this.tabControl.ResumeLayout(false);
			this.tabPageStudents.ResumeLayout(false);
			this.tabPageStudents.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).EndInit();
			this.tabPageGroups.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgvGroups)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox cbGoups;
		private System.Windows.Forms.ComboBox cbDirections;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabPageStudents;
		private System.Windows.Forms.TabPage tabPageGroups;
		private System.Windows.Forms.DataGridView dgvStudents;
		private System.Windows.Forms.DataGridView dgvGroups;
		private System.Windows.Forms.ComboBox cbGroupsDirection;
		private System.Windows.Forms.Label lblTimer;
		private System.Windows.Forms.Timer timerCache;
	}
}

