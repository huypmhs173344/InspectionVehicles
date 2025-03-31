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
using Wpf.ViewModels;

namespace Wpf.Views
{
    /// <summary>
    /// Interaction logic for PoliceNotification.xaml
    /// </summary>
    public partial class PoliceNotification : UserControl
    {
        public PoliceNotification()
        {
            InitializeComponent();
            this.DataContext = new PoliceNotificationViewModel();
        }
    }
}
