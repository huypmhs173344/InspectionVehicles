using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Services;
using System.Windows.Input;
using Wpf.Utils;

namespace Wpf.ViewModels
{
    public class VehicleManagementViewModel : ViewModelBase
    {
        private readonly IVehicleService _vehicleService;
        private ObservableCollection<Vehicle> _vehicles;
        private Vehicle _selectedVehicle;
        private string _searchText;
        private bool _isEditing;
        private string _plateNumberError;
        private string _brandError;
        private string _modelError;
        private string _manufactureYearError;
        private string _engineNumberError;
        private string _ownerIdError;
        public string OwnerIdError
        {
            get => _ownerIdError;
            set => SetProperty(ref _ownerIdError, value);
        }
        public string PlateNumberError
        {
            get => _plateNumberError;
            set => SetProperty(ref _plateNumberError, value);
        }

        public string BrandError
        {
            get => _brandError;
            set => SetProperty(ref _brandError, value);
        }

        public string ModelError
        {
            get => _modelError;
            set => SetProperty(ref _modelError, value);
        }

        public string ManufactureYearError
        {
            get => _manufactureYearError;
            set => SetProperty(ref _manufactureYearError, value);
        }

        public string EngineNumberError
        {
            get => _engineNumberError;
            set => SetProperty(ref _engineNumberError, value);
        }

        private void ClearErrors()
        {
            PlateNumberError = string.Empty;
            BrandError = string.Empty;
            ModelError = string.Empty;
            ManufactureYearError = string.Empty;
            EngineNumberError = string.Empty;
        }

        private bool ValidateVehicle()
        {
            ClearErrors();
            bool isValid = true;
            var vehicles = _vehicleService.GetAllVehicles();

            // Validate PlateNumber
            SelectedVehicle.PlateNumber = SelectedVehicle.PlateNumber?.Trim();
            if (string.IsNullOrWhiteSpace(SelectedVehicle.PlateNumber))
            {
                PlateNumberError = "Biển số xe không được để trống";
                isValid = false;
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(SelectedVehicle.PlateNumber, @"^\d{2}[A-Z]-\d{5}$"))
            {
                PlateNumberError = "Biển số xe không hợp lệ (VD: 30A-12345)";
                isValid = false;
            }
            else if (SelectedVehicle.PlateNumber.Length > 10)
            {
                PlateNumberError = "Biển số xe không được vượt quá 10 ký tự";
                isValid = false;
            }
            else if (vehicles.Any(v => v.PlateNumber == SelectedVehicle.PlateNumber && v.PlateNumber != SelectedVehicle.PlateNumber))
            {
                PlateNumberError = "Biển số xe bị trùng";
                isValid = false;
            }

            // Validate Brand
            SelectedVehicle.Brand = SelectedVehicle.Brand?.Trim();
            if (string.IsNullOrWhiteSpace(SelectedVehicle.Brand))
            {
                BrandError = "Hãng xe không được để trống";
                isValid = false;
            }
            else if (SelectedVehicle.Brand.Length < 2 || SelectedVehicle.Brand.Length > 50)
            {
                BrandError = "Hãng xe phải từ 2-50 ký tự";
                isValid = false;
            }

            // Validate Model
            SelectedVehicle.Model = SelectedVehicle.Model?.Trim();
            if (string.IsNullOrWhiteSpace(SelectedVehicle.Model))
            {
                ModelError = "Model xe không được để trống";
                isValid = false;
            }
            else if (SelectedVehicle.Model.Length < 2 || SelectedVehicle.Model.Length > 50)
            {
                ModelError = "Model xe phải từ 2-50 ký tự";
                isValid = false;
            }

            // Validate ManufactureYear
            int currentYear = DateTime.Now.Year;
            if (SelectedVehicle.ManufactureYear < 1900)
            {
                ManufactureYearError = "Năm sản xuất không được nhỏ hơn 1900";
                isValid = false;
            }
            else if (SelectedVehicle.ManufactureYear > currentYear)
            {
                ManufactureYearError = "Năm sản xuất không được lớn hơn năm hiện tại";
                isValid = false;
            }

            // Validate EngineNumber
            SelectedVehicle.EngineNumber = SelectedVehicle.EngineNumber?.Trim();
            if (string.IsNullOrWhiteSpace(SelectedVehicle.EngineNumber))
            {
                EngineNumberError = "Số máy không được để trống";
                isValid = false;
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(SelectedVehicle.EngineNumber, @"^[A-Za-z0-9]{5,20}$"))
            {
                EngineNumberError = "Số máy chỉ chấp nhận chữ và số, độ dài 5-20 ký tự";
                isValid = false;
            }
            else if (vehicles.Any(v => v.EngineNumber == SelectedVehicle.EngineNumber && v.PlateNumber != SelectedVehicle.PlateNumber))
            {
                EngineNumberError = "Số máy bị trùng";
                isValid = false;
            }

            // Validate OwnerId
            if (!vehicles.Any(v => v.OwnerId == SelectedVehicle.OwnerId))
            {
                OwnerIdError = "Không tìm thấy chủ xe";
                isValid = false;
            }

            return isValid;
        }


