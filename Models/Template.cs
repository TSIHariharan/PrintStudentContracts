﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintStudentContracts.Models
{
    public class Template
    {
        public int TemplateID { get; set; }
        public int TemplateTypeID { get; set; }
        public string TemplateName { get; set; }
        public string TemplateReportName { get; set; }
    }
}
