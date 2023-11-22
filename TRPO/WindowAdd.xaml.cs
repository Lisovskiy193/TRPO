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
using System.Text.RegularExpressions;

namespace TRPO
{
    public partial class WindowAdd : Window
    {
        readonly SqlConnection connection;
        SqlDataAdapter sda;
        DataTable dt;
        readonly int typedata;
        readonly string namep;
        
        public WindowAdd(int number, SqlConnection connection, string name)//Подключение к бд и инициализация компонетов
        {
            InitializeComponent();
            this.connection = connection;
            namep = name;
            WindowLoad(number);

            typedata = number;
        }

        private void Button_Click(object sender, RoutedEventArgs e)//Переход к гриду
        {

                Owner.Show();
                this.Hide();
           
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)//Добавление внесённой информации с формы в бд
        {
            switch (typedata)
            {
                case 1://Ученик
                    {
                        try
                        { // 33 25 29 44
                            if (!string.IsNullOrWhiteSpace(ФИО.Text) & !string.IsNullOrWhiteSpace(Номер.Text) & !string.IsNullOrWhiteSpace(класс.Text))
                            {
                                Regex reg = new Regex(@"375(33|44|29|25)[0-9]{7}");
                                if (Номер.Text.Length == 12 & reg.IsMatch(Номер.Text))
                                {
                                    if (ФИО.Text.Length <= 50)
                                    {
                                        SqlCommand sql = new SqlCommand($"insert into [Ученик](ИмяУченика, Класс, НомерТелефона, КоличествоПосещений,НесдалКниг) values('{ФИО.Text} ','{класс.Text}', '{Номер.Text}','{0}','{0}')", connection);
                                        var info = sql.ExecuteNonQuery();
                                        if (info != 0)
                                            MessageBox.Show("Ученик успешно добавлен!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                                    }
                                    else
                                        MessageBox.Show("ФИО ученика не должно превышать 50 символов");
                                }
                                else
                                    MessageBox.Show("Номер телефона должен состоять из тренадцати символов и должен начинаться с (375), а так же код оператора должен быть действительным, к примеру (25, 44, 33, 29).", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            else MessageBox.Show("Заполните все поля!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Такой номер уже существует", "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        break;
                    }
                case 2://Книга
                    {
                        try
                        {
                            if (!string.IsNullOrWhiteSpace(НазваниеКниги.Text) &
                                !string.IsNullOrWhiteSpace(ДатаВЫП.Text) &
                                !string.IsNullOrWhiteSpace(Колличество.Text) &
                                !string.IsNullOrEmpty(Жанр_книги.Text)&
                                !string.IsNullOrEmpty(Автор.Text))
                                
                            {
                                if (НазваниеКниги.Text.Length <= 100)
                                {
                                    if (ДатаВЫП.Text.Length == 4 & 1000 < Convert.ToInt32(ДатаВЫП.Text) & DateTime.Now.Year > Convert.ToInt32(ДатаВЫП.Text))
                                    {
                                        if (Колличество.Text.Length <= 3)
                                        {
                                            SqlCommand sql = new SqlCommand($"insert into [Книга](НазваниеКниги, ГодВыпуска, Автор, Жанр, КоличествоКниг, Рейтинг) " +
                                                $"values('{НазваниеКниги.Text}'," +
                                                $"'{ДатаВЫП.Text}'," +
                                                $"'{Автор.Text}'," +
                                                $"'{Жанр_книги.Text}'," +
                                                $"'{Колличество.Text}'," +
                                                 $"'{0}')", connection);
                                            var info = sql.ExecuteNonQuery();
                                            if (info != 0)
                                                MessageBox.Show("Книга успешно добавлена!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                                        }
                                        else
                                            MessageBox.Show("Количество одинаковых книг в библиотеке не должно быть больше 1000");
                                    }
                                    else
                                        MessageBox.Show("Книга должна быть выпущена не раньше 1000г н.э. и не позже текущего года");
                                }
                                else
                                    MessageBox.Show("Название книги не может превышать 100 символов");

                            }
                            else MessageBox.Show("Заполните все поля!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        break;
                    }
                case 3://Запись
                    {
                        try
                        {
                            if(!string.IsNullOrEmpty(КтоБерёт.Text) &
                               !string.IsNullOrEmpty(ЧтоБерёт.Text) &
                               !string.IsNullOrEmpty(КодКниги.Text)) 
                            {
                                if (КодКниги.Text.Length == 8 )
                                {
                                    dt = new DataTable();
                                    DataTable dt1 = new DataTable();
                                    SqlDataAdapter da = new SqlDataAdapter($"select Ученик, Книга from Запись where Ученик ='" + КтоБерёт.Text + "'and Книга ='" + ЧтоБерёт.Text + "' ", connection);
                                    da.Fill(dt);
                                    SqlDataAdapter da1 = new SqlDataAdapter($"select Возврат from Запись where Ученик ='" + КтоБерёт.Text + "'and Книга ='" + ЧтоБерёт.Text + "' ", connection);
                                    da1.Fill(dt1);
                                    bool isfalse = true;
                                    if (dt.Rows.Count > 0)
                                    {
                                        for (int i = 0; i <= dt1.Rows.Count - 1; i++)
                                            if (!Convert.ToBoolean(dt1.Rows[i][0]))
                                            {
                                                isfalse = false;
                                            }
                                        if (isfalse == true)
                                        {
                                            SqlCommand SQL = new SqlCommand($"insert into [Запись](Ученик,Книга,КодКниги,ДатаВыдачи,ДатаВозврата,Возврат,Ответственный) values('{КтоБерёт.Text}','{ЧтоБерёт.Text}','{КодКниги.Text}','{DateTime.Now.ToString("d")}','{DateTime.Now.AddDays(30).ToString("d")}','{0}','{namep}')", connection);
                                            SQL.ExecuteNonQuery();
                                            dt = new DataTable();
                                            sda = new SqlDataAdapter($"select КоличествоКниг from Книга where НазваниеКниги='{ЧтоБерёт.Text}'", connection);
                                            sda.Fill(dt);
                                            if (true)
                                            {
                                                SQL = new SqlCommand($"update Книга set КоличествоКниг='{Convert.ToInt32(dt.Rows[0][0]) - 1}', Рейтинг=Рейтинг+'{1}'  where НазваниеКниги='{ЧтоБерёт.Text}'", connection);
                                                SqlCommand SQL1 = new SqlCommand($"update Ученик set КоличествоПосещений=КоличествоПосещений + '{1}', НесдалКниг=НесдалКниг+ '{1}' where ИмяУченика='{КтоБерёт.Text}'", connection);
                                                SQL1.ExecuteNonQuery();
                                            }
                                            var info = SQL.ExecuteNonQuery();
                                            if (info != 0)
                                                MessageBox.Show("Запись добавлена!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                                            ЧтоБерёт.Items.Clear();
                                            dt = new DataTable();
                                            sda = new SqlDataAdapter("select НазваниеКниги,КоличествоКниг from Книга", connection);
                                            sda.Fill(dt);
                                            for (int i = 0; i < dt.Rows.Count; i++)
                                            {
                                                if (Convert.ToInt32(dt.Rows[i][1]) > 0)
                                                    ЧтоБерёт.Items.Add(dt.Rows[i][0]);
                                            }
                                        }
                                        else
                                            MessageBox.Show("Данным учеником уже была взята такая книга.");
                                    }
                                   
                                    else if (dt.Rows.Count == 0)
                                    {
                                        SqlCommand SQL = new SqlCommand($"insert into [Запись](Ученик,Книга,КодКниги,ДатаВыдачи,ДатаВозврата,Возврат,Ответственный) values('{КтоБерёт.Text}','{ЧтоБерёт.Text}','{КодКниги.Text}','{DateTime.Now.ToString("d")}','{DateTime.Now.AddDays(30).ToString("d")}','{0}','{namep}')", connection);
                                        SQL.ExecuteNonQuery();
                                        dt = new DataTable();
                                        sda = new SqlDataAdapter($"select КоличествоКниг from Книга where НазваниеКниги='{ЧтоБерёт.Text}'", connection);
                                        sda.Fill(dt);
                                        if (true)
                                        {
                                            SQL = new SqlCommand($"update Книга set КоличествоКниг='{Convert.ToInt32(dt.Rows[0][0]) - 1}', Рейтинг=Рейтинг+'{1}' where НазваниеКниги='{ЧтоБерёт.Text}'", connection);
                                            SqlCommand SQL1 = new SqlCommand($"update Ученик set КоличествоПосещений=КоличествоПосещений + '{1}', НесдалКниг=НесдалКниг+ '{1}' where ИмяУченика='{КтоБерёт.Text}'", connection);
                                            SQL1.ExecuteNonQuery();
                                        }
                                        var info = SQL.ExecuteNonQuery();
                                        if (info != 0)
                                            MessageBox.Show("Запись добавлена!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                                        ЧтоБерёт.Items.Clear();
                                        dt = new DataTable();
                                        sda = new SqlDataAdapter("select НазваниеКниги,КоличествоКниг from Книга", connection);
                                        sda.Fill(dt);
                                        for (int i = 0; i < dt.Rows.Count; i++)
                                        {
                                            if (Convert.ToInt32(dt.Rows[i][1]) > 0)
                                                ЧтоБерёт.Items.Add(dt.Rows[i][0]);
                                        }

                                    }
                                }
                                else
                                    MessageBox.Show("Код книги должен состоять из 8-ми символов.");
                            }
                            else MessageBox.Show("Заполните все поля!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        
                        break;
                    }
                case 4://Автор
                    {
                        try
                        {
                            if (!string.IsNullOrWhiteSpace(ФИОАВТОРА.Text) &
                          !string.IsNullOrEmpty(ДатаРождения.Text))
                          
                            {
                                if (ФИОАВТОРА.Text.Length <= 50)
                                {
                                    SqlCommand SQL = new SqlCommand("insert into [Автор](ИмяАвтора,ДатаРождения) values(" +
                                        "'" + ФИОАВТОРА.Text.Trim().ToString() + "'," +
                                        "'" + ДатаРождения.Text.Trim().ToString() + "')", connection);
                                    var info = SQL.ExecuteNonQuery();
                                    if (info != 0)
                                        MessageBox.Show("Автор добавлен!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                else
                                    MessageBox.Show("ФИО автора не должно состоять больше чем из 50 символов");
                            }
                            else MessageBox.Show("Заполните все поля!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        break;
                    }
                case 5://Пользователь
                    {
                        try
                        {
                            if (!string.IsNullOrWhiteSpace(Name.Text) &
                                !string.IsNullOrWhiteSpace(Login.Text) &
                                !string.IsNullOrWhiteSpace(Password.Text) &
                                !string.IsNullOrEmpty(TypePerson.Text) )

                            {
                                if (Login.Text.Length >= 4 & Password.Text.Length >= 4)
                                {
                                    SqlCommand sql = new SqlCommand($"insert into [Пользователь](ИмяПользователя, Логин, Пароль, Тип) " +
                                    $"values('{Name.Text}'," +
                                    $"'{Login.Text}'," +
                                    $"'{Password.Text}'," +
                                    $"'{(TypePerson.Text == "Администратор")}')", connection);
                                    var info = sql.ExecuteNonQuery();
                                    if (info != 0)
                                        MessageBox.Show("Пользователь успешно добавлен!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                else MessageBox.Show("Пароль или логин должены быть не менее 4-ех символов!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            else MessageBox.Show("Заполните все поля!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        break;
                    }
            }
        }
        public void WindowLoad(int number)//Прогрузка форм заполнения в соответствии с гридом
        {
            switch (number)
            {
                case 1://Ученик
                    {
                        AddStudent.Visibility = Visibility.Visible;
                        dt = new DataTable();
                        sda = new SqlDataAdapter("select Класс from Класс", connection);
                        sda.Fill(dt);
                        for (int i = 0; i < dt.Rows.Count; i++)
                            класс.Items.Add(dt.Rows[i][0]);
                        dt = new DataTable();
                        break;
                    }
                case 2://Книга
                    {
                        AddBook.Visibility = Visibility.Visible;
                        dt = new DataTable();
                        sda = new SqlDataAdapter("select НазваниеЖанра from Жанр", connection);
                        sda.Fill(dt);
                        for (int i = 0; i < dt.Rows.Count; i++)
                            Жанр_книги.Items.Add(dt.Rows[i][0]);

                        dt = new DataTable();
                        sda = new SqlDataAdapter("select ИмяАвтора from Автор", connection);
                        sda.Fill(dt);
                        for (int i = 0; i < dt.Rows.Count; i++)
                            Автор.Items.Add(dt.Rows[i][0]);
                        dt = new DataTable();
                        break;
                    }
                case 3://Запись
                    {
                        AddZ.Visibility = Visibility.Visible;
                        dt = new DataTable();
                        sda = new SqlDataAdapter("select ИмяУченика from Ученик", connection);
                        sda.Fill(dt);
                        for (int i = 0; i < dt.Rows.Count; i++)
                                КтоБерёт.Items.Add(dt.Rows[i][0]);
                        dt = new DataTable();
                        sda = new SqlDataAdapter("select НазваниеКниги,КоличествоКниг from Книга", connection);
                        sda.Fill(dt);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if(Convert.ToInt32(dt.Rows[i][1]) > 0)
                            ЧтоБерёт.Items.Add(dt.Rows[i][0]);
                        }
                        break;
                    }
                case 4://Автор
                    {
                        AddAuthor.Visibility = Visibility.Visible;
                        break;
                    }
                case 5://Пользователь
                    {
                        AddPerson.Visibility = Visibility.Visible;
                        break;
                    }
            }
        }
        private void Колличество_PreviewTextInput(object sender, TextCompositionEventArgs e)//Проверка на количество книг
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }

        private void ДатаВЫП_PreviewTextInput(object sender, TextCompositionEventArgs e)//Проверка на год выпуска книги
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }

        private void КодКниги_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsLetterOrDigit(e.Text, 0)) e.Handled = true;
        }

        private void Колличество_TextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }

        private void ДатаВЫП_TextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }

        private void Номер_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }

        private void ФИО_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsLetter(e.Text, 0)) e.Handled = true;
        }

        private void НазваниеКниги_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsLetter(e.Text, 0)) e.Handled = true;
        }

        private void ФИОАВТОРА_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsLetter(e.Text, 0)) e.Handled = true;
        }

        private void Name_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsLetter(e.Text, 0)) e.Handled = true;
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
