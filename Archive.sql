USE [PPRS1]
GO
/****** Object:  StoredProcedure [dbo].[DataArchive]    Script Date: 12/04/2012 16:46:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Brett Snyder
-- Create date: 11/21/2012
-- Description:	
-- =============================================
ALTER PROCEDURE [dbo].[DataArchive] 
	-- Add the parameters for the stored procedure here
	@ArchiveDate as DateTime
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

BEGIN TRANSACTION;


-- [EmploymentSource]
SET IDENTITY_INSERT [PPRS1_History].[dbo].[EmploymentSource] ON 
INSERT INTO [PPRS1_History].[dbo].[EmploymentSource] ( [Id],[FacilityId],[SourceCode],[SourceName],[LastModifiedBy],[LastModifiedOn],[Active] )
SELECT  [Id],[FacilityId],[SourceCode],	[SourceName],[LastModifiedBy],[LastModifiedOn],[Active] FROM [PPRS1].[dbo].[EmploymentSource]
WHERE [Id] NOT IN (SELECT [Id] FROM [PPRS1_History].[dbo].[EmploymentSource])
SET IDENTITY_INSERT [PPRS1_History].[dbo].[EmploymentSource] OFF 

-- [EmployeeTempHold]
INSERT INTO [PPRS1_History].[dbo].[EmployeeTempHold] 
	([Id],[EmployeeNumber],[TempNumber],[LastName],[FirstName],[MiddleInitial],[HireDate],[InactiveDate],
	[TempStartDate],[TerminationDate],[Address1],[Address2],[City],[State],[Zip],[SSN],[BirthDate],
	[EmployeePhone],[EmergencyContact],[ContactPhone],[TempEmployee],[Salaried],[EmploymentSourceId],
	[FacilityID],[LastModifiedBy],[LastModifiedOn],[Active])
SELECT [Id],[EmployeeNumber],[TempNumber],[LastName],[FirstName],[MiddleInitial],[HireDate],[InactiveDate],
	[TempStartDate],[TerminationDate],[Address1],[Address2],[City],[State],[Zip],[SSN],[BirthDate],
	[EmployeePhone],[EmergencyContact],[ContactPhone],[TempEmployee],[Salaried],[EmploymentSourceId],
	[FacilityID],[LastModifiedBy],[LastModifiedOn],[Active]  
FROM [PPRS1].[dbo].[EmployeeTempHold]	
WHERE [Id] NOT IN (SELECT [Id] FROM [PPRS1_History].[dbo].[EmployeeTempHold])
 
-- [EmployeeTaskWorkedPay] 
INSERT INTO [PPRS1_History].[dbo].[EmployeeTaskWorkedPay] 
([EmployeeTaskWorkedId], [PayMultiplier], [PayRate], [HoursPaid],
[PayAmount], [PayrollStatus], [LastModifiedBy], [LastModifiedOn] )
SELECT 
 tblSource.[EmployeeTaskWorkedId], tblSource.[PayMultiplier], tblSource.[PayRate], tblSource.[HoursPaid],
 tblSource.[PayAmount], tblSource.[PayrollStatus], tblSource.[LastModifiedBy], tblSource.[LastModifiedOn]
FROM [PPRS1].[dbo].[EmployeeTaskWorkedPay] AS tblSource
LEFT JOIN [PPRS1_History].[dbo].[EmployeeTaskWorkedPay] As tblDest
ON tblSource.[EmployeeTaskWorkedId] = tblDest.[EmployeeTaskWorkedId]
AND tblSource.[PayMultiplier] = tblDest.[PayMultiplier]
WHERE tblDest.[EmployeeTaskWorkedId] IS NULL

 
-- [EmployeeTaskWorked]
SET IDENTITY_INSERT [PPRS1_History].[dbo].[EmployeeTaskWorked] ON 
INSERT INTO [PPRS1_History].[dbo].[EmployeeTaskWorked] 
([Id],[TaskID],[OtherTaskID],[FacilityID],[EmployeeId],[RebillDetailID],[WorkDate],
	[ShiftID],[UPM],[HoursWorked],[OutOfTownType],[PayrollStatus],[LastModifiedBy],
	[LastModifiedOn],[Notes],[RebillSubTaskID] )
SELECT 
 	[Id],[TaskID],[OtherTaskID],[FacilityID],[EmployeeId],[RebillDetailID],[WorkDate],
	[ShiftID],[UPM],[HoursWorked],[OutOfTownType],[PayrollStatus],[LastModifiedBy],
	[LastModifiedOn],[Notes],[RebillSubTaskID]
FROM [PPRS1].[dbo].[EmployeeTaskWorked]
WHERE [Id] NOT IN (SELECT [Id] FROM [PPRS1_History].[dbo].[EmployeeTaskWorked])
SET IDENTITY_INSERT [PPRS1_History].[dbo].[EmployeeTaskWorked] OFF 

-- [EmployeeRebillRates]
SET IDENTITY_INSERT [PPRS1_History].[dbo].[EmployeeRebillRates] ON 
INSERT INTO [PPRS1_History].[dbo].[EmployeeRebillRates] 
([ID],[FacilityID],[TaskID],[ShiftID],[EmployeeID],[UnitsPayRate],
[HoursPayRate],[EffectiveDate],[LastModifiedBy],[LastModifiedOn] )
SELECT 
 	[ID],[FacilityID],[TaskID],[ShiftID],[EmployeeID],[UnitsPayRate],
	[HoursPayRate],[EffectiveDate],[LastModifiedBy],[LastModifiedOn]
FROM [PPRS1].[dbo].[EmployeeRebillRates]
WHERE [Id] NOT IN (SELECT [Id] FROM [PPRS1_History].[dbo].[EmployeeRebillRates])
SET IDENTITY_INSERT [PPRS1_History].[dbo].[EmployeeRebillRates] OFF

-- [EmployeeRates]
SET IDENTITY_INSERT [PPRS1_History].[dbo].[EmployeeRates] ON 
INSERT INTO [PPRS1_History].[dbo].[EmployeeRates] 
([ID],[FacilityID],[TaskID],[ShiftID],[EmployeeID],[UnitsPayRate],
	[HoursPayRate],[EffectiveDate],[LastModifiedBy],[LastModifiedOn])
SELECT 
 	[ID],[FacilityID],[TaskID],[ShiftID],[EmployeeID],[UnitsPayRate],
	[HoursPayRate],[EffectiveDate],[LastModifiedBy],[LastModifiedOn]
FROM [PPRS1].[dbo].[EmployeeRates]
WHERE [Id] NOT IN (SELECT [Id] FROM [PPRS1_History].[dbo].[EmployeeRates])
SET IDENTITY_INSERT [PPRS1_History].[dbo].[EmployeeRates] OFF

-- [Employeehold2]
INSERT INTO [PPRS1_History].[dbo].[Employeehold2] 
([Id],[EmployeeNumber],[TempNumber],[LastName],[FirstName],[MiddleInitial],[HireDate],[InactiveDate],
	[TempStartDate],[TerminationDate],[Address1],[Address2],[City],[State],[Zip],[SSN],[BirthDate],
	[EmployeePhone],[EmergencyContact],[ContactPhone],[TempEmployee],[Salaried],[EmploymentSourceId],
	[FacilityID],[LastModifiedBy],[LastModifiedOn],[Active] )
SELECT 
 	[Id],[EmployeeNumber],[TempNumber],[LastName],[FirstName],[MiddleInitial],[HireDate],[InactiveDate],
	[TempStartDate],[TerminationDate],[Address1],[Address2],[City],[State],[Zip],[SSN],[BirthDate],
	[EmployeePhone],[EmergencyContact],[ContactPhone],[TempEmployee],[Salaried],[EmploymentSourceId],
	[FacilityID],[LastModifiedBy],[LastModifiedOn],[Active]
FROM [PPRS1].[dbo].[Employeehold2]
WHERE [Id] NOT IN (SELECT [Id] FROM [PPRS1_History].[dbo].[Employeehold2])

-- [Employee]
SET IDENTITY_INSERT [PPRS1_History].[dbo].[Employee] ON 
INSERT INTO [PPRS1_History].[dbo].[Employee] 
([Id],[EmployeeNumber],[TempNumber],[LastName],[FirstName],[MiddleInitial],[HireDate],[InactiveDate],
	[TempStartDate],[TerminationDate],[Address1],[Address2],[City],[State],[Zip],[SSN],[BirthDate],
	[EmployeePhone],[EmergencyContact],[ContactPhone],[TempEmployee],[Salaried],[EmploymentSourceId],
	[FacilityID],[LastModifiedBy],[LastModifiedOn],[Active] )
SELECT 
 	[Id],[EmployeeNumber],[TempNumber],[LastName],[FirstName],[MiddleInitial],[HireDate],[InactiveDate],
	[TempStartDate],[TerminationDate],[Address1],[Address2],[City],[State],[Zip],[SSN],[BirthDate],
	[EmployeePhone],[EmergencyContact],[ContactPhone],[TempEmployee],[Salaried],[EmploymentSourceId],
	[FacilityID],[LastModifiedBy],[LastModifiedOn],[Active]
FROM [PPRS1].[dbo].[Employee]
WHERE [Id] NOT IN (SELECT [Id] FROM [PPRS1_History].[dbo].[Employee])
SET IDENTITY_INSERT [PPRS1_History].[dbo].[Employee] OFF


-- [AssociatedFacility]
INSERT INTO [PPRS1_History].[dbo].[AssociatedFacility] 
([FacilityID],[AssociatedFacilityID] )
SELECT 
 tblSource.[FacilityID],tblSource.[AssociatedFacilityID]
FROM [PPRS1].[dbo].[AssociatedFacility] AS tblSource
LEFT JOIN [PPRS1_History].[dbo].[AssociatedFacility] AS tblDest
ON tblSource.[FacilityID] = tblDest.[FacilityID] 
AND tblSource.[AssociatedFacilityID] = tblDest.[AssociatedFacilityID] 
WHERE tblDest.[FacilityID] IS NULL

-- [UserTypes]
SET IDENTITY_INSERT [PPRS1_History].[dbo].[UserTypes] ON 
INSERT INTO [PPRS1_History].[dbo].[UserTypes] 
([id],[Type] )
SELECT 
 [id],[Type]
FROM [PPRS1].[dbo].[UserTypes]
WHERE [Id] NOT IN (SELECT [Id] FROM [PPRS1_History].[dbo].[UserTypes])
SET IDENTITY_INSERT [PPRS1_History].[dbo].[UserTypes] OFF

-- [UserProfile]
SET IDENTITY_INSERT [PPRS1_History].[dbo].[UserProfile] ON 
INSERT INTO [PPRS1_History].[dbo].[UserProfile] 
([Id],[UserID],[UserName],[Password],[UserType],
	[UserLongName],[LastModifiedBy],[LastModifiedOn] )
SELECT 
 	[Id],[UserID],[UserName],[Password],[UserType],
	[UserLongName],[LastModifiedBy],[LastModifiedOn]
FROM [PPRS1].[dbo].[UserProfile]
WHERE [Id] NOT IN (SELECT [Id] FROM [PPRS1_History].[dbo].[UserProfile])
SET IDENTITY_INSERT [PPRS1_History].[dbo].[UserProfile] OFF


-- [Teams]
SET IDENTITY_INSERT [PPRS1_History].[dbo].[Teams] ON 
INSERT INTO [PPRS1_History].[dbo].[Teams] 
([ID],[TeamName],[FacilityID],[TeamMembers],[LastModifiedBy],[LastModifiedOn],[Active])
SELECT 
 [ID],[TeamName],[FacilityID],[TeamMembers],[LastModifiedBy],[LastModifiedOn],[Active]
FROM [PPRS1].[dbo].[Teams]
WHERE [Id] NOT IN (SELECT [Id] FROM [PPRS1_History].[dbo].[Teams])
SET IDENTITY_INSERT [PPRS1_History].[dbo].[Teams] OFF
	

--[Tasks]
SET IDENTITY_INSERT [PPRS1_History].[dbo].[Tasks] ON 
INSERT INTO [PPRS1_History].[dbo].[Tasks] 
([Id],[TaskCode],[TaskDescription],[GLAcctNumber],[PayType],
	[Rebillable],[LastModifiedBy],[LastModifiedOn],[Active] )
SELECT 
 	[Id],[TaskCode],[TaskDescription],[GLAcctNumber],[PayType],
	[Rebillable],[LastModifiedBy],[LastModifiedOn],[Active]
FROM [PPRS1].[dbo].[Tasks]
WHERE [Id] NOT IN (SELECT [Id] FROM [PPRS1_History].[dbo].[Tasks])
SET IDENTITY_INSERT [PPRS1_History].[dbo].[Tasks] OFF


--[Shifts]
SET IDENTITY_INSERT [PPRS1_History].[dbo].[Shifts] ON 
INSERT INTO [PPRS1_History].[dbo].[Shifts] 
([ID],[Shift] )
SELECT 
 [ID],[Shift]
FROM [PPRS1].[dbo].[Shifts]
WHERE [Id] NOT IN (SELECT [Id] FROM [PPRS1_History].[dbo].[Shifts])
SET IDENTITY_INSERT [PPRS1_History].[dbo].[Shifts] OFF

--[RegionalRights]
INSERT INTO [PPRS1_History].[dbo].[RegionalRights] 
( [UserProfileId],[RegionId],[LastModifiedBy],[LastModifiedOn])
SELECT 
tblSource.[UserProfileId],tblSource.[RegionId],tblSource.[LastModifiedBy],tblSource.[LastModifiedOn]
FROM [PPRS1].[dbo].[RegionalRights] AS tblSource
LEFT JOIN [PPRS1_History].[dbo].[RegionalRights] As tblDest
ON tblSource.[UserProfileId] = tblDest.[UserProfileId]
AND tblSource.[RegionId] = tblDest.[RegionId]
WHERE tblDest.[UserProfileId] IS NULL

-- [RebillSubTasks]
SET IDENTITY_INSERT [PPRS1_History].[dbo].[RebillSubTasks] ON 
INSERT INTO [PPRS1_History].[dbo].[RebillSubTasks] 
([Id],[Description],[TaskID],[FacilityCustomerId],
	[LastModifiedBy],[LastModifiedOn],[Active],[HoursOrUnits] )
SELECT 
 	[Id],[Description],[TaskID],[FacilityCustomerId],
	[LastModifiedBy],[LastModifiedOn],[Active],[HoursOrUnits]
FROM [PPRS1].[dbo].[RebillSubTasks]
WHERE [Id] NOT IN (SELECT [Id] FROM [PPRS1_History].[dbo].[RebillSubTasks])
SET IDENTITY_INSERT [PPRS1_History].[dbo].[RebillSubTasks] OFF

--[RebillSubTaskRates]
SET IDENTITY_INSERT [PPRS1_History].[dbo].[RebillSubTaskRates] ON 
INSERT INTO [PPRS1_History].[dbo].[RebillSubTaskRates] 
([Id],[RebillSubTasksId],[EffectiveDate],[RebillRate],[RebillType],
	[ExpirationDate],[LastModifiedBy],[LastModifiedOn],[HoursOrUnits])
SELECT 
 	[Id],[RebillSubTasksId],[EffectiveDate],[RebillRate],[RebillType],
	[ExpirationDate],[LastModifiedBy],[LastModifiedOn],[HoursOrUnits]
FROM [PPRS1].[dbo].[RebillSubTaskRates]
WHERE [Id] NOT IN (SELECT [Id] FROM [PPRS1_History].[dbo].[RebillSubTaskRates])
SET IDENTITY_INSERT [PPRS1_History].[dbo].[RebillSubTaskRates] OFF

--[RebillDetail]
SET IDENTITY_INSERT [PPRS1_History].[dbo].[RebillDetail] ON 
INSERT INTO [PPRS1_History].[dbo].[RebillDetail] 
([Id],[WorkDate],[ShiftID],[WorkDescription],[TotalHours],[RebillStatus],[FacilityID],[RebillSubTasksId],
	[LastModifiedBy],[LastModifiedOn],[TotalUnits],[MaterialCosts],[Vendors],[InvoiceNumber],[Rebilled] )
SELECT 
 	[Id],[WorkDate],[ShiftID],[WorkDescription],[TotalHours],[RebillStatus],[FacilityID],[RebillSubTasksId],
	[LastModifiedBy],[LastModifiedOn],[TotalUnits],[MaterialCosts],[Vendors],[InvoiceNumber],[Rebilled]
FROM [PPRS1].[dbo].[RebillDetail]
WHERE [Id] NOT IN (SELECT [Id] FROM [PPRS1_History].[dbo].[RebillDetail])
SET IDENTITY_INSERT [PPRS1_History].[dbo].[RebillDetail] OFF 
 
--[RebillAttachments] 
SET IDENTITY_INSERT [PPRS1_History].[dbo].[RebillAttachments] ON 
INSERT INTO [PPRS1_History].[dbo].[RebillAttachments] 
([Id],[RebillDetailId],[Title],[Path] )
SELECT 
 [Id],[RebillDetailId],[Title],[Path]
FROM [PPRS1].[dbo].[RebillAttachments]
WHERE [Id] NOT IN (SELECT [Id] FROM [PPRS1_History].[dbo].[RebillAttachments])
SET IDENTITY_INSERT [PPRS1_History].[dbo].[RebillAttachments] OFF
 
--[PayPeriod]
INSERT INTO [PPRS1_History].[dbo].[PayPeriod] 
([Id],[Description],[PayPeriodEnd] )
SELECT 
 [Id],[Description],[PayPeriodEnd]
FROM [PPRS1].[dbo].[PayPeriod]
WHERE [Id] NOT IN (SELECT [Id] FROM [PPRS1_History].[dbo].[PayPeriod])

--[OvertimeBasis]
INSERT INTO [PPRS1_History].[dbo].[OvertimeBasis] 
([OvertimeCalcBasis],[Description] )
SELECT 
 [OvertimeCalcBasis],[Description]
FROM [PPRS1].[dbo].[OvertimeBasis]
WHERE [OvertimeCalcBasis] NOT IN (SELECT [OvertimeCalcBasis] FROM [PPRS1_History].[dbo].[OvertimeBasis])

-- [OtherTasks]
SET IDENTITY_INSERT [PPRS1_History].[dbo].[OtherTasks] ON 
INSERT INTO [PPRS1_History].[dbo].[OtherTasks] 
([Id],[TaskCode],[TaskDescription],[GLAcctNumber],[PayType],[LastModifiedBy],[LastModifiedOn],
 [Active],[CalcMethod],[FlatFeeAmount],[SingleFacility],[ExcludeFacility] )
SELECT 
[Id],[TaskCode],[TaskDescription],[GLAcctNumber],[PayType],[LastModifiedBy],[LastModifiedOn],
[Active],[CalcMethod],[FlatFeeAmount],[SingleFacility],[ExcludeFacility]
FROM [PPRS1].[dbo].[OtherTasks]
WHERE [Id] NOT IN (SELECT [Id] FROM [PPRS1_History].[dbo].[OtherTasks])
SET IDENTITY_INSERT [PPRS1_History].[dbo].[OtherTasks] OFF

--[NoProductionData]
INSERT INTO [PPRS1_History].[dbo].[NoProductionData] 
([WorkDate],[FacilityID],[ApprovalStatus],[LastModifiedBy],[LastModifiedOn])
SELECT 
 tblSource.[WorkDate],tblSource.[FacilityID],tblSource.[ApprovalStatus],tblSource.[LastModifiedBy],tblSource.[LastModifiedOn]
FROM [PPRS1].[dbo].[NoProductionData] AS tblSource
LEFT JOIN [PPRS1_History].[dbo].[NoProductionData] AS tblDest
ON tblSource.[FacilityID] = tblDest.[FacilityID] 
AND tblSource.[WorkDate] = tblDest.[WorkDate] 
WHERE tblDest.[FacilityID] IS NULL
 
--[Newpeople]
SET IDENTITY_INSERT [PPRS1_History].[dbo].[Newpeople] ON 
INSERT INTO [PPRS1_History].[dbo].[Newpeople] 
([id],[employeeNumber],[Name],[LastName],[FirstName],[address1],[address2],[City],[state],[Zip],
	[employeePhone],[BIRTHDATE],[HIREDATE],[hd],[ssnold],[FacilityID],[ssn] )
SELECT 
 	[id],[employeeNumber],[Name],[LastName],[FirstName],[address1],[address2],[City],[state],[Zip],
	[employeePhone],[BIRTHDATE],[HIREDATE],[hd],[ssnold],[FacilityID],[ssn]
FROM [PPRS1].[dbo].[Newpeople]
WHERE [Id] NOT IN (SELECT [Id] FROM [PPRS1_History].[dbo].[Newpeople])
SET IDENTITY_INSERT [PPRS1_History].[dbo].[Newpeople] OFF

 --[NewNames2]
INSERT INTO [PPRS1_History].[dbo].[NewNames2] 
([employeeNumber],[Name],[LastName],[FirstName],[address1],[address2],[City],[state],[Zip],
	[employeePhone],[BIRTHDATE],[HIREDATE],[hd],[ssnold],[FacilityID],[ssn] )
SELECT 
 	tblSource.[employeeNumber],tblSource.[Name],tblSource.[LastName],tblSource.[FirstName],tblSource.[address1],
 	tblSource.[address2],tblSource.[City],tblSource.[state],tblSource.[Zip],tblSource.[employeePhone],tblSource.[BIRTHDATE],
 	tblSource.[HIREDATE],tblSource.[hd],tblSource.[ssnold],tblSource.[FacilityID],tblSource.[ssn]
FROM [PPRS1].[dbo].[NewNames2] AS tblSource
LEFT JOIN [PPRS1_History].[dbo].[NewNames2] As tblDest
ON tblSource.[employeeNumber] = tblDest.[employeeNumber]
AND tblSource.[Name] = tblDest.[Name]
AND tblSource.[LastName]   = tblDest.[LastName]
AND tblSource.[FirstName]   = tblDest.[FirstName]
AND tblSource.[address1]   = tblDest.[address1]
AND tblSource.[address2]   = tblDest.[address2]
AND tblSource.[City]   = tblDest.[City]
AND tblSource.[state]   = tblDest.[state]
AND tblSource.[Zip]   = tblDest.[Zip]
AND tblSource.[employeePhone]   = tblDest.[employeePhone]
AND tblSource.[BIRTHDATE]   = tblDest.[BIRTHDATE]
AND tblSource.[HIREDATE]   = tblDest.[HIREDATE]
AND tblSource.[hd]   = tblDest.[hd]
AND tblSource.[ssnold]   = tblDest.[ssnold]
AND tblSource.[FacilityID]   = tblDest.[FacilityID]
AND tblSource.[ssn]   = tblDest.[ssn]
WHERE tblDest.[employeeNumber] IS NULL

--[NewNames]
SET IDENTITY_INSERT [PPRS1_History].[dbo].[NewNames] ON 
INSERT INTO [PPRS1_History].[dbo].[NewNames] 
([id],[employeeNumber],[Name],[LastName],[FirstName],[address1],[address2],[City],[state],[Zip],
	[employeePhone],[BIRTHDATE],[HIREDATE],[hd],[ssnold],[FacilityID],[ssn] )
SELECT 
 	[id],[employeeNumber],[Name],[LastName],[FirstName],[address1],[address2],[City],[state],[Zip],
	[employeePhone],[BIRTHDATE],[HIREDATE],[hd],[ssnold],[FacilityID],[ssn]
FROM [PPRS1].[dbo].[NewNames]
WHERE [Id] NOT IN (SELECT [Id] FROM [PPRS1_History].[dbo].[NewNames])
SET IDENTITY_INSERT [PPRS1_History].[dbo].[NewNames] OFF

-- [newEmployee]
SET IDENTITY_INSERT [PPRS1_History].[dbo].[newEmployee] ON 
INSERT INTO [PPRS1_History].[dbo].[newEmployee] 
([Id],[EmployeeNumber],[TempNumber],[LastName],[FirstName],[MiddleInitial],[HireDate],[InactiveDate],
	[TempStartDate],[TerminationDate],[Address1],[Address2],[City],[State],[Zip],[SSN],[BirthDate],
	[EmployeePhone],[EmergencyContact],[ContactPhone],[TempEmployee],[Salaried],[EmploymentSourceId],
	[FacilityID],[LastModifiedBy],[LastModifiedOn],[Active] )
SELECT 
 	[Id],[EmployeeNumber],[TempNumber],[LastName],[FirstName],[MiddleInitial],[HireDate],[InactiveDate],
	[TempStartDate],[TerminationDate],[Address1],[Address2],[City],[State],[Zip],[SSN],[BirthDate],
	[EmployeePhone],[EmergencyContact],[ContactPhone],[TempEmployee],[Salaried],[EmploymentSourceId],
	[FacilityID],[LastModifiedBy],[LastModifiedOn],[Active]
FROM [PPRS1].[dbo].[newEmployee]
WHERE [Id] NOT IN (SELECT [Id] FROM [PPRS1_History].[dbo].[newEmployee])
SET IDENTITY_INSERT [PPRS1_History].[dbo].[newEmployee] OFF

--[MonthlyBudgetPerc]
SET IDENTITY_INSERT [PPRS1_History].[dbo].[MonthlyBudgetPerc] ON 
INSERT INTO [PPRS1_History].[dbo].[MonthlyBudgetPerc] 
([Id],[ReportingYear],[ReportingMonth],[MonthlyPercentage] )
SELECT 
 [Id],[ReportingYear],[ReportingMonth],[MonthlyPercentage]
FROM [PPRS1].[dbo].[MonthlyBudgetPerc]
WHERE [Id] NOT IN (SELECT [Id] FROM [PPRS1_History].[dbo].[MonthlyBudgetPerc])
SET IDENTITY_INSERT [PPRS1_History].[dbo].[MonthlyBudgetPerc] OFF


--[IRGRegion]
SET IDENTITY_INSERT [PPRS1_History].[dbo].[IRGRegion] ON 
INSERT INTO [PPRS1_History].[dbo].[IRGRegion] 
([ID],[RegionCode],[RegionDescription],[LastModifiedBy],[LastModifiedOn] )
SELECT 
 [ID],[RegionCode],[RegionDescription],[LastModifiedBy],[LastModifiedOn]
FROM [PPRS1].[dbo].[IRGRegion]
WHERE [Id] NOT IN (SELECT [Id] FROM [PPRS1_History].[dbo].[IRGRegion])
SET IDENTITY_INSERT [PPRS1_History].[dbo].[IRGRegion] OFF
	
--[IRGRailCarType]
SET IDENTITY_INSERT [PPRS1_History].[dbo].[IRGRailCarType] ON 
INSERT INTO [PPRS1_History].[dbo].[IRGRailCarType] 
([ID],[CarTypeCode],[CarTypeDescription],[LastModifiedBy],[LastModifiedOn],[SortOrder] )
SELECT 
 [ID],[CarTypeCode],[CarTypeDescription],[LastModifiedBy],[LastModifiedOn],[SortOrder]
FROM [PPRS1].[dbo].[IRGRailCarType]
WHERE [Id] NOT IN (SELECT [Id] FROM [PPRS1_History].[dbo].[IRGRailCarType])
SET IDENTITY_INSERT [PPRS1_History].[dbo].[IRGRailCarType] OFF

--[IRGRailCarrier]
SET IDENTITY_INSERT [PPRS1_History].[dbo].[IRGRailCarrier] ON 
INSERT INTO [PPRS1_History].[dbo].[IRGRailCarrier] 
([ID],[RailCarrierCode],[RailCarrierName],[LastModifiedBy],[LastModifiedOn] )
SELECT 
 [ID],[RailCarrierCode],[RailCarrierName],[LastModifiedBy],[LastModifiedOn]
FROM [PPRS1].[dbo].[IRGRailCarrier]
WHERE [Id] NOT IN (SELECT [Id] FROM [PPRS1_History].[dbo].[IRGRailCarrier])
SET IDENTITY_INSERT [PPRS1_History].[dbo].[IRGRailCarrier] OFF
 
--[IRGOrigin]
SET IDENTITY_INSERT [PPRS1_History].[dbo].[IRGOrigin] ON 
INSERT INTO [PPRS1_History].[dbo].[IRGOrigin] 
([ID],[OriginCode],[OriginName],[LastModifiedBy],[LastModifiedOn] )
SELECT 
 [ID],[OriginCode],[OriginName],[LastModifiedBy],[LastModifiedOn]
FROM [PPRS1].[dbo].[IRGOrigin]
WHERE [Id] NOT IN (SELECT [Id] FROM [PPRS1_History].[dbo].[IRGOrigin])
SET IDENTITY_INSERT [PPRS1_History].[dbo].[IRGOrigin] OFF

--[IRGManufacturer]
SET IDENTITY_INSERT [PPRS1_History].[dbo].[IRGManufacturer] ON 
INSERT INTO [PPRS1_History].[dbo].[IRGManufacturer] 
([ID],[ManufacturerCode],[ManufacturerName],[LastModifiedBy],[LastModifiedOn] )
SELECT 
 [ID],[ManufacturerCode],[ManufacturerName],[LastModifiedBy],[LastModifiedOn]
FROM [PPRS1].[dbo].[IRGManufacturer]
WHERE [Id] NOT IN (SELECT [Id] FROM [PPRS1_History].[dbo].[IRGManufacturer])
SET IDENTITY_INSERT [PPRS1_History].[dbo].[IRGManufacturer] OFF

--[IRGCompany]
SET IDENTITY_INSERT [PPRS1_History].[dbo].[IRGCompany] ON 
INSERT INTO [PPRS1_History].[dbo].[IRGCompany] 
([Id],[CompanyID],[CompanyName],[LogoPath],[PayPeriodID],[PayrollCompanyCode],[OutOfTownRate],
	[OutOfTownHoursPerDay],[LastModifiedBy],[LastModifiedOn],[Active] )
SELECT 
 	[Id],[CompanyID],[CompanyName],[LogoPath],[PayPeriodID],[PayrollCompanyCode],[OutOfTownRate],
	[OutOfTownHoursPerDay],[LastModifiedBy],[LastModifiedOn],[Active]
FROM [PPRS1].[dbo].[IRGCompany]
WHERE [Id] NOT IN (SELECT [Id] FROM [PPRS1_History].[dbo].[IRGCompany])
SET IDENTITY_INSERT [PPRS1_History].[dbo].[IRGCompany] OFF
		
--[FacilityProductionDetail]
SET IDENTITY_INSERT [PPRS1_History].[dbo].[FacilityProductionDetail] ON 
INSERT INTO [PPRS1_History].[dbo].[FacilityProductionDetail] 
([Id],[WorkDate],[ShiftID],[Units],[OriginID],[CarTypeID],[RailCarNumber],[LevelType],[NewUsed],[FacilityID],
	[FacilityCustomerId],[TaskId],[ManufacturerID],[ApprovalStatus],[Notes],[LastModifiedBy],[LastModifiedOn] )
SELECT 
 	[Id],[WorkDate],[ShiftID],[Units],[OriginID],[CarTypeID],[RailCarNumber],[LevelType],[NewUsed],[FacilityID],
	[FacilityCustomerId],[TaskId],[ManufacturerID],[ApprovalStatus],[Notes],[LastModifiedBy],[LastModifiedOn]
FROM [PPRS1].[dbo].[FacilityProductionDetail]
WHERE [Id] NOT IN (SELECT [Id] FROM [PPRS1_History].[dbo].[FacilityProductionDetail])
SET IDENTITY_INSERT [PPRS1_History].[dbo].[FacilityProductionDetail] OFF

-- [FacilityMonthlyBudgetEntry]
SET IDENTITY_INSERT [PPRS1_History].[dbo].[FacilityMonthlyBudgetEntry] ON 
INSERT INTO [PPRS1_History].[dbo].[FacilityMonthlyBudgetEntry] 
([Id],[ReportingYear],[ReportingMonth],[FacilityId],[BudgetValue],[WorkType] )
SELECT 
 [Id],[ReportingYear],[ReportingMonth],[FacilityId],[BudgetValue],[WorkType]
FROM [PPRS1].[dbo].[FacilityMonthlyBudgetEntry]
WHERE [Id] NOT IN (SELECT [Id] FROM [PPRS1_History].[dbo].[FacilityMonthlyBudgetEntry])
SET IDENTITY_INSERT [PPRS1_History].[dbo].[FacilityMonthlyBudgetEntry] OFF
 
-- [FacilityMonitoringDataEntry]
SET IDENTITY_INSERT [PPRS1_History].[dbo].[FacilityMonitoringDataEntry] ON 
INSERT INTO [PPRS1_History].[dbo].[FacilityMonitoringDataEntry] 
([ID],[FacilityID],[WorkDate],[DataSection],[FieldName],[DataValue] )
SELECT 
 [ID],[FacilityID],[WorkDate],[DataSection],[FieldName],[DataValue]
FROM [PPRS1].[dbo].[FacilityMonitoringDataEntry]
WHERE [Id] NOT IN (SELECT [Id] FROM [PPRS1_History].[dbo].[FacilityMonitoringDataEntry])
SET IDENTITY_INSERT [PPRS1_History].[dbo].[FacilityMonitoringDataEntry] OFF
 
--[FacilityCustomer] 
SET IDENTITY_INSERT [PPRS1_History].[dbo].[FacilityCustomer] ON 
INSERT INTO [PPRS1_History].[dbo].[FacilityCustomer] 
([Id],[FacilityId],[CustomerCode],[CustomerName],[ContactName],[ContactAddress1],[ContactAddress2],[ContactAddress3],
	[DefaultCustomer],[LastModifiedBy],[LastModifiedOn],[Active] )
SELECT 
 	[Id],[FacilityId],[CustomerCode],[CustomerName],[ContactName],[ContactAddress1],[ContactAddress2],[ContactAddress3],
	[DefaultCustomer],[LastModifiedBy],[LastModifiedOn],[Active]
FROM [PPRS1].[dbo].[FacilityCustomer]
WHERE [Id] NOT IN (SELECT [Id] FROM [PPRS1_History].[dbo].[FacilityCustomer])
SET IDENTITY_INSERT [PPRS1_History].[dbo].[FacilityCustomer] OFF
 
--[FacilityAnnualBudget]
SET IDENTITY_INSERT [PPRS1_History].[dbo].[FacilityAnnualBudget] ON 
INSERT INTO [PPRS1_History].[dbo].[FacilityAnnualBudget] 
([Id],[FacilityId],[LoadTotal],[UnloadTotal],[ReportingYear],
	[BudgetedCPU],[MiscellaneousCPU],[SpottingTotal] )
SELECT 
 	[Id],[FacilityId],[LoadTotal],[UnloadTotal],[ReportingYear],
	[BudgetedCPU],[MiscellaneousCPU],[SpottingTotal]
FROM [PPRS1].[dbo].[FacilityAnnualBudget]
WHERE [Id] NOT IN (SELECT [Id] FROM [PPRS1_History].[dbo].[FacilityAnnualBudget])
SET IDENTITY_INSERT [PPRS1_History].[dbo].[FacilityAnnualBudget] OFF

--[Facility]
SET IDENTITY_INSERT [PPRS1_History].[dbo].[Facility] ON 
INSERT INTO [PPRS1_History].[dbo].[Facility] 
([Id],[FacilityNumber],[AlphaCode],[Name],[Address1],[Address2],[Address3],[DefaultTaskID],
	[DefaultShiftID],[RegionID],[OvertimeCalcBasis],[GLCostCenter],[IRGCompanyId],[LastModifiedBy],
	[LastModifiedOn],[Active],[BudgetedCPU] )
SELECT 
 	[Id],[FacilityNumber],[AlphaCode],[Name],[Address1],[Address2],[Address3],[DefaultTaskID],
	[DefaultShiftID],[RegionID],[OvertimeCalcBasis],[GLCostCenter],[IRGCompanyId],[LastModifiedBy],
	[LastModifiedOn],[Active],[BudgetedCPU]
FROM [PPRS1].[dbo].[Facility]
WHERE [Id] NOT IN (SELECT [Id] FROM [PPRS1_History].[dbo].[Facility])
SET IDENTITY_INSERT [PPRS1_History].[dbo].[Facility] OFF

 --[importedFacility]
INSERT INTO [PPRS1_History].[dbo].[importedFacility] 
([Facility #],[Alpha Code],[Name],[Company],[CompanyID],[ADP Code],[Customer],
	[CustomerID],[Region],[RegionID] )
SELECT 
 	tblSource.[Facility #],tblSource.[Alpha Code],tblSource.[Name],tblSource.[Company],
 	tblSource.[CompanyID],tblSource.[ADP Code],tblSource.[Customer],tblSource.[CustomerID],
 	tblSource.[Region],tblSource.[RegionID]
FROM [PPRS1].[dbo].[importedFacility] AS tblSource
LEFT JOIN [PPRS1_History].[dbo].[importedFacility] As tblDest
ON tblSource.[Facility #] = tblDest.[Facility #]
AND tblSource.[Alpha Code] = tblDest.[Alpha Code]
AND tblSource.[Name] = tblDest.[Name]
AND tblSource.[Company] = tblDest.[Company]
AND tblSource.[CompanyID] = tblDest.[CompanyID]
AND tblSource.[ADP Code] = tblDest.[ADP Code]
AND tblSource.[Customer] = tblDest.[Customer]
AND tblSource.[CustomerID] = tblDest.[CustomerID]
AND tblSource.[Region] = tblDest.[Region]
AND tblSource.[RegionID] = tblDest.[RegionID]
WHERE tblDest.[Facility #] IS NULL


 --[ImportedEmployees] ***
INSERT INTO [PPRS1_History].[dbo].[ImportedEmployees] 
([File #],[Name],[Street],[Street 2 ],[City],[NoName],
	[Zip],[Home Phone],[BIRTHDATE],[HIREDATE],[FACILITY])
SELECT 
	tblSource.[File #],tblSource.[Name],tblSource.[Street],tblSource.[Street 2 ],
	tblSource.[City],tblSource.[NoName],tblSource.[Zip],tblSource.[Home Phone],
	tblSource.[BIRTHDATE],tblSource.[HIREDATE],tblSource.[FACILITY]
FROM [PPRS1].[dbo].[ImportedEmployees] AS tblSource
LEFT JOIN [PPRS1_History].[dbo].[ImportedEmployees] As tblDest
ON tblSource.[File #] = tblDest.[File #]
AND tblSource.[Name] = tblDest.[Name]
AND tblSource.[Street] = tblDest.[Street]
AND tblSource.[Street 2 ] = tblDest.[Street 2 ]
AND tblSource.[City] = tblDest.[City]
AND tblSource.[NoName] = tblDest.[NoName]
AND tblSource.[Zip] = tblDest.[Zip]
AND tblSource.[Home Phone] = tblDest.[Home Phone]
AND tblSource.[BIRTHDATE] = tblDest.[BIRTHDATE]
AND tblSource.[HIREDATE] = tblDest.[HIREDATE]
AND tblSource.[FACILITY] = tblDest.[FACILITY]
WHERE tblDest.[File #] IS NULL AND tblSource.[File #] IS NOT NULL

 --[FacilityTasks]
INSERT INTO [PPRS1_History].[dbo].[FacilityTasks] 
([FacilityID],[TaskId],[LastModifiedBy],[LastModifiedOn])
SELECT 
	tblSource.[FacilityID],tblSource.[TaskId],tblSource.[LastModifiedBy],tblSource.[LastModifiedOn]
FROM [PPRS1].[dbo].[FacilityTasks] AS tblSource
LEFT JOIN [PPRS1_History].[dbo].[FacilityTasks] As tblDest
ON tblSource.[FacilityID] = tblDest.[FacilityID]
AND tblSource.[TaskId] = tblDest.[TaskId]
WHERE tblDest.[FacilityID] IS NULL


 --[FacilityAnnualBudgetTask]
INSERT INTO [PPRS1_History].[dbo].[FacilityAnnualBudgetTask] 
([FacilityId],[TaskID],[ReportingYear],[BudgetedCPU])
SELECT 
	tblSource.[FacilityId],tblSource.[TaskID],tblSource.[ReportingYear],tblSource.[BudgetedCPU]
FROM [PPRS1].[dbo].[FacilityAnnualBudgetTask] AS tblSource
LEFT JOIN [PPRS1_History].[dbo].[FacilityAnnualBudgetTask] As tblDest
ON tblSource.[FacilityID] = tblDest.[FacilityID]
AND tblSource.[TaskId] = tblDest.[TaskId]
WHERE tblDest.[FacilityID] IS NULL

 --[UserRights]
INSERT INTO [PPRS1_History].[dbo].[UserRights] 
([UserProfileId],[FacilityId],[LastModifiedBy],[LastModifiedOn],[RegionalRight])
SELECT 
	tblSource.[UserProfileId],tblSource.[FacilityId],tblSource.[LastModifiedBy],tblSource.[LastModifiedOn],tblSource.[RegionalRight]
FROM [PPRS1].[dbo].[UserRights] AS tblSource
LEFT JOIN [PPRS1_History].[dbo].[UserRights] As tblDest
ON tblSource.[UserProfileId] = tblDest.[UserProfileId]
AND tblSource.[FacilityId] = tblDest.[FacilityId]
WHERE tblDest.[UserProfileId] IS NULL

IF (@@ERROR = 0 ) 

  BEGIN
      COMMIT TRANSACTION;  /* Everything OK so COMMIT */
      PRINT 'Data Transfered'
     
      --Call stored proc to truncate tables.
       exec DataArchive_TruncateData @ArchiveDate

       --Shrink Database
       DBCC SHRINKDATABASE (PPRS1, 10);
 
  END 
  
ELSE 

  BEGIN
      ROLLBACK TRAN 
      PRINT 'Data Transfered Failed'   
  END  



END
