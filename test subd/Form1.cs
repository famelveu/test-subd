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
using System.Data.Common;

namespace test_subd
{
    public partial class frmAuthorization : Form
    {

        static SqlConnection connect = new SqlConnection(Properties.Settings.Default.connectionString);


        public frmAuthorization()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnConnect_Click(object sender, EventArgs e) // Войти
        {

            string login = tbLogin.Text.Trim();
            string password = tbPassword.Text.Trim();

            try
            {
                connect.Open();

                // Проверка логина и пароля
                string query = "SELECT COUNT(*) FROM Users WHERE tel_num_u = @Username AND pw = @Password";
                SqlCommand command = new SqlCommand(query, connect);
                command.Parameters.AddWithValue("@Username", login);
                command.Parameters.AddWithValue("@Password", password);

                int count = (int)command.ExecuteScalar();

                if (count > 0)
                {
                    // Получаем роль пользователя
                    string roleCheckQuery = "SELECT role_u FROM Users WHERE tel_num_u = @Username AND pw = @Password";
                    SqlCommand roleCheckCommand = new SqlCommand(roleCheckQuery, connect);
                    roleCheckCommand.Parameters.AddWithValue("@Username", login);
                    roleCheckCommand.Parameters.AddWithValue("@Password", password);

                    // Используем ExecuteScalar для получения единственного значения
                    object roleObj = roleCheckCommand.ExecuteScalar();

                    // Проверяем, что значение роли не равно NULL
                    if (roleObj != DBNull.Value)
                    {
                        // Преобразуем значение роли в int
                        int role = Convert.ToInt32(roleObj);

                        MessageBox.Show("Авторизация успешна!");
                        
                        this.Hide();
                        
                        // Здесь можно перейти к другой форме или выполнить другие действия
                        Form2 fm = new Form2(connect, role);
                        fm.ShowDialog();

                        
                    }
                    else
                    {
                        MessageBox.Show("Роль пользователя не найдена!");
                    }
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
            finally
            {
                connect.Close();
            }


            /* если создана процедура CheckLogin

            SqlCommand command = new SqlCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "CheckLogin";
            command.Parameters.AddWithValue("@user_login", tbLogin.Text);
            command.Parameters.AddWithValue("@user_password", tbPassword.Text); */





        }
    }
}