        public VehicleManagementViewModel()
        {
            _vehicleService = new VehicleService();
            LoadVehicles();
            InitializeCommands();
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
                    (EditCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    (DeleteCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    FilterVehicles();
                }
            }
        }

        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                if (SetProperty(ref _isEditing, value))
                {
                    (EditCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    (DeleteCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public ICommand AddCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        private void InitializeCommands()
        {
            AddCommand = new RelayCommand(param => ExecuteAdd());
            EditCommand = new RelayCommand(param => ExecuteEdit(), param => CanExecuteEdit());
            DeleteCommand = new RelayCommand(param => ExecuteDelete(), param => CanExecuteDelete());
            SaveCommand = new RelayCommand(param => ExecuteSave());
            CancelCommand = new RelayCommand(param => ExecuteCancel());
        }

        private void LoadVehicles()
        {
            var vehicles = _vehicleService.GetAllVehicles();
            Vehicles = new ObservableCollection<Vehicle>(vehicles);
        }

        private void FilterVehicles()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                LoadVehicles();
                return;
            }

            var filteredVehicles = _vehicleService
                .GetAllVehicles()
                .Where(v =>
                    v.PlateNumber.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                    || v.Brand.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                    || v.Model.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                )
                .ToList();

            Vehicles = new ObservableCollection<Vehicle>(filteredVehicles);
        }

        private void FilterVehiclesbyOrnerID()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                LoadVehicles();
                return;
            }

            var filteredVehicles = _vehicleService
                .GetVehiclesByOwnerIdWithInspections(SessionManager.CurrentUser.UserId)
                .Where(v =>
                    v.PlateNumber.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                    || v.Brand.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                    || v.Model.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                )
                .ToList();

            Vehicles = new ObservableCollection<Vehicle>(filteredVehicles);
        }

        private void ExecuteAdd()
        {
            SelectedVehicle = new Vehicle();
            IsEditing = true;
        }

        private void ExecuteEdit()
        {
            if (SelectedVehicle != null)
            {
                IsEditing = true;
            }
        }

        private bool CanExecuteEdit()
        {
            return SelectedVehicle != null && !IsEditing;
        }

        private void ExecuteDelete()
        {
            if (SelectedVehicle != null)
            {
                _vehicleService.DeleteVehicle(SelectedVehicle.VehicleId);
                LoadVehicles();
            }
        }

        private bool CanExecuteDelete()
        {
            return SelectedVehicle != null && !IsEditing;
        }

        private void ExecuteSave()
        {
            if (!ValidateVehicle())
            {
                return;
            }

            if (SelectedVehicle.VehicleId == 0)
            {
                _vehicleService.CreateVehicle(SelectedVehicle);
            }
            else
            {
                _vehicleService.UpdateVehicle(SelectedVehicle);
            }

            IsEditing = false;
            LoadVehicles();
        }

        private void ExecuteCancel()
        {
            IsEditing = false;
            SelectedVehicle = null;
        }
    }
}
