using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Runtime.Remoting.Contexts;
using Microsoft.SqlServer.Server;
using static System.Windows.Forms.DataFormats;
using System.Data.Common;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Reflection.Emit;


namespace test_subd
{

    public partial class Form2 : Form
    {

        SqlConnection connect;
        int roleForm;

        public Form2(SqlConnection cnct, int rl)
        {
            connect = cnct;
            roleForm = rl;
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

            if (roleForm != 1)
            {
                btDelString.Enabled = false;
                btEditString.Enabled = false;
                comboBox1.Items.RemoveAt(3);
                comboBox1.Items.RemoveAt(3);
                comboBox1.Items.RemoveAt(3);
            }   
            SqlConnection sqlConnect = new SqlConnection(Properties.Settings.Default.connectionString);  
                
            sqlConnect.Open();
            SqlCommand showTable = new SqlCommand();
            showTable.CommandText = $"SELECT * FROM Clients";
            showTable.Connection = sqlConnect;

            // SqlAdapter - прослойка между источником данных и базой данных
            SqlDataAdapter adapter = new SqlDataAdapter(showTable);
            DataSet dataSet = new DataSet();
            // заполняем источник данных полученными из адаптера записями
            adapter.Fill(dataSet);
            dataGridView1.DataSource = dataSet.Tables[0];

            UpdateComboBoxWithDataGridColumns(cbColumn, dataGridView1);

            sqlConnect.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlConnection sqlConnect = new SqlConnection(Properties.Settings.Default.connectionString);
            SqlCommand logRequst = new SqlCommand();
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    logRequst.CommandText = $"SELECT * FROM Clients";
                    break;
                case 1:
                    logRequst.CommandText = $"SELECT Orders.id, Users.name_u AS name_master, Clients.name_c AS name_client, Orders.price, " +
                        $"Orders.date_order, Orders.date_complete FROM Orders  JOIN Users ON Users.id = Orders.id_master JOIN Clients ON Clients.id = Orders.id_client";
                    break;
                case 2:
                    logRequst.CommandText = $"SELECT Services_to_orders.id_o, Services.name_s AS name_serv FROM Services_to_orders JOIN Services " +
                        $"ON Services.id = Services_to_orders.id_s";
                    break;
                case 3:
                    logRequst.CommandText = $"SELECT * FROM Services";
                    break;
                case 4:
                    logRequst.CommandText = $"SELECT Users.id, Roles.name_r AS role_u, Users.name_u, Users.surname_u," +
                        $" Users.tel_num_u, Users.pw, Users.listed FROM Users JOIN Roles ON Roles.id = Users.role_u";
                    break;
                case 5:
                    logRequst.CommandText = $"SELECT * FROM Roles";
                    break;
            }

            logRequst.Connection = sqlConnect;

            // SqlAdapter - прослойка между источником данных и базой данных
            SqlDataAdapter adapter = new SqlDataAdapter(logRequst);
            DataSet dataSet = new DataSet();
            // заполняем источник данных полученными из адаптера записями
            adapter.Fill(dataSet);
            dataGridView1.DataSource = dataSet.Tables[0];

            UpdateComboBoxWithDataGridColumns(cbColumn, dataGridView1);

            tbDataSearch.Clear();

            sqlConnect.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            Form mainForm = Application.OpenForms["frmAuthorization"];
            mainForm.Show();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void addString_Click(object sender, EventArgs e)
        {
            int boxItem = comboBox1.SelectedIndex;
            // Переменная для определеня назначения Form3
            bool typeform = true;
            this.Hide();
            Form3 fm = new Form3(connect, boxItem, roleForm, typeform, "0");
            fm.ShowDialog();
            connect.Close();
        }


        private void btEditString_Click(object sender, EventArgs e)
        {
            int boxItem = comboBox1.SelectedIndex;
            // Переменная для определеня назначения Form3
            bool typeform = false;
            this.Hide();
            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
            string selID = selectedRow.Cells[0].Value.ToString();
            Form3 fm = new Form3(connect, boxItem, roleForm, typeform, selID);
            fm.ShowDialog();
        }

        private void btDelString_Click(object sender, EventArgs e)
        {
            // Получаем выбранную строку
            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

            // Формируем текст для подтверждения удаления
            string confirmationText = $"Вы действительно хотите удалить из таблицы ";

            // Получаем значения всех столбцов выбранной строки и добавляем их к тексту подтверждения
            for (int i = 0; i < selectedRow.Cells.Count; i++)
            {
                confirmationText += $"{selectedRow.Cells[i].Value.ToString()}";
                if (i < selectedRow.Cells.Count - 1)
                    confirmationText += " - ";
            }

            // Показываем окно подтверждения
            DialogResult result = MessageBox.Show(confirmationText, "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Если пользователь подтвердил удаление, выполняем операцию
            if (result == DialogResult.Yes)
            {
                // Устанавливаем полученное значение в Label
                label10.Text = selectedRow.Cells[0].Value.ToString();
            }
        }

        private void tbDataSeatch_TextChanged(object sender, EventArgs e)
        {
            /*string filterValue = tbDataSearch.Text;
            if (!string.IsNullOrEmpty(filterValue))
            {
                if (int.TryParse(filterValue, out int numericValue))
                {
                    (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"CONVERT([{cbColumn.Text}], 'System.Int32') = {numericValue}";
                }
                else
                {
                    (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"[{cbColumn.Text}] LIKE '%{filterValue}%'";
                }
            }
            else
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Empty;
            }*/

            string searchText = tbDataSearch.Text;

            // Получаем CurrencyManager, который управляет привязкой данных к DataGridView
            CurrencyManager currencyManager = (CurrencyManager)BindingContext[dataGridView1.DataSource];

            // Перебираем строки в DataGridView
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Проверяем, что строка не является строкой заголовка (Header) и не является строкой, связанной с CurrencyManager
                if (!row.IsNewRow && row.Index != currencyManager.Position)
                {
                    bool rowVisible = false;

                    // Перебираем ячейки в каждой строке
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        // Проверяем содержимое ячейки на соответствие поисковому запросу
                        if (cell.Value != null && cell.Value.ToString().ToLower().Contains(searchText))
                        {
                            rowVisible = true; // Если ячейка соответствует, отмечаем строку как видимую
                            break; // Прекращаем проверку ячеек текущей строки
                        }
                    }

                    // Устанавливаем видимость строки в DataGridView в зависимости от результата поиска
                    row.Visible = rowVisible;
                }
            }
        }

        private void UpdateComboBoxWithDataGridColumns(System.Windows.Forms.ComboBox comboBox, DataGridView dataGridView)
        {
            // Очистка коллекции элементов ComboBox перед добавлением новых значений
            comboBox.Items.Clear();

            // Получение названий столбцов из DataGridView
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                // Добавление названий столбцов в коллекцию элементов ComboBox
                comboBox.Items.Add(column.HeaderText);
            }

            // Выбор первого элемента в ComboBox (если нужно)
            if (comboBox.Items.Count > 0)
            {
                comboBox.SelectedIndex = 0;
            }
        }

        private void cbColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbDataSearch.Clear();
        }


    }
}