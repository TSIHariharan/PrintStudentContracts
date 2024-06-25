using PrintStudentContracts.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;

namespace PrintStudentContracts
{
    public class PrintContract
    {
        public static string SchoolIndividualContractReportName = "";
        public static string SchoolInvoiceReportName = "";
        public static string SchoolTranscriptReportName = "";
        public static string SchoolPaymentReportName = "";
        public static string SchoolReceiptReportName = "";
        public static string SchoolCertificateContractReportName = "";
        public static string SchoolDiplomaContractReportName = "";
        public static PrintURLResponse PrintContractsWithValidation(string ContractID, string UserID, string SchoolID)
        {
            PrintURLResponse response = new PrintURLResponse();
            try
            {
                response.withValidation = true;
                var getFirstNode = Reports.GetUserDataProgramParticipationMark(UserID, ContractID);
                LogWriter.LogWrite("getFirstNode: " + JsonSerializer.Serialize(getFirstNode));
                if (getFirstNode.cstatus.ToString() != Constants.ContractStatusCPL.ToString())
                {
                    var Province = Reports.GetUserSchoolProvince(UserID);
                    LogWriter.LogWrite("Province: " + JsonSerializer.Serialize(Province));
                    if (Province == "SK" || Province == "Saskatchewan")
                    {
                        response = CheckValidation(getFirstNode, response, UserID, SchoolID, "SK");
                        LogWriter.LogWrite(Province + " response: " + JsonSerializer.Serialize(response));
                    }
                    else if (Province == "MB" || Province == "Manitoba")
                    {
                        response = CheckValidation(getFirstNode, response, UserID, SchoolID, "MB");
                        LogWriter.LogWrite(Province + " response: " + JsonSerializer.Serialize(response));
                    }
                    else if (Province == "ON" || Province == "Ontario")
                    {
                        LogWriter.LogWrite(Province + " program type " + getFirstNode.ptypeid);
                        if (getFirstNode.ptypeid == 1 || getFirstNode.ptypeid == 2)
                        {
                            response = CheckValidationON(getFirstNode, response, UserID, SchoolID, "ON");
                            LogWriter.LogWrite(Province + " response: " + JsonSerializer.Serialize(response));
                        }
                        else
                        {
                            response.message = "Sorry, you cannot print the contract here.";
                        }
                    }
                    else
                    {
                        response.message = "Sorry, you cannot print the contract here.";
                    }
                }
                else
                {
                    response.message = "Sorry, you cannot print the CPL contract here.";
                }
            } catch(Exception ex)
            {
                LogWriter.LogWrite($"error: {ex?.ToString()}");
            }
            return response;
        }
        public static PrintURLResponse CheckValidation(UserDataProgram udProgramData, PrintURLResponse response, string UserID, string SchoolID, string Province)
        {
            try
            {
                if (udProgramData != null)
                {
                    string RequiredMsgStr = "";
                    udProgramData.ContractExt = Reports.DownloadContractExt(udProgramData.contractid);
                    var UserDetails = Reports.UserManagementDownload(Convert.ToInt32(UserID));
                    var Contact = Reports.DownloadContactEx(Convert.ToInt32(UserID));
                    var Course = Reports.DownloadUserEducationBackground(Convert.ToInt32(UserID));
                    LogWriter.LogWrite(Province + " UserDetails: " + JsonSerializer.Serialize(UserDetails));
                    LogWriter.LogWrite(Province + " Contact: " + JsonSerializer.Serialize(Contact));
                    LogWriter.LogWrite(Province + " Course: " + JsonSerializer.Serialize(Course));

                    if (UserDetails?.first_name?.Trim() == "" || UserDetails?.initials?.Trim() == "" || UserDetails?.last_name?.Trim() == "")
                    {
                        RequiredMsgStr += "Student First Name, Initials or Last Name.\n";
                    }
                    if (UserDetails?.title?.Trim() == "")
                    {
                        RequiredMsgStr += "Student Title.\n";
                    }
                    if (UserDetails?.birth_date_found == false)
                    {
                        RequiredMsgStr += "Student Date of Birth.\n";
                    }
                    if (udProgramData.pname?.Trim() == "")
                    {
                        RequiredMsgStr += "Program Name.\n";
                    }
                    if (udProgramData.cschedstart == null || udProgramData.cschedend == null || udProgramData.hours == 0)
                    {
                        RequiredMsgStr += "Contract Start Date, End Date or Hours.\n";
                    }
                    if (udProgramData.ContractExt == null || udProgramData.ContractExt?.LanguageOfInstruction?.Trim() == "" || udProgramData.ContractExt?.LocationOfPracticum?.Trim() == "")
                    {
                        RequiredMsgStr += "Language of Instruction or Location Of Practicum.\n";
                    }
                    if (udProgramData.ContractExt == null || (udProgramData.ContractExt?.IsFullTime == false && udProgramData.ContractExt?.IsPartTime == false))
                    {
                        RequiredMsgStr += "Class Schedule Full Time - Part Time. \n";
                    }
                    if (udProgramData.ContractExt == null || udProgramData.ContractExt?.IsMonToFri == false)
                    {
                        RequiredMsgStr += "Mon to Fri.\n";
                    }
                    if (udProgramData.ContractExt == null || udProgramData.ContractExt?.Days?.Trim() == "")
                    {
                        RequiredMsgStr += "Days for Class Schedule.\n";
                    }
                    if (udProgramData.ContractExt == null || udProgramData.ContractExt?.Times1?.Trim() == "")
                    {
                        RequiredMsgStr += "Mon to Fri - Class Times.\n";
                    }
                    if (udProgramData.ContractExt == null || udProgramData.ContractExt?.Times2?.Trim() == "")
                    {
                        RequiredMsgStr += "Days - Class Times.\n";
                    }
                    if (UserDetails.referral_id == 0 || Contact == null || Contact.GradeYear?.Trim() == "")
                    {
                        RequiredMsgStr += "Last Grade Completed or Grade Year.\n";
                    }
                    if (Course == null || Course.course_taken?.Trim() == "")
                    {
                        RequiredMsgStr += "Additional Education/Courses Taken.\n";
                    }
                    if (UserDetails?.address1?.Trim() == "" && UserDetails?.address2?.Trim() == "")
                    {
                        RequiredMsgStr += "Permanent Address.\n";
                    }
                    if (UserDetails?.city?.Trim() == "" || UserDetails?.province?.Trim() == "" || UserDetails?.postal?.Trim() == "")
                    {
                        RequiredMsgStr += "Permanent City, Province or Postal Code.\n";
                    }
                    if (UserDetails?.home_phone?.Trim() == "" || UserDetails?.work_phone?.Trim() == "")
                    {
                        RequiredMsgStr += "Phone, Alternative or Cell Number.\n";
                    }
                    if (Contact == null || Contact.MailingAddress1?.Trim() == "" && Contact.MailingAptUnit?.Trim() == "")
                    {
                         RequiredMsgStr += "Mailing Address.\n";
                    }
                    if (Contact == null || Contact.MailingCity?.Trim() == "" || Contact.MailingProvince?.Trim() == "" || Contact.MailingPostalCode == "")
                    {
                         RequiredMsgStr += "Mailing City, Province or Postal Code.\n";
                    }
                    if (Contact == null || Contact.MailingPhone?.Trim() == "" || Contact.MailingAlternative?.Trim() == "" || Contact.MailingCell?.Trim() == "")
                    {
                         RequiredMsgStr += "Mailing Phone, Alternative or Cell Number.\n";
                    }
                    if (udProgramData.ContractExt == null || udProgramData.ContractExt?.TuitionFees == 0 || udProgramData.ContractExt?.OtherCompulsoryFees == 0 || udProgramData.ContractExt?.MajorEquipment == 0)
                    {
                        RequiredMsgStr += "Tuition, Other Compulsory or Major Equipment.\n";
                    }
                    if (udProgramData.ContractExt == null || udProgramData.ContractExt?.InternationalStudentFees == 0)
                    {
                        RequiredMsgStr += "International Student Fees.\n";
                    }
                    if (udProgramData.ContractExt == null || udProgramData.ContractExt?.OptionalFees == 0)
                    {
                        RequiredMsgStr += "Optional Fees.\n";
                    }
                    if (udProgramData.ContractExt == null || udProgramData.ContractExt?.BookFees == 0)
                    {
                        RequiredMsgStr += "Book Fees.\n";
                    }
                    if (udProgramData.ContractExt == null || udProgramData.ContractExt?.UniformAndEquipment == 0)
                    {
                        RequiredMsgStr += "Uniform And Equipment Fees.\n";
                    }
                    if (udProgramData.ContractExt == null || udProgramData.ContractExt?.ExpendableSupplies == 0)
                    {
                        RequiredMsgStr += "Expendable Supplies Fees.\n";
                    }
                    if (udProgramData.ContractExt == null || udProgramData.ContractExt?.FieldTrips == 0)
                    {
                        RequiredMsgStr += "Field Trips Fees.\n";
                    }
                    if (udProgramData.ContractExt == null || udProgramData.ContractExt?.ProfessionalExamFees == 0)
                    {
                        RequiredMsgStr += "Professional Exam Fees.\n";
                    }
                    if (udProgramData.ContractExt == null || udProgramData.ContractExt?.GST == 0)
                    {
                        RequiredMsgStr += "GST.\n";
                    }
                    if (udProgramData.ContractExt == null || (udProgramData.ContractExt?.AdmissionReq1 == false && udProgramData.ContractExt?.AdmissionReq2 == false && udProgramData.ContractExt?.AdmissionReq3 == false && udProgramData.ContractExt?.AdmissionReq4 == false))
                    {
                        RequiredMsgStr += "Select the Admissions Requirements\n";
                    }
                    if (udProgramData.ContractExt == null || udProgramData.ContractExt?.SourceOfEnrollment?.Trim() == "" || udProgramData.ContractExt?.MethodOfPayment?.Trim() == "" || udProgramData.ContractExt?.Instalments?.Trim() == "")
                    {
                        RequiredMsgStr += "Source of Enrollment or MeInstalmentsthod of Payment or Installments.\n";
                    }
                    if (udProgramData.ContractExt == null || udProgramData.ContractExt?.SC?.Trim() == "")
                    {
                        RequiredMsgStr += "HRDC.\n";
                    }
                    if (udProgramData.ContractExt == null || udProgramData.ContractExt?.GovernmentLoan?.Trim() == "")
                    {
                        RequiredMsgStr += "Government Loan.\n";
                    }
                    if (udProgramData.ContractExt == null || udProgramData.ContractExt?.OtherLoan?.Trim() == "")
                    {
                        RequiredMsgStr += "Other Loan.\n";
                    }
                    if (udProgramData.ContractExt == null || udProgramData.ContractExt?.CASTTestScore?.Trim() == "" || udProgramData.ContractExt?.WonderlicTestScore?.Trim() == "")
                    {
                        RequiredMsgStr += "CAST Test Score or Wonderlic Test Score.\n";
                    }
                    if (UserDetails?.emergency_contact?.Trim() == "" || UserDetails?.emergency_contact_relationship?.Trim() == "")
                    {
                        RequiredMsgStr += "Emergency Name, Relationship.\n";
                    }
                    if (Contact == null || Contact?.EmergencyAddress1?.Trim() == "" || Contact?.EmergencyAptUnit?.Trim() == "")
                    {
                        RequiredMsgStr += "Emergency Address.\n";
                    }
                    if (Contact == null || Contact?.EmergencyCity?.Trim() == "" || Contact?.EmergencyProvince?.Trim() == "" || Contact?.EmergencyPostalCode?.Trim() == "")
                    {
                        RequiredMsgStr += "Emergency City, Province or Postal.\n";
                    }
                    if (UserDetails?.emergency_contact_phone?.Trim() == "" || Contact == null || Contact?.EmergencyAlternative?.Trim() == "" || Contact?.EmergencyCell?.Trim() == "")
                    {
                        RequiredMsgStr += "Emergency Phone, Alternative, Cell Number.\n";
                    }
                    LogWriter.LogWrite(" RequiredMsgStr: " + RequiredMsgStr);
                    if (RequiredMsgStr != "")
                    {
                        response.message = $"The following fields in student’s contract DO NOT contain any data. You can add the missing information in CMS.\n\n {RequiredMsgStr} \nDo you still want to continue with empty fields?";
                    }
                    else
                    {
                        response.status = true;
                        response.linkURL = PrintURL(udProgramData.contractid.ToString(), Province);
                    }
                    LogWriter.LogWrite(" Response: " + JsonSerializer.Serialize(response));
                }
                return response;
            } catch(Exception ex)
            {
                throw ex;
            }
        }

