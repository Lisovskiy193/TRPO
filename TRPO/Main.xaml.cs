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
using LiveCharts;
using LiveCharts.Wpf;
namespace TRPO
{

    public partial class Main : Window
    {
        DataСhange dataСhange;
        readonly SqlConnection connection;
        readonly Help help = new Help();
        readonly bool ROLE;
        readonly string namep;
        public Main(bool role,string name, SqlConnection connection)//Главная форма
        {
            InitializeComponent();
            ROLE = role;
            namep = name;
            this.connection = connection;
            if (role)
                AddPerson.Visibility = Visibility.Visible;
            LoadChart();
        }

        private void Button_Click(object sender, RoutedEventArgs e)//Переход к таблице авторов
        {

            dataСhange = new DataСhange(4, connection, ROLE, namep)
            {
                Owner = this
            };
            dataСhange.Show();

            this.Hide();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)//Переход к таблице учеников
        {
            dataСhange = new DataСhange(1, connection, ROLE, namep)
            {
                Owner = this
            };
            dataСhange.Show();
            this.Hide();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)//Переход к таблице записей
        {
            dataСhange = new DataСhange(3, connection, ROLE, namep)
            {
                Owner = this
            };
            dataСhange.Show();
            this.Hide();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)//Переход к таблице книг
        {
            dataСhange = new DataСhange(2, connection, ROLE, namep)
            {
                Owner = this
            };
            dataСhange.Show();
            this.Hide();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)//Переход к списку пользователей
        {
            dataСhange = new DataСhange(5, connection, ROLE, namep)
            {
                Owner = this
            };
            dataСhange.Show();
            this.Hide();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)//Переход к окну справки
        {
            help.Owner = this;
            help.Show();
            this.Hide();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)//Выход к окну авторизации
        {
            Owner.Show();
            this.Hide();
        }

        private void PieChart_MouseDoubleClick(object sender, MouseButtonEventArgs e)//Обновление диаграммы по двойнному нажатию
        {
            SqlDataAdapter sda = new SqlDataAdapter("Select НазваниеКниги from Книга order by Рейтинг DESC", connection);
            DataSet ds = new DataSet();
            DataTable dt;
            sda.Fill(ds, "Книга");
            dt = ds.Tables["Книга"];
            SeriesCollection series = new SeriesCollection();
            List<string> name = new List<string>();
            int i = 0;
            foreach (DataRow row in dt.Rows)
            {
                if (i < 10)
                {
                    name.Add(row["НазваниеКниги"].ToString());
                    SqlDataAdapter ddd = new SqlDataAdapter($"Select  Рейтинг from Книга Where НазваниеКниги ='{row["НазваниеКниги"]}'", connection);
                    DataTable dd = new DataTable();
                    ddd.Fill(dd);
                    ChartValues<int> amount = new ChartValues<int>
                    {
                        Convert.ToInt32(dd.Rows[0][0])
                    };
                    PieSeries pie = new PieSeries
                    {
                        Title = name[i],
                        Values = amount
                    };
                    series.Add(pie);
                    i++;
                }
            }
            DP.Series = series;
        }
        private void LoadChart()//Прогрузка диаграммы при переходи на главную форму
        {
            SqlDataAdapter sda = new SqlDataAdapter("Select НазваниеКниги from Книга order by Рейтинг DESC", connection);
            DataSet ds = new DataSet();
            DataTable dt;
            sda.Fill(ds, "Книга");
            dt = ds.Tables["Книга"];
            SeriesCollection series = new SeriesCollection();
            List<string> name = new List<string>();
            int i = 0;
            foreach (DataRow row in dt.Rows)
            {
                if (i < 10)
                {
                    name.Add(row["НазваниеКниги"].ToString());
                    SqlDataAdapter ddd = new SqlDataAdapter($"Select  Рейтинг from Книга Where НазваниеКниги ='{row["НазваниеКниги"]}'", connection);
                    DataTable dd = new DataTable();
                    ddd.Fill(dd);
                    ChartValues<int> amount = new ChartValues<int>
                    {
                        Convert.ToInt32(dd.Rows[0][0])
                    };
                    PieSeries pie = new PieSeries
                    {
                        Title = name[i],
                        Values = amount
                    };
                    series.Add(pie);
                    i++;
                }
            }
            DP.Series = series;
        }

