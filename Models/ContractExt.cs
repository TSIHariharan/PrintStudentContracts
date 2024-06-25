using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintStudentContracts.Models
{
    public class ContractExt
    {
        public bool IsFullTime { get; set; }
        public bool IsPartTime { get; set; }
        public bool IsMonToFri { get; set; }
        public string Days { get; set; }
        public string Times1 { get; set; }
        public string Times2 { get; set; }
        public bool AdmissionReq1 { get; set; }
        public bool AdmissionReq2 { get; set; }
        public bool AdmissionReq3 { get; set; }
        public bool AdmissionReq4 { get; set; }
        public decimal TuitionFees { get; set; }
        public decimal OtherCompulsoryFees { get; set; }
        public decimal MajorEquipment { get; set; }
        public decimal InternationalStudentFees { get; set; }
        public string OptionalFeesContent { get; set; }
        public decimal OptionalFees { get; set; }
        public decimal BookFees { get; set; }
        public decimal UniformAndEquipment { get; set; }
        public decimal ExpendableSupplies { get; set; }
        public decimal FieldTrips { get; set; }
        public decimal ProfessionalExamFees { get; set; }
        public decimal GST { get; set; }
        public string LanguageOfInstruction { get; set; }
        public string LocationOfPracticum { get; set; }
        public string CASTTestScore { get; set; }
        public string WonderlicTestScore { get; set; }
        public string SourceOfEnrollment { get; set; }
        public string MethodOfPayment { get; set; }
        public string Instalments { get; set; }
        public string SC { get; set; }
        public string GovernmentLoan { get; set; }
        public string OtherLoan { get; set; }
    }
}
