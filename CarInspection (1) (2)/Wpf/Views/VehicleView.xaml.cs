using System.Windows;
using System.Windows.Controls;
using Wpf.ViewModels;

namespace Wpf.Views
{
    public partial class VehicleView : UserControl
    {
        public VehicleView()
        {
            InitializeComponent();
            this.DataContext = new VehicleViewModel();
        }
    }
}