        public static PrintURLResponse CheckValidationON(UserDataProgram udProgramData, PrintURLResponse response, string UserID, string SchoolID, string Province)
        {
            try
            {
                if (udProgramData != null)
                {
                    string RequiredMsgStr = "";
                    udProgramData.ContractExt = Reports.DownloadContractExt(udProgramData.contractid);

                    var UserDetails = Reports.UserManagementDownload(Convert.ToInt32(UserID));
                    var Contact = Reports.DownloadContactEx(Convert.ToInt32(UserID));
                    var Course = Reports.DownloadUserEducationBackground(Convert.ToInt32(UserID));

                    int totalHours = Reports.GetUserDataProgramHours(SchoolID, udProgramData.programid.ToString());
                    int totalWeeks = Reports.GetUserDataProgramWeeks(SchoolID, udProgramData.programid.ToString());

                    LogWriter.LogWrite(Province + " UserDetails: " + JsonSerializer.Serialize(UserDetails));
                    LogWriter.LogWrite(Province + " Contact: " + JsonSerializer.Serialize(Contact));
                    LogWriter.LogWrite(Province + " Course: " + JsonSerializer.Serialize(Course));
                    LogWriter.LogWrite(Province + " totalHours: " + JsonSerializer.Serialize(totalHours));
                    LogWriter.LogWrite(Province + " totalWeeks: " + JsonSerializer.Serialize(totalWeeks));

                    if (udProgramData?.contractcode == "")
                    {
                        RequiredMsgStr += "Student V Number.\n";
                    }
                    if (UserDetails?.first_name?.Trim() == "")
                    {
                        RequiredMsgStr += "Student First Name.\n";
                    }
                    if (UserDetails?.last_name?.Trim() == "")
                    {
                        RequiredMsgStr += "Student Last Name.\n";
                    }
                    if (UserDetails?.sex_type_id == 0)
                    {
                        RequiredMsgStr += "Student Gender.\n";
                    }
                    if (UserDetails?.birth_date_found == false)
                    {
                        RequiredMsgStr += "Student Date of Birth.\n";
                    }
                    if (udProgramData.printName?.Trim() == "")
                    {
                        RequiredMsgStr += "Program Name.\n";
                    }
                    if (udProgramData.cstartdate == null)
                    {
                        RequiredMsgStr += "Contract Start Date.\n";
                    }
                    if (udProgramData.cschedend == null)
                    {
                        RequiredMsgStr += "Contract End Date.\n";
                    }
                    if (totalHours == 0)
                    {
                        RequiredMsgStr += "Program Hours.\n";
                    }
                    if (udProgramData.hoursPerWeek == null)
                    {
                        RequiredMsgStr += "Hours/Week.\n";
                    }
                    if (udProgramData.ptypeid == 0)
                    {
                        RequiredMsgStr += "Program Type.\n";
                    }
                    if (udProgramData.isFullTime == null)
                    {
                        RequiredMsgStr += "Class Schedule Full Time/Part Time. \n";
                    }
                    if (UserDetails?.address1?.Trim() == "" && UserDetails?.address2?.Trim() == "")
                    {
                        RequiredMsgStr += "Current Address.\n";
                    }
                    if (UserDetails?.city?.Trim() == "")
                    {
                        RequiredMsgStr += "City.\n";
                    }
                    if ( UserDetails?.province?.Trim() == "")
                    {
                        RequiredMsgStr += "State/Province.\n";
                    }
                    if (UserDetails?.postal?.Trim() == "")
                    {
                        RequiredMsgStr += "Postal Code.\n";
                    }
                    if (UserDetails?.cell_phone?.Trim() == "")
                    {
                        RequiredMsgStr += "Cell Number.\n";
                    }
                    if (UserDetails?.email?.Trim() == "")
                    {
                        RequiredMsgStr += "Email Address .\n";
                    }
                    if (!Contact.IsSameAddress && (Contact.MailingAddress1?.Trim() == "" && Contact.MailingAptUnit?.Trim() == ""))
                    {
                        RequiredMsgStr += "Mailing Address.\n";
                    }
                    if (!Contact.IsSameAddress && Contact.MailingCity?.Trim() == "")
                    {
                        RequiredMsgStr += "Mailing City.\n";
                    }
                    if (!Contact.IsSameAddress && Contact.MailingProvince?.Trim() == "")
                    {
                        RequiredMsgStr += "Mailing State/Province.\n";
                    }
                    if (!Contact.IsSameAddress && Contact.MailingPostalCode == "")
                    {
                        RequiredMsgStr += "Mailing Postal Code.\n";
                    }
                    if (!Contact.IsSameAddress && Contact.MailingCountry == "")
                    {
                        RequiredMsgStr += "Mailing Country.\n";
                    }
                    LogWriter.LogWrite(" RequiredMsgStr: " + RequiredMsgStr);
                    if (RequiredMsgStr != "")
                    {
                        response.message = $"The following fields in student’s contract DO NOT contain any data. You can add the missing information in CMS.\n\n {RequiredMsgStr} \nDo you still want to continue with empty fields?";
                    }
                    else
                    {
                        response.status = true;
                        response.linkURL = PrintURL(udProgramData.contractid.ToString(), Province);
                    }
                    LogWriter.LogWrite(" Response: " + JsonSerializer.Serialize(response));
                }
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void setTemplateData(Template SchoolDetails)
        {
            switch (SchoolDetails.TemplateTypeID)
            {
                case 0:
                    SchoolIndividualContractReportName = SchoolDetails.TemplateReportName;
                    break;
                case 1:
                    SchoolInvoiceReportName = SchoolDetails.TemplateReportName;
                    break;
                case 2:
                    SchoolTranscriptReportName = SchoolDetails.TemplateReportName;
                    break;
                case 3:
                    SchoolPaymentReportName = SchoolDetails.TemplateReportName;
                    break;
                case 4:
                    SchoolReceiptReportName = SchoolDetails.TemplateReportName;
                    break;
                case 5:
                    SchoolCertificateContractReportName = SchoolDetails.TemplateReportName;
                    break;
                case 6:
                    SchoolDiplomaContractReportName = SchoolDetails.TemplateReportName;
                    break;
            }
        }
        public static PrintURLResponse PrintContractsWithoutValidation(string ContractID, string Province)
        {
            PrintURLResponse response = new PrintURLResponse();
            if (Province == "SK" || Province == "Saskatchewan")
            {
                response.status = true;
                response.linkURL = PrintURL(ContractID, "SK");
            }
            else if (Province == "MB" || Province == "Manitoba")
            {
                response.status = true;
                response.linkURL = PrintURL(ContractID, "MB");
            }
            else if (Province == "ON" || Province == "Ontario")
            {
                response.status = true;
                response.linkURL = PrintURL(ContractID, "ON");
            }
            return response;
        }
        public static string PrintURL(string ContractID, string Province)
        {
            string reportsURL = System.Configuration.ConfigurationManager.AppSettings["ReportsURL"];
            string baseUrl = $"{reportsURL}/DocXTest/Default.aspx";

            // Construct the dynamic URL with parameters
            string dynamicUrl = $"{baseUrl}?ContractID={Uri.EscapeDataString(ContractID)}&Province={Uri.EscapeDataString(Province)}";

            return dynamicUrl;
        }
    }
}
