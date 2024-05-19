using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test_subd
{
    public partial class Form4 : Form
    {
        SqlConnection connect;
        private bool tfDateEdit;
        DataGridViewRow selectedRowTemp;

        public Form4(SqlConnection cnct)
        {
            connect = cnct;
            InitializeComponent();
        }
        
        private void Form4_Load_1(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.CellClick += dataGridView1_CellClick;
                dataGridView1.CellDoubleClick += DataGridView1_CellDoubleClick;
                tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
                tabControl1.MouseClick += TabControl_MouseClick;

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

                // Создаем словарь для хранения значений из таблицы Users
                Dictionary<int, Tuple<string, string>> usersDictionary = new Dictionary<int, Tuple<string, string>>();

                string queryUsers = "SELECT id, name_u, tel_num_u FROM Users WHERE listed = 1 AND role_u = 3";
                using (SqlCommand command = new SqlCommand(queryUsers, connect))
                {
                    // Выполняем запрос и получаем результаты
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Перебираем строки результата
                        while (reader.Read())
                        {
                            // Получаем значения столбцов id, name_u и tel_num_u
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            string telNum = reader.GetString(2);

                            // Добавляем значения в словарь в виде кортежа
                            usersDictionary.Add(id, Tuple.Create(name, telNum));
                        }
                    }
                }
                foreach (var kvp in usersDictionary)
                {
                    cbOrdersId_maser.Items.Add($"{kvp.Key}. {kvp.Value.Item1} - {kvp.Value.Item2}");
                }

                // Создаем словарь для хранения значений из таблицы Clients
                Dictionary<int, Tuple<string, string>> clientsDictionary = new Dictionary<int, Tuple<string, string>>();

                string queryClients = "SELECT id, name_c, tel_num_c FROM Clients";
                using (SqlCommand command = new SqlCommand(queryClients, connect))
                {
                    // Выполняем запрос и получаем результаты
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Перебираем строки результата
                        while (reader.Read())
                        {
                            // Получаем значения столбцов id, name_c и tel_num_c
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            string telNum = reader.GetString(2);

                            // Добавляем значения в словарь в виде кортежа
                            clientsDictionary.Add(id, Tuple.Create(name, telNum));
                        }
                    }
                }
                foreach (var kvp in clientsDictionary)
                {
                    cbOrdersId_client.Items.Add($"{kvp.Key}. {kvp.Value.Item1} - {kvp.Value.Item2}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void tbDataSearch_TextChanged(object sender, EventArgs e)
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataIntoDataGridView();
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
                    button3.Visible = false;
                    listBox1.Visible = false;
                    dataGridView1.Columns[0].Visible = false;
                }
                else
                {
                    button3.Visible = true;
                    listBox1.Visible = true;
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
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (comboBox1.SelectedItem == "Заказы" && e.RowIndex >= 0)
                {

                    // Очищаем выделение в ListBox
                    //listBox1.ClearSelected();

                    // Соединение с базой данных
                    using (SqlConnection sqlConnect = new SqlConnection(Properties.Settings.Default.connectionString))
                    {
                        // Открытие соединения
                        sqlConnect.Open();

                        // Получение всех услуг из таблицы Services
                        string servicesQuery = "SELECT name_s FROM Services";

                        // Создание команды для получения всех услуг
                        using (SqlCommand servicesCommand = new SqlCommand(servicesQuery, sqlConnect))
                        {
                            // Выполнение запроса и получение результатов
                            using (SqlDataReader servicesReader = servicesCommand.ExecuteReader())
                            {
                                // Очищаем ListBox перед заполнением новыми данными
                                listBox1.Items.Clear();

                                // Заполняем ListBox всеми значениями name_s
                                while (servicesReader.Read())
                                {
                                    listBox1.Items.Add(servicesReader["name_s"].ToString());
                                }
                            }
                        }

                        // Получение выделенной строки в DataGridView
                        DataGridViewRow selectedRow = dataGridView1.CurrentRow;

                        // Запрос к базе данных для получения услуг для выбранного заказа
                        
                        string query = $"SELECT name_s FROM Services_to_orders JOIN Services ON Services_to_orders.id_s = Services.id WHERE id_o = {selectedRow.Cells[0].Value}";


                        // Создание команды
                        using (SqlCommand command = new SqlCommand(query, sqlConnect))
                        {
                            // Выполнение запроса и получение результатов
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                // Отмечаем в ListBox те элементы, которые соответствуют услугам для выбранного заказа
                                while (reader.Read())
                                {
                                    string serviceName = reader["name_s"].ToString();
                                    int index = listBox1.FindStringExact(serviceName);
                                    if (index != ListBox.NoMatches)
                                    {
                                        listBox1.SetSelected(index, true);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Получаем id выбранного заказа из DataGridView
            int orderId = (int)dataGridView1.CurrentRow.Cells[0].Value;

            // Очищаем все связанные услуги для данного заказа
            ClearServicesForOrder(orderId);

            // Добавляем новые связи для выбранных услуг
            foreach (var item in listBox1.SelectedItems)
            {
                string serviceName = item.ToString();
                // Получаем id услуги по ее названию
                int serviceId = GetServiceId(serviceName);

                // Добавляем новую связь в таблицу Services_to_orders
                AddServiceToOrder(orderId, serviceId);
            }
            LoadDataIntoDataGridView();
        }

        private void ClearServicesForOrder(int orderId)
        {
            string deleteQuery = "DELETE FROM Services_to_orders WHERE id_o = @orderId";

            using (SqlCommand command = new SqlCommand(deleteQuery, connect))
            {
                command.Parameters.AddWithValue("@orderId", orderId);
                command.ExecuteNonQuery();
            }
        }

        private int GetServiceId(string serviceName)
        {
            string selectQuery = "SELECT id FROM Services WHERE name_s = @serviceName";

            using (SqlCommand command = new SqlCommand(selectQuery, connect))
            {
                command.Parameters.AddWithValue("@serviceName", serviceName);
                // Выполняем запрос и возвращаем результат
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
                else
                {
                    // Возвращаем -1 в случае, если услуга с заданным именем не найдена
                    return -1;
                }
            }
        }

        private void AddServiceToOrder(int orderId, int serviceId)
        {
            string insertQuery = "INSERT INTO Services_to_orders (id_o, id_s) VALUES (@orderId, @serviceId)";

            using (SqlCommand command = new SqlCommand(insertQuery, connect))
            {
                command.Parameters.AddWithValue("@orderId", orderId);
                command.Parameters.AddWithValue("@serviceId", serviceId);
                command.ExecuteNonQuery();
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0) // Проверяем, что выбрана первая вкладка
            {
                this.Width = 816; // Устанавливаем ширину формы
                this.Height = 489; // Устанавливаем высоту формы 
            }
            if (tabControl1.SelectedIndex == 1) // Проверяем, что выбрана первая вкладка
            {
                this.Width = 416; // Устанавливаем ширину формы
                this.Height = 240; // Устанавливаем высоту формы
                radioButton1.Checked = true;
            }
            ClearAllElements();
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            panelOrders.Location = new Point(0,0);
            panelClients.Location = new Point(392, 274);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            panelOrders.Location = new Point(392, 274);
            panelClients.Location = new Point(0, 0);
        }

        //Очистка элементов
        private void ClearAllElements()
        {
            tbClientsAdress_c.Text = string.Empty;
            tbClientsNane_c.Text = string.Empty;
            tbClientsTel_num_c.Text = string.Empty;
            cbOrdersId_client.Text = string.Empty;
            cbOrdersId_maser.Text = string.Empty;
            comboBox2.Text = string.Empty;
            checkBox1.Checked = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlCommand logRequest = new SqlCommand();
            logRequest.Connection = connect;
            if (tfDateEdit == false)
            {
                if (radioButton1.Checked == true)
                {
                    logRequest.CommandText = $"INSERT INTO Clients (name_c, tel_num_c, addres_c) " +
                                        $"VALUES ('{tbClientsNane_c.Text}', '{tbClientsTel_num_c.Text}', '{tbClientsAdress_c.Text}')";
                    MessageBox.Show("Строка добавлена");
                }

                if (radioButton2.Checked == true)
                {
                    int indexOfDotClient = cbOrdersId_client.Text.IndexOf('.');
                    string id_client = cbOrdersId_client.Text.Substring(0, indexOfDotClient);

                    if (checkBox1.Checked == false)
                    {
                        int indexOfDotMaster = cbOrdersId_maser.Text.IndexOf('.');
                        string id_master = cbOrdersId_maser.Text.Substring(0, indexOfDotMaster);
                        logRequest.CommandText = $"INSERT INTO Orders (id_master, id_client, date_order, date_complete) VALUES ({id_master}, {id_client}, " +
                                                $"'{dtpOrdersDate_order.Value.ToString("yyyy-MM-dd")}', '{dtpOrdersDate_complete.Value.ToString("yyyy-MM-dd")}')";
                        MessageBox.Show("Строка добавлена");
                    }
                    else
                    {

                        int indexOfDotMaster = cbOrdersId_maser.Text.IndexOf('.');
                        string id_master = cbOrdersId_maser.Text.Substring(0, indexOfDotMaster);
                        logRequest.CommandText = $"INSERT INTO Orders (id_master, id_client, date_order) VALUES ({id_master}, {id_client}, " +
                                                $"'{dtpOrdersDate_order.Value.ToString("yyyy-MM-dd")}')";
                        MessageBox.Show("Строка добавлена");

                    }
                }
            }
            else
            {
                int indexOfDotMaster = comboBox2.Text.IndexOf('.');
                string id_master = comboBox2.Text.Substring(0, indexOfDotMaster);

                logRequest.CommandText = $"UPDATE Orders SET id_master = {id_master}, date_complete = '{dateTimePicker1.Value.ToString("yyyy-MM-dd")}' " +
                                    $"WHERE id = {selectedRowTemp.Cells[0].Value}";
                MessageBox.Show("Строка изменена");
                tabControl1.SelectedIndex = 0;
            }
            logRequest.ExecuteNonQuery();
            LoadDataIntoDataGridView();
            ClearAllElements();
        }

        private void TabControl_MouseClick(object sender, MouseEventArgs e)
        {
            tfDateEdit = false;
            panelClients.Location = new Point(0, 0);
            panelOrders.Location = new Point(392, 274);
            panelComplete.Location = new Point(392, 274);
            radioButton1.Checked = true;
            radioButton1.Visible = true;
            radioButton2.Visible = true;
            button2.Text = "Добавить";
        }

        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Проверяем, что двойной щелчок произошел в пятом столбце
            if (e.RowIndex >= 0 && comboBox1.SelectedItem == "Заказы") // Убедитесь, что строка действительная
            {
                tfDateEdit = true;
                button2.Text = "Изменить";
                tabControl1.SelectedIndex = 1;
                radioButton1.Visible = false;
                radioButton2.Visible = false;
                panelClients.Location = new Point(392, 274);
                panelOrders.Location = new Point(392, 274);
                panelComplete.Location = new Point(0, 0);

                // Создаем словарь для хранения значений из таблицы Users
                Dictionary<int, Tuple<string, string>> usersDictionary = new Dictionary<int, Tuple<string, string>>();

                string queryUsers = "SELECT id, name_u, tel_num_u FROM Users WHERE listed = 1 AND role_u = 3";
                using (SqlCommand command = new SqlCommand(queryUsers, connect))
                {
                    // Выполняем запрос и получаем результаты
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Перебираем строки результата
                        while (reader.Read())
                        {
                            // Получаем значения столбцов id, name_u и tel_num_u
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            string telNum = reader.GetString(2);

                            // Добавляем значения в словарь в виде кортежа
                            usersDictionary.Add(id, Tuple.Create(name, telNum));
                        }
                    }
                }
                foreach (var kvp in usersDictionary)
                {
                    comboBox2.Items.Add($"{kvp.Key}. {kvp.Value.Item1} - {kvp.Value.Item2}");
                }
                selectedRowTemp = dataGridView1.CurrentRow;
                
                // Заполняем элементы из БД
                string queryOrders = $"SELECT id_master, id_client, date_order, date_complete FROM Orders WHERE id = @selectedId";
                using (SqlCommand command = new SqlCommand(queryOrders, connect))
                {
                    command.Parameters.AddWithValue("@selectedId", selectedRowTemp.Cells[0].Value);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int id_master = reader.GetInt32(0);
                            //int id_client = reader.GetInt32(1);
                            //DateTime date_order = reader.GetDateTime(2);
                            if (!reader.IsDBNull(3)) // Проверяем, не равно ли значение NULL
                            {
                                DateTime date_complete = reader.GetDateTime(3);
                                dateTimePicker1.Value = date_complete;
                            }
                            else
                            {
                                DateTime date_complete = DateTime.Now;
                                dateTimePicker1.Value = date_complete;
                            }

                            // Закрыть текущий DataReader
                            reader.Close();

                            // Заполнение элементов формы из базы данных для мастера
                            string queryMaster = $"SELECT name_u, tel_num_u FROM Users WHERE id = @id_master";
                            using (SqlCommand masterCommand = new SqlCommand(queryMaster, connect))
                            {
                                masterCommand.Parameters.AddWithValue("@id_master", id_master);
                                using (SqlDataReader masterReader = masterCommand.ExecuteReader())
                                {
                                    if (masterReader.Read() && selectedRowTemp.Cells[1].Value != null)
                                    {
                                        string name_master = masterReader.GetString(0);
                                        string telNum_master = masterReader.GetString(1);
                                        comboBox2.Text = $"{id_master}. {name_master} - {telNum_master}";
                                    }
                                }
                            }
                            

                        }
                    }
                }
                // Создаем временный список для хранения уникальных элементов
                List<string> uniqueItems = new List<string>();

                // Проходим по всем элементам ComboBox
                foreach (var item in comboBox2.Items)
                {
                    // Если элемент еще не добавлен в список уникальных элементов, добавляем его
                    if (!uniqueItems.Contains(item.ToString()))
                    {
                        uniqueItems.Add(item.ToString());
                    }
                }

                // Очищаем ComboBox
                comboBox2.Items.Clear();

                // Добавляем уникальные элементы обратно в ComboBox
                foreach (var item in uniqueItems)
                {
                    comboBox2.Items.Add(item);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            Form mainForm = Application.OpenForms["frmAuthorization"];
            mainForm.Show();
        }
    }
}
