﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;

namespace test_subd
{
    public partial class FormClient : Form
    {
        SqlConnection connect;
        //cписок объектов, де-факто Корзина
        List<Good> listGoods = new List<Good>();

        public FormClient(SqlConnection cnct)
        {
            connect = cnct;
            InitializeComponent();
        }

        private void FormClient_Load(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.CellClick += dataGridView1_CellClick;
                dataGridView1.AllowUserToAddRows = false;

                SqlConnection sqlConnect = new SqlConnection(Properties.Settings.Default.connectionString);

                sqlConnect.Open();
                SqlCommand showTable = new SqlCommand();
                showTable.CommandText = $"SELECT * FROM Услуги";
                showTable.Connection = sqlConnect;

                // SqlAdapter - прослойка между источником данных и базой данных
                SqlDataAdapter adapter = new SqlDataAdapter(showTable);
                DataSet dataSet = new DataSet();
                // заполняем источник данных полученными из адаптера записями
                adapter.Fill(dataSet);
                dataGridView1.DataSource = dataSet.Tables[0];
                dataGridView1.Columns[0].Visible = false;
                sqlConnect.Close();
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                sqlConnect.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        public void sumOnLabel()
        {
            label2.Text = listGoods.Count.ToString();
            int sum = 0;
            foreach (Good gd in listGoods)
            {
                sum += gd.price * gd.quantity;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
            string name = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            int price = Convert.ToInt32(dataGridView1.CurrentRow.Cells[2].Value);

            bool inBasket = false;
            //поиск товара в корзине
            foreach (Good gd in listGoods)
            {
                if (gd.name == name)
                {
                    inBasket = true;
                    MessageBox.Show("Товар уже в корзине. Изменить количество можно в корзине");
                }
            }
            //если так и не нашли
            if (!inBasket)
            {
                listGoods.Add(new Good(id, name, price, 1));
                sumOnLabel();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listGoods.Count > 0)
            {
                FormBusket fmBusket = new FormBusket(connect, listGoods);
                fmBusket.ShowDialog();
            }
            else
            {
                MessageBox.Show("В корзину чета добавь");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow selectedRow = dataGridView1.CurrentRow;

            // Получаем значения из нужных столбцов по индексу столбца
            string id = selectedRow.Cells[0].Value.ToString();

            // Запрос к базе данных для получения изображения по ID
            string query = "SELECT pic_s FROM Services WHERE id = @ID";

            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ID", id);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        // Получите данные изображения из базы данных
                        byte[] imageData = (byte[])reader["pic_s"];

                        // Отобразите изображение в PictureBox
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            // Устанавливаем масштабирование по размеру PictureBox
                            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox1.Image = Image.FromStream(ms);
                        }
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }
    }
}
