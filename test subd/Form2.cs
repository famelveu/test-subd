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
            
            //if (roleForm == 2 || roleForm == 3 || roleForm == 4 || roleForm == 6 || roleForm == 7 || roleForm == 10)
            //{
            //    addString.Visible = false;
            //    delString.Visible = false;
            //}   
                SqlConnection sqlConnect = new SqlConnection(Properties.Settings.Default.connectionString); // тут надо вставить переменную  

            sqlConnect.Open();
            SqlCommand logRequst = new SqlCommand();
            logRequst.CommandText = $"SELECT * FROM Users";

            logRequst.Connection = sqlConnect;


            // SqlAdapter - прослойка между источником данных и базой данных
            SqlDataAdapter adapter = new SqlDataAdapter(logRequst);
            DataSet dataSet = new DataSet();
            // заполняем источник данных полученными из адаптера записями
            adapter.Fill(dataSet);
            dataGridView1.DataSource = dataSet.Tables[0];

            label1.Text = Convert.ToString(dataSet.Tables[0].Rows.Count); //считаем строчки

            sqlConnect.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string filterValue = textBox1.Text;
            if (!string.IsNullOrEmpty(filterValue))
            {
                if (int.TryParse(filterValue, out int numericValue))
                {
                    (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"CONVERT([{textBoxColumn.Text}], 'System.Int32') = {numericValue}";
                }
                else
                {
                    (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"[{textBoxColumn.Text}] LIKE '%{filterValue}%'";
                }
            }
            else
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Empty;
            }
            // Оставляем строки, которые удовлетворяют условию

            // диапазон даты

            // пересчитываем количество записей в лейбле
            label1.Text = (dataGridView1.Rows.Count - 1).ToString();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlConnection sqlConnect = new SqlConnection(Properties.Settings.Default.connectionString);
            SqlCommand logRequst = new SqlCommand();
            logRequst.CommandText = $"SELECT * FROM {comboBox1.SelectedItem.ToString()}";

            logRequst.Connection = sqlConnect;


            // SqlAdapter - прослойка между источником данных и базой данных
            if (roleForm == 1 || roleForm == 5 || roleForm == 8 || roleForm == 9)
            {
                SqlDataAdapter adapter = new SqlDataAdapter(logRequst);
                DataSet dataSet = new DataSet();
                // заполняем источник данных полученными из адаптера записями
                adapter.Fill(dataSet);

                dataGridView1.DataSource = dataSet.Tables[0];

                label1.Text = Convert.ToString(dataSet.Tables[0].Rows.Count); //считаем строчки

                sqlConnect.Close();
            }
            else
            {
                if (comboBox1.SelectedItem.ToString() != "Goods" && comboBox1.SelectedItem.ToString() != "TypeGoods" && comboBox1.SelectedItem.ToString() != "FirmsGoods")
                {
                    MessageBox.Show("У вас нет доступа к этой таблице");
                }
                else
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(logRequst);
                    DataSet dataSet = new DataSet();
                    // заполняем источник данных полученными из адаптера записями
                    adapter.Fill(dataSet);

                    dataGridView1.DataSource = dataSet.Tables[0];

                    label1.Text = Convert.ToString(dataSet.Tables[0].Rows.Count); //считаем строчки

                    sqlConnect.Close();
                }
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            Form fm = Application.OpenForms["Form1"];
            fm.ShowDialog();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void addString_Click(object sender, EventArgs e)
        {
            string boxItem = comboBox1.SelectedItem.ToString(); 
            this.Hide();
            //Form fm = Application.OpenForms["Form3"];
            Form3 fm = new Form3(connect, roleForm, boxItem);
            fm.ShowDialog();
        }
    }
    /*
private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
{
(dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"Date > {dateTimePicker1.Value} AND Date < {dateTimePicker2}";
}

private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
{
(dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"Date > {dateTimePicker1.Value} AND Date < {dateTimePicker2}";
}
*/
}