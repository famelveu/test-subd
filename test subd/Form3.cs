using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace test_subd
{
    public partial class Form3 : Form
    {
        private SqlConnection connect;
        private int selectedTable;
        private int rlu;
        private bool tfAdd;
        private int selectedId;
        private string selectedIds;

        public Form3(SqlConnection cnct, int boxItem, int role, bool typeForm, string selID, string selIDs)
        {
            connect = cnct;;
            InitializeComponent();
            rlu = role;
            selectedTable = boxItem;
            tfAdd = typeForm;
            selectedId = Convert.ToInt32(selID);
            selectedIds = selIDs;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            try
            {
                if (tfAdd == false)
                {

                    button1.Text = "Изменить";

                    //Заполняем вкладки для изменения
                    switch(selectedTable)
                    {
                        case 0:
                            using (SqlCommand command = new SqlCommand())
                            {
                                command.Connection = connect; 

                                command.CommandText = $"SELECT name_c, tel_num_c, addres_c FROM Clients WHERE id = @selectedId";
                                command.Parameters.AddWithValue("@selectedId", selectedId);

                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        // Заполняем элементы формы данными из базы данных
                                        tbClientsNane_c.Text = reader["name_c"].ToString();
                                        tbClientsTel_num_c.Text = reader["tel_num_c"].ToString();
                                        tbClientsAdress_c.Text = reader["addres_c"].ToString();
                                    }
                                }
                            }
                            break;
                        case 1:
                            string queryOrders = $"SELECT id_master, id_client, date_order, date_complete FROM Orders WHERE id = @selectedId";
                            using (SqlCommand command = new SqlCommand(queryOrders, connect))
                            {
                                command.Parameters.AddWithValue("@selectedId", selectedId);
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        int id_master = reader.GetInt32(0);
                                        int id_client = reader.GetInt32(1);
                                        DateTime date_order = reader.GetDateTime(2);
                                        DateTime date_complete = reader.GetDateTime(3);

                                        // Закрыть текущий DataReader
                                        reader.Close();

                                        // Заполнение элементов формы из базы данных для мастера
                                        string queryMaster = $"SELECT name_u, tel_num_u FROM Users WHERE id = @id_master";
                                        using (SqlCommand masterCommand = new SqlCommand(queryMaster, connect))
                                        {
                                            masterCommand.Parameters.AddWithValue("@id_master", id_master);
                                            using (SqlDataReader masterReader = masterCommand.ExecuteReader())
                                            {
                                                if (masterReader.Read())
                                                {
                                                    string name_master = masterReader.GetString(0);
                                                    string telNum_master = masterReader.GetString(1);
                                                    cbOrdersId_maser.Text = $"{id_master}. {name_master} - {telNum_master}";
                                                }
                                            }
                                        }

                                        // Заполнение элементов формы из базы данных для клиента
                                        string queryClient = $"SELECT name_c, tel_num_c FROM Clients WHERE id = @id_client";
                                        using (SqlCommand clientCommand = new SqlCommand(queryClient, connect))
                                        {
                                            clientCommand.Parameters.AddWithValue("@id_client", id_client);
                                            using (SqlDataReader clientReader = clientCommand.ExecuteReader())
                                            {
                                                if (clientReader.Read())
                                                {
                                                    string name_client = clientReader.GetString(0);
                                                    string telNum_client = clientReader.GetString(1);
                                                    cbOrdersId_client.Text = $"{id_client}. {name_client} - {telNum_client}";
                                                }
                                            }
                                        }

                                        // Заполнение даты из базы данных
                                        dtpOrdersDate_order.Value = date_order;
                                        dtpOrdersDate_complete.Value = date_complete;
                                    }
                                }
                            }

                            break;
                        case 2:
                            string querySelectedOrder = "SELECT id_o, id_s FROM Services_to_orders " +
                                "INNER JOIN Services ON Services_to_orders.id_s = Services.id " +
                                "WHERE id_o = @selectedId_o AND name_s = @selectedName_s";

                            using (SqlCommand command = new SqlCommand(querySelectedOrder, connect))
                            {
                                command.Parameters.AddWithValue("@selectedId_o", selectedId);
                                command.Parameters.AddWithValue("@selectedName_s", selectedIds);
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        int id_o = reader.GetInt32(0);
                                        int id_s = reader.GetInt32(1);

                                        // Закрыть текущий DataReader
                                        reader.Close();

                                        // Заполнение элементов формы из базы данных для id_o
                                        cbS_t_oId_o.Text = id_o.ToString();

                                        // Заполнение элементов формы из базы данных для id_s
                                        cbS_t_oId_s.Text = $"{id_s}. {selectedIds}";
                                    }
                                }
                            }


                            break;
                        case 3:
                            using (SqlCommand command = new SqlCommand())
                            {
                                command.Connection = connect; 

                                command.CommandText = $"SELECT name_s, cost_s FROM Services WHERE id = @selectedId";
                                command.Parameters.AddWithValue("@selectedId", selectedId);

                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        // Заполняем элементы формы данными из базы данных
                                        tbServicesName_s.Text = reader["name_s"].ToString();
                                        tbServicesCost_s.Text = reader["cost_s"].ToString();
                                    }
                                }
                            }
                            break;
                        case 4:
                            // SQL-запрос для выборки данных пользователя по его id
                            string query = "SELECT roles.name_r, name_u, surname_u, tel_num_u, pw, listed FROM Users " +
                                   "INNER JOIN roles ON Users.role_u = roles.id " +
                                   "WHERE Users.id = @selectedId";

                            using (SqlCommand command = new SqlCommand(query, connect))
                            {
                                command.Parameters.AddWithValue("@selectedId", selectedId);

                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    if (reader.HasRows)
                                    {
                                        while (reader.Read())
                                        {
                                            cbUsersRole_u.Text = reader["name_r"].ToString(); // Обратите внимание на изменение здесь
                                            tbUsersName_u.Text = reader["name_u"].ToString();
                                            tbUsersSurname_u.Text = reader["surname_u"].ToString();
                                            tbUsersTel_num_u.Text = reader["tel_num_u"].ToString();
                                            tbUsersPw.Text = reader["pw"].ToString();
                                            chbUsersListed.Checked = Convert.ToBoolean(reader["listed"]);
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("No rows found.");
                                    }
                                }
                            }

                            break;
                        case 5:
                        
                            break;

                    }
                }
                else
                {
                    button1.Text = "Добавить";
                }
                if (rlu != 1)
                { 
                    tabControl1.TabPages[3].Hide();
                    tabControl1.TabPages[4].Hide();
                    tabControl1.TabPages[5].Hide();
                }

                switch (selectedTable)
                {
                    case 0:
                        tabControl1.SelectedIndex = selectedTable;
                        break;
                    case 1:
                        tabControl1.SelectedIndex = selectedTable;

                        // Создаем словарь для хранения значений из таблицы Users
                        Dictionary<int, Tuple<string, string>> usersDictionary = new Dictionary<int, Tuple<string, string>>();

                        string queryUsers = "SELECT id, name_u, tel_num_u FROM Users";
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

                        break;
                    case 2:
                        tabControl1.SelectedIndex = selectedTable;

                        // Создаем словарь для хранения значений из таблицы Services
                        Dictionary<int, string> servicesDictionary = new Dictionary<int, string>();

                        // Создаем команду для выполнения запроса к таблице Services
                        string queryServices = "SELECT id, name_s FROM Services";
                        using (SqlCommand command = new SqlCommand(queryServices, connect))
                        {
                            // Выполняем запрос и получаем результаты
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                // Перебираем строки результата
                                while (reader.Read())
                                {
                                    // Получаем значения столбцов id и name_s
                                    int id = reader.GetInt32(0);
                                    string name_s = reader.GetString(1);

                                    // Добавляем значения в словарь
                                    servicesDictionary.Add(id, name_s);
                                }
                            }
                        }

                        // Выводим содержимое словаря в cbS_t_oId_s
                        foreach (var kvp in servicesDictionary)
                        {
                            cbS_t_oId_s.Items.Add($"{kvp.Key}. {kvp.Value}");
                        }

                        // Создаем список для хранения id заказов
                        List<int> ordersList = new List<int>();

                        // Создаем команду для выполнения запроса к таблице Orders
                        string query = "SELECT id FROM Orders";
                        using (SqlCommand command = new SqlCommand(query, connect))
                        {
                            // Выполняем запрос и получаем результаты
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                // Перебираем строки результата и добавляем id заказов в список
                                while (reader.Read())
                                {
                                    int orderId = reader.GetInt32(0);
                                    ordersList.Add(orderId);
                                }
                            }
                        }
                        // Добавляем элементы списка в ComboBox
                        foreach (int orderId in ordersList)
                        {
                            cbS_t_oId_o.Items.Add(orderId);
                        }
                        break;
                    case 3:
                        tabControl1.SelectedIndex = selectedTable;
                        break;
                    case 4:
                        tabControl1.SelectedIndex = selectedTable;

                        // Создаем словарь для хранения значений из таблицы Roles
                        Dictionary<int, string> rolesDictionary = new Dictionary<int, string>();

                        // Создаем команду для выполнения запроса к таблице Roles
                        string queryRoles = "SELECT id, name_r FROM Roles";
                        using (SqlCommand command = new SqlCommand(queryRoles, connect))
                        {
                            // Выполняем запрос и получаем результаты
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                // Перебираем строки результата
                                while (reader.Read())
                                {
                                    // Получаем значения столбцов id и name_r
                                    int id = reader.GetInt32(0);
                                    string name_r = reader.GetString(1);

                                    // Добавляем значения в словарь
                                    rolesDictionary.Add(id, name_r);
                                }
                            }
                        }

                        // Выводим содержимое словаря в cbUsersRole_u
                        foreach (var kvp in rolesDictionary)
                        {
                            cbUsersRole_u.Items.Add($"{kvp.Key}. {kvp.Value}");
                        }

                        break;
                    case 5:
                        tabControl1.SelectedIndex = selectedTable;
                        break;
                }

                for (int i = 0; i < tabControl1.TabCount; i++)
                {
                    // Устанавливаем Enabled = false для всех вкладок, кроме выбранной
                    if (i != selectedTable)
                    {
                        tabControl1.TabPages[i].Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.connectionString))
                {
                    conn.Open();
                    SqlCommand logRequest = new SqlCommand();
                    logRequest.Connection = conn;

                    switch (selectedTable)
                    {
                        case 0:
                            if (tfAdd == true)
                            {
                                logRequest.CommandText = $"INSERT INTO Clients (name_c, tel_num_c, addres_c) " +
                                    $"VALUES ('{tbClientsNane_c.Text}', '{tbClientsTel_num_c.Text}', '{tbClientsAdress_c.Text}')";
                                MessageBox.Show("Строка добавлена");
                            }

                            else
                            {

                                logRequest.CommandText = $"UPDATE Clients SET name_c = '{tbClientsNane_c.Text}', tel_num_c = '{tbClientsTel_num_c.Text}', addres_c = '{tbClientsAdress_c.Text}' WHERE id = {selectedId}";
                                MessageBox.Show("Строка изменена");
                            }

                            break;

                        case 1:
                            if (tfAdd == true)
                            {
                                int indexOfDotMaster = cbOrdersId_maser.Text.IndexOf('.');
                                string id_master = cbOrdersId_maser.Text.Substring(0, indexOfDotMaster);

                                int indexOfDotClient = cbOrdersId_client.Text.IndexOf('.');
                                string id_client = cbOrdersId_client.Text.Substring(0, indexOfDotClient);

                                logRequest.CommandText = $"INSERT INTO Orders (id_master, id_client, date_order, date_complete) VALUES ({id_master}, {id_client}, " +
                                    $"'{dtpOrdersDate_order.Value.ToString("yyyy-MM-dd")}', '{dtpOrdersDate_complete.Value.ToString("yyyy-MM-dd")}')";
                                MessageBox.Show("Строка добавлена");
                            }

                            else
                            {
                                int indexOfDotMaster = cbOrdersId_maser.Text.IndexOf('.');
                                string id_master = cbOrdersId_maser.Text.Substring(0, indexOfDotMaster);

                                int indexOfDotClient = cbOrdersId_client.Text.IndexOf('.');
                                string id_client = cbOrdersId_client.Text.Substring(0, indexOfDotClient);

                                logRequest.CommandText = $"UPDATE Orders SET id_master = {id_master}, id_client = {id_client}, " +
                                    $"date_order = '{dtpOrdersDate_order.Value.ToString("yyyy-MM-dd")}', " +
                                    $"date_complete = '{dtpOrdersDate_complete.Value.ToString("yyyy-MM-dd")}' " +
                                    $"WHERE id = {selectedId}";
                                MessageBox.Show("Строка изменена");
                            }

                            break;

                        case 2:
                            if (tfAdd == true)
                            {
                                int indexOfDot_s = cbS_t_oId_s.Text.IndexOf('.');
                                string id_o = cbS_t_oId_o.Text;
                                string id_s = cbS_t_oId_s.Text.Substring(0, indexOfDot_s);

                                logRequest.CommandText = $"INSERT INTO Services_to_orders (id_o, id_s) VALUES ({id_o}, {id_s})";
                                MessageBox.Show("Строка добавлена");
                            }

                            else
                            {
                                int indexOfDot_s = cbS_t_oId_s.Text.IndexOf('.');
                                string id_o = cbS_t_oId_o.Text;
                                string id_s = cbS_t_oId_s.Text.Substring(0, indexOfDot_s);

                                logRequest.CommandText = $"UPDATE Services_to_orders SET id_o = {id_o}, id_s = {id_s} WHERE id_o = {selectedId} AND id_s = {selectedIds}";
                                MessageBox.Show("Строка изменена");
                            }

                            break;

                        case 3:
                            if (tfAdd == true)
                            {
                                logRequest.CommandText = $"INSERT INTO Services (name_s, cost_s) VALUES ('{tbServicesName_s.Text}', {tbServicesCost_s.Text})";
                                MessageBox.Show("Строка добавлена");
                            }

                            else
                            {
                                logRequest.CommandText = $"UPDATE Services SET name_s = '{tbServicesName_s.Text}', cost_s = {tbServicesCost_s.Text} WHERE id = {selectedId}";
                                MessageBox.Show("Строка изменена");
                            }
                            break;
                        case 4:
                            if (tfAdd == true)
                            {
                                int indexOfDot = cbUsersRole_u.Text.IndexOf('.');
                                string role_u = cbUsersRole_u.Text.Substring(0, indexOfDot);

                                logRequest.CommandText = $"INSERT INTO Users (role_u, name_u, surname_u, tel_num_u, pw, listed) VALUES ('{role_u}'," +
                                    $" '{tbUsersName_u.Text}', '{tbUsersSurname_u.Text}', '{tbUsersTel_num_u.Text}', '{tbUsersPw.Text}', {(chbUsersListed.Checked ? 1 : 0)})";

                                MessageBox.Show("Строка добавлена");
                            }
                            else
                            {
                                int indexOfDot = cbUsersRole_u.Text.IndexOf('.');
                                string role_u = cbUsersRole_u.Text.Substring(0, indexOfDot);
                                logRequest.CommandText = $"UPDATE Users SET role_u = '{role_u}', name_u = '{tbUsersName_u.Text}', " +
                                    $"surname_u = '{tbUsersSurname_u.Text}', tel_num_u = '{tbUsersTel_num_u.Text}', " +
                                    $"pw = '{tbUsersPw.Text}', listed = {(chbUsersListed.Checked ? 1 : 0)} WHERE id = {selectedId}";
                                MessageBox.Show("Строка изменена");
                            }

                            break;

                        case 5:
                            if (tfAdd == true)
                            {
                                logRequest.CommandText = $"INSERT INTO Roles (name_r) VALUES ('{tbRolesName_r.Text}')";
                                MessageBox.Show("Строка добавлена");
                            }

                            else
                            {
                                logRequest.CommandText = $"UPDATE Roles SET name_r = '{tbRolesName_r.Text}' WHERE id = {selectedId}";
                                MessageBox.Show("Строка изменена");
                            }

                            break;
                    }
                    logRequest.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            Form2 fm = Application.OpenForms["Form2"] as Form2;
            fm.Show();
            fm.LoadDataIntoDataGridView();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }

}
