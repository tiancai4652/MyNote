using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void Button_Click(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
           
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //AddInlineUIContainer();
            var result= IsNewLine();
            if (result)
            {
                richTextBox.Focus();
                richTextBox.CaretPosition = richTextBox.CaretPosition.InsertParagraphBreak();
                var p = richTextBox.CaretPosition.Paragraph;
            }
            MessageBox.Show(result.ToString());

        }

        bool IsNewLine(bool isIgnoreSpace=false)
        {
            var p = richTextBox.Selection.Start;
            var selectP = richTextBox.CaretPosition;
            var isSame = p.Equals(selectP);
            //判断有没有选中内容
            if (isSame)
            {
                if (p.Paragraph == null)
                {
                    return true;
                }
                if (p.Paragraph.Inlines != null && p.Paragraph.Inlines.Count == 1)
                {
                    var first = p.Paragraph.Inlines.First();
                    if (first is Run)
                    {
                        Run run = (Run)first;
                        if (isIgnoreSpace)
                        {
                            return string.IsNullOrEmpty(run.Text.Trim());
                        }
                        else
                        {
                            return string.IsNullOrEmpty(run.Text);
                        }
                    }
                }
            }
            return false;
        }

        Paragraph GetInsertPositionParagraph()
        {
            richTextBox.Focus();
            richTextBox.CaretPosition = richTextBox.CaretPosition.InsertParagraphBreak();
            var p = richTextBox.CaretPosition.Paragraph;
            return p;
        }

        void AddInlineUIContainer()
        {
            Paragraph paragraph = new Paragraph();
            InlineUIContainer inlineUIContainer = new InlineUIContainer();
            Image image = new Image();
            image.HorizontalAlignment = HorizontalAlignment.Left;
            image.Stretch = Stretch.Fill;
            image.Source = img.Source;
            var size = MeasureString();
            image.Width = size.Height;
            image.Height = size.Height;
            inlineUIContainer.Child = image;
            paragraph.Inlines.Add("Text to precede the button...");
            paragraph.Inlines.Add(inlineUIContainer);
            paragraph.Inlines.LastInline.BaselineAlignment = BaselineAlignment.Bottom;
            paragraph.Inlines.Add(" Text to follow the button...");

            myFlowDocument.Blocks.Add(paragraph);
        }

        private Size MeasureString(string candidate="123ABC")
        {
            var formattedText = new FormattedText(
                candidate,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(this.myFlowDocument.FontFamily, this.myFlowDocument.FontStyle, this.myFlowDocument.FontWeight, this.myFlowDocument.FontStretch),
                this.myFlowDocument.FontSize,
                Brushes.Black,
                new NumberSubstitution(),
                1);

            return new Size(formattedText.Width, formattedText.Height);
        }
    }
}
