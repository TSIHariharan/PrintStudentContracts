using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintStudentContracts.Models
{
    public class ContactEx
    {
        public  bool IsSameAddress { get; set; }
        public string MailingAddress1 { get; set; }
        public string MailingAptUnit { get; set; }
        public string MailingCity { get; set; }
        public string MailingProvince { get; set; }
        public string MailingPostalCode { get; set; }
        public string MailingCountry { get; set; }
        public string MailingPhone { get; set; }
        public string MailingAlternative { get; set; }
        public string MailingCell { get; set; }
        public bool IsInternationalStudent { get; set; }
        public string GradeYear { get; set; }
        public string EmergencyAddress1 { get; set; }
        public string EmergencyAptUnit { get; set; }
        public string EmergencyCity { get; set; }
        public string EmergencyProvince { get; set; }
        public string EmergencyPostalCode { get; set; }
        public string EmergencyCountry { get; set; }
        public string EmergencyAlternative { get; set; }
        public string EmergencyCell { get; set; }
    }
}
