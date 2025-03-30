namespace MoviesForms
{
	partial class MoviesForm
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
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnSearch = new System.Windows.Forms.Button();
			this.btnShowAll = new System.Windows.Forms.Button();
			this.dataGridMovies = new System.Windows.Forms.DataGridView();
			((System.ComponentModel.ISupportInitialize)(this.dataGridMovies)).BeginInit();
			this.SuspendLayout();
			// 
			// btnAdd
			// 
			this.btnAdd.Location = new System.Drawing.Point(80, 25);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(200, 30);
			this.btnAdd.TabIndex = 0;
			this.btnAdd.Text = "Добавить фильм или режиссёра";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnSearch
			// 
			this.btnSearch.Location = new System.Drawing.Point(300, 25);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(200, 30);
			this.btnSearch.TabIndex = 1;
			this.btnSearch.Text = "Поиск";
			this.btnSearch.UseVisualStyleBackColor = true;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// btnShowAll
			// 
			this.btnShowAll.Location = new System.Drawing.Point(520, 25);
			this.btnShowAll.Name = "btnShowAll";
			this.btnShowAll.Size = new System.Drawing.Size(200, 30);
			this.btnShowAll.TabIndex = 2;
			this.btnShowAll.Text = "Показать все фильмы";
			this.btnShowAll.UseVisualStyleBackColor = true;
			this.btnShowAll.Click += new System.EventHandler(this.btnShowAll_Click);
			// 
			// dataGridMovies
			// 
			this.dataGridMovies.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
				| System.Windows.Forms.AnchorStyles.Left)
				| System.Windows.Forms.AnchorStyles.Right)));
			this.dataGridMovies.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridMovies.Location = new System.Drawing.Point(12, 70);
			this.dataGridMovies.Name = "dataGridMovies";
			this.dataGridMovies.Size = new System.Drawing.Size(776, 368);
			this.dataGridMovies.TabIndex = 3;
			// 
			// MoviesForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.dataGridMovies);
			this.Controls.Add(this.btnShowAll);
			this.Controls.Add(this.btnSearch);
			this.Controls.Add(this.btnAdd);
			this.Name = "MoviesForm";
			this.Text = "Movies";
			this.Load += new System.EventHandler(this.MoviesForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.dataGridMovies)).EndInit();
			this.ResumeLayout(false);
		

		}

		#endregion

		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.Button btnShowAll;
		private System.Windows.Forms.DataGridView dataGridMovies;
	}
}

