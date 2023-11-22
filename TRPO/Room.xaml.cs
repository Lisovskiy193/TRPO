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
    /// Логика взаимодействия для Room.xaml
    /// </summary>
    public partial class Room : Window
    {
        SqlConnection connection = new SqlConnection("Data Source=WIN-5J64M63P12T\\SQLEXPRESS;Initial Catalog=Sanatoriy;Integrated Security=True");
        int ID;
        public Room(int id)
        {
            InitializeComponent();
            connection.Open();
            ID = id;
            SqlDataAdapter l = new SqlDataAdapter("select * from Putevka where IDPerson = '"+ID+"' ",connection);
            DataTable AmountP = new DataTable();
            l.Fill(AmountP);
            List.ItemsSource = AmountP.AsDataView();


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Putevka putevka = new Putevka(ID);
            putevka.Show();
            Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SqlCommand del = new SqlCommand("delete from Putevka where ID = '"+Del.Text+"'",connection);
            del.ExecuteNonQuery();
            MessageBox.Show("Путёвка удалена");
            SqlDataAdapter l = new SqlDataAdapter("select * from Putevka where IDPerson = '" + ID + "' ", connection);
            DataTable AmountP = new DataTable();
            l.Fill(AmountP);
            List.ItemsSource = AmountP.AsDataView();
        }
    }
}
