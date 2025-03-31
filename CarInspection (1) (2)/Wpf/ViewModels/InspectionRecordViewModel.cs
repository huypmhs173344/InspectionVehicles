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
using Wpf.Views;
using Microsoft.Data.SqlClient;
using Dapper;

namespace Wpf.ViewModels
{
    public class InspectionRecordViewModel : ViewModelBase
    {
        private readonly IInspectionRecordService _recordService;
        private ObservableCollection<InspectionRecord> _record;
        private readonly IInspectionRecordService _vehicleService;
        private ObservableCollection<Vehicle> _vehicles;
        private InspectionRecord _selectedRecord;
        private string _searchText;
        private bool _isEditing;
        private string _resulError;
        private string _stationError;
        private string _co2Error;
        private string _hcError;
        private string _dateError;

        public string ResultError
        {
            get => _resulError;
            set => SetProperty(ref _resulError, value);
        }

        public string StationError
        {
            get => _stationError;
            set => SetProperty(ref _stationError, value);
        }

        public string CO2Error
        {
            get => _co2Error;
            set => SetProperty(ref _co2Error, value);
        }

        public string HCError
        {
            get => _hcError;
            set => SetProperty(ref _hcError, value);
        }
        public string DateError
        {
            get => _dateError;
            set => SetProperty(ref _dateError, value);
        }

        private void ClearErrors()
        {
            ResultError = string.Empty;
            StationError = string.Empty;
            CO2Error = string.Empty;
            HCError = string.Empty;
        }

        private bool ValidateRecord()
        {
            ClearErrors();
            bool isValid = true;

            if (!SelectedRecord.InspectionDate.HasValue)
            {
                DateError = "Ngày kiểm định không được để trống";
                isValid = false;
            }
            else if (SelectedRecord.InspectionDate > DateTime.Now)
            {
                DateError = "Ngày kiểm định không được là ngày trong tương lai";
                isValid = false;
            }
            if (!SelectedRecord.Co2emission.HasValue)
            {
                DateError = "Lượng khí CO2 không được để trống";
                isValid = false;
            }
            if (!SelectedRecord.Hcemission.HasValue)
            {
                DateError = "Lượng khí HC không được để trống";
                isValid = false;
            }
            if (SelectedRecord.Result.Equals(""))
            {
                DateError = "Kết Quả không được để trống";
                isValid = false;
            }
            if (SelectedRecord.Co2emission <= 0)
            {
                CO2Error = "Lượng khí CO2 không thể nhỏ hơn 0";
                isValid = false;
            }

            if (SelectedRecord.Hcemission <= 0)
            {
                HCError = "Lượng khí HC không thể nhỏ hơn 0";
                isValid = false;
            }

            return isValid;
        }

        public InspectionRecordViewModel()
        {
            _recordService = new InspectionRecordService();           
            LoadRecord();
            InitializeCommands();
        }
        public ObservableCollection<InspectionRecord> Records
        {
            get => _record;
            set => SetProperty(ref _record, value);
        }
        public InspectionRecord SelectedRecord
        {
            get => _selectedRecord;
            set
            {
                if (SetProperty(ref _selectedRecord, value))
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
                    FilterRecord();
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

        private void LoadRecord()
        {
            var records = GetRecordDetail();
            Records = new ObservableCollection<InspectionRecord>(records);
        }

        //private void FilterRecord()
        //{
        //    if (string.IsNullOrWhiteSpace(SearchText))
        //    {
        //        LoadRecord();
        //        return;
        //    }

        //    var filteredRecord = _recordService
        //        .GetAllInspectionRecords()
        //        .Where(r =>
        //            r.Result.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
        //        )
        //        .ToList();

        //    Records = new ObservableCollection<InspectionRecord>(filteredRecord);
        //}
        private void FilterRecord()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                LoadRecord();
                return;
            }

            var filteredRecord = GetRecordDetail()
                .Where(r =>
                    (r.Result?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (r.Station?.Name?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (r.Vehicle?.PlateNumber?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false)
                )
                .ToList();

            Records = new ObservableCollection<InspectionRecord>(filteredRecord);
        }
        private readonly string _connectionString = "Server=MHuy;uid=sa;password=123456;database=CarInspectionDB;Encrypt=True;TrustServerCertificate=True;";
        public List<InspectionRecord> GetRecordDetail()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
    SELECT 
        ir.RecordID,
        ir.InspectionDate,
        ir.Result,
        ir.CO2Emission,
        ir.HCEmission,
        ir.Comments,
        v.VehicleID,
        v.PlateNumber,
        s.StationID,
        s.Name,
        v.OwnerID
    FROM InspectionRecords ir
    JOIN Vehicles v ON ir.VehicleID = v.VehicleID
    JOIN InspectionStations s ON ir.StationID = s.StationID";

                var records = connection.Query<RecordDTO>(query).Select(r => new InspectionRecord
                {
                    RecordId = r.RecordId,
                    InspectionDate = r.InspectionDate,
                    Result = r.Result,
                    Co2emission = r.Co2emission,
                    Hcemission = r.Hcemission,
                    Comments = r.Comments,
                    VehicleId = r.VehicleId,
                    StationId = r.StationId,
                    Vehicle = new Vehicle

                    {
                        VehicleId = r.VehicleId,
                        PlateNumber = r.PlateNumber,
                        OwnerId = r.OwnerId,
                    },
                    Station = new InspectionStation
                    {
                        StationId = r.StationId,
                        Name = r.Name,
                    }
                }).ToList();

                return records;
            }
        }
        private void ExecuteAdd()
        {
            SelectedRecord = new InspectionRecord();
            IsEditing = true;
        }

        private void ExecuteEdit()
        {
            if (SelectedRecord != null)
            {
                IsEditing = true;
            }
        }

        private bool CanExecuteEdit()
        {
            return SelectedRecord != null && !IsEditing;
        }

        private void ExecuteDelete()
        {
            if (SelectedRecord != null)
            {
                _recordService.DeleteInspectionRecord(SelectedRecord.RecordId);
                LoadRecord();
            }
        }

        private bool CanExecuteDelete()
        {
            return SelectedRecord != null && !IsEditing;
        }

        private void ExecuteSave()
        {
            if (!ValidateRecord())
            {
                return;
            }

            if (SelectedRecord.RecordId == 0)
            {
                _recordService.CreateInspectionRecord(SelectedRecord);
            }
            else
            {
                _recordService.UpdateInspectionRecord(SelectedRecord);
            }

            IsEditing = false;
            LoadRecord();
        }

        private void ExecuteCancel()
        {
            IsEditing = false;
            SelectedRecord = null;
        }
    }
}
