using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace test_subd
{
    public partial class Form3 : Form
    {
        private SqlConnection connect;
        private int roleForm;
        private string boxItem;

        public Form3(SqlConnection cnct, int rl, string item)
        {
            connect = cnct;
            roleForm = rl;
            boxItem = item;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.connectionString))
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
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            if (connect.State == ConnectionState.Open)
            {
                connect.Close();
            }

            if (boxItem == "Goods")
            {
                textBox1.Visible = true;
                textBox2.Visible = true;
                textBox3.Visible = true;
                textBox4.Visible = true;
                textBox5.Visible = true;
                textBox6.Visible = true;

                comboBox1.Visible = true;

                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = true;
                label4.Visible = true;
                label5.Visible = true;
                label6.Visible = true;
                label7.Visible = true;

                using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.connectionString))
                {
                    conn.Open();
                    SqlCommand logRequest = new SqlCommand("Select fgid, fgfirms from FirmsGoods", conn);

                    using (SqlDataReader rdr = logRequest.ExecuteReader())
                    {
                        List<Categories> lstCategories = new List<Categories>();

                        while (rdr.Read())
                        {
                            lstCategories.Add(new Categories(Convert.ToInt32(rdr["fgid"]), rdr["fgfirms"].ToString()));
                        }

                        comboBox1.DataSource = lstCategories;
                        comboBox1.DisplayMember = "name";
                        comboBox1.ValueMember = "id";
                        comboBox1.Text = "";
                    }
                }
            }
            else if (boxItem == "FirmsGoods")
            {
                label1.Text = "fgid";
                label2.Text = "fgfirms";

                label1.Visible = true;
                label3.Visible = true;

                textBox1.Visible = true;
                textBox2.Visible = true;
            }
            else if (boxItem == "EmployeeDriver")
            {
                label1.Text = "employeeDriverID";
                label2.Text = "employeeDriverFIO";

                label1.Visible = true;
                label3.Visible = true;

                textBox1.Visible = true;
                textBox2.Visible = true;
            }
            else if (boxItem == "Container")
            {
                label1.Text = "containerID";
                label2.Text = "containerType";

                label1.Visible = true;
                label3.Visible = true;

                textBox1.Visible = true;
                textBox2.Visible = true;
            }
            else if (boxItem == "Delivery")
            {

                label1.Text = "deliveryID";
                label2.Text = "orderID";
                label3.Text = "employeesDriverID";
                label4.Text = "transportID";
                label5.Text = "containerID";
                label6.Text = "destination";

                textBox1.Visible = true;
                textBox5.Visible = true;

                comboBox1.Visible = true;
                comboBox2.Visible = true;
                comboBox3.Visible = true;
                comboBox4.Visible = true;

                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = true;
                label4.Visible = true;
                label5.Visible = true;
                label6.Visible = true;

                using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.connectionString))
                {
                    conn.Open();
                    // order
                    SqlCommand orderCommand = new SqlCommand("Select orderid from Orders", conn);

                    SqlDataReader reader1 = orderCommand.ExecuteReader();

                    if (reader1.HasRows)
                    {
                        while (reader1.Read())
                        {
                            comboBox1.Items.Add(Convert.ToInt32(reader1["orderid"]));
                        }
                    }
                    reader1.Close();

                    // driver
                    SqlCommand employeeDriverCommand = new SqlCommand("Select employeeDriverID, employeeDriverFIO from EmployeeDriver", conn);

                    reader1 = employeeDriverCommand.ExecuteReader();
                    List<Categories> lstCategories1 = new List<Categories>();

                    while (reader1.Read())
                    {
                        lstCategories1.Add(new Categories(Convert.ToInt32(reader1["employeeDriverID"]), reader1["employeeDriverFIO"].ToString()));
                    }


                    comboBox2.DataSource = lstCategories1;
                    comboBox2.DisplayMember = "name";
                    comboBox2.ValueMember = "id";
                    comboBox2.Text = "";

                    reader1.Close();

                    // transport
                    SqlCommand transportCommand = new SqlCommand("Select transportID, transport from Transport", conn);

                    reader1 = transportCommand.ExecuteReader();
                    lstCategories1 = new List<Categories>();

                    while (reader1.Read())
                    {
                        lstCategories1.Add(new Categories(Convert.ToInt32(reader1["transportID"]), reader1["transport"].ToString()));
                    }

                    comboBox3.DataSource = lstCategories1;
                    comboBox3.DisplayMember = "name";
                    comboBox3.ValueMember = "id";
                    comboBox3.Text = "";

                    reader1.Close();

                    // container
                    SqlCommand containerCommand = new SqlCommand("Select containerID, containerType from Container", conn);

                    reader1 = containerCommand.ExecuteReader();
                    lstCategories1 = new List<Categories>();

                    while (reader1.Read())
                    {
                        lstCategories1.Add(new Categories(Convert.ToInt32(reader1["containerID"]), reader1["containerType"].ToString()));
                    }

                    comboBox4.DataSource = lstCategories1;
                    comboBox4.DisplayMember = "name";
                    comboBox4.ValueMember = "id";
                    comboBox4.Text = "";

                }

            }
            else if (boxItem == "Orders")
            {
                label1.Text = "orderid";
                label2.Text = "employeeid";
                label3.Text = "customerid";
                label4.Text = "goodid";
                label5.Text = "orderamount";

                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = true;
                label4.Visible = true;
                label5.Visible = true;

                textBox1.Visible = true;
                textBox4.Visible = true;

                comboBox1.Visible = true;
                comboBox2.Visible = true;
                comboBox3.Visible = true;


                // employee
                using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.connectionString))
                {
                    conn.Open();
                    SqlCommand employeeCommand = new SqlCommand("Select userID, userFIO from Users where roles in (1,5,8,9)", conn);

                    SqlDataReader reader1 = employeeCommand.ExecuteReader();

                    List<Categories> lstCategories1 = new List<Categories>();

                    while (reader1.Read())
                    {
                        lstCategories1.Add(new Categories(Convert.ToInt32(reader1["userID"]), reader1["userFIO"].ToString()));
                    }

                    comboBox1.DataSource = lstCategories1;
                    comboBox1.DisplayMember = "name";
                    comboBox1.ValueMember = "id";
                    comboBox1.Text = "";

                    reader1.Close();


                    // customer
                    SqlCommand customerCommand = new SqlCommand("Select userID, userFIO from Users where roles in (2,3,4,6,7,10)", conn);

                    reader1 = customerCommand.ExecuteReader();
                    lstCategories1 = new List<Categories>();

                    while (reader1.Read())
                    {
                        lstCategories1.Add(new Categories(Convert.ToInt32(reader1["userID"]), reader1["userFIO"].ToString()));
                    }

                    comboBox2.DataSource = lstCategories1;
                    comboBox2.DisplayMember = "name";
                    comboBox2.ValueMember = "id";
                    comboBox2.Text = "";

                    reader1.Close();


                    // good
                    SqlCommand goodCommand = new SqlCommand("Select goodID, goodModel from Goods", conn);

                    reader1 = goodCommand.ExecuteReader();
                    lstCategories1 = new List<Categories>();

                    while (reader1.Read())
                    {
                        lstCategories1.Add(new Categories(Convert.ToInt32(reader1["goodID"]), reader1["goodModel"].ToString()));
                    }

                    comboBox3.DataSource = lstCategories1;
                    comboBox3.DisplayMember = "name";
                    comboBox3.ValueMember = "id";
                    comboBox3.Text = "";

                    reader1.Close();
                }
            }
            else if (boxItem == "Roles")
            {
                label1.Text = "RolesID";
                label2.Text = "RolesName";

                label1.Visible = true;
                label3.Visible = true;

                textBox1.Visible = true;
                textBox2.Visible = true;
            }
            else if (boxItem == "Transport")
            {
                label1.Text = "transportID";
                label2.Text = "typeDeliveryID";
                label3.Text = "transport";

                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = true;

                comboBox1.Visible = true;

                textBox1.Visible = true;
                textBox2.Visible = true;

                // type
                using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.connectionString))
                {
                    conn.Open();
                    SqlCommand typeCommand = new SqlCommand("Select typeDeliveryID, typeDeliveryVehicle from Typeofdelivery", conn);

                    SqlDataReader reader1 = typeCommand.ExecuteReader();

                    List<Categories> lstCategories1 = new List<Categories>();

                    while (reader1.Read())
                    {
                        lstCategories1.Add(new Categories(Convert.ToInt32(reader1["typeDeliveryID"]), reader1["typeDeliveryVehicle"].ToString()));
                    }

                    comboBox1.DataSource = lstCategories1;
                    comboBox1.DisplayMember = "name";
                    comboBox1.ValueMember = "id";
                    comboBox1.Text = "";

                    reader1.Close();
                }
            }

            else if (boxItem == "TypeGoods")
            {
                label1.Text = "typeGoodsID";
                label3.Text = "typeGood";

                label1.Visible = true;
                label3.Visible = true;

                textBox1.Visible = true;
                textBox2.Visible = true;
            }
            else if (boxItem == "Typeofdelivery")
            {
                label1.Text = "typeDeliveryID";
                label3.Text = "typeDeliveryVehicle";

                label1.Visible = true;
                label3.Visible = true;

                textBox1.Visible = true;
                textBox2.Visible = true;
            }
            else if (boxItem == "Users")
            {
                label1.Text = "userID";
                label2.Text = "userFIO";
                label3.Text = "roles";
                label4.Text = "loginRoles";
                label5.Text = "passwordRoles";

                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = true;
                label4.Visible = true;
                label5.Visible = true;

                textBox1.Visible = true;
                textBox7.Visible = true;
                textBox3.Visible = true;
                textBox4.Visible = true;

                comboBox2.Visible = true;

                // roles
                using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.connectionString))
                {
                    conn.Open();
                    SqlCommand rolesCommand = new SqlCommand("Select RolesID, RolesName from Roles", conn);

                    SqlDataReader reader1 = rolesCommand.ExecuteReader();

                    List<Categories> lstCategories1 = new List<Categories>();

                    while (reader1.Read())
                    {
                        lstCategories1.Add(new Categories(Convert.ToInt32(reader1["RolesID"]), reader1["RolesName"].ToString()));
                    }

                    comboBox2.DataSource = lstCategories1;
                    comboBox2.DisplayMember = "name";
                    comboBox2.ValueMember = "id";
                    comboBox2.Text = "";

                    reader1.Close();
                }

            }
            else if (boxItem == "Warehouses")
            {
                label1.Text = "warehouseID";
                label3.Text = "warehouseAdres";

                label1.Visible = true;
                label3.Visible = true;

                textBox1.Visible = true;
                textBox2.Visible = true;
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
