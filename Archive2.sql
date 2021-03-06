USE [PPRS1]
GO
/****** Object:  StoredProcedure [dbo].[DataArchive_TruncateData]    Script Date: 12/04/2012 17:02:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Brett Snyder
-- Create date: 11/21/2012
-- Description:	
-- =============================================
ALTER PROCEDURE [dbo].[DataArchive_TruncateData] 
   
    @ArchiveDate AS DateTime
    
AS
BEGIN


BEGIN TRANSACTION;


DELETE FROM dbo.EmployeeTaskWorkedPay WHERE [EmployeeTaskWorkedId] IN (SELECT [ID] From dbo.EmployeeTaskWorked WHERE [WorkDate] <= @ArchiveDate)
DELETE FROM dbo.EmployeeTaskWorked WHERE [WorkDate] <= @ArchiveDate
DELETE FROM dbo.FacilityMonitoringDataEntry WHERE [WorkDate] <= @ArchiveDate
DELETE FROM dbo.FacilityProductionDetail WHERE [WorkDate] <= @ArchiveDate
DELETE FROM dbo.NoProductionData WHERE [WorkDate] <= @ArchiveDate
DELETE FROM dbo.RebillDetail WHERE [WorkDate] <= @ArchiveDate


COMMIT TRANSACTION;
END

