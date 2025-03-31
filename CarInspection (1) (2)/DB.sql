USE [master]
GO
/****** Object:  Database [CarInspectionDB]    Script Date: 3/26/2025 7:50:21 PM ******/
CREATE DATABASE [CarInspectionDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CarInspectionDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLSERVER\MSSQL\DATA\CarInspectionDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'CarInspectionDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLSERVER\MSSQL\DATA\CarInspectionDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [CarInspectionDB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CarInspectionDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CarInspectionDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CarInspectionDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CarInspectionDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CarInspectionDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CarInspectionDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [CarInspectionDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CarInspectionDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CarInspectionDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CarInspectionDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CarInspectionDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CarInspectionDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CarInspectionDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CarInspectionDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CarInspectionDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CarInspectionDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [CarInspectionDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CarInspectionDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CarInspectionDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CarInspectionDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CarInspectionDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CarInspectionDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CarInspectionDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CarInspectionDB] SET RECOVERY FULL 
GO
ALTER DATABASE [CarInspectionDB] SET  MULTI_USER 
GO
ALTER DATABASE [CarInspectionDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CarInspectionDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CarInspectionDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CarInspectionDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [CarInspectionDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [CarInspectionDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'CarInspectionDB', N'ON'
GO
ALTER DATABASE [CarInspectionDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [CarInspectionDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [CarInspectionDB]
GO
/****** Object:  Table [dbo].[InspectionRecords]    Script Date: 3/26/2025 7:50:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InspectionRecords](
	[RecordID] [int] NOT NULL,
	[VehicleID] [int] NOT NULL,
	[StationID] [int] NOT NULL,
	[InspectorID] [int] NULL,
	[InspectionDate] [datetime] NULL,
	[Result] [nvarchar](10) NULL,
	[CO2Emission] [decimal](5, 2) NULL,
	[HCEmission] [decimal](5, 2) NULL,
	[Comments] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[RecordID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InspectionStations]    Script Date: 3/26/2025 7:50:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InspectionStations](
	[StationID] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Address] [nvarchar](max) NOT NULL,
	[Phone] [nvarchar](15) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[StationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Logs]    Script Date: 3/26/2025 7:50:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Logs](
	[LogID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[Action] [nvarchar](100) NOT NULL,
	[Timestamp] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notifications]    Script Date: 3/26/2025 7:50:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notifications](
	[NotificationID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[SentDate] [datetime] NULL,
	[IsRead] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[NotificationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 3/26/2025 7:50:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[Role] [nvarchar](20) NOT NULL,
	[Phone] [nvarchar](15) NULL,
	[Address] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vehicles]    Script Date: 3/26/2025 7:50:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vehicles](
	[VehicleID] [int] NOT NULL,
	[OwnerID] [int] NULL,
	[PlateNumber] [nvarchar](15) NOT NULL,
	[Brand] [nvarchar](50) NOT NULL,
	[Model] [nvarchar](50) NOT NULL,
	[ManufactureYear] [int] NOT NULL,
	[EngineNumber] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[VehicleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[InspectionRecords] ([RecordID], [VehicleID], [StationID], [InspectorID], [InspectionDate], [Result], [CO2Emission], [HCEmission], [Comments]) VALUES (1, 1, 1, 3, CAST(N'2023-01-15T09:30:00.000' AS DateTime), N'Đạt', CAST(2.50 AS Decimal(5, 2)), CAST(0.03 AS Decimal(5, 2)), N'Xe đạt tiêu chuẩn kiểm định')
GO
INSERT [dbo].[InspectionRecords] ([RecordID], [VehicleID], [StationID], [InspectorID], [InspectionDate], [Result], [CO2Emission], [HCEmission], [Comments]) VALUES (2, 2, 2, 4, CAST(N'2023-02-20T10:45:00.000' AS DateTime), N'Đạt', CAST(2.30 AS Decimal(5, 2)), CAST(0.02 AS Decimal(5, 2)), N'Xe hoạt động tốt')
GO
INSERT [dbo].[InspectionRecords] ([RecordID], [VehicleID], [StationID], [InspectorID], [InspectionDate], [Result], [CO2Emission], [HCEmission], [Comments]) VALUES (3, 3, 3, 3, CAST(N'2023-03-10T14:15:00.000' AS DateTime), N'Không đạt', CAST(3.80 AS Decimal(5, 2)), CAST(0.06 AS Decimal(5, 2)), N'Lượng khí thải CO2 vượt quá tiêu chuẩn')
GO
INSERT [dbo].[InspectionRecords] ([RecordID], [VehicleID], [StationID], [InspectorID], [InspectionDate], [Result], [CO2Emission], [HCEmission], [Comments]) VALUES (4, 4, 4, 4, CAST(N'2023-04-05T11:20:00.000' AS DateTime), N'Đạt', CAST(2.10 AS Decimal(5, 2)), CAST(0.01 AS Decimal(5, 2)), N'Xe mới, các chỉ số đều tốt')
GO
INSERT [dbo].[InspectionRecords] ([RecordID], [VehicleID], [StationID], [InspectorID], [InspectionDate], [Result], [CO2Emission], [HCEmission], [Comments]) VALUES (5, 5, 5, 3, CAST(N'2023-05-12T13:40:00.000' AS DateTime), N'Đạt', CAST(2.70 AS Decimal(5, 2)), CAST(0.03 AS Decimal(5, 2)), N'Cần bảo dưỡng hệ thống xả sau 3 tháng')
GO
INSERT [dbo].[InspectionRecords] ([RecordID], [VehicleID], [StationID], [InspectorID], [InspectionDate], [Result], [CO2Emission], [HCEmission], [Comments]) VALUES (6, 6, 1, 4, CAST(N'2023-06-18T09:15:00.000' AS DateTime), N'Đạt', CAST(2.20 AS Decimal(5, 2)), CAST(0.02 AS Decimal(5, 2)), N'Xe đạt tiêu chuẩn kiểm định')
GO
INSERT [dbo].[InspectionRecords] ([RecordID], [VehicleID], [StationID], [InspectorID], [InspectionDate], [Result], [CO2Emission], [HCEmission], [Comments]) VALUES (7, 7, 2, 3, CAST(N'2023-07-21T10:30:00.000' AS DateTime), N'Không đạt', CAST(4.10 AS Decimal(5, 2)), CAST(0.05 AS Decimal(5, 2)), N'Cần điều chỉnh hệ thống xả và kiểm tra lại')
GO
INSERT [dbo].[InspectionRecords] ([RecordID], [VehicleID], [StationID], [InspectorID], [InspectionDate], [Result], [CO2Emission], [HCEmission], [Comments]) VALUES (8, 8, 3, 4, CAST(N'2023-08-09T15:45:00.000' AS DateTime), N'Đạt', CAST(2.40 AS Decimal(5, 2)), CAST(0.02 AS Decimal(5, 2)), N'Xe hoạt động tốt')
GO
INSERT [dbo].[InspectionRecords] ([RecordID], [VehicleID], [StationID], [InspectorID], [InspectionDate], [Result], [CO2Emission], [HCEmission], [Comments]) VALUES (9, 9, 4, 3, CAST(N'2023-09-14T11:50:00.000' AS DateTime), N'Đạt', CAST(2.60 AS Decimal(5, 2)), CAST(0.02 AS Decimal(5, 2)), N'Xe đạt tiêu chuẩn kiểm định')
GO
INSERT [dbo].[InspectionRecords] ([RecordID], [VehicleID], [StationID], [InspectorID], [InspectionDate], [Result], [CO2Emission], [HCEmission], [Comments]) VALUES (10, 10, 5, 4, CAST(N'2023-10-25T13:20:00.000' AS DateTime), N'Đạt', CAST(2.00 AS Decimal(5, 2)), CAST(0.01 AS Decimal(5, 2)), N'Xe hoạt động rất tốt')
GO
INSERT [dbo].[InspectionRecords] ([RecordID], [VehicleID], [StationID], [InspectorID], [InspectionDate], [Result], [CO2Emission], [HCEmission], [Comments]) VALUES (11, 3, 3, 3, CAST(N'2023-04-10T14:15:00.000' AS DateTime), N'Đạt', CAST(2.50 AS Decimal(5, 2)), CAST(0.03 AS Decimal(5, 2)), N'Đã sửa chữa hệ thống xả, đạt tiêu chuẩn')
GO
INSERT [dbo].[InspectionRecords] ([RecordID], [VehicleID], [StationID], [InspectorID], [InspectionDate], [Result], [CO2Emission], [HCEmission], [Comments]) VALUES (12, 7, 2, 3, CAST(N'2023-08-21T10:30:00.000' AS DateTime), N'Đạt', CAST(2.80 AS Decimal(5, 2)), CAST(0.03 AS Decimal(5, 2)), N'Đã sửa chữa hệ thống xả, đạt tiêu chuẩn')
GO
INSERT [dbo].[InspectionRecords] ([RecordID], [VehicleID], [StationID], [InspectorID], [InspectionDate], [Result], [CO2Emission], [HCEmission], [Comments]) VALUES (13, 1, 1, NULL, CAST(N'2025-03-18T19:44:26.330' AS DateTime), NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[InspectionRecords] ([RecordID], [VehicleID], [StationID], [InspectorID], [InspectionDate], [Result], [CO2Emission], [HCEmission], [Comments]) VALUES (14, 11, 1, NULL, CAST(N'2025-03-18T20:30:46.043' AS DateTime), NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[InspectionRecords] ([RecordID], [VehicleID], [StationID], [InspectorID], [InspectionDate], [Result], [CO2Emission], [HCEmission], [Comments]) VALUES (15, 11, 2, NULL, CAST(N'2025-03-18T20:53:53.697' AS DateTime), N'ok', CAST(1.00 AS Decimal(5, 2)), CAST(1.00 AS Decimal(5, 2)), N'1')
GO
INSERT [dbo].[InspectionRecords] ([RecordID], [VehicleID], [StationID], [InspectorID], [InspectionDate], [Result], [CO2Emission], [HCEmission], [Comments]) VALUES (16, 12, 2, NULL, CAST(N'2025-03-19T00:00:00.000' AS DateTime), N'đạt', CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(5, 2)), N'0')
GO
INSERT [dbo].[InspectionRecords] ([RecordID], [VehicleID], [StationID], [InspectorID], [InspectionDate], [Result], [CO2Emission], [HCEmission], [Comments]) VALUES (17, 11, 3, NULL, CAST(N'2025-03-22T00:00:00.000' AS DateTime), N'ok', CAST(0.01 AS Decimal(5, 2)), CAST(0.01 AS Decimal(5, 2)), N'ok')
GO
INSERT [dbo].[InspectionRecords] ([RecordID], [VehicleID], [StationID], [InspectorID], [InspectionDate], [Result], [CO2Emission], [HCEmission], [Comments]) VALUES (18, 11, 3, NULL, CAST(N'2025-03-22T00:00:00.000' AS DateTime), N'đạt', CAST(0.20 AS Decimal(5, 2)), CAST(0.20 AS Decimal(5, 2)), N'ok')
GO
INSERT [dbo].[InspectionRecords] ([RecordID], [VehicleID], [StationID], [InspectorID], [InspectionDate], [Result], [CO2Emission], [HCEmission], [Comments]) VALUES (19, 13, 5, NULL, CAST(N'2025-03-22T00:00:00.000' AS DateTime), N'dat', CAST(1.00 AS Decimal(5, 2)), CAST(1.00 AS Decimal(5, 2)), N'1')
GO
INSERT [dbo].[InspectionRecords] ([RecordID], [VehicleID], [StationID], [InspectorID], [InspectionDate], [Result], [CO2Emission], [HCEmission], [Comments]) VALUES (20, 14, 3, NULL, CAST(N'2025-03-20T08:54:38.297' AS DateTime), N'1', NULL, NULL, NULL)
GO
INSERT [dbo].[InspectionRecords] ([RecordID], [VehicleID], [StationID], [InspectorID], [InspectionDate], [Result], [CO2Emission], [HCEmission], [Comments]) VALUES (21, 11, 3, NULL, CAST(N'2025-03-21T00:00:00.000' AS DateTime), N'ok', CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(5, 2)), N'1')
GO
INSERT [dbo].[InspectionRecords] ([RecordID], [VehicleID], [StationID], [InspectorID], [InspectionDate], [Result], [CO2Emission], [HCEmission], [Comments]) VALUES (22, 12, 3, NULL, CAST(N'2025-03-21T00:00:00.000' AS DateTime), N'dat', CAST(1.00 AS Decimal(5, 2)), CAST(1.00 AS Decimal(5, 2)), N'1')
GO
INSERT [dbo].[InspectionRecords] ([RecordID], [VehicleID], [StationID], [InspectorID], [InspectionDate], [Result], [CO2Emission], [HCEmission], [Comments]) VALUES (23, 12, 2, NULL, CAST(N'2025-03-23T13:44:48.403' AS DateTime), N'dat', CAST(1.00 AS Decimal(5, 2)), CAST(1.00 AS Decimal(5, 2)), N'1')
GO
INSERT [dbo].[InspectionRecords] ([RecordID], [VehicleID], [StationID], [InspectorID], [InspectionDate], [Result], [CO2Emission], [HCEmission], [Comments]) VALUES (24, 12, 3, NULL, CAST(N'2025-03-26T00:00:00.000' AS DateTime), NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[InspectionRecords] ([RecordID], [VehicleID], [StationID], [InspectorID], [InspectionDate], [Result], [CO2Emission], [HCEmission], [Comments]) VALUES (25, 12, 3, NULL, CAST(N'2025-03-23T15:14:55.437' AS DateTime), N'dat', CAST(1.00 AS Decimal(5, 2)), CAST(1.00 AS Decimal(5, 2)), N'1')
GO
INSERT [dbo].[InspectionRecords] ([RecordID], [VehicleID], [StationID], [InspectorID], [InspectionDate], [Result], [CO2Emission], [HCEmission], [Comments]) VALUES (26, 16, 2, NULL, CAST(N'2025-03-24T10:35:37.590' AS DateTime), N'1', CAST(1.00 AS Decimal(5, 2)), CAST(1.00 AS Decimal(5, 2)), N'1')
GO
INSERT [dbo].[InspectionRecords] ([RecordID], [VehicleID], [StationID], [InspectorID], [InspectionDate], [Result], [CO2Emission], [HCEmission], [Comments]) VALUES (27, 16, 6, NULL, CAST(N'2025-03-24T10:56:44.373' AS DateTime), N'đạt', CAST(1.50 AS Decimal(5, 2)), CAST(1.00 AS Decimal(5, 2)), N'okok')
GO
INSERT [dbo].[InspectionRecords] ([RecordID], [VehicleID], [StationID], [InspectorID], [InspectionDate], [Result], [CO2Emission], [HCEmission], [Comments]) VALUES (28, 16, 6, NULL, CAST(N'2025-03-24T20:19:32.600' AS DateTime), N'1', CAST(1.00 AS Decimal(5, 2)), CAST(1.00 AS Decimal(5, 2)), N'1')
GO
INSERT [dbo].[InspectionStations] ([StationID], [Name], [Address], [Phone], [Email]) VALUES (1, N'Trung tâm Đăng kiểm 50-01S', N'141 Lý Thường Kiệt, Quận 11, TP.HCM', N'02838688251', N'dangkiem5001s@gmail.com')
GO
INSERT [dbo].[InspectionStations] ([StationID], [Name], [Address], [Phone], [Email]) VALUES (2, N'Trung tâm Đăng kiểm 50-02D', N'307 Điện Biên Phủ, Quận Bình Thạnh, TP.HCM', N'02835125926', N'dangkiem5002d@gmail.com')
GO
INSERT [dbo].[InspectionStations] ([StationID], [Name], [Address], [Phone], [Email]) VALUES (3, N'Trung tâm Đăng kiểm 50-03S', N'269 Nguyễn Trọng Tuyển, Quận Tân Bình, TP.HCM', N'02838426255', N'dangkiem5003s@gmail.com')
GO
INSERT [dbo].[InspectionStations] ([StationID], [Name], [Address], [Phone], [Email]) VALUES (4, N'Trung tâm Đăng kiểm 50-04D', N'600 Quang Trung, Quận Gò Vấp, TP.HCM', N'02839890491', N'dangkiem5004d@gmail.com')
GO
INSERT [dbo].[InspectionStations] ([StationID], [Name], [Address], [Phone], [Email]) VALUES (5, N'Trung tâm Đăng kiểm 50-05V', N'1053 Phạm Văn Đồng, Quận Thủ Đức, TP.HCM', N'02837208889', N'dangkiem5005v@gmail.com')
GO
INSERT [dbo].[InspectionStations] ([StationID], [Name], [Address], [Phone], [Email]) VALUES (6, N'trung tâm đăng kiểm 11', N'hanoi 2', N'0582154857', N'huy@gmail.com')
GO
INSERT [dbo].[InspectionStations] ([StationID], [Name], [Address], [Phone], [Email]) VALUES (7, N'trung tâm 1212', N'hhhasd', N'0852126584', N'aaa@gmail.com')
GO
INSERT [dbo].[InspectionStations] ([StationID], [Name], [Address], [Phone], [Email]) VALUES (8, N'123', N'hanoi', N'0528513547', N'hh@gmail.com')
GO
INSERT [dbo].[Notifications] ([NotificationID], [UserID], [Message], [SentDate], [IsRead]) VALUES (1, 16, N'xe vi pham', CAST(N'2025-03-03T00:00:00.000' AS DateTime), 1)
GO
INSERT [dbo].[Notifications] ([NotificationID], [UserID], [Message], [SentDate], [IsRead]) VALUES (2, 16, N'loi', CAST(N'2525-02-02T00:00:00.000' AS DateTime), 0)
GO
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Password], [Role], [Phone], [Address]) VALUES (2, N'Trần Thị Bình', N'tranthibinh@gmail.com', N'hashed_password_456', N'Chủ phương tiện', N'0912345678', N'45 Lê Lợi, Quận 5, TP.HCM')
GO
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Password], [Role], [Phone], [Address]) VALUES (3, N'Lê Văn Cường', N'levancuong@gmail.com', N'hashed_password_789', N'Công nhân kiểm tra', N'0923456789', N'78 Võ Văn Tần, Quận 3, TP.HCM')
GO
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Password], [Role], [Phone], [Address]) VALUES (4, N'Phạm Thị Dung', N'phamthidung@gmail.com', N'hashed_password_101', N'Công nhân kiểm tra', N'0934567890', N'90 Điện Biên Phủ, Quận Bình Thạnh, TP.HCM')
GO
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Password], [Role], [Phone], [Address]) VALUES (5, N'Hoàng Văn Em', N'hoangvanem@gmail.com', N'hashed_password_202', N'Cảnh sát giao thông', N'0945678901', N'111 Nam Kỳ Khởi Nghĩa, Quận 10, TP.HCM')
GO
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Password], [Role], [Phone], [Address]) VALUES (6, N'Ngô Thị Phương', N'ngothiphuong@gmail.com', N'hashed_password_303', N'Công nhân kiểm tra', N'0956789012', N'222 Nguyễn Văn Cừ, Quận 5, TP.HCM')
GO
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Password], [Role], [Phone], [Address]) VALUES (7, N'Đặng Văn Giáp', N'dangvangiap@gmail.com', N'hashed_password_404', N'Cảnh sát giao thông', N'0967890123', N'333 Đường 3/2, Quận 10, TP.HCM')
GO
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Password], [Role], [Phone], [Address]) VALUES (8, N'Lý Thị Hương', N'lythihuong@gmail.com', N'hashed_password_505', N'Cảnh sát giao thông', N'0978901234', N'444 Cách Mạng Tháng 8, Quận 3, TP.HCM')
GO
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Password], [Role], [Phone], [Address]) VALUES (9, N'Vũ Văn Ích', N'vuvanich@gmail.com', N'hashed_password_606', N'Chủ phương tiện', N'0989012345', N'555 Lý Thường Kiệt, Quận 10, TP.HCM')
GO
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Password], [Role], [Phone], [Address]) VALUES (10, N'Mai Thị Kiều', N'maithikieu@gmail.com', N'hashed_password_707', N'Chủ phương tiện', N'0990123456', N'666 Nguyễn Đình Chiểu, Quận 3, TP.HCM')
GO
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Password], [Role], [Phone], [Address]) VALUES (11, N'huypham', N'huynnn@gmail.com', N'5mKeBmwqKEYfrPAtxcXaN5q51PmzaWSZ9gUI+aLtV7h22y9q', N'Chủ phương tiện', N'0856633135', N'ninh binh')
GO
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Password], [Role], [Phone], [Address]) VALUES (12, N'huypham', N'huyaa@gmail.com', N'jM3Xayh9zfEWgag2PyPkvWJj4qK4+ANiiDI464DM/Q2hsKRx', N'Chủ phương tiện', N'0856611252', N'aa')
GO
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Password], [Role], [Phone], [Address]) VALUES (13, N'test', N'test@gmail.com', N'EupcpIENy5QRh+7+55+bMw26wBMmqu0EqX4yLiUO3Zq8B8Wx', N'Chủ phương tiện', N'0123123123', N'123')
GO
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Password], [Role], [Phone], [Address]) VALUES (14, N'phamhuy', N'huyhhh@gmail.com', N'xWLfOYNjFJQSsWqlTiHB6F4UtOQAgy9SYyva2ST3/z7VTR8f', N'Chủ phương tiện', N'0853315201', N'a')
GO
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Password], [Role], [Phone], [Address]) VALUES (15, N'huypham', N'huyaaaaa@gmail.com', N'wIMv/jOg+ub4cHdh25UY0EhncPPF7q17NEMQTCfeKec369FA', N'Owner', N'0856633134', N'a')
GO
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Password], [Role], [Phone], [Address]) VALUES (16, N'huyaa', N'huynbkd@gmail.com', N'FHDR9mzwWpQyhOL8jGWSRdY49KlD8lbn3fAT/7SnKoL/eFwI', N'Chủ phương tiện', N'0853215258', N'a')
GO
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Password], [Role], [Phone], [Address]) VALUES (17, N'huyaaaa', N'huyaaadsaa@gmail.com', N'C6IuwDfBWA1Oe6wuOTlgMzXXCxRMV/DI26zQoSwIML8YSjTx', N'Owner', N'0852365485', N'4')
GO
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Password], [Role], [Phone], [Address]) VALUES (18, N'phamhuy123', N'huy112233@gmail.com', N'xEunqwMAW4ytvkwBFZpK5s31vpAzlui8APMOf1zfE5WjVK9h', N'Owner', N'0852135425', N'hanoi')
GO
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Password], [Role], [Phone], [Address]) VALUES (19, N'dfgvdfgdfg', N'huyqwer@gmail.com', N'vO+5QUySNwX54i4GQxRBLrXcFPT95DQFI/Bc/zE7vTtPg+iS', N'Công nhân kiểm tra', N'0542185725', N'122')
GO
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Password], [Role], [Phone], [Address]) VALUES (20, N'admin', N'admin@gmail.com', N'1m9cocUMOwAi2LGad1ApLuhif2BBdh/wwyvB8bDf5s9FwW2H', N'Quản trị viên', N'0521584521', N'aa')
GO
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Password], [Role], [Phone], [Address]) VALUES (21, N'huyaaaa', N'huyasd@gmail.com', N'rWpDh4TCIUhVtHNAv/IFB7306drw6BOCId/bkpUKt4ZXcbfD', N'Cảnh sát giao thông', N'0528542515', N'123456')
GO
INSERT [dbo].[Vehicles] ([VehicleID], [OwnerID], [PlateNumber], [Brand], [Model], [ManufactureYear], [EngineNumber]) VALUES (1, 1, N'51F-123.45', N'Toyota', N'Vios', 2018, N'T18B123456789')
GO
INSERT [dbo].[Vehicles] ([VehicleID], [OwnerID], [PlateNumber], [Brand], [Model], [ManufactureYear], [EngineNumber]) VALUES (2, 1, N'51F-456.78', N'Honda', N'City', 2019, N'H19C987654321')
GO
INSERT [dbo].[Vehicles] ([VehicleID], [OwnerID], [PlateNumber], [Brand], [Model], [ManufactureYear], [EngineNumber]) VALUES (3, 2, N'51G-789.01', N'Mazda', N'CX-5', 2020, N'M20D567891234')
GO
INSERT [dbo].[Vehicles] ([VehicleID], [OwnerID], [PlateNumber], [Brand], [Model], [ManufactureYear], [EngineNumber]) VALUES (4, 2, N'51G-901.23', N'Hyundai', N'Accent', 2021, N'H21E432198765')
GO
INSERT [dbo].[Vehicles] ([VehicleID], [OwnerID], [PlateNumber], [Brand], [Model], [ManufactureYear], [EngineNumber]) VALUES (5, 9, N'51H-345.67', N'Ford', N'Ranger', 2022, N'F22F789123456')
GO
INSERT [dbo].[Vehicles] ([VehicleID], [OwnerID], [PlateNumber], [Brand], [Model], [ManufactureYear], [EngineNumber]) VALUES (6, 9, N'51H-678.90', N'Kia', N'Seltos', 2021, N'K21G654321987')
GO
INSERT [dbo].[Vehicles] ([VehicleID], [OwnerID], [PlateNumber], [Brand], [Model], [ManufactureYear], [EngineNumber]) VALUES (7, 10, N'51K-231.45', N'Toyota', N'Fortuner', 2020, N'T20H321456789')
GO
INSERT [dbo].[Vehicles] ([VehicleID], [OwnerID], [PlateNumber], [Brand], [Model], [ManufactureYear], [EngineNumber]) VALUES (8, 10, N'51K-564.78', N'Mitsubishi', N'Xpander', 2019, N'M19I198765432')
GO
INSERT [dbo].[Vehicles] ([VehicleID], [OwnerID], [PlateNumber], [Brand], [Model], [ManufactureYear], [EngineNumber]) VALUES (9, 1, N'51L-897.01', N'Nissan', N'Navara', 2021, N'N21J789456123')
GO
INSERT [dbo].[Vehicles] ([VehicleID], [OwnerID], [PlateNumber], [Brand], [Model], [ManufactureYear], [EngineNumber]) VALUES (10, 2, N'51L-230.45', N'Suzuki', N'XL7', 2022, N'S22K456123789')
GO
INSERT [dbo].[Vehicles] ([VehicleID], [OwnerID], [PlateNumber], [Brand], [Model], [ManufactureYear], [EngineNumber]) VALUES (11, 16, N'32Q-12343', N'vios', N'123', 2000, N'123213')
GO
INSERT [dbo].[Vehicles] ([VehicleID], [OwnerID], [PlateNumber], [Brand], [Model], [ManufactureYear], [EngineNumber]) VALUES (12, 16, N'33A-15254', N'112asd', N'1111111', 2000, N'sdfs14')
GO
INSERT [dbo].[Vehicles] ([VehicleID], [OwnerID], [PlateNumber], [Brand], [Model], [ManufactureYear], [EngineNumber]) VALUES (13, 16, N'30Q-25125', N'huyndai', N'i10', 2020, N'31asf535d')
GO
INSERT [dbo].[Vehicles] ([VehicleID], [OwnerID], [PlateNumber], [Brand], [Model], [ManufactureYear], [EngineNumber]) VALUES (14, 18, N'30A-12355', N'1sads', N'ssd', 2020, N's3d5vsv')
GO
INSERT [dbo].[Vehicles] ([VehicleID], [OwnerID], [PlateNumber], [Brand], [Model], [ManufactureYear], [EngineNumber]) VALUES (15, 18, N'25A-15248', N'123', N'546f', 2020, N'1425dv')
GO
INSERT [dbo].[Vehicles] ([VehicleID], [OwnerID], [PlateNumber], [Brand], [Model], [ManufactureYear], [EngineNumber]) VALUES (16, 16, N'35A-12525', N'toyota', N'cam', 2020, N'1sd6gsg5')
GO
ALTER TABLE [dbo].[InspectionRecords]  WITH CHECK ADD FOREIGN KEY([InspectorID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[InspectionRecords]  WITH CHECK ADD FOREIGN KEY([StationID])
REFERENCES [dbo].[InspectionStations] ([StationID])
GO
ALTER TABLE [dbo].[InspectionRecords]  WITH CHECK ADD FOREIGN KEY([VehicleID])
REFERENCES [dbo].[Vehicles] ([VehicleID])
GO
ALTER TABLE [dbo].[Logs]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Notifications]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
USE [master]
GO
ALTER DATABASE [CarInspectionDB] SET  READ_WRITE 
GO
