using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintStudentContracts.Models
{
    public class EducationVM
    {
        public int grade { get; set; }
        public bool ishighschoolgraduate { get; set; }
        public string high_school_name { get; set; }
        public string high_school_location { get; set; }
        public string favsubject { get; set; }
        public string bestsubject { get; set; }
        public string leastfavsubject { get; set; }
        public DateTime high_school_gradyr { get; set; }
        public int education_type_id { get; set; }
        public string other_name { get; set; }
        public string post_school_name { get; set; }
        public string post_school_location { get; set; }
        public string course_taken { get; set; }
        public int course_length { get; set; }
        public bool ispostschoolgraduate { get; set; }
        public DateTime post_school_gradyr { get; set; }
    }
}
