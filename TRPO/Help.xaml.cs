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

namespace TRPO
{
    /// <summary>
    /// Логика взаимодействия для Help.xaml
    /// </summary>
    public partial class Help : Window
    {
        public Help()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var doc = Reader.Document;
            var text = doc.ContentStart;
            var docRange = new TextRange(doc.ContentStart, doc.ContentEnd);
            docRange.ClearAllProperties();
            string search = textsearch.Text;
            while (true)
            {
                var next = text.GetNextContextPosition(LogicalDirection.Forward);
                if (next == null)
                {
                    break;
                }

                var txt = new TextRange(text, next);

                int indx = txt.Text.IndexOf(search);
                if (indx > 0)
                {
                    try
                    {
                        var sta = text.GetPositionAtOffset(indx);
                        var end = text.GetPositionAtOffset(indx + search.Length);
                        var textR = new TextRange(sta, end);

                        textR.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(Colors.Red));

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
                text = next;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Owner.Show();
            Hide();
        }

    }
}
