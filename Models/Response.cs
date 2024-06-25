using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintStudentContracts.Models
{
    public class PrintURLResponse
    {
        // Default constructor
        public PrintURLResponse()
        {
            // Set default values for properties
            status = false;
            linkURL = string.Empty;
            message = string.Empty;
            withValidation = false;
        }
        public bool status { get; set; }
        public string linkURL { get; set; }
        public string message {  get; set; }
        public bool withValidation { get; set; }
    }
}
