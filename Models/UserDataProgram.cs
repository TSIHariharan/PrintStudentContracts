using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintStudentContracts.Models
{
    public class UserDataProgram
    {
        public int userid { get; set; }
        public string name { get; set; }
        public int userprogramid { get; set; }
        public string pname { get; set; }
        public string printName { get; set; }
        public int contractid { get; set; }
        public string contractcode { get; set; }
        public bool? isFullTime { get; set; }
        public int ccid { get; set; }
        public int cid { get; set; }
        public string coursecode { get; set; }
        public string coursename { get; set; }
        public string status { get; set; }
        public DateTime? datestart { get; set; }
        public DateTime? datefin { get; set; }
        public string exammark { get; set; }
        public string cost { get; set; }
        public int studystatusid { get; set; }
        public int examtakenid { get; set; }
        public DateTime? examstartdate { get; set; }
        public DateTime? examenddate { get; set; }
        public DateTime? cstartdate { get; set; }
        public DateTime? cenddate { get; set; }
        public int upstatus { get; set; }
        public int cstatus { get; set; }
        public DateTime? upschedstart { get; set; }
        public DateTime? upschedend { get; set; }
        public int programid { get; set; }
        public DateTime? cschedstart { get; set; }
        public DateTime? cschedend { get;set; }
        public int hours { get; set; }
        public decimal? hoursPerWeek { get; set; }
        public int fundid { get; set; }
        public int markedas { get; set; }
        public int regiontype { get; set; }
        public int ctypeid { get; set; }
        public string control { get; set; }
        public int thours { get; set; }
        public string notes { get; set; }
        public int ptypeid { get; set; }
        public DateTime? regdate { get; set; }
        public string ctnotes { get; set; }
        public string mastercode { get; set; }
        public int Lesson { get; set; }
        public int Lab { get; set; }
        public int Assignment { get; set; }
        public int Project { get; set; }
        public decimal ContractMark { get; set; }
        public decimal ProgramActualCost { get; set; }
        public decimal ContractActualCost { get; set; }
        public int ContractSponsorID { get; set; }
        public string ContractReferenceNo { get; set; }
        public int SupportID { get; set; }
        public int CourseOrder { get; set; }
        public DateTime? CourseScheduleStartDate { get; set; }
        public DateTime? CourseScheduleEndDate { get; set; }
        public bool IsLibraryCopy { get; set; }
        public decimal Credits { get; set; }
        public DateTime? datecertificateissued { get; set; }
        public decimal ProgramAverageMark { get; set; }
        public string DateDiplomaPrinted { get; set; }
        public decimal LabFee { get; set; }
        public decimal BooksFee { get; set;}
        public decimal UserParticipationMark { get;set; }
        public int MapID { get; set; }
        public string TitleOfDoc { get; set; }
        public string NameOfOrg { get; set; }
        public int RegisteredProgramHours { get; set; }
        public int amountPendingPrintRequestContract { get; set; }
        public int amountPendingPrintRequestContractCourse { get; set; }
        public int courseGrantCertificate { get; set; }
        public ContractExt ContractExt { get; set; }
    }
}
