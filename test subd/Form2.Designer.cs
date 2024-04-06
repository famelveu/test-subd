namespace test_subd
{
    partial class Form2
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
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btAddString = new System.Windows.Forms.Button();
            this.btDelString = new System.Windows.Forms.Button();
            this.btEditString = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbColumn = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbDataSearch = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(13, 96);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(745, 343);
            this.dataGridView1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(659, 446);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(99, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Назад";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Клиенты",
            "Заказы",
            "Услуги к заказам",
            "Услуги",
            "Пользователи",
            "Роли"});
            this.comboBox1.Location = new System.Drawing.Point(6, 28);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(132, 21);
            this.comboBox1.TabIndex = 7;
            this.comboBox1.Text = "Клиенты";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // btAddString
            // 
            this.btAddString.Location = new System.Drawing.Point(10, 28);
            this.btAddString.Name = "btAddString";
            this.btAddString.Size = new System.Drawing.Size(80, 23);
            this.btAddString.TabIndex = 9;
            this.btAddString.Text = "Добавить";
            this.btAddString.UseVisualStyleBackColor = true;
            this.btAddString.Click += new System.EventHandler(this.addString_Click);
            // 
            // btDelString
            // 
            this.btDelString.Location = new System.Drawing.Point(200, 28);
            this.btDelString.Name = "btDelString";
            this.btDelString.Size = new System.Drawing.Size(80, 23);
            this.btDelString.TabIndex = 10;
            this.btDelString.Text = "Удалить";
            this.btDelString.UseVisualStyleBackColor = true;
            // 
            // btEditString
            // 
            this.btEditString.Location = new System.Drawing.Point(105, 28);
            this.btEditString.Name = "btEditString";
            this.btEditString.Size = new System.Drawing.Size(80, 23);
            this.btEditString.TabIndex = 17;
            this.btEditString.Text = "Изменить";
            this.btEditString.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btAddString);
            this.groupBox3.Controls.Add(this.btEditString);
            this.groupBox3.Controls.Add(this.btDelString);
            this.groupBox3.Location = new System.Drawing.Point(468, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(290, 73);
            this.groupBox3.TabIndex = 18;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Строка";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.comboBox1);
            this.groupBox4.Location = new System.Drawing.Point(13, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(144, 73);
            this.groupBox4.TabIndex = 19;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Таблица";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbColumn);
            this.groupBox1.Location = new System.Drawing.Point(6, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(130, 44);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Столбец";
            // 
            // cbColumn
            // 
            this.cbColumn.FormattingEnabled = true;
            this.cbColumn.Location = new System.Drawing.Point(7, 18);
            this.cbColumn.Name = "cbColumn";
            this.cbColumn.Size = new System.Drawing.Size(117, 21);
            this.cbColumn.TabIndex = 0;
            this.cbColumn.SelectedIndexChanged += new System.EventHandler(this.cbColumn_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbDataSearch);
            this.groupBox2.Location = new System.Drawing.Point(142, 16);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(130, 44);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Данные";
            // 
            // tbDataSearch
            // 
            this.tbDataSearch.Location = new System.Drawing.Point(7, 18);
            this.tbDataSearch.Name = "tbDataSearch";
            this.tbDataSearch.Size = new System.Drawing.Size(117, 20);
            this.tbDataSearch.TabIndex = 0;
            this.tbDataSearch.TextChanged += new System.EventHandler(this.tbDataSeatch_TextChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.groupBox1);
            this.groupBox5.Controls.Add(this.groupBox2);
            this.groupBox5.Location = new System.Drawing.Point(168, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(289, 73);
            this.groupBox5.TabIndex = 20;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Поиск";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(770, 479);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Form2";
            this.Text = "tabel";
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.IO.FileSystemWatcher fileSystemWatcher1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btDelString;
        private System.Windows.Forms.Button btAddString;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btEditString;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbColumn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tbDataSearch;
        private System.Windows.Forms.GroupBox groupBox4;
    }
}