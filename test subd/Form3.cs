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

        public Form3(SqlConnection cnct, int boxItem, int role, bool typeForm, string selID)
        {
            connect = cnct;;
            InitializeComponent();
            rlu = role;
            selectedTable = boxItem;
            tfAdd = typeForm;
            selectedId = Convert.ToInt32(selID);
        }

        private void Form3_Load(object sender, EventArgs e)
        {

            

            if(tfAdd == false)
            {
                button1.Text = "Изменить";
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

                    // Создаем словарь для хранения значений из таблицы Roles
                    Dictionary<int, string> servicesDictionary = new Dictionary<int, string>();

                    // Создаем команду для выполнения запроса к таблице Roles
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

        private void button1_Click(object sender, EventArgs e)
        {
            /*using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.connectionString))
            {
                conn.Open();
                SqlCommand logRequest = new SqlCommand();
                logRequest.Connection = conn;

                if (boxItem == "Goods")
                {
                    logRequest.CommandText = $"INSERT INTO [Goods] (goodID, firmGoodID, typeDeliveryID, goodModel, goodAmount, goodSize, warehouseID) " +
                                             $"VALUES ({textBox1.Text}, {comboBox1.SelectedIndex + 1}, {textBox2.Text}, '{textBox3.Text}', {textBox4.Text}, {textBox5.Text}, {textBox6.Text})";
                }
                else if (boxItem == "FirmsGoods")
                {
                    logRequest.CommandText = $"INSERT INTO [FirmsGoods] (fgid, fgfirms) VALUES ({textBox1.Text}, {textBox2.Text})";
                }

                else if (boxItem == "EmployeeDriver")
                {
                    logRequest.CommandText = $"INSERT INTO [EmployeeDriver] (employeeDriverID, employeeDriverFIO) VALUES ({textBox1.Text}, {textBox2.Text})";
                }
                else if (boxItem == "Container")
                {
                    logRequest.CommandText = $"INSERT INTO [Container] (containerID, containerType) VALUES ({textBox1.Text}, {textBox2.Text})";
                }
                else if (boxItem == "Delivery")
                {
                    logRequest.CommandText = $"INSERT INTO [Delivery] (deliveryID, orderID, employeesDriverID, transportID, containerID, destination) " +
                                             $"VALUES ({textBox1.Text}, {comboBox1.SelectedIndex + 1}, {comboBox2.SelectedIndex + 1}, {comboBox3.SelectedIndex + 1}, {comboBox4.SelectedIndex + 1}, '{textBox5.Text}')";
                }
                else if (boxItem == "Orders")
                {
                    logRequest.CommandText = $"INSERT INTO [Orders] (orderid, employeeid, customerid, goodid, orderamount) " +
                                             $"VALUES ({textBox1.Text}, {comboBox1.SelectedIndex + 1}, {comboBox2.SelectedIndex + 1}, {comboBox3.SelectedIndex + 1}, '{textBox4.Text}')";
                }
                else if (boxItem == "Roles")
                {
                    logRequest.CommandText = $"INSERT INTO [Roles] (RolesID, RolesName) VALUES ({textBox1.Text}, {textBox2.Text})";
                }
                else if (boxItem == "Transport")
                {
                    logRequest.CommandText = $"INSERT INTO [Transport] (transportID, typeDeliveryID, transport) VALUES ({textBox1.Text}, {comboBox1.SelectedIndex + 1},{textBox2.Text})";
                }
                else if (boxItem == "TypeGoods")
                {
                    logRequest.CommandText = $"INSERT INTO [TypeGoods] (typeGoodsID, typeGood) VALUES ({textBox1.Text}, {textBox2.Text})";
                }
                else if (boxItem == "Typeofdelivery")
                {
                    logRequest.CommandText = $"INSERT INTO [Typeofdelivery] (typeDeliveryID, typeDeliveryVehicle) VALUES ({textBox1.Text}, {textBox2.Text})";
                }
                else if (boxItem == "Users")
                {
                    logRequest.CommandText = $"INSERT INTO [Users] (userID, userFIO, roles, loginRules, passwordRules) VALUES ({textBox1.Text}, {textBox7.Text}), {comboBox2.SelectedIndex + 1}, {textBox4.Text}, {textBox5.Text}";
                }
                else if (boxItem == "Warehouses")
                {
                    logRequest.CommandText = $"INSERT INTO [Warehouses] (warehouseID, warehouseAdres) VALUES ({textBox1.Text}, {textBox2.Text})";
                }

                logRequest.ExecuteNonQuery();
                MessageBox.Show("Запись успешно добавлена");
            }*/

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
                            //logRequest.CommandText = 
                        }

                        break;

                    case 1:
                        if (tfAdd == true)
                        {
                            int indexOfDot = cbOrdersId_client.Text.IndexOf('.');
                            string id_master = cbOrdersId_maser.Text.Substring(0, indexOfDot);
                            string id_client = cbOrdersId_client.Text.Substring(0, indexOfDot);

                            logRequest.CommandText = $"INSERT INTO Orders (id_master, id_client, date_order, date_complete) VALUES ({id_master}, {id_client}, " +
                                $"'{dtpOrdersDate_order.Value.ToString("yyyy-MM-dd")}', '{dtpOrdersDate_complete.Value.ToString("yyyy-MM-dd")}')";
                        }

                        else
                        {
                            //logRequest.CommandText = 
                        }

                        break;

                    case 2:
                        if (tfAdd == true)
                        {
                            int indexOfDot_o = cbS_t_oId_o.Text.IndexOf('.');
                            int indexOfDot_s = cbS_t_oId_s.Text.IndexOf('.');
                            string id_o = cbS_t_oId_o.Text.Substring(0, indexOfDot_o);
                            string id_s = cbS_t_oId_s.Text.Substring(0, indexOfDot_s);

                            logRequest.CommandText = $"INSERT INTO Services_to_orders (id_o, id_s) VALUES ({id_o}, {id_s})";
                        }

                        else
                        {
                            //logRequest.CommandText = 
                        }

                        break;

                    case 3:
                        if (tfAdd == true)
                        {
                            logRequest.CommandText = $"INSERT INTO Services (name_s, cost_s) VALUES ('{tbServicesName_s.Text}', {tbServicesCost_s.Text})";
                        }

                        else
                        {
                            //logRequest.CommandText = 
                        }
                        break;
                    case 4:
                        if (tfAdd == true)
                        {
                            int indexOfDot = cbUsersRole_u.Text.IndexOf('.');
                            string role_u = cbUsersRole_u.Text.Substring(0, indexOfDot);

                            logRequest.CommandText = $"INSERT INTO Services (role_u, name_u, surname_u, tel_num_u, pw, listed) VALUES ('{role_u}'," +
                                $" '{tbUsersName_u.Text}', '{tbUsersSurname_u.Text}', '{tbUsersTel_num_u.Text}', '{tbUsersPw.Text}', {(chbUsersListed.Checked ? 1 : 0)})";

                        }
                        else
                        {
                            //logRequest.CommandText =
                        }

                        break;

                    case 5:
                        if (tfAdd == true)
                        {
                            logRequest.CommandText = $"INSERT INTO Services (name_r) VALUES ('{tbRolesName_r.Text}')";
                        }

                        else
                        {
                            //logRequest.CommandText = 
                        }

                        break;
                }
                logRequest.ExecuteNonQuery();
                conn.Close();
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            Form fm = Application.OpenForms["Form2"];
            fm.Show();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }

    public class Categories
    {
        public int id { get; set; }
        public string name { get; set; }

        public Categories(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
