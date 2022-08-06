using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using MyNote.Base;
using MyNote.Settings;
using System.Threading;
using MyNote.RecentFile;

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
            window.Closed += Window_Closed;
            window.Closing += Window_Closing;
            richTextBox.TextChanged += RichTextBox_TextChanged;
            richTextBox.KeyDown += RichTextBox_KeyDown;
            richTextBox.MouseWheel += RichTextBox_MouseWheel;
        }

      

        void InitCorlor()
        {
            MainBackgroundColor = (Color)ColorConverter.ConvertFromString("#202020");
            TextBackgroundColor = (Color)ColorConverter.ConvertFromString("#272727");
            WindowBorderCorlor = (Color)ColorConverter.ConvertFromString("#459BAC");
        }

        #region Properties


        double _TextFontSize = 19;
        public double TextFontSize
        {
            get
            {
                return _TextFontSize;
            }
            set
            {
                SetProperty(ref _TextFontSize, value);
            }
        }


        #endregion

        #region Key&Mouse

        bool isCtrl;
        bool isAlt;
        bool isShift;
        private void RichTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            CheckModifierKeys();
            //打开最近的文件夹
            if (isAlt && Keyboard.IsKeyDown(Key.R))
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Recent);
                System.Diagnostics.Process.Start("Explorer.exe", path);
            }
            //设置目标
            else if (isAlt && Keyboard.IsKeyDown(Key.T))
            {
                Paragraph paragraph = new Paragraph();
                InlineUIContainer inlineUIContainer = new InlineUIContainer();
                Image image = new Image();
                image.Width = image.Height = 50;
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(Environment.CurrentDirectory + "//Resource" + "//target.png", UriKind.RelativeOrAbsolute);
                bi.EndInit();
                image.Source = bi;
                inlineUIContainer.Child = image;
                paragraph.Inlines.Add(inlineUIContainer);
                myFlowDocument.Blocks.Add(paragraph);
            }
        }

        void CheckModifierKeys()
        {
            isCtrl = (Keyboard.GetKeyStates(Key.LeftCtrl) & KeyStates.Down) > 0 ||
                   (Keyboard.GetKeyStates(Key.RightCtrl) & KeyStates.Down) > 0;
            isAlt = (Keyboard.GetKeyStates(Key.LeftAlt) & KeyStates.Down) > 0 ||
                 (Keyboard.GetKeyStates(Key.RightAlt) & KeyStates.Down) > 0;
            isShift = (Keyboard.GetKeyStates(Key.LeftShift) & KeyStates.Down) > 0 ||
                 (Keyboard.GetKeyStates(Key.RightShift) & KeyStates.Down) > 0;
        }

        double fontSizeChangeDelta = 1;
        double fontSizeMinValue = 5;
        /// <summary>
        /// Zoom=Ctrl+MouseWheel
        /// </summary>
        /// <param name="e"></param>
        void Zoom(MouseWheelEventArgs e)
        {
            CheckModifierKeys();
            if (isCtrl)
            {
                var mouseWheelDelta = e.Delta;
                if (mouseWheelDelta > 0)
                {
                    TextFontSize += fontSizeChangeDelta;
                }
                else if (mouseWheelDelta < 0)
                {
                    TextFontSize = TextFontSize - fontSizeChangeDelta > fontSizeMinValue ? TextFontSize - fontSizeChangeDelta : TextFontSize;
                }
            }
        }

        #endregion

        #region AutoSave/ReadContent

        void SaveContent()
        {
            SaveXamlPackage(GlobalParams.CurrentFile, richTextBox);
            UpdateConfig();
        }

        void ReadContent()
        {
           var configDataTemp = ReadConfig();
            if (configDataTemp != null)
            {
                CurrentConfigData = configDataTemp;
                var str = File.ReadAllText(CurrentConfigData.CurrentFile);
                if (!string.IsNullOrEmpty(str))
                {
                    LoadXamlPackage(CurrentConfigData.CurrentFile, richTextBox);
                }
            }
            else
            {
                CurrentConfigData = new CacheConfig();
                CurrentConfigData.CreateTime = DateTime.Now;
                CurrentConfigData.UpdateTime = DateTime.Now;
                CurrentConfigData.CurrentFile = GlobalParams.CurrentFile;
            }
        }

        void SaveXamlPackage(string _fileName, RichTextBox richTB)
        {
            TextRange range;
            FileStream fStream;
            range = new TextRange(richTB.Document.ContentStart, richTB.Document.ContentEnd);
            fStream = new FileStream(_fileName, FileMode.OpenOrCreate);
            richTB.Dispatcher.Invoke(() =>
            {
                range.Save(fStream, DataFormats.XamlPackage);
            });
            fStream.Close();
            //File.WriteAllText(_fileName, range.Text);
        }

        void LoadXamlPackage(string _fileName, RichTextBox richTB)
        {
            TextRange range;
            FileStream fStream;
            if (File.Exists(_fileName))
            {
                range = new TextRange(richTB.Document.ContentStart, richTB.Document.ContentEnd);
                fStream = new FileStream(_fileName, FileMode.OpenOrCreate);
                range.Load(fStream, DataFormats.XamlPackage);
                fStream.Close();
            }
        }

        void PrintCommand(RichTextBox richTB)
        {
            PrintDialog pd = new PrintDialog();
            if ((pd.ShowDialog() == true))
            {
                //use either one of the below
                pd.PrintVisual(richTB as Visual, "printing as visual");
                pd.PrintDocument((((IDocumentPaginatorSource)richTB.Document).DocumentPaginator), "printing as paginator");
            }
        }

        int _Steps;
        public int Steps
        {
            get
            {
                lockForSteps.EnterReadLock();
                try
                {
                    return _Steps;
                }
                finally
                {
                    lockForSteps.ExitReadLock();
                }
            }
            set
            {
                lockForSteps.EnterWriteLock();
                try
                {
                    _Steps = value;
                }
                finally
                {
                    lockForSteps.ExitWriteLock();
                }
            }
        }

        ReaderWriterLockSlim lockForSteps = new ReaderWriterLockSlim();
        CancellationTokenSource cancellationTokenSourceForSaveContent;
        DateTime? lastSaveContentTime;
        void SaveContentInTask()
        {
            cancellationTokenSourceForSaveContent = new CancellationTokenSource();
            Task.Factory.StartNew(() =>
            {
                if (lastSaveContentTime == null)
                {
                    lastSaveContentTime = DateTime.Now;
                }
                while (true)
                {
                    bool isOverTime = (DateTime.Now - lastSaveContentTime)?.TotalMilliseconds > AutoSaveConfig.SaveInternalMs;
                    bool isOverSteps = Steps > AutoSaveConfig.SaveSteps;
                    if (isOverTime|| isOverSteps)
                    {
                        SaveContent();
                        lastSaveContentTime = DateTime.Now;
                        Steps = 0;
                    }
                    Thread.Sleep(500);
                }
            }, cancellationTokenSourceForSaveContent.Token);
        }


        #endregion

        #region Config

        CacheConfig _CurrentConfigData;
        CacheConfig CurrentConfigData
        {
            get
            {
               
                return _CurrentConfigData;
            }
            set
            {
                _CurrentConfigData = value;
            }
        }
        CacheConfig ReadConfig()
        {
            if (File.Exists(GlobalParams.ConfigFile))
            {
                var str = File.ReadAllText(GlobalParams.ConfigFile);
                if (str.Length != 0)
                {
                    CacheConfig configData = JsonConvert.DeserializeObject<CacheConfig>(str);
                    return configData;
                }
            }
            return null;
        }
        void UpdateConfig()
        {

            CurrentConfigData.UpdateTime = DateTime.Now;
            var str = JsonConvert.SerializeObject(CurrentConfigData);
            File.WriteAllText(GlobalParams.ConfigFile, str);
        }

        #endregion

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
        private void RichTextBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Zoom(e);
        }

        private void Window_Closing(object? sender, CancelEventArgs e)
        {
            cancellationTokenSourceForSaveContent?.Cancel();
            while (cancellationTokenSourceForSaveContent != null && !cancellationTokenSourceForSaveContent.IsCancellationRequested)
            {
                Thread.Sleep(10);
            }
            SaveContent();
        }


        private void Window_Closed(object? sender, EventArgs e)
        {

        }

        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Steps++;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ReadContent();
            SaveContentInTask();
           
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
        public event PropertyChangedEventHandler? PropertyChanged;

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
