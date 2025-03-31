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
    public class StationViewModel : ViewModelBase
    {
        private readonly IStationService _stationService;
        private ObservableCollection<InspectionStation> _station;
        private InspectionStation _selectedStation;
        private string _searchText;
        private bool _isEditing;
        private string _nameError;
        private string _addressError;
        private string _phoneError;
        private string _emailError;

        public string NameError
        {
            get => _nameError;
            set => SetProperty(ref _nameError, value);
        }

        public string AddressError
        {
            get => _addressError;
            set => SetProperty(ref _addressError, value);
        }

        public string PhoneError
        {
            get => _phoneError;
            set => SetProperty(ref _phoneError, value);
        }

        public string EmailError
        {
            get => _emailError;
            set => SetProperty(ref _emailError, value);
        }

        private void ClearErrors()
        {
            NameError = string.Empty;
            AddressError = string.Empty;
            EmailError = string.Empty;
            PhoneError = string.Empty;
        }

        private bool ValidateStation()
        {
            ClearErrors();
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(SelectedStation.Email))
            {
                EmailError = "Email không được để trống";
                isValid = false;
            }else if (!SelectedStation.Email.Contains("@") || !SelectedStation.Email.Contains("."))
            {
                EmailError = "Email không hợp lệ";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(SelectedStation.Name))
            {
                NameError = "Tên trạm xe không được để trống";
                isValid = false;
            }
            else if (Stations.Any(s => s.Name == SelectedStation.Name && s != SelectedStation))
            {
                NameError = "Tên trạm xe đã tồn tại";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(SelectedStation.Phone))
            {
                PhoneError = "Số điện thoại không được để trống";
                isValid = false;
            }

            else if (!System.Text.RegularExpressions.Regex.IsMatch(SelectedStation.Phone, @"^[0-9]{10}$"))
            {
                PhoneError = "Số điện thoại không hợp lệ";
                isValid = false;
            }
            return isValid;
        }

        public StationViewModel()
        {
            _stationService = new StationService();
            LoadStations();
            InitializeCommands();
        }

        public ObservableCollection<InspectionStation> Stations
        {
            get => _station;
            set => SetProperty(ref _station, value);
        }

        public InspectionStation SelectedStation
        {
            get => _selectedStation;
            set
            {
                if (SetProperty(ref _selectedStation, value))
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
                    FilterStations();
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

        private void LoadStations()
        {
            var stations = _stationService.GetAllStation();
            Stations = new ObservableCollection<InspectionStation>(stations);
        }

        private void FilterStations()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                LoadStations();
                return;
            }

            var filteredStations = _stationService
                .GetAllStation()
                .Where(s =>
                    s.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                    || s.Address.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                )
                .ToList();

            Stations = new ObservableCollection<InspectionStation>(filteredStations);
        }

        private void ExecuteAdd()
        {
            SelectedStation = new InspectionStation();
            IsEditing = true;
        }

        private void ExecuteEdit()
        {
            if (SelectedStation != null)
            {
                IsEditing = true;
            }
        }

        private bool CanExecuteEdit()
        {
            return SelectedStation != null && !IsEditing;
        }

        private void ExecuteDelete()
        {
            if (SelectedStation != null)
            {
                _stationService.DeleteStation(SelectedStation.StationId);
                LoadStations();
            }
        }

        private bool CanExecuteDelete()
        {
            return SelectedStation != null && !IsEditing;
        }

        private void ExecuteSave()
        {
            if (!ValidateStation())
            {
                return;
            }

            if (SelectedStation.StationId == 0)
            {
                _stationService.CreateStation(SelectedStation);
            }
            else
            {
                _stationService.UpdateStation(SelectedStation);
            }

            IsEditing = false;
            LoadStations();
        }

        private void ExecuteCancel()
        {
            IsEditing = false;
            SelectedStation = null;
        }
    }
}
