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
    /// Логика взаимодействия для Putevka.xaml
    /// </summary>
    public partial class Putevka : Window
    {
        int Money = 0;
        int ID;
        SqlConnection connection = new SqlConnection("Data Source=WIN-5J64M63P12T\\SQLEXPRESS;Initial Catalog=Sanatoriy;Integrated Security=True");
        public Putevka(int id)
        {

            InitializeComponent();
            connection.Open();
            ID = id;
            for (int i = 1; i < 5; i++)
            {
                Amount.Items.Add(i);
            }

            Type_room.Items.Add("Люкс");
            Type_room.Items.Add("Эконом");
            Type_room.Items.Add("Стандарт");
            Type_room.Items.Add("Премиум");
            Type_room.Items.Add("Депутатский");

            for (int i = 1; i < 5; i++)
            {
                Places.Items.Add(i);
            }

            Type_put.Items.Add("Лечебная");
            Type_put.Items.Add("Развлекательная");
            Type_put.Items.Add("Оздоровительная");
            Type_put.Items.Add("Реабилитационная");
            Type_put.Items.Add("Детская");

            Start.Text = DateTime.Now.ToString();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Room room = new Room((ID));
            room.Show();
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                int SUM = 0;

                SqlDataAdapter sum = new SqlDataAdapter("select Cost from AmountP where amount = '" + Amount.SelectedItem.ToString() + "' ", connection);
                DataTable AmountP = new DataTable();
                sum.Fill(AmountP);
                SUM += (int)AmountP.Rows[0][0];

                sum = new SqlDataAdapter("select Cost from Typeroom where Name_ = '" + Type_room.SelectedItem.ToString() + "' ", connection);
                sum.Fill(AmountP);
                SUM += (int)AmountP.Rows[0][0];

                sum = new SqlDataAdapter("select Cost from Typeplaces where amount = '" + Places.SelectedItem.ToString() + "' ", connection);
                sum.Fill(AmountP);
                SUM += (int)AmountP.Rows[0][0];

                sum = new SqlDataAdapter("select Cost from Typeput where Name_ = '" + Type_put.SelectedItem.ToString() + "' ", connection);
                sum.Fill(AmountP);
                SUM += (int)AmountP.Rows[0][0];

                string[] arr = Start.Text.Split('.');
                string[] arr1 = End.Text.Split('.');
                int start = 0, end = 0;
                start = Convert.ToInt32(arr[0]);
                end = Convert.ToInt32(arr1[0]);
                end -= start;
                Cost.Content = "Стоимость: " + (end * SUM);
                Money = (end * SUM);
            }
            catch (Exception Exc)
            {

                MessageBox.Show(Exc.Message);
            }
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
             try
            {
                if (!string.IsNullOrWhiteSpace(Start.Text) || !string.IsNullOrWhiteSpace(End.Text) ||
                    !string.IsNullOrWhiteSpace(Amount.Text) || !string.IsNullOrWhiteSpace(Type_put.Text)
                     || !string.IsNullOrWhiteSpace(Type_room.Text) || !string.IsNullOrWhiteSpace(Places.Text))
                {
                    SqlCommand add = new SqlCommand("insert Putevka (IDPerson,TRName_,AmountP,AmountPl,TPName_,Cost) values('" + ID + "','" + Type_room.Text 
                        + "','"+Amount.Text+"','"+Places.Text+"','"+Type_put.Text+"', '"+Money+"')", connection);
                    add.ExecuteNonQuery();
                  
                        MessageBox.Show("Путёвка оформлена");

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
