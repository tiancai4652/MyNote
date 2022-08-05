using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            InitEvent();
            InitCorlor();
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

            window.Loaded += Window_Loaded;
        }



        void InitCorlor()
        {
            MainBackgroundColor = (Color)ColorConverter.ConvertFromString("#202020");
            TextBackgroundColor = (Color)ColorConverter.ConvertFromString("#272727");
            WindowBorderCorlor = (Color)ColorConverter.ConvertFromString("#459BAC");
        }


        #region Color

        private Color _MainBackgroundColor;
        public Color MainBackgroundColor
        {
            get { return _MainBackgroundColor; }
            set { SetProperty(ref _MainBackgroundColor, value); }
        }

        private Color _TextBackgroundColor;
        public Color TextBackgroundColor
        {
            get { return _TextBackgroundColor; }
            set { SetProperty(ref _TextBackgroundColor, value); }
        }

        private Color _WindowBorderCorlor;
        public Color WindowBorderCorlor
        {
            get { return _WindowBorderCorlor; }
            set { SetProperty(ref _WindowBorderCorlor, value); }
        }

        #endregion


        #region Event

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
          
        }

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

        #region INotifyPropertyChanged

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Checks if a property already matches a desired value. Sets the property and
        /// notifies listeners only when necessary.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="storage">Reference to a property with both getter and setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="propertyName">Name of the property used to notify listeners. This
        /// value is optional and can be provided automatically when invoked from compilers that
        /// support CallerMemberName.</param>
        /// <returns>True if the value was changed, false if the existing value matched the
        /// desired value.</returns>
        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value)) return false;

            storage = value;
            RaisePropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// Checks if a property already matches a desired value. Sets the property and
        /// notifies listeners only when necessary.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="storage">Reference to a property with both getter and setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="propertyName">Name of the property used to notify listeners. This
        /// value is optional and can be provided automatically when invoked from compilers that
        /// support CallerMemberName.</param>
        /// <param name="onChanged">Action that is called after the property value has been changed.</param>
        /// <returns>True if the value was changed, false if the existing value matched the
        /// desired value.</returns>
        protected virtual bool SetProperty<T>(ref T storage, T value, Action onChanged, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value)) return false;

            storage = value;
            onChanged?.Invoke();
            RaisePropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Name of the property used to notify listeners. This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="CallerMemberNameAttribute"/>.</param>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="args">The PropertyChangedEventArgs</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
        }



        #endregion
    }
}
