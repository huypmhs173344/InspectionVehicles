using System;
using System.Collections.ObjectModel;
using System.Windows;
using BusinessObject.Models;
using Services;
using Wpf.Utils;

namespace Wpf.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly IInspectionService _inspectionService;
        private readonly IUserService _userService;

        private int _totalVehicles;
        private int _totalInspectionsThisMonth;
        private int _totalInspectionStations;
        private int _unreadNotifications;
        private DateTime _selectedDate;
        private ObservableCollection<InspectionRecord> _recentInspections;
        private ObservableCollection<Notification> _recentNotifications;

        public HomeViewModel()
        {
            _vehicleService = new VehicleService();
            _inspectionService = new InspectionService();
            _userService = new UserService();
            _selectedDate = DateTime.Now;
            _recentInspections = new ObservableCollection<InspectionRecord>();
            _recentNotifications = new ObservableCollection<Notification>();

            LoadData();
        }

        public int TotalVehicles
        {
            get => _totalVehicles;
            set => SetProperty(ref _totalVehicles, value);
        }

        public int TotalInspectionsThisMonth
        {
            get => _totalInspectionsThisMonth;
            set => SetProperty(ref _totalInspectionsThisMonth, value);
        }

        public int TotalInspectionStations
        {
            get => _totalInspectionStations;
            set => SetProperty(ref _totalInspectionStations, value);
        }

        public int UnreadNotifications
        {
            get => _unreadNotifications;
            set => SetProperty(ref _unreadNotifications, value);
        }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (SetProperty(ref _selectedDate, value))
                {
                    LoadData();
                }
            }
        }

        public ObservableCollection<InspectionRecord> RecentInspections
        {
            get => _recentInspections;
            set => SetProperty(ref _recentInspections, value);
        }

        public ObservableCollection<Notification> RecentNotifications
        {
            get => _recentNotifications;
            set => SetProperty(ref _recentNotifications, value);
        }

        private void LoadData()
        {
            try
            {
                // Load tổng số phương tiện
                var vehicles = _vehicleService.GetVehiclesByOwnerId(
                    SessionManager.CurrentUser.UserId
                );
                TotalVehicles = vehicles.Count();
                var vehicleIds = vehicles.Select(v => v.VehicleId).ToList(); // Lấy danh sách Id của các xe
                var inspections = _inspectionService
                    .GetAllInspections()
                    .Where(i => vehicleIds.Contains(i.VehicleId));

                // Load số lượt kiểm định trong tháng
                var thisMonth = DateTime.Now;
                TotalInspectionsThisMonth = inspections.Count(i =>
                    i.InspectionDate?.Month == thisMonth.Month
                    && i.InspectionDate?.Year == thisMonth.Year
                );

                // Load số trạm đăng kiểm
                var stations = _inspectionService.GetAllStations();
                TotalInspectionStations = stations.Count();

                // Load 5 lượt kiểm định gần nhất
                var recentInspections = inspections
                    .OrderByDescending(i => i.InspectionDate)
                    .Take(5)
                    .ToList();
                RecentInspections = new ObservableCollection<InspectionRecord>(recentInspections);

                // Load thông báo
                var currentUser = SessionManager.CurrentUser;
                if (currentUser != null)
                {
                    var notifications = currentUser.Notifications.ToList();
                    UnreadNotifications = notifications.Count();
                    var recentNotifications = notifications
                        .OrderByDescending(n => n.SentDate)
                        .Take(5)
                        .ToList();
                    RecentNotifications = new ObservableCollection<Notification>(
                        recentNotifications
                    );
                }
                else
                {
                    UnreadNotifications = 0;
                    RecentNotifications.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi khi tải dữ liệu: " + ex.Message,
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }
    }
}
