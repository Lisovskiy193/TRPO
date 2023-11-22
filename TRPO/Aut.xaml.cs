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
using System.Drawing;
using System.Data.SqlClient;

namespace TRPO
{
    public partial class Aut : Window
    {
        Main main;
        readonly SqlConnection connection;
        public Aut()//Подключение к БД
        {
            InitializeComponent();
            connection = new SqlConnection(@"Data Source=LIZKIN;Initial Catalog=librarydb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=true;Application Intent=ReadWrite;Multi Subnet Failover=False");
            connection.Open();
            var uri = new Uri("Dictionary1.xaml", UriKind.Relative);
            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)//Проверка данных в текстбоксах по кнопке авторизации
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter($"select Тип, ИмяПользователя from Пользователь where Логин = '{Login.Text}' and Пароль = '{Password.Password}' ", connection);
            sda.Fill(dt);
            if (dt.Rows.Count != 0)
            {
                main = new Main(Convert.ToBoolean(dt.Rows[0][0]), dt.Rows[0][1].ToString(), connection)
                {
                    Owner = this
                };
                main.Show();
                Hide();
                Login.Text = string.Empty;
                Password.Password = string.Empty;
            }
            else MessageBox.Show("Данного пользователя не существует!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Button_Click(object sender, RoutedEventArgs e)//Выход из программы по кнопке
        {
            connection.Close();
            System.Environment.Exit(0);
        }





        private void Login_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsLetterOrDigit(e.Text, 0)) e.Handled = true;
        }

        private void Password_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsLetterOrDigit(e.Text, 0)) e.Handled = true;
        }
    }
}
