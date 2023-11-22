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
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;

namespace TRPO
{
    public partial class DataСhange : Window
    {
        readonly SqlConnection connection;
        SqlDataAdapter sda;
        DataTable dt = new DataTable();
        WindowAdd windowAdd;
        readonly int typedata;
        readonly bool ROLE;
        readonly string namep;

        public DataСhange(int number, SqlConnection connection, bool role,string name)// Заполнение грида информацией
        {
            InitializeComponent();
            this.connection = connection;
            ROLE = role;
            namep = name;
            DownloadInformation(number);
            typedata = number;


        }

        //Основные методы
        private void DownloadInformation(int number)//Именование, видимость кнопок и лейблов в соответствии с выбранной таблицой
        {
            switch (number)
            {
                case 1://Ученик
                    {
                        Change.Visibility = Visibility.Hidden;
                        labelName.Content = "Ученики";
                        labelList.Content = "Список учеников";
                        Addbutton.Content = "Добавить ученика";
                        Delbutton.Content = "Удалить ученика";
                        Ticket.Visibility = Visibility.Visible;

                        if (ROLE == false)
                        {
                            Delbutton.Visibility = Visibility.Hidden;
                        }
                        UpdateGrid("Ученик");

                        if (ROLE == true)
                        {
                            for (int i = 0; i <= dt.Columns.Count - 1; i++)
                                Список.Items.Add(dt.Columns[i].ToString());
                        }
                        else
                            for (int i = 1; i <= dt.Columns.Count - 1; i++)
                                Список.Items.Add(dt.Columns[i].ToString());

                        break;
                    }
                case 2://Книга
                    {
                        Change.Visibility = Visibility.Hidden;
                        labelName.Content = "Книга";
                        labelList.Content = "Список Книг";
                        Addbutton.Content = "Добавить Книгу";
                        Delbutton.Content = "Удалить Книгу";
                        Добавление.Visibility = Visibility.Visible;
                        КоличествоД.Visibility = Visibility.Visible;
                        Ticket.Visibility = Visibility.Hidden;
                        if (ROLE == false)
                        {
                            Delbutton.Visibility = Visibility.Hidden;
                        }
                        UpdateGrid("Книга");
                        for (int i = 0; i <= dt.Columns.Count - 1; i++)
                            Список.Items.Add(dt.Columns[i].ToString());
                        break;
                    }
                case 3://Запись
                    {
                        labelName.Content = "Записи";
                        labelList.Content = "Записи";
                        Addbutton.Content = "Добавить запись";
                        Delbutton.Content = "Удалить запись";
                        Change.Visibility = Visibility.Visible;
                        Ticket.Visibility = Visibility.Hidden;
                        if (ROLE == true)
                        {
                            To.Visibility = Visibility.Visible;
                            From.Visibility = Visibility.Visible;
                            Create.Visibility = Visibility.Visible;
                        }
                        if (ROLE == false)
                        {
                            Delbutton.Visibility = Visibility.Hidden;
                        }
                        UpdateGrid("Запись");
                        for (int i = 0; i <= dt.Columns.Count - 1; i++)
                            Список.Items.Add(dt.Columns[i].ToString());
                        break;
                    }
                case 4://Автор
                    {
                        Change.Visibility = Visibility.Hidden;
                        labelName.Content = "Авторы";
                        labelList.Content = "Список Авторов";
                        Addbutton.Content = "Добавить Автора";
                        Delbutton.Content = "Удалить Автора";
                        Ticket.Visibility = Visibility.Hidden;

                        UpdateGrid("Автор");
                        for (int i = 0; i <= dt.Columns.Count - 1; i++)
                            Список.Items.Add(dt.Columns[i].ToString());
                        break;
                    }
                case 5://Пользователь
                    {
                        Change.Visibility = Visibility.Hidden;
                        labelName.Content = "Пользователи";
                        labelList.Content = "Список пользователей";
                        Addbutton.Content = "Добавить пользователя";
                        Delbutton.Content = "Удалить пользователя";
                        Ticket.Visibility = Visibility.Hidden;

                        UpdateGrid("Пользователь");
                        for (int i = 0; i <= dt.Columns.Count - 1; i++)
                            Список.Items.Add(dt.Columns[i].ToString());
                        break;
                    }
            }
        }
        private void Grid_Loaded(object sender, RoutedEventArgs e)//Скрытие поля в гриде если зашёл не администратор
        {
            if (ROLE == false)
                List.Columns[0].Visibility = Visibility.Hidden;
        }
        private void UpdateGrid(string nameobject)//
        {

            dt = new DataTable();
            sda = new SqlDataAdapter($"select * from {nameobject}", connection);
            sda.Fill(dt);
            List.ItemsSource = dt.AsDataView();

        }
        private void Button_Click(object sender, RoutedEventArgs e)//Переход на окно добавления по начатию на кнопку
        {
            windowAdd = new WindowAdd(typedata, connection, namep)
            {
                Owner = this
            };
            windowAdd.Show();
            Hide();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)//Удаление выделенной записи по кнопке
        {
            string name = "";
            string firstCellInSql = "";
            string secondCellInSql = "";
            if (typedata == 1)
            {
                name = "Ученик";
                firstCellInSql = "ИмяУченика";
            }
            else if (typedata == 2)
            {
                name = "Книга";
                firstCellInSql = "НазваниеКниги";
            }
            else if (typedata == 3)
            {
                name = "Запись";
                firstCellInSql = "Ученик";
                secondCellInSql = "Книга";
            }
            else if (typedata == 4)
            {
                name = "Автор";
                firstCellInSql = "ИмяАвтора";
            }
            else if (typedata == 5)
            {
                name = "Пользователь";
                firstCellInSql = "ИмяПользователя";
            }

            if (List.SelectedItem != null)
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter sd;
                    var conv = new DataGridCellInfo(List.SelectedItem, List.Columns[1]);
                    var div = conv.Column.GetCellContent(conv.Item) as TextBlock;
                    if (typedata == 3)
                    {
                        var conv2 = new DataGridCellInfo(List.SelectedItem, List.Columns[2]);
                        var div2 = conv2.Column.GetCellContent(conv2.Item) as TextBlock;

                        sd = new SqlDataAdapter($"select ИД from {name} where "+ firstCellInSql + " = '" + div.Text + "' and " + secondCellInSql + " = '" + div2.Text + "'", connection);
                        var conv3 = new DataGridCellInfo(List.SelectedItem, List.Columns[6]);

                        var div3 = conv3.Column.GetCellContent(conv3.Item) as CheckBox;
                        if (div3.IsChecked == false)
                        {
                            MessageBox.Show("Чтоб удалить запись, книга должна быть возвращена");
                            return;
                        }
                    }

                    else sd = new SqlDataAdapter($"select ИД from {name} where "+firstCellInSql+"= '" + div.Text + "'", connection);
                    sd.Fill(dt);
                    int id = Convert.ToInt32(dt.Rows[0][0]);
                    SqlCommand sda = new SqlCommand($"delete from {name} where ИД='" + id + "' ", connection);
                    if (sda.ExecuteNonQuery() != 0)
                        MessageBox.Show($"{name} удален(а)!");
                    else MessageBox.Show($"Строка из таблицы {name} не удалена!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                UpdateGrid(name);
                List.Columns[0].Visibility = Visibility.Visible;
            }
            else MessageBox.Show("Введите идентификатор в поле!");

        }
        private void Button_Click_2(object sender, RoutedEventArgs e)//Кнопка возврата на главную форму
        {
            Owner.Show();
            this.Hide();
        }
        private void Find_Click(object sender, RoutedEventArgs e)//Кнопка поиска по выбранному критерию
        {
            string name = "";
            if (typedata == 1)
            {
                name = "Ученик";
            }
            else if (typedata == 2)
            {
                name = "Книга";
            }
            else if (typedata == 3)
            {
                name = "КодКниги";
            }
            else if (typedata == 4)
            {
                name = "Запись";
            }
            else if (typedata == 5)
            {
                name = "Автор";
            }
            else if (typedata == 6)
            {
                name = "Пользователь";
            }

            if (Список.SelectedItem != null & (СтрокаП.Text != ""))
            {
                dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter($"select * from {name} where {Список.SelectedItem} like '%{СтрокаП.Text}%'", connection);
                da.Fill(dt);
                List.ItemsSource = dt.AsDataView();
            }
            if (ROLE == false)
            {
                List.Columns[0].Visibility = Visibility.Hidden;
            }
        }
        private void UpdateBtn_Click(object sender, RoutedEventArgs e)//Кнопка обновления таблицы
        {

            switch (typedata)
            {
                case 1://Ученик
                    {
                        UpdateGrid("Ученик");
                        if (ROLE == false)
                            List.Columns[0].Visibility = Visibility.Hidden;
                        break;
                    }
                case 2://Книга
                    {
                        UpdateGrid("Книга");
                        if (ROLE == false)
                            List.Columns[0].Visibility = Visibility.Hidden;
                        break;
                    }
                case 3://Запись
                    {
                        UpdateGrid("Запись");
                        if (ROLE == false)
                            List.Columns[0].Visibility = Visibility.Hidden;
                        break;
                    }
                case 4://Автор
                    {
                        UpdateGrid("Автор");
                        if (ROLE == false)
                            List.Columns[0].Visibility = Visibility.Hidden;
                        break;
                    }
                case 5://Автор
                    {
                        UpdateGrid("Пользователь");
                        if (ROLE == false)
                            List.Columns[0].Visibility = Visibility.Hidden;
                        break;
                    }

            }
        }
        private void Add_Click(object sender, RoutedEventArgs e)//Кнопка добавления количества книг
        {
            try
            {
                var conv = new DataGridCellInfo(List.SelectedItem, List.Columns[0]);
                var div = conv.Column.GetCellContent(conv.Item) as TextBlock;
                int id = Convert.ToInt32(div.Text);
                if (Convert.ToInt32(КоличествоД.Text) > 0 & КоличествоД.Text.Length <= 3 )
                {
                    SqlCommand sqlCommand = new SqlCommand($"update Книга set КоличествоКниг=КоличествоКниг +'{Convert.ToInt32(КоличествоД.Text)}' where ИД='{id}'", connection);
                    sqlCommand.ExecuteNonQuery();
                    UpdateGrid("Книга");
                }
                else
                    MessageBox.Show("Добавляемое количество не может быть отрицательным, а так же не может быть больше 999");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void ChangeClick(object sender, RoutedEventArgs e)//Кнопка сохранения изменений в гриде
        {
            try
            {
                DataTable dt = new DataTable();
                DataRowView row = (DataRowView)List.SelectedItems[0];
                SqlCommand SQL = new SqlCommand($"Update Запись set Ученик='{row[1]}',Книга='{row[2]}',КодКниги={row[3]},ДатаВыдачи='{row[4]}',ДатаВозврата='{row[5]}',Возврат='{row[6]}',Ответственный='{row[7]}' where ИД='{row[0]}'", connection);
                SqlDataAdapter tr = new SqlDataAdapter($"Select Возврат From Запись where Ученик='{row[1]}' and Книга ='{row[2]}'", connection);
                SQL.ExecuteNonQuery();
                tr.Fill(dt);
                if (Convert.ToBoolean(dt.Rows[0][0]) == true)
                {
                    SQL = new SqlCommand($"update Книга set КоличествоКниг=КоличествоКниг + 1 where НазваниеКниги='{row[2]}'", connection);
                    SqlCommand SQL1 = new SqlCommand($"update Запись set ДатаВозврата='{DateTime.Now.ToString("d")}'where Ученик='{row[1]}'and Книга='{row[2]}' and ДатаВозврата='{row[5]}' ", connection);
                    SqlCommand SQL2 = new SqlCommand($"update Ученик set НесдалКниг= НесдалКниг- '{1}' where ИмяУченика='{row[1]}'", connection);
                    SQL.ExecuteNonQuery();
                    SQL1.ExecuteNonQuery();
                    SQL2.ExecuteNonQuery();
                }
                else
                {
                    SQL = new SqlCommand($"update Книга set КоличествоКниг=КоличествоКниг - 1 where НазваниеКниги='{row[2]}' ", connection);
                    SqlCommand SQL1 = new SqlCommand($"update Ученик set НесдалКниг= НесдалКниг + '{1}' where ИмяУченика='{row[1]}'", connection);
                    SqlCommand SQL2 = new SqlCommand($"update Запись set ДатаВозврата= '{Convert.ToDateTime(row[4]).AddDays(30)}' where Ученик='{row[1]}' and ИД='{row[0]}' and Книга='{row[2]}'", connection);
                    SQL.ExecuteNonQuery();
                    SQL1.ExecuteNonQuery();
                    SQL2.ExecuteNonQuery();
                }
                MessageBox.Show("Данные таблицы обнавлены");
            }
            catch (Exception)
            { MessageBox.Show("Вы не выбрали запись."); }
        }
        //Конец Основные Методы

        private void ReportButton(object sender, RoutedEventArgs e)//ОтчетExcel
        {
            try
            {
                if (From.SelectedDate < To.SelectedDate)
                {
                    Excel.Application excelApp = new Excel.Application();
                    excelApp.Workbooks.Add();
                    Excel._Worksheet workSheet = (Excel.Worksheet)excelApp.ActiveSheet;
                    dt = new DataTable();
                    sda = new SqlDataAdapter($"select Ученик, Книга, КодКниги, ДатаВыдачи, ДатаВозврата, Возврат, Ответственный from Запись WHERE ДатаВозврата BETWEEN '"+ From.SelectedDate + "' AND '"+ To.SelectedDate+ "'", connection);
                    sda.Fill(dt);

                    string[] letters = { "A", "B", "C", "D", "E", "F", "G", "H" };

                    workSheet.Cells[2, "C"] = "Отчёт о Выданных книгах";
                    excelApp.get_Range("A2:F2", Type.Missing).Merge(Type.Missing);
                    workSheet.Cells[4, "E"] = "Библиотека №__________";
                    workSheet.Cells[5, "E"] = "Отчёт №__________";

                    workSheet.Cells[7, "A"] = "Ученик";
                    workSheet.Cells[7, "B"] = "Книга";
                    workSheet.Cells[7, "C"] = "Код книги";
                    workSheet.Cells[7, "D"] = "Дата выдачи";
                    workSheet.Cells[7, "E"] = "Дата Возврата";
                    workSheet.Cells[7, "F"] = "Возврат";
                    workSheet.Cells[7, "G"] = "Ответственный";

                    int count = 0;

                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        for (int j = 0; j <= 6; j++)
                        {

                            if (j == 5)
                            {
                                if (Convert.ToBoolean(dt.Rows[i][j]) == false)
                                {
                                    count++;
                                    workSheet.Cells[i + 8, letters[j]] = "Не сданна";
                                }
                                else
                                    workSheet.Cells[i + 8, letters[j]] = "Сданна";
                            }
                            else
                            {
                                workSheet.Cells[i + 8, letters[j]] = dt.Rows[i][j].ToString();
                            }
                        }
                         
                    }

                    workSheet.Cells[dt.Rows.Count + 1 + 8, "F"] = "Должны вернуть:" + count;

                    workSheet.Cells[dt.Rows.Count + 5 + 6, "A"] = $"Ответственный: {namep}";
                    excelApp.get_Range("A"+ (dt.Rows.Count + 5 + 8) + ":D" + (dt.Rows.Count + 5 + 8) + "", Type.Missing).Merge(Type.Missing);
                   
                    workSheet.Cells[dt.Rows.Count + 5 + 6, "E"] = "Подпись,инициалы_________________";
                    excelApp.get_Range("E" + (dt.Rows.Count + 5 + 8) + ":F" + (dt.Rows.Count + 5 + 8) + "", Type.Missing).Merge(Type.Missing);
                    
                    workSheet.Cells[dt.Rows.Count + 6 + 6, "A"] = $"Дата отчёта: {DateTime.Now.ToShortDateString()}г.";
                    excelApp.get_Range("A" + (dt.Rows.Count + 6 + 8) + ":C" + (dt.Rows.Count + 6 + 8) + "", Type.Missing).Merge(Type.Missing);
                 
                    excelApp.get_Range("A" + (dt.Rows.Count + 7 + 8) + ":C" + (dt.Rows.Count + 7 + 8) + "", Type.Missing).Merge(Type.Missing);
                    workSheet.Cells[dt.Rows.Count + 7 + 6, "E"] = "МП";
                   

                    Excel.Range range1 = workSheet.get_Range("A2", "F2");
                    range1.Font.Size = 14;

                    Excel.Range range2 = workSheet.get_Range("A1", "I100");
                    range2.Font.FontStyle = "Times New Roman";

                    Excel.Range range4 = workSheet.get_Range("A7", "G7");
                    range4.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                    range4.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                    range4.Borders.get_Item(Excel.XlBordersIndex.xlInsideHorizontal).LineStyle = Excel.XlLineStyle.xlContinuous;
                    range4.Borders.get_Item(Excel.XlBordersIndex.xlInsideVertical).LineStyle = Excel.XlLineStyle.xlContinuous;
                    range4.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                    range4.Font.Bold = true;

                    Excel.Range range3 = workSheet.get_Range("A8", "G" + (dt.Rows.Count + 7) + "");
                    range3.BorderAround2();

                    workSheet.get_Range("A2", "D2").Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    workSheet.get_Range("A7", "G7").Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    workSheet.Columns.EntireColumn.AutoFit();
                    workSheet.Rows.EntireColumn.AutoFit();
                    excelApp.Workbooks.Close();
                    MessageBox.Show("Отчет сформирован!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

                }
                else
                    MessageBox.Show("Отчёт не может быть составлен От большей даты к меньшей");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (List.SelectedItem != null)
                {
                    Word._Application word_app = new Word.Application();
                    word_app.Visible = true;
                    object missing = Type.Missing;
                    Word._Document word_doc = word_app.Documents.Add(ref missing, ref missing, ref missing, ref missing);

                    Word.Paragraph para = word_doc.Paragraphs.Add(ref missing);
                    
                        var conv = new DataGridCellInfo(List.SelectedItem, List.Columns[3]);
                        var num = conv.Column.GetCellContent(conv.Item) as TextBlock;
                        var conv1 = new DataGridCellInfo(List.SelectedItem, List.Columns[1]);
                        var fio = conv1.Column.GetCellContent(conv1.Item) as TextBlock;
                        var conv2 = new DataGridCellInfo(List.SelectedItem, List.Columns[2]);
                        var clas = conv2.Column.GetCellContent(conv2.Item) as TextBlock;
                        string old_font = para.Range.Font.Name;

                        para.Range.Text = "Средняя школа №_____"; para.Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight; para.Range.Font.Name = "Times New Roman";
                        para.Range.Font.Size = 16;
                        para.Range.InsertParagraphAfter();
                        para.Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter; para.Range.Font.Size = 16; para.Range.Font.Name = "Times New Roman";
                        para.Range.Text = "Читательский билет №__________";
                        para.Range.InsertParagraphAfter();
                        para.Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft; para.Range.Font.Name = "Times New Roman"; para.Range.Font.Size = 12;
                        para.Range.Text = $"ФИО: {fio.Text}";
                        para.Range.InsertParagraphAfter();
                        para.Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft; para.Range.Text = $"Номер телефона: {num.Text}";
                        para.Range.InsertParagraphAfter();
                        para.Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft; para.Range.Text = $"Класс: {clas.Text}";
                        para.Range.InsertParagraphAfter(); para.Range.Text = $"Подпись ученика_____________";
                        para.Range.InsertParagraphAfter(); para.Range.Text = $"Выдал: { namep}";
                        para.Range.InsertParagraphAfter(); para.Range.Text = $"Дата выдачи: {DateTime.Now.ToShortDateString()}г.                                                          Подпись_______________";

                    object save_changes = false;

                    word_doc.Close(ref save_changes, ref missing, ref missing);

                    word_app.Quit(ref save_changes, ref missing, ref missing);
                }
                else
                {
                    MessageBox.Show("Вы не выбрали строку");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Вы не сохранили документ");
            }
        }

        private void СтрокаП_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsLetterOrDigit(e.Text, 0)) e.Handled = true;
        }

        private void КоличествоД_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }
    }
}

        //ОтчетExcel

    



