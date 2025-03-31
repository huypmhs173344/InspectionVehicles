using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using BusinessObject.Models;
using Services;
using Wpf.Utils;

namespace Wpf.ViewModels
{
    public class InspectionViewModel : ViewModelBase
    {
        private readonly IInspectionService _inspectionService;
        private readonly IVehicleService _vehicleService;
        private ObservableCollection<InspectionStation> _stations;
        private ObservableCollection<Vehicle> _vehicles;
        private InspectionStation _selectedStation;
        private Vehicle _selectedVehicle;
        private DateTime _inspectionDate;

        public InspectionViewModel()
        {
            _inspectionService = new InspectionService();
            _vehicleService = new VehicleService();
            _inspectionDate = DateTime.Now;
            RegisterCommand = new RelayCommand(Register, CanRegister);
            LoadData();
        }

        public ObservableCollection<InspectionStation> Stations
        {
            get => _stations;
            set => SetProperty(ref _stations, value);
        }

        public ObservableCollection<Vehicle> Vehicles
        {
            get => _vehicles;
            set => SetProperty(ref _vehicles, value);
        }

        public InspectionStation SelectedStation
        {
            get => _selectedStation;
            set
            {
                if (SetProperty(ref _selectedStation, value))
                {
                    (RegisterCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public Vehicle SelectedVehicle
        {
            get => _selectedVehicle;
            set
            {
                if (SetProperty(ref _selectedVehicle, value))
                {
                    (RegisterCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public DateTime InspectionDate
        {
            get => _inspectionDate;
            set
            {
                if (SetProperty(ref _inspectionDate, value))
                {
                    (RegisterCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public ICommand RegisterCommand { get; }

        private void LoadData()
        {
            try
            {
                var stations = _inspectionService.GetAllStations();
                var vehicles = _vehicleService.GetVehiclesByOwnerId(
                    SessionManager.CurrentUser.UserId
                );

                Stations = new ObservableCollection<InspectionStation>(stations);
                Vehicles = new ObservableCollection<Vehicle>(vehicles);
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

        private bool CanRegister(object parameter)
        {
            return SelectedStation != null
                && SelectedVehicle != null
                && InspectionDate >= DateTime.Today;
        }

        private void Register(object parameter)
        {
            try
            {
                // Tạo record mới chỉ với các thông tin cần thiết
                var record = new InspectionRecord
                {
                    VehicleId = SelectedVehicle.VehicleId,
                    StationId = SelectedStation.StationId,
                    InspectionDate = InspectionDate,
                };

                _inspectionService.CreateInspectionRegistration(record);

                MessageBox.Show(
                    "Đăng ký kiểm định thành công!",
                    "Thông báo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                // Reset form
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi khi đăng ký kiểm định: " + ex.Message,
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void ResetForm()
        {
            SelectedStation = null;
            SelectedVehicle = null;
            InspectionDate = DateTime.Now;
        }
    }
}
