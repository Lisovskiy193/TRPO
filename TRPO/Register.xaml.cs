using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;

namespace TRPO
{
    /// <summary>
    /// Логика взаимодействия для Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        SqlConnection connection = new SqlConnection("Data Source=WIN-5J64M63P12T\\SQLEXPRESS;Initial Catalog=Sanatoriy;Integrated Security=True");
        public Register()
        {
            InitializeComponent();
            connection.Open();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
                MainWindow mainwindow = new MainWindow();
                mainwindow.Show();
                Close();
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Email.Text)|| !string.IsNullOrWhiteSpace(Password.Text))
                {
                    SqlCommand add = new SqlCommand("insert Person (Email,Password_) values('" + Email.Text + "','" + Password.Text + "')", connection);
                    add.ExecuteNonQuery();
                    if (add.ExecuteNonQuery() == -1)
                    {
                        MessageBox.Show("Аккаунт создан");
                    }
                    else
                    {
                        MessageBox.Show("Аккаунт не создан");
                    }
                }
                else
                {
                    MessageBox.Show("Не заполнено одно из полей");
                }
            }
            catch (Exception Exc)
            {
                MessageBox.Show(Exc.Message);
            }
            
            
        }
    }
}
