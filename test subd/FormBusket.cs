using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test_subd
{
    public partial class FormBusket : Form
    {
        SqlConnection connection;
        List<Good> goods;

        public FormBusket(SqlConnection connect, List<Good> goods)
        {
            connection = connect;
            this.goods = goods;
            InitializeComponent();
        }

        private void FormBusket_Load(object sender, EventArgs e)
        {
            dataGridView1.CellClick += dataGridView1_CellClick;
            //создаем строки циклично
            foreach (Good gd in this.goods)
            {
                int rowNumber = dataGridView1.Rows.Add();
                dataGridView1.Rows[rowNumber].Cells[0].Value = gd.id.ToString();
                dataGridView1.Rows[rowNumber].Cells[1].Value = gd.name.ToString();
                dataGridView1.Rows[rowNumber].Cells[3].Value = gd.price.ToString();
                dataGridView1.Rows[rowNumber].Cells[2].Value = gd.quantity.ToString();
                int sum = gd.price * gd.quantity;
                dataGridView1.Rows[rowNumber].Cells[4].Value = (sum).ToString();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int rowIndex = dataGridView1.CurrentCell.RowIndex;
            dataGridView1.Rows[rowIndex].Cells[2].Value = numericUpDown1.Value;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            if (rowIndex >= 0 && rowIndex < dataGridView1.Rows.Count)
            {
                numericUpDown1.Value = Convert.ToDecimal(dataGridView1.Rows[rowIndex].Cells[2].Value);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataGridViewRow currentRow = dataGridView1.CurrentRow;
            if (currentRow != null)
            {
                int idToDelete = Convert.ToInt32(currentRow.Cells[0].Value);
                dataGridView1.Rows.Remove(currentRow);
                if (idToDelete != 0)
                {
                    foreach (var item in goods.ToList())
                    {
                        if (item.id == idToDelete)
                        {
                            goods.Remove(item);
                            break;
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormClient fm = Application.OpenForms["FormClient"] as FormClient;
            fm.sumOnLabel();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Создание новой строки в таблице Orders
            string insertOrderQuery = "INSERT INTO Orders DEFAULT VALUES; SELECT SCOPE_IDENTITY();";
            SqlCommand insertOrderCommand = new SqlCommand(insertOrderQuery, connection);
            int lastOrderId = Convert.ToInt32(insertOrderCommand.ExecuteScalar());

            // Перебор строк в dataGridView1 для добавления их в таблицу Services_to_orders
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                int quantity = Convert.ToInt32(row.Cells[2].Value);
                int serviceId = Convert.ToInt32(row.Cells[0].Value);

                // Создание новой строки в таблице Services_to_orders
                string insertServiceOrderQuery = "INSERT INTO Services_to_orders (id_o, id_s, quantity) VALUES (@orderId, @serviceId, @quantity);";
                SqlCommand insertServiceOrderCommand = new SqlCommand(insertServiceOrderQuery, connection);
                insertServiceOrderCommand.Parameters.AddWithValue("@orderId", lastOrderId);
                insertServiceOrderCommand.Parameters.AddWithValue("@serviceId", serviceId);
                insertServiceOrderCommand.Parameters.AddWithValue("@quantity", quantity);
                insertServiceOrderCommand.ExecuteNonQuery();
            }

            // Подсчёт итоговой стоимости всех услуг для данного заказа
            decimal totalCost = CalculateTotalCostForOrder(lastOrderId);

            // Обновление столбца price в таблице Orders для последней добавленной строки
            string updateOrderQuery = "UPDATE Orders SET price = @totalCost WHERE id = @orderId;";
            SqlCommand updateOrderCommand = new SqlCommand(updateOrderQuery, connection);
            updateOrderCommand.Parameters.AddWithValue("@totalCost", totalCost);
            updateOrderCommand.Parameters.AddWithValue("@orderId", lastOrderId);
            updateOrderCommand.ExecuteNonQuery();
        }

        private decimal CalculateTotalCostForOrder(int orderId)
        {
            decimal totalCost = 0;

            string selectServicesQuery = "SELECT s.cost_s, sto.quantity FROM Services_to_orders sto JOIN Services s ON sto.id_s = s.id WHERE sto.id_o = @orderId;";
            SqlCommand selectServicesCommand = new SqlCommand(selectServicesQuery, connection);
            selectServicesCommand.Parameters.AddWithValue("@orderId", orderId);
            SqlDataReader reader = selectServicesCommand.ExecuteReader();

            while (reader.Read())
            {
                decimal serviceCost = Convert.ToDecimal(reader["cost_s"]);
                int quantity = Convert.ToInt32(reader["quantity"]);
                totalCost += serviceCost * quantity;
            }

            reader.Close();

            return totalCost;
        }
    }
}
