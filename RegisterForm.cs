using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExampleSQLApp
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();

            name.Text = "Введите имя";
            surName.Text = "Введите фамилию";
        }
        #region hover effect and close window
        private void closeButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void closeButton_MouseEnter(object sender, EventArgs e)
        {
            closeButton.ForeColor = Color.Green;
        }
        private void closeButton_MouseLeave(object sender, EventArgs e)
        {
            closeButton.ForeColor = Color.White;
        }
        #endregion
        #region Возможность передвигать окно
        Point lastPoint;
        private void MainPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }
        private void MainPanel_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }
        private void headBlock_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }
        private void headBlock_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }
        #endregion
        #region Плейсхолдеры
        private void firstNameField_Enter(object sender, EventArgs e)
        {
            if (name.Text == "Введите имя")
                name.Text = "";
        }
        private void secondNameField_Enter(object sender, EventArgs e)
        {
            if (surName.Text == "Введите фамилию")
                surName.Text = "";
        }
        private void firstNameField_Leave(object sender, EventArgs e)
        {
            if (name.Text == "")
                name.Text = "Введите имя";
        }
        private void secondNameField_Leave(object sender, EventArgs e)
        {
            if (surName.Text == "")
                surName.Text = "Введите фамилию";
        }
        #endregion
        #region ссылка на окно авторизации
        private void registerLabel_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
        }
        private void registerLabel_MouseEnter(object sender, EventArgs e)
        {
            closeButton.ForeColor = Color.Green;
        }
        private void registerLabel_MouseLeave(object sender, EventArgs e)
        {
            closeButton.ForeColor = Color.White;
        }
        #endregion
        private void buttonRegister_Click(object sender, EventArgs e)
        {
            if (loginField.Text == "Введите имя" || loginField.Text == "")
            {
                MessageBox.Show("Имя обязательно для заполнения");
                return;
            }
            if (name.Text == "")
            {
                MessageBox.Show("Имя обязательно для заполнения");
                return;
            }

            if (isUserExists())
                return;

            DB db = new DB();
            MySqlCommand command = new MySqlCommand("INSERT INTO `users` (`login`, `password`, `name`, `surname`) VALUES (@login, @password, @name, @surname);", db.getConnection());

            command.Parameters.Add("@login", MySqlDbType.VarChar).Value = loginField.Text;
            command.Parameters.Add("@password", MySqlDbType.VarChar).Value = passField.Text;
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = name.Text;
            command.Parameters.Add("@surname", MySqlDbType.VarChar).Value = surName.Text;

            db.OpenConnection();
            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Аккаунт был создан");
            }
            else
            {
                MessageBox.Show("Аккаунт не был создан");
            }
            db.CloseConnection();
        }

        public bool isUserExists()
        {
            DB db = new DB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE `login` = @uL", db.getConnection());
            command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = loginField.Text;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Логин уже имеется");
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
