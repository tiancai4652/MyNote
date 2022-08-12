using MyNote.Base;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyNote.Controls
{
    /// <summary>
    /// TimeTarget.xaml 的交互逻辑
    /// </summary>
    public partial class TimeTarget : UserControl
    {
        public TimeTarget(int workingTimeS)
        {
            InitializeComponent();
            TimeTargetModel = new TimeTargetModel(workingTimeS);
            this.DataContext = TimeTargetModel;
        }

        public TimeTargetModel TimeTargetModel { get; set; }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
