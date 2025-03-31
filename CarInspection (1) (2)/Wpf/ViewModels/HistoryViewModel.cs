using System;
using System.Collections.ObjectModel;
using System.Windows;
using BusinessObject.Models;
using Services;
using Wpf.Utils;

namespace Wpf.ViewModels
{
    public class HistoryViewModel : ViewModelBase
    {
        private readonly IInspectionService _inspectionService;
        private readonly IVehicleService _vehicleService;
        private ObservableCollection<InspectionRecord> _inspectionRecords;
        private Vehicle _selectedVehicle;
        private ObservableCollection<Vehicle> _vehicles;

        public HistoryViewModel()
        {
            _inspectionService = new InspectionService();
            _vehicleService = new VehicleService();
            LoadData();
        }

        public ObservableCollection<InspectionRecord> InspectionRecords
        {
            get => _inspectionRecords;
            set => SetProperty(ref _inspectionRecords, value);
        }

        public ObservableCollection<Vehicle> Vehicles
        {
            get => _vehicles;
            set => SetProperty(ref _vehicles, value);
        }

        public Vehicle SelectedVehicle
        {
            get => _selectedVehicle;
            set
            {
                if (SetProperty(ref _selectedVehicle, value))
                {
                    LoadInspectionHistory();
                }
            }
        }

        private void LoadData()
        {
            try
            {
                var vehicles = _vehicleService.GetVehiclesByOwnerId(
                    SessionManager.CurrentUser.UserId
                );
                Vehicles = new ObservableCollection<Vehicle>(vehicles);
                InspectionRecords = new ObservableCollection<InspectionRecord>();
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

        private void LoadInspectionHistory()
        {
            if (SelectedVehicle == null)
            {
                InspectionRecords.Clear();
                return;
            }

            try
            {
                var records = _inspectionService.GetInspectionsByVehicleIdWithDetails(
                    SelectedVehicle.VehicleId
                );
                InspectionRecords = new ObservableCollection<InspectionRecord>(records);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi khi tải lịch sử kiểm định: " + ex.Message,
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }
    }
}
