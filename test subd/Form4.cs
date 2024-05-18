using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test_subd
{
    public partial class Form4 : Form
    {
        SqlConnection connect;
        private bool isDragging = false; // Флаг, указывающий, что идет перетаскивание
        private int startIndex = -1; // Начальный индекс выделения

        public Form4(SqlConnection cnct)
        {
            connect = cnct;
            InitializeComponent();
        }

        private void Form4_Load_1(object sender, EventArgs e)
        {
            try
            {
                // Добавляем обработчик события CellDoubleClick
                //dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
                dataGridView1.CellClick += dataGridView1_CellClick;
                listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
                //listBox1.Click += listBox1_Click;
                //listBox1.MouseUp += listBox1_MouseUp;
                //listBox1.MouseMove += listBox1_MouseMove;

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
                    listBox1.Visible = false;
                    if (comboBox1.SelectedItem != "Услуги к заказам")
                    {
                        dataGridView1.Columns[0].Visible = false;
                    }
                }
                else
                {
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
                if (comboBox1.SelectedItem == "Заказы")
                {
                    // Очищаем выделение в ListBox
                    listBox1.ClearSelected();

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


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
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

    }
}
