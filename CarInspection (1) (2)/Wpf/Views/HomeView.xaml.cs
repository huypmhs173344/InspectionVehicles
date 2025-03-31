using System.Windows.Controls;

namespace Wpf.Views
{
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            try
            {
                InitializeComponent();
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Lỗi khởi tạo giao diện: {ex.Message}",
                    "Lỗi",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error
                );
            }
        }
    }
}
