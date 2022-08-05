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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyNote
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitEvent();
        }

        void InitEvent()
        {
            window.MouseDown += window_MouseDown;
            window.MouseUp += window_MouseUp;
            window.PreviewMouseDown += window_PreviewMouseDown;
            window.PreviewMouseUp += window_PreviewMouseUp;
            titleGrid.PreviewMouseLeftButtonDown += TitleGrid_MouseLeftButtonDown;

            grid.PreviewMouseDown += richTextBox_PreviewMouseDown;
            grid.PreviewMouseUp += richTextBox_PreviewMouseUp;
            grid.AddHandler(TextBox.MouseDownEvent, new MouseButtonEventHandler(richTextBox_MouseDown), true);
            grid.AddHandler(TextBox.MouseUpEvent, new MouseButtonEventHandler(richTextBox_MouseUp), true);
        }





        #region Event

        private void TitleGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("window_MouseDown");

        }

        private void window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("window_MouseUp");

        }

        private void window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("window_PreviewMouseDown");

        }

        private void window_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("window_PreviewMouseUp");

        }

        private void richTextBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("grid_MouseDown");

        }

        private void richTextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("grid_PreviewMouseDown");

        }

        private void richTextBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("grid_MouseUp");

        }

        private void richTextBox_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("grid_PreviewMouseUp");

        }

        #endregion
    }
}
