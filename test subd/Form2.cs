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
            try
            {
                dataGridView1.AllowUserToAddRows = false;
                dataGridView2.AllowUserToAddRows = false;
                // Добавляем обработчик события CellDoubleClick
                dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
                dataGridView1.CellClick += dataGridView1_CellClick;

                if (roleForm != 1)
                {
                    groupBox3.Visible = false;
                    comboBox1.Items.RemoveAt(3);
                    comboBox1.Items.RemoveAt(3);
                    comboBox1.Items.RemoveAt(3);
                    comboBox1.Items.RemoveAt(2);
                }   
                SqlConnection sqlConnect = new SqlConnection(Properties.Settings.Default.connectionString);  
                
                sqlConnect.Open();
                SqlCommand showTable = new SqlCommand();
                showTable.CommandText = $"SELECT * FROM Клиенты";
                showTable.Connection = sqlConnect;

                // SqlAdapter - прослойка между источником данных и базой данных
                SqlDataAdapter adapter = new SqlDataAdapter(showTable);
                DataSet dataSet = new DataSet();
                // заполняем источник данных полученными из адаптера записями
                adapter.Fill(dataSet);
                dataGridView1.DataSource = dataSet.Tables[0];
                dataGridView1.Columns[0].Visible = false;
                sqlConnect.Close();
                UpdatePrice();
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void UpdatePrice()
        {
            // Создаем SQL-запрос
            string sql = @"
            UPDATE Orders
            SET Price = (
                SELECT SUM(s.cost_s)
                FROM Services_to_orders so
                INNER JOIN Services s ON so.id_s = s.ID
                WHERE so.id_o = Orders.ID
            )";

            // Создаем команду и связываем ее с подключением и SQL-запросом
            using (SqlCommand command = new SqlCommand(sql, connect))
            {
                // Выполняем SQL-запрос
                int rowsAffected = command.ExecuteNonQuery();

                // Выводим информацию о количестве обновленных строк
                Console.WriteLine("Обновлено строк: " + rowsAffected);
            }
        }

        public void LoadDataIntoDataGridView()
        {
            try
            {
                SqlConnection sqlConnect = new SqlConnection(Properties.Settings.Default.connectionString);
                SqlCommand logRequst = new SqlCommand();
                UpdatePrice();
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        logRequst.CommandText = $"SELECT * FROM Клиенты";
                        break;
                    case 1:
                        logRequst.CommandText = $"SELECT * FROM Заказы";
                        break;
                    case 2:
                        logRequst.CommandText = $"SELECT * FROM Услугикзаказам";
                        break;
                    case 3:
                        logRequst.CommandText = $"SELECT * FROM Услуги";
                        break;
                    case 4:
                        logRequst.CommandText = $"SELECT * FROM Пользователи";
                        break;
                    case 5:
                        logRequst.CommandText = $"SELECT * FROM Роли";
                        break;
                }

                logRequst.Connection = sqlConnect;

                // SqlAdapter - прослойка между источником данных и базой данных
                SqlDataAdapter adapter = new SqlDataAdapter(logRequst);
                DataSet dataSet = new DataSet();
                // заполняем источник данных полученными из адаптера записями
                adapter.Fill(dataSet);
                dataGridView1.DataSource = dataSet.Tables[0];
                tbDataSearch.Clear();

                if (comboBox1.SelectedItem != "Заказы")
                {
                    dataGridView2.Visible = false;
                    if (comboBox1.SelectedItem != "Услуги к заказам")
                    {
                        dataGridView1.Columns[0].Visible = false;
                    }
                }
                else
                {
                    dataGridView2.Visible = true;
                    dataGridView1.Columns[0].Visible = true;
                }

                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                sqlConnect.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataIntoDataGridView();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (comboBox1.SelectedItem == "Заказы")
                {
                    SqlConnection sqlConnect = new SqlConnection(Properties.Settings.Default.connectionString);
                    SqlCommand logRequst1 = new SqlCommand();
                    dataGridView2.Visible = true;
                    dataGridView1.Columns[0].Visible = true;
                    DataGridViewRow selectedRow = dataGridView1.CurrentRow;
                    logRequst1.CommandText = $"SELECT name_s AS 'Услуга' FROM Services_to_orders JOIN Services ON Services_to_orders.id_s = Services.id WHERE id_o = {selectedRow.Cells[0].Value}";

                    logRequst1.Connection = sqlConnect;

                    // SqlAdapter - прослойка между источником данных и базой данных
                    SqlDataAdapter adapter1 = new SqlDataAdapter(logRequst1);
                    DataSet dataSet1 = new DataSet();
                    // заполняем источник данных полученными из адаптера записями
                    adapter1.Fill(dataSet1);
                    dataGridView2.DataSource = dataSet1.Tables[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
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
            try
            {
                int boxItem = comboBox1.SelectedIndex;
                // Переменная для определеня назначения Form3
                bool typeform = true;
                this.Hide();
                Form3 fm = new Form3(connect, boxItem, roleForm, typeform, "0", "0");
                fm.Text = "Добавление";
                fm.ShowDialog();
                //connect.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (roleForm == 1)
                {
                    int boxItem = comboBox1.SelectedIndex;
                    // Переменная для определения назначения Form3
                    bool typeform = false;
                    this.Hide();

                    // Проверяем, что индекс строки действителен
                    if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
                    {
                        DataGridViewRow selectedRow = dataGridView1.CurrentRow;

                        // Получаем значения из нужных столбцов по индексу столбца
                        string selID = selectedRow.Cells[0].Value.ToString();
                        string selIDs = selectedRow.Cells[1].Value.ToString();

                        // Создаем экземпляр Form3 и передаем необходимые значения
                        Form3 fm = new Form3(connect, boxItem, roleForm, typeform, selID, selIDs);
                        fm.Text = "Изменение";
                        fm.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void btDelString_Click(object sender, EventArgs e)
        {
            try
            {
                // Получаем выбранную строку
                DataGridViewRow selectedRow = dataGridView1.CurrentRow;

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
                    string id = selectedRow.Cells[0].Value.ToString();
                    string id_s = selectedRow.Cells[1].Value.ToString();

                    SqlCommand logRequest = new SqlCommand();
                    logRequest.Connection = connect;

                    switch (comboBox1.SelectedIndex)
                    {
                        case 0:
                            logRequest.CommandText = $"DELETE FROM Clients WHERE ID = {id}";
                            break;
                        case 1:
                            logRequest.CommandText = $"DELETE FROM Orsers WHERE ID = {id}";
                            break;
                        case 2:
                            // Получаем название услуги из выбранной строки DataGridView
                            string serviceName = dataGridView1.CurrentRow.Cells[1].Value.ToString();

                            // Создаем SQL-запрос для получения ID услуги по названию услуги
                            string sql = "SELECT ID FROM Services WHERE Name_s = @ServiceName";

                            // Создаем команду SQL с параметром
                            using (SqlCommand cmd = new SqlCommand(sql, connect))
                            {
                                // Добавляем параметр @ServiceName и устанавливаем его значение
                                cmd.Parameters.AddWithValue("@ServiceName", serviceName);

                                // Открываем соединение с базой данных
                                //connect.Open();

                                // Выполняем запрос и получаем ID услуги
                                int serviceId = (int)cmd.ExecuteScalar();

                                logRequest.CommandText = $"DELETE FROM Services_to_orders WHERE ID_O = {id} AND ID_S = {serviceId}";

                                // Закрываем соединение
                                //connect.Close();
                            }
                        
                            break;
                        case 3:
                            logRequest.CommandText = $"DELETE FROM Services WHERE ID = {id}";
                            break;
                        case 4:
                            logRequest.CommandText = $"DELETE FROM Users WHERE ID = {id}";
                            break;
                        case 5:
                            logRequest.CommandText = $"DELETE FROM Roles WHERE ID = {id}";
                            break;
                    }
                    logRequest.ExecuteNonQuery();
                }
                UpdatePrice();
                LoadDataIntoDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void tbDataSeatch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // Получаем названия всех столбцов DGV1
                var columnNames = dataGridView1.Columns.Cast<DataGridViewColumn>()
                                .Select(x => x.HeaderText)
                                .ToList();

                // Создаем строку фильтрации
                string filterExpression = string.Empty;
                foreach (var columnName in columnNames)
                {
                    if (!string.IsNullOrEmpty(filterExpression))
                        filterExpression += " OR ";

                    filterExpression += $"CONVERT([{columnName}], 'System.String') LIKE '%{tbDataSearch.Text}%'";
                }

                // Применяем фильтр к строкам DGV1
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = filterExpression;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
    }
}