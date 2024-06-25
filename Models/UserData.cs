using System;
using System.Collections.Generic;

namespace PrintStudentContracts.Models
{
    public class UserData
    {
        public int role_id { get; set; }
        public int lock_id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public DateTime date_created { get; set; }
        public bool date_created_found { get; set; }
        public string school_name { get; set; }
        public int client_type_id { get; set; }
        public int study_type_id { get; set; }
        public int company_id { get; set; }
        public int education_type_id { get; set; }
        public int income_level_id { get; set; }
        public int sex_type_id { get; set; }
        public int age_at_entrance_id { get; set; }
        public int referral_id { get; set; }
        public int first_language_id { get; set; }
        public int funding_source_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string initials { get; set; }
        public string full_name { get; set; }
        public string title { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string postal { get; set; }
        public string country { get; set; }
        public string home_phone { get; set; }
        public string work_phone { get; set; }
        public string cell_phone { get; set; }
        public string email { get; set; }
        public string sin { get; set; }
        public string emergency_contact { get; set; }
        public string emergency_contact_phone { get; set; }
        public string emergency_contact_relationship { get; set; }
        public string student_number { get; set; }
        public string archive_box { get; set; }
        public string notes { get; set; }
        public DateTime birth_date { get; set; }
        public bool birth_date_found { get; set; }
        public int marked_as { get; set; }
        public bool is_staff { get; set; }
        public int referral_type_id { get; set; }
        public string referencenumber { get; set; }
        public int CampusLoginID { get; set; }

        // Collections for related entities
        public List<Role> Roles { get; set; }
        public List<Lock> Locks { get; set; }
        public List<SexType> SexTypes { get; set; }
        public List<StudyType> StudyTypes { get; set; }
        public List<ReferralType> ReferralTypes { get; set; }
        public List<Referral> Referrals { get; set; }
        public List<IncomeLevel> IncomeLevels { get; set; }
        public List<AgeAtEntrance> AgeAtEntrances { get; set; }
        public List<EducationType> EducationTypes { get; set; }
        public List<FirstLanguage> FirstLanguages { get; set; }
        public List<FundingSource> FundingSources { get; set; }
        public List<ClientType> ClientTypes { get; set; }
        public List<Company> Companies { get; set; }

        // Additional property
        public string LeadFundingReferenceNo { get; set; }
    }

    public class Role
    {
        public int RoleID { get; set; }
        public string Name { get; set; }
    }

    public class Lock
    {
        public int LockID { get; set; }
        public string Name { get; set; }
    }

    public class SexType
    {
        public int SexTypeID { get; set; }
        public string Name { get; set; }
    }

    public class StudyType
    {
        public int StudyTypeID { get; set; }
        public string Name { get; set; }
    }

    public class ReferralType
    {
        public int ReferralTypeID { get; set; }
        public string Name { get; set; }
    }

    public class Referral
    {
        public int ReferralID { get; set; }
        public int ReferralTypeID { get; set; }
        public string Name { get; set; }
    }

    public class IncomeLevel
    {
        public int IncomeLevelID { get; set; }
        public string Name { get; set; }
    }

    public class AgeAtEntrance
    {
        public int AgeAtEntranceID { get; set; }
        public string Name { get; set; }
    }

    public class EducationType
    {
        public int EducationTypeID { get; set; }
        public string Name { get; set; }
    }

    public class FirstLanguage
    {
        public int FirstLanguageID { get; set; }
        public string Name { get; set; }
    }

    public class FundingSource
    {
        public int FundingSourceID { get; set; }
        public string Name { get; set; }
    }

    public class ClientType
    {
        public int ClientTypeID { get; set; }
        public string Name { get; set; }
    }

    public class Company
    {
        public int CompanyID { get; set; }
        public string Name { get; set; }
    }
}
