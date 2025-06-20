USE [master]
GO
/****** Object:  Database [OPS_PROD]    Script Date: 12/06/2025 16:32:33 ******/
CREATE DATABASE [OPS_PROD]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'OPS_PROD', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\OPS_PROD.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'OPS_PROD_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\OPS_PROD_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [OPS_PROD] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [OPS_PROD].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [OPS_PROD] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [OPS_PROD] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [OPS_PROD] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [OPS_PROD] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [OPS_PROD] SET ARITHABORT OFF 
GO
ALTER DATABASE [OPS_PROD] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [OPS_PROD] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [OPS_PROD] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [OPS_PROD] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [OPS_PROD] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [OPS_PROD] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [OPS_PROD] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [OPS_PROD] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [OPS_PROD] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [OPS_PROD] SET  DISABLE_BROKER 
GO
ALTER DATABASE [OPS_PROD] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [OPS_PROD] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [OPS_PROD] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [OPS_PROD] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [OPS_PROD] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [OPS_PROD] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [OPS_PROD] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [OPS_PROD] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [OPS_PROD] SET  MULTI_USER 
GO
ALTER DATABASE [OPS_PROD] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [OPS_PROD] SET DB_CHAINING OFF 
GO
ALTER DATABASE [OPS_PROD] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [OPS_PROD] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [OPS_PROD] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [OPS_PROD] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [OPS_PROD] SET QUERY_STORE = ON
GO
ALTER DATABASE [OPS_PROD] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [OPS_PROD]
GO
/****** Object:  Table [dbo].[DMSConcession]    Script Date: 12/06/2025 16:32:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DMSConcession](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Order_Number] [varchar](max) NULL,
	[Batch_Number] [varchar](max) NULL,
	[Part_Number] [varchar](max) NULL,
	[Description] [varchar](max) NULL,
	[Concession] [varchar](max) NULL,
	[Remarks] [varchar](max) NULL,
	[LabelForm] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DMSFIKFC]    Script Date: 12/06/2025 16:32:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DMSFIKFC](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Doc_Number] [int] NOT NULL,
	[KFC_Number] [varchar](max) NULL,
	[Customer] [varchar](max) NULL,
	[Part_Number] [varchar](max) NULL,
	[Description] [varchar](max) NULL,
	[Issue] [varchar](max) NULL,
	[Issue_Date] [date] NULL,
	[Batch_Number] [varchar](max) NULL,
	[Rejection_Ref] [varchar](max) NULL,
	[Originator] [varchar](max) NULL,
	[Status] [varchar](max) NULL,
	[Closed_Date] [date] NULL,
	[Remarks] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DMSFILog]    Script Date: 12/06/2025 16:32:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DMSFILog](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Doc_Number] [int] NULL,
	[Order_Number] [varchar](max) NULL,
	[Part_Number] [varchar](max) NULL,
	[Description] [varchar](max) NULL,
	[Drawing_Rev] [varchar](50) NULL,
	[Batch_Number] [varchar](50) NULL,
	[Qty] [numeric](18, 0) NULL,
	[Acc] [numeric](18, 0) NULL,
	[Rejected] [numeric](18, 0) NULL,
	[Job_Type] [varchar](max) NULL,
	[Operation] [varchar](max) NULL,
	[Start_Date] [date] NULL,
	[Start_Time] [time](7) NULL,
	[End_Date] [date] NULL,
	[End_Time] [time](7) NULL,
	[Labor] [decimal](18, 2) NULL,
	[Week] [varchar](max) NULL,
	[EID] [varchar](max) NULL,
	[Name] [varchar](max) NULL,
	[Std_Price] [decimal](18, 2) NULL,
	[Cell] [varchar](max) NULL,
	[QN] [varchar](max) NULL,
	[KFR] [varchar](max) NULL,
	[Concession] [varchar](max) NULL,
	[Production_Permit] [varchar](max) NULL,
	[Serial_Number] [varchar](max) NULL,
	[Remarks] [varchar](max) NULL,
	[Status] [varchar](50) NULL,
 CONSTRAINT [PK_DMSFILog] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DMSMaterialMaster]    Script Date: 12/06/2025 16:32:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DMSMaterialMaster](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Part_Number] [varchar](128) NULL,
	[Description] [varchar](128) NULL,
	[CoS] [decimal](18, 2) NULL,
	[Price] [decimal](18, 2) NULL,
	[Weight] [decimal](18, 2) NULL,
	[Shipping_Point] [varchar](128) NULL,
	[Bin_Number] [varchar](128) NULL,
	[Customer_Name] [varchar](128) NULL,
	[Serialize] [varchar](128) NULL,
	[Batch_Size] [varchar](max) NULL,
	[OuterContent] [numeric](18, 0) NULL,
	[Cell] [varchar](max) NULL,
	[FI_Std_Labor] [decimal](18, 2) NULL,
	[FI_Tag] [varchar](50) NULL,
	[Box_Type] [varchar](max) NULL,
	[Rec_Item_Check] [varchar](max) NULL,
	[Rev_Check] [varchar](50) NULL,
	[FG_CT] [decimal](18, 2) NULL,
	[Alert_Number] [int] NULL,
 CONSTRAINT [PK_DMSMaterialMaster] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DMSProductionPermit]    Script Date: 12/06/2025 16:32:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DMSProductionPermit](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Part_Number] [varchar](max) NULL,
	[Description] [varchar](max) NULL,
	[Production_Permit] [varchar](max) NULL,
	[Start_Date] [date] NULL,
	[End_Date] [date] NULL,
	[Remarks] [varchar](max) NULL,
	[Qty] [numeric](18, 0) NULL,
	[Remaining_Qty] [numeric](18, 0) NULL,
	[Status] [varchar](max) NULL,
	[Alert_Qty] [numeric](18, 0) NULL,
	[Alert_Email] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DMSSRouting]    Script Date: 12/06/2025 16:32:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DMSSRouting](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Part_Number] [varchar](250) NULL,
	[Work_Center] [varchar](250) NULL,
	[Operation] [varchar](250) NULL,
 CONSTRAINT [PK_DMSSRouting] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[eLogJIG]    Script Date: 12/06/2025 16:32:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[eLogJIG](
	[Jig_Drawing_Number] [varchar](315) NOT NULL,
	[Jig_Description] [varchar](315) NULL,
	[Part_Number] [varchar](315) NULL,
	[Part_Description] [varchar](315) NULL,
	[Serial_Number] [varchar](315) NULL,
	[Process_Name] [varchar](315) NULL,
	[Line] [varchar](315) NULL,
	[Load_Number] [int] NULL,
	[Max_Loading_Number] [varchar](315) NULL,
	[Load_Warning] [int] NULL,
	[Life_Time] [int] NULL,
	[Expiry_Date] [datetime] NULL,
	[Exchange_Type] [varchar](315) NULL,
	[Status] [varchar](315) NULL,
	[Remarks] [varchar](315) NULL,
 CONSTRAINT [PK_eLogJIG] PRIMARY KEY CLUSTERED 
(
	[Jig_Drawing_Number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[eLOGJigRecord]    Script Date: 12/06/2025 16:32:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[eLOGJigRecord](
	[PartNumber] [nvarchar](255) NULL,
	[ProcessName] [nvarchar](255) NULL,
	[JigNumber] [nvarchar](255) NULL,
	[JigSerialNumber] [nvarchar](255) NULL,
	[LoadNumber] [int] NULL,
	[DateOfStartUse] [datetime] NULL,
	[Status] [nvarchar](255) NULL,
	[Remarks] [nvarchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[eLOGLoginInformation]    Script Date: 12/06/2025 16:32:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[eLOGLoginInformation](
	[OK] [nvarchar](255) NULL,
	[UserName] [nvarchar](255) NULL,
	[Name] [nvarchar](50) NULL,
	[Base] [nvarchar](255) NULL,
	[EmailAddress] [nvarchar](255) NULL,
	[Password] [nvarchar](255) NULL,
	[Group] [nvarchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[eLOGMachineList]    Script Date: 12/06/2025 16:32:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[eLOGMachineList](
	[ProcessName] [nvarchar](255) NULL,
	[MachineNumber] [nvarchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[eLOGMainTable]    Script Date: 12/06/2025 16:32:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[eLOGMainTable](
	[RefNo] [nvarchar](255) NULL,
	[OrderNumber] [nvarchar](255) NULL,
	[PartNumber] [nvarchar](255) NULL,
	[Description] [nvarchar](255) NULL,
	[BatchNumber] [nvarchar](255) NULL,
	[StartDate] [datetime] NULL,
	[StartTime] [time](7) NULL,
	[FinishDate] [datetime] NULL,
	[FinishTime] [time](7) NULL,
	[Process] [nvarchar](255) NULL,
	[Operation] [nvarchar](255) NULL,
	[Quantity] [int] NULL,
	[Acc] [int] NULL,
	[Rework] [int] NULL,
	[Scrap] [int] NULL,
	[Specification] [nvarchar](255) NULL,
	[ReportNo] [nvarchar](255) NULL,
	[NCRNo] [nvarchar](255) NULL,
	[Stamp] [nvarchar](255) NULL,
	[EID] [nvarchar](255) NULL,
	[OperatorName] [nvarchar](255) NULL,
	[ReportedDate] [datetime] NULL,
	[ReportedShift] [nvarchar](255) NULL,
	[Line] [nvarchar](255) NULL,
	[Machine] [nvarchar](255) NULL,
	[Remarks] [nvarchar](255) NULL,
	[JigNumber] [nvarchar](255) NULL,
	[JigSerialNumber] [nvarchar](255) NULL,
	[UserAccount] [nvarchar](255) NULL,
	[UserName] [nvarchar](255) NULL,
	[Room_Temperature] [nvarchar](255) NULL,
	[Penetrant_Temperature] [nvarchar](255) NULL,
	[Reject] [varchar](4) NULL,
	[SpecRev] [nvarchar](255) NULL,
	[AccptCriteria] [nvarchar](255) NULL,
	[AccCriteriaRev] [nvarchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[eLOGMaterialMaster]    Script Date: 12/06/2025 16:32:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[eLOGMaterialMaster](
	[OrderNumber] [nvarchar](255) NULL,
	[PartNumber] [nvarchar](255) NULL,
	[Description] [nvarchar](255) NULL,
	[Quantity] [float] NULL,
	[BatchNumber] [nvarchar](255) NULL,
	[Order_Status] [varchar](max) NULL,
	[Current_OP] [varchar](50) NULL,
	[Current_WC] [varchar](max) NULL,
	[id] [int] IDENTITY(1,1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[eLOGMiniRouting]    Script Date: 12/06/2025 16:32:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[eLOGMiniRouting](
	[PartNumber] [nvarchar](255) NULL,
	[ProcessName] [nvarchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[eLOGPlan]    Script Date: 12/06/2025 16:32:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[eLOGPlan](
	[Date] [int] NULL,
	[HT] [int] NULL,
	[ECM] [int] NULL,
	[BRL] [int] NULL,
	[NDT] [int] NULL,
	[Line-1] [int] NULL,
	[Line-2] [int] NULL,
	[Line-3] [int] NULL,
	[Line-4] [int] NULL,
	[Line-5] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[eLOGProcessName]    Script Date: 12/06/2025 16:32:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[eLOGProcessName](
	[Line] [nvarchar](255) NULL,
	[ProcessName] [nvarchar](255) NULL,
	[LoadingNumber] [int] NULL,
	[LoadingProfile] [nvarchar](255) NULL,
	[CheckMonth] [nvarchar](255) NULL,
	[ReportNumberPrefix] [nvarchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[eLOGQualityAlert]    Script Date: 12/06/2025 16:32:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[eLOGQualityAlert](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Part_Number] [varchar](100) NULL,
	[Image] [varchar](150) NULL,
 CONSTRAINT [PK_eLOGQualityAlert] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[eLOGRefNo]    Script Date: 12/06/2025 16:32:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[eLOGRefNo](
	[OK] [nvarchar](255) NULL,
	[RefNo] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[eLOGSpecification]    Script Date: 12/06/2025 16:32:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[eLOGSpecification](
	[PartNumber] [nvarchar](255) NULL,
	[ProcessName] [nvarchar](255) NULL,
	[Specification] [nvarchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[eLOGStamp]    Script Date: 12/06/2025 16:32:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[eLOGStamp](
	[UserName] [nvarchar](255) NULL,
	[StampNumber] [nvarchar](255) NULL,
	[IDNumber] [nvarchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[eLOGUserList]    Script Date: 12/06/2025 16:32:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[eLOGUserList](
	[UserName] [nvarchar](50) NULL,
	[Name] [nvarchar](255) NULL,
	[Base] [nvarchar](255) NULL,
	[EmailAddress] [nvarchar](255) NULL,
	[Password] [nvarchar](255) NULL,
	[Group] [nvarchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EngineeringPortal_PartNumber]    Script Date: 12/06/2025 16:32:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EngineeringPortal_PartNumber](
	[PartNumber] [varchar](100) NOT NULL,
	[PartName] [varchar](250) NULL,
	[DrawingID] [varchar](100) NULL,
	[FamilyPart] [varchar](50) NULL,
	[Customer_ID] [varchar](500) NULL,
	[Cutomer2_ID] [varchar](50) NULL,
	[Customer3_ID] [varchar](50) NULL,
	[ProcessPlanNo] [varchar](50) NULL,
	[part_image] [varchar](255) NULL,
	[PartType] [varchar](100) NULL,
	[cost] [varchar](50) NULL,
 CONSTRAINT [PK_EngineeringPortal_PartNumber] PRIMARY KEY CLUSTERED 
(
	[PartNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
USE [master]
GO
ALTER DATABASE [OPS_PROD] SET  READ_WRITE 
GO