        private void ButtonRateOfBook(object sender, RoutedEventArgs e)// Диаграмма рейтинга книг
        {
            НазваниеДи.Content = "Рейтинг книг";
            SqlDataAdapter sda = new SqlDataAdapter("Select НазваниеКниги from Книга order by Рейтинг DESC", connection);
            DataSet ds = new DataSet();
            DataTable dt;
            sda.Fill(ds, "Книга");
            dt = ds.Tables["Книга"];
            SeriesCollection series = new SeriesCollection();
            List<string> name = new List<string>();
            int i = 0;
            foreach (DataRow row in dt.Rows)
            {
                if (i < 10)
                {
                    name.Add(row["НазваниеКниги"].ToString());
                    SqlDataAdapter ddd = new SqlDataAdapter($"Select  Рейтинг from Книга Where НазваниеКниги ='{row["НазваниеКниги"]}'", connection);
                    DataTable dd = new DataTable();
                    ddd.Fill(dd);
                    ChartValues<int> amount = new ChartValues<int>
                    {
                        Convert.ToInt32(dd.Rows[0][0])
                    };
                    PieSeries pie = new PieSeries
                    {
                        Title = name[i],
                        Values = amount
                    };
                    series.Add(pie);
                    i++;
                }
            }
            DP.Series = series;
        }

        private void ButtonDebt(object sender, RoutedEventArgs e)//Диаграмма должников
        {
            НазваниеДи.Content = "Злостные должники";
            SqlDataAdapter sda = new SqlDataAdapter("Select ИмяУченика from Ученик order by НесдалКниг DESC", connection);
            DataSet ds = new DataSet();
            DataTable dt;
            sda.Fill(ds, "Ученик");
            dt = ds.Tables["Ученик"];
            SeriesCollection series = new SeriesCollection();
            List<string> name = new List<string>();
            int i = 0;
            foreach (DataRow row in dt.Rows)
            {
                if (i<10)
                {
                    name.Add(row["ИмяУченика"].ToString());
                    SqlDataAdapter ddd = new SqlDataAdapter($"Select НесдалКниг from Ученик Where ИмяУченика ='{row["ИмяУченика"]}'", connection);
                    DataTable dd = new DataTable();
                    ddd.Fill(dd);
                    ChartValues<int> amount = new ChartValues<int>
                    {
                        Convert.ToInt32(dd.Rows[0][0])
                    };
                    PieSeries pie = new PieSeries
                    {
                        Title = name[i],
                        Values = amount
                    };
                    series.Add(pie);
                    i++;
                }
            }
            DP.Series = series;
        }

        private void ButtonRateOfStudent(object sender, RoutedEventArgs e)//Диаграмма топа среди учеников
        {
            НазваниеДи.Content = "Топ поситителей библиотеки";
            SqlDataAdapter sda = new SqlDataAdapter("Select ИмяУченика from Ученик order by КоличествоПосещений DESC", connection);
            DataSet ds = new DataSet();
            DataTable dt;
            sda.Fill(ds, "Ученик");
            dt = ds.Tables["Ученик"];
            SeriesCollection series = new SeriesCollection();
            List<string> name = new List<string>();
            int i = 0;
            foreach (DataRow row in dt.Rows)
            {
                if (i < 10)
                {
                    name.Add(row["ИмяУченика"].ToString());
                    SqlDataAdapter ddd = new SqlDataAdapter($"Select КоличествоПосещений from Ученик Where ИмяУченика ='{row["ИмяУченика"]}'", connection);
                    DataTable dd = new DataTable();
                    ddd.Fill(dd);
                    ChartValues<int> amount = new ChartValues<int>
                    {
                        Convert.ToInt32(dd.Rows[0][0])
                    };
                    PieSeries pie = new PieSeries
                    {
                        Title = name[i],
                        Values = amount
                    };
                    series.Add(pie);
                    i++;
                }
            }
            DP.Series = series;
        }
    }
}
