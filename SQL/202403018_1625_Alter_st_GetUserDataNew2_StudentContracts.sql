USE [ACME_AOL_TEST]
GO
/****** Object:  StoredProcedure [dbo].[st_GetUserDataNew2_StudentContracts]    Script Date: 18-03-2024 16:23:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Mohana
-- Create date: 2024-02-12
-- Description:	Get User Program and Contract Data
-- History:
-- Mohana       18/03/2024        changed fieldnames and added is fulltime fields 
-- =============================================
ALTER PROCEDURE [dbo].[st_GetUserDataNew2_StudentContracts]
	@UserID int,
	@ContractID int
AS
BEGIN
	--  select all contract and program info into temp table to prevent joining on a per-course basis
	--  this is to speed up this sp, since it originally joined all courses against contract and program
	--  info in one table
	SELECT up.UserID,
		   up.UserProgramID,
		   p.Name,
		   p.PrintName,
		   c.ContractID,
		   c.Code,
		   c.IsFullTime,
		   c.ContractStartDate,
		   c.ContractFinishDate,
		   up.StudyStatusID AS UPStudyStatusID,
		   c.StudyStatusID AS CStudyStatusID,
		   up.ScheduledStart,
		   up.ScheduledEnd,
		   up.ProgramID,
		   c.ScheduledStartDate,
		   c.ScheduledEndDate,
		   c.HoursPerDay,
		   p.HoursPerWeek,
		   c.FundingSourceID,
		   p.ProgramTypeID,
		   c.RegistrationDate,
		   c.Notes,
		   dbo.aolContractAverageMark(c.ContractID) AS ContractAvgMark,
		   case when (up.UserProgramID is null OR up.ProgramID = 14121) then 0 else dbo.nf_GetUserProgramInheritedPrice(up.UserProgramID) end as UPActualCost,
		   CASE WHEN up.ProgramID = 14121 THEN 0 ELSE c.ActualCost END AS ActualCost,
		   c.SponsorID,
		   c.ReferenceNumber,
		   case when up.UserProgramID is null then 0 else dbo.aolUserProgramAverageMark(up.UserProgramID) end UPAverageMark, 
		   up.DateDiplomaPrinted,
		   p.LabFee,
		   p.BooksFee,
		   ISNULL(up.UserParticipationMark,0) UserParticipationMark, 
		   ISNULL(up.MapID, 0) MapID,
		   ACME_AOL_TEST.dbo.nf_GetSchoolProgramTotalHours(up.ProgramID, u.SchoolID) AS RegisteredProgramHours
	INTO #ProgramTempTable
	FROM ACME_AOL_TEST.dbo.UserProgram up
	LEFT JOIN ACME_AOL_TEST.dbo.Contract c ON c.UserProgramID = up.UserProgramID
	INNER JOIN ACME_AOL_TEST.dbo.Program p ON p.ProgramID = up.ProgramID
	INNER JOIN ACME_MAIN_TEST.dbo.[User] u ON u.UserID = up.UserID
	WHERE up.UserID = @UserID

	SELECT top 1 
		   u.userid,
		   ud.firstname + ' ' + ud.lastname as name,
		   ptt.userprogramid, 
		   ptt.name as pname, 
		   ptt.PrintName as printName,
		   ptt.contractid,
		   ptt.code as contractcode,
		   ptt.IsFullTime as isFullTime,
		   cc.contractcourseid as ccid,
		   cr.courseid as cid,
		   cr.code as coursecode,
		   cr.name as coursename,
		   ss.name as status,
		   cc.datestarted as datestart,
		   cc.datefinished as datefin,
		   cc.exammark,
		   cc.actualcost as cost,
		   cc.studystatusid,
		   cc.examtakenid,
		   ea.startdate as examstartdate,
		   ea.examdate as examenddate,
		   ptt.contractstartdate as cstartdate,
		   ptt.contractfinishdate as cenddate,
		   ptt.UPStudyStatusID as upstatus,
		   ptt.CStudyStatusID as cstatus,
		   ptt.scheduledstart as upschedstart,
		   ptt.scheduledend as upschedend,
		   ptt.programid, 
		   ptt.scheduledstartdate as cschedstart,
		   ptt.scheduledenddate as cschedend,
		   ptt.hoursperday as hours,
		   ptt.HoursPerWeek as hoursPerWeek,
		   ptt.fundingsourceid as fundid,
		   ud.markedas,
		   s.regiontype,
		   cc.coursestatusid as ctypeid,
		   cc.controlnumber as control, 
		   ISNULL(cc.CourseHour, dbo.nf_GetSchoolCourseInheritedSumHours(u.SchoolID, cc.CourseID)) AS thours, 
		   cc.notes,
		   ptt.programtypeid as ptypeid,
		   ptt.registrationdate as regdate,
		   ptt.notes as ctnotes,
		   cr.mastercoursecode as mastercode, 
		   cr.numlesson as Lesson,
		   CE.Lab,
		   CE.Assignment,
		   CE.Report as Project, 
		   ptt.ContractAvgMark as ContractMark,
		   ptt.UPActualCost as ProgramActualCost, 
		   ptt.ActualCost as ContractActualCost, 
		   ptt.SponsorID as ContractSponsorID,
		   ptt.ReferenceNumber as ContractReferenceNo, 
		   (SELECT top 1 us.UserSupportID FROM ACME_AOL_TEST.dbo.UserSupport us WITH(NOLOCK) WHERE (us.UserID = ud.userid) AND (us.CourseID = cc.CourseID)) as SupportID,
		   cc.courseorder as CourseOrder,
		   cc.ScheduleStartDate as CourseScheduleStartDate,
		   cc.ScheduleEndDate as CourseScheduleEndDate,
		   cc.IslibraryCopy,
		   cc.CourseCredit as Credits,
		   cc.DateCertificateIssued as datecertificateissued, 
		   ptt.UPAverageMark as ProgramAverageMark,
		   ptt.DateDiplomaPrinted,
		   ptt.Labfee,
		   ptt.Booksfee,
		   ptt.UserParticipationMark,
		   ptt.MapID,
		   cc.[NameOnDocument] as TitleOfDoc,
		   cc.[CoursePrintDescription] as NameOfOrg,
		   ptt.RegisteredProgramHours,
		   (SELECT COUNT(PR.RequestId)
		FROM ACME_AOL_TEST.dbo.PrintRequest PR
		WHERE PR.RequestPrintObjectId = ptt.contractid 
		AND PrintDate IS NULL)
		 AS AmountPrintRequestContract,
		(SELECT COUNT(RequestId)
		FROM ACME_AOL_TEST.dbo.PrintRequest
		INNER JOIN ACME_AOL_TEST.dbo.Course C
		ON C.CourseId = cc.CourseId 
		WHERE RequestPrintObjectId = cc.contractCourseId
		--AND C.GrantCertificate = 1
		--coursestatus is pass, honours, complete or transfered
		--AND cc.coursestatusid IN (2,3,10,108)
		AND PrintDate IS NULL) AS AmountPrintRequestContractCourse,
		cr.GrantCertificate AS CourseGrantCertificate
	FROM acme_main_test.dbo.[user] u WITH(NOLOCK)
	INNER JOIN userdetail ud WITH(NOLOCK) on u.userid = ud.userid 
	INNER JOIN acme_main_test.dbo.school s WITH(NOLOCK) on u.schoolid = s.schoolid 
	LEFT JOIN #ProgramTempTable ptt ON ptt.UserID = u.UserID
	--LEFT JOIN userprogram up WITH(NOLOCK) on u.userid = up.userid 
	--LEFT JOIN program pr WITH(NOLOCK) on up.programid = pr.programid 
	--LEFT JOIN contract c WITH(NOLOCK) on up.userprogramid = c.userprogramid 
	LEFT JOIN contractcourse cc WITH(NOLOCK) on ptt.contractid = cc.contractid 
	LEFT JOIN course cr WITH(NOLOCK) on cc.courseid = cr.courseid 
	LEFT JOIN studystatus ss WITH(NOLOCK) on cc.studystatusid = ss.studystatusid 
	LEFT JOIN acme_exam.dbo.examattempt ea WITH(NOLOCK) on cc.examtakenid = ea.examattemptid 
	LEFT JOIN CourseExt CE WITH(NOLOCK) ON CE.CourseID = cc.CourseID 
	WHERE u.userid = @UserID 
	and ptt.contractid = @ContractID
	ORDER BY ptt.userprogramid desc,ptt.contractid,cr.code 
END
