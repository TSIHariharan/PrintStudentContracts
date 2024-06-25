using System;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Collections.Generic;
using PrintStudentContracts.Models;
using System.Collections;
using Dapper;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace PrintStudentContracts
{
    public class Reports
    {
        public static string GetUserSchoolProvince(string UserID)
        {
            string province = "";
            string sqltext =
                "SELECT CASE WHEN (SD.Province IS NOT NULL AND SD.Province != ' ') THEN Upper(SD.Province) ELSE SD.Country END Province FROM ACME_AOL_TEST.dbo.SchoolDetail SD " +
                "INNER JOIN ACME_MAIN_TEST.dbo.[User] U ON U.SchoolID = SD.SchoolID " +
                "WHERE U.UserID = @UserID ";
            DBConnection dbConnection = new DBConnection("AOL");
            dbConnection.connection.Open();
            using (SqlCommand cmd = new SqlCommand(sqltext, dbConnection.connection))
            {
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;
                object objProvince = cmd.ExecuteScalar();
                if (objProvince != null || objProvince != DBNull.Value)
                    province = objProvince.ToString();
            }
            dbConnection.connection.Close();
            return province;
        }

        public static UserDataProgram GetUserDataProgramParticipationMark(string UserID, string contractID)
        {
            UserDataProgram up = new UserDataProgram();
            try
            {
                string sqltext = "ACME_AOL_TEST.dbo.st_GetUserDataNew2_StudentContracts";

                DBConnection dbConnection = new DBConnection("AOL");
                dbConnection.connection.Open();

                up = dbConnection.connection.Query<UserDataProgram>(sqltext,
                new { UserID = Convert.ToInt32(UserID), ContractID = Convert.ToInt32(contractID) },
                commandType: CommandType.StoredProcedure).FirstOrDefault();

                dbConnection.connection.Close();

                return up;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int GetUserDataProgramHours(string schoolID, string programID)
        {
            int totalHours = 0; 
            try
            {
                string sqltext = "ACME_AOL_TEST.dbo.aol_GetSchoolProgramAllHours";

                DBConnection dbConnection = new DBConnection("AOL");
                dbConnection.connection.Open();

                // Define DynamicParameters to handle output parameters
                var parameters = new DynamicParameters();
                parameters.Add("@SchoolID", Convert.ToInt32(schoolID));
                parameters.Add("@ProgramID", Convert.ToInt32(programID));
                parameters.Add("@InclassHours", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 8);
                parameters.Add("@PracticumHours", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 8);
                parameters.Add("@ProgramTotalHours", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 8);

                // Execute the stored procedure with output parameters
                dbConnection.connection.Execute(sqltext, parameters, commandType: CommandType.StoredProcedure);

                totalHours = parameters.Get<int>("@ProgramTotalHours");

                dbConnection.connection.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return totalHours;
        }

        public static int GetUserDataProgramWeeks(string schoolID, string programID)
        {
            int totalweeks = 0;
            try
            {
                string sqltext = @"
                                SELECT ISNULL(MaxWeeks, 0) AS MaxWeeks
                                FROM ACME_AOL_TEST.dbo.SchoolProgram 
                                WHERE ProgramID = @ProgramID AND SchoolID = @SchoolID;
                            ";

                DBConnection dbConnection = new DBConnection("AOL");
                dbConnection.connection.Open();

                // Define parameters using an anonymous type
                var parameters = new { SchoolID = Convert.ToInt32(schoolID), ProgramID = Convert.ToInt32(programID) };

                // Execute the query and retrieve the result
                var result = dbConnection.connection.QueryFirstOrDefault<dynamic>(sqltext, parameters);

                // Check if the result is not null before accessing the property
                if (result != null)
                {
                    totalweeks = Convert.ToInt32(result.MaxWeeks);
                }

                dbConnection.connection.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return totalweeks;
        }

        public static bool GetEnableProvinceParams(string SchlID)
        {
            bool EnableProviceContract = false;
            try
            {
                int SchoolID = Convert.ToInt32(SchlID);
                string sql = @"SELECT EnableProvinceContract
                           FROM ACME_AOL_TEST.dbo.SchoolDetail 
                           WHERE SchoolID = @SchoolID ";


                DBConnection dbConnection = new DBConnection("AOL");
                dbConnection.connection.Open();
                using (SqlCommand cmd = new SqlCommand(sql, dbConnection.connection))
                {
                    cmd.Parameters.Add("@SchoolID", SqlDbType.Int).Value = SchoolID;

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        EnableProviceContract = reader.IsDBNull(0) ? false : reader.GetBoolean(0);
                    }
                }
                dbConnection.connection.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return EnableProviceContract;
        }

        public static ContractExt DownloadContractExt(int ContractID)
        {
            ContractExt contExt = new ContractExt();
            try
            {
                string sqltext =
                "SELECT IsFullTime, IsPartTime, IsMonToFri, Days, Times1, Times2, " +
                "  AdmissionReq1, AdmissionReq2, AdmissionReq3, AdmissionReq4, " +
                "  0.0 AS TuitionFees, OtherCompulsoryFees, MajorEquipment, InternationalStudentFees, OptionalFeeContent, OptionalFees, " +
                "  BookFees, UniformAndEquipment, ExpendableSupplies, FieldTrips, ProfessionalExamFees, GST, " +
                "  LanguageOfInstruction, LocationOfPracticum, " +
                "  CASTTestScore, WonderlicTestScore, SourceOfEnrollment, MethodOfPayment, Instalments, SC, GovernmentLoan, OtherLoan " +
                "FROM ACME_AOL_TEST.dbo.ContractExt " +
                "WHERE ContractID = @ContractID ";

                DBConnection dbConnection = new DBConnection("AOL");
                dbConnection.connection.Open();
                contExt = dbConnection.connection.QueryFirstOrDefault<ContractExt>(sqltext, new { ContractID = ContractID });
                dbConnection.connection.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return contExt;
        }

        public static ContactEx DownloadContactEx(int userID)
        {
            ContactEx contactEx = new ContactEx();
            try
            {
                string sqltext = @"SELECT 
                                    UD.IsSameAddress, 
                                    MA.Address1 as MailingAddress1, 
                                    MA.AptUnit as MailingAptUnit, 
                                    MA.City as MailingCity, 
                                    MA.Province as MailingProvince, 
                                    MA.Postal as MailingPostalCode, 
                                    MA.Country as MailingCountry, 
                                    UD.ResidentialPhoneNumber as MailingPhone, 
                                    UD.AlternativePhoneNumber as MailingAlternative, 
                                    UD.ResidentialCellNumber as MailingCell, 
                                    UD.IsInternationalStudent as IsInternationalStudent, 
                                    UD.LastGradeCompletedYear as GradeYear, 
                                    EA.Address1 as EmergencyAddress1,  
                                    EA.AptUnit as EmergencyAptUnit, 
                                    EA.City as EmergencyCity, 
                                    EA.Province as EmergencyProvince, 
                                    EA.Postal as EmergencyPostalCode, 
                                    EA.Country as EmergencyCountry, 
                                    UD.EmergencyAlternativePhone as EmergencyAlternative, 
                                    UD.EmergencyCell as EmergencyCell
                                FROM ACME_AOL_TEST.dbo.UserDetail1 UD 
                                LEFT JOIN ACME_AOL_TEST.dbo.Address MA ON MA.AddressID = UD.ResidentialAddressID 
                                LEFT JOIN ACME_AOL_TEST.dbo.Address EA ON EA.AddressID = UD.EmergencyAddressID 
                                WHERE UD.UserID = @UserID";

                DBConnection dbConnection = new DBConnection("MAIN");
                dbConnection.connection.Open();
                contactEx = dbConnection.connection.QueryFirstOrDefault<ContactEx>(sqltext, new { UserID = userID });
                dbConnection.connection.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return contactEx;
        }

        public static EducationVM DownloadUserEducationBackground(int userID)
        {
            EducationVM contactEx = new EducationVM();
            try
            {
                string sqltext = @"SELECT 
                                    hs.Grade as grade, 
                                    hs.IsGraduate as ishighschoolgraduate,
                                    hs.[Name] as high_school_name, 
                                    hs.Location as high_school_location,
                                    hs.FavSubject as favsubject,
                                    hs.BestSubject as bestsubject,
                                    hs.LeastFavSubject as leastfavsubject, 
                                    hs.GraduationYr as high_school_gradyr, 
                                    ps.EducationTypeID as education_type_id,
                                    ps.OtherName as other_name,
                                    ps.[Name] as post_school_name,
                                    ps.Location as post_school_location,
                                    ps.CourseTaken as course_taken,
                                    ps.CourseLength as course_length,
                                    ps.IsGraduate as ispostschoolgraduate,
                                    ps.GraduationYr as post_school_gradyr
                FROM ACME_AOL_TEST.dbo.UserHighSchool hs 
                INNER JOIN ACME_AOL_TEST.dbo.UserPostSchool ps ON hs.UserID = ps.UserID 
                WHERE ps.UserID = @UserID and hs.UserID = @UserID";

                DBConnection dbConnection = new DBConnection("MAIN");
                dbConnection.connection.Open();
                contactEx = dbConnection.connection.QueryFirstOrDefault<EducationVM>(sqltext, new { UserID = userID });
                dbConnection.connection.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return contactEx;
        }

        private static string GetStringFromReader(SqlDataReader reader, int i)		// return emptystring if DBNull or blank
        {
            char empty_char = Convert.ToChar(1);
            string empty_string = new string(empty_char, 1);

            string return_string = reader.IsDBNull(i) ? string.Empty : reader.GetString(i);

            if (return_string == string.Empty)
                return_string = empty_string;

            return return_string;
        }

        public static UserData UserManagementDownload(int UserID)
        {
            UserData userDetails = new UserData();
            try
            {
                int manage_user_id = getUserID(UserID);
                bool access_granted = getUserAccess(UserID);
                string extra_roles = "(1,2,3"; // This variable not used instead directly added in line 190 . To Get Roles List
                int school_id = getSchoolIDforUser(manage_user_id);
                int current_admin_rank = 0;
                if (manage_user_id == 0) return userDetails;

                DBConnection dbConnection = new DBConnection("MAIN");
                dbConnection.connection.Open();

                string sql = "SELECT U.RoleID, R.Rank FROM [User] U INNER JOIN Role R ON R.RoleID = U.RoleID WHERE U.UserID = @UserID";

                var result = dbConnection.connection.QueryFirstOrDefault(sql, new { UserID });
                if (result != null && result.Rank != null)
                {
                    current_admin_rank = result.Rank;
                }

                if (current_admin_rank >= 4)
                {
                    extra_roles += ",4";
                }
                if (current_admin_rank >= 5)
                {
                    extra_roles += ",5";
                }
                if (current_admin_rank >= 6)
                {
                    extra_roles += ",6";
                }
                extra_roles += ")";

                string sqltext = @"SELECT 
                                    U.UserID,
                                    U.SchoolID,
                                    U.RoleID as role_id,
                                    U.LockID as lock_id,
                                    U.Username as username,
                                    U.[Password] as password,
                                    U.DateCreated as date_created,
                                    CASE 
                                        WHEN U.DateCreated IS NULL OR U.DateCreated = '' THEN 0 
                                        ELSE 1 
                                    END AS date_created_found,
                                    S.[Name] AS school_name,
                                    UD.ClientTypeID as client_type_id,
                                    UD.StudyTypeID as study_type_id,
                                    UD.CompanyID as company_id,
                                    UD.EducationTypeID as education_type_id,
                                    UD.IncomeLevelID as income_level_id,
                                    UD.SexTypeID as sex_type_id,
                                    UD.AgeAtEntranceID as age_at_entrance_id,
                                    UD.ReferralID as referral_id,
                                    UD.FirstLanguageID as first_language_id,
                                    UD.FundingSourceID as funding_source_id,
                                    UD.FirstName as first_name,
                                    UD.LastName as last_name,
                                    UD.Initials as initials,
                                    UD.FullName as full_name,
                                    UD.Title as title,
                                    UD.Address1 as address1,
                                    UD.Address2 as address2,
                                    UD.City as city,
                                    Upper(UD.Province) as province,
                                    UD.Postal as postal,
                                    UD.Country as country,
                                    UD.HomePhone as home_phone,
                                    UD.WorkPhone as work_phone,
                                    UD.CellPhone as cell_phone,
                                    UD.Email as email,
                                    UD.[SIN] as sin,
                                    UD.EmergencyContact as emergency_contact,
                                    UD.EmergencyContactPhone as emergency_contact_phone,
                                    UD.EmergencyContactRelationship as emergency_contact_relationship,
                                    UD.StudentNumber as student_number,
                                    UD.ArchiveBox as archive_box,
                                    CASE 
                                        WHEN UD.Notes IS NOT NULL OR DATALENGTH(UD.Notes) > 255 
                                        THEN SUBSTRING(UD.Notes, 0, 255) 
                                        ELSE UD.Notes 
                                    END AS notes,
                                    UD.BirthDate AS birth_date,
                                    CASE 
                                        WHEN UD.BirthDate IS NULL OR UD.BirthDate = '' 
                                        THEN 0 
                                        ELSE 1 
                                    END AS birth_date_found,
                                    CASE 
                                        WHEN U.RoleID > 2 THEN 2 
                                        ELSE 1 
                                    END marked_as,
                                    UD.IsStaff AS is_staff,
                                    UD.ReferralTypeID as referral_type_id,
                                    U.LeadTrackingID as CampusLoginID
                                FROM 
                                    [User] U
                                INNER JOIN 
                                    School S ON S.SchoolID = U.SchoolID
                                INNER JOIN 
                                    ACME_AOL_TEST.dbo.UserDetail UD ON UD.UserID = U.UserID
                                WHERE 
                                    U.UserID = @ManageUserID;

                                    SELECT RoleID, [Name] 
                                    FROM Role 
                                    WHERE (SchoolID = @SchoolID OR RoleID IN (1,2,3)) AND Rank <= @CurrentAdminRank 
                                    ORDER BY [Name];

                                    SELECT LockID, [Name] FROM Lock ORDER BY [Name];

                                    SELECT SexTypeID, [Name] FROM ACME_AOL_TEST.dbo.SexType;

                                    SELECT StudyTypeID, [Name] FROM ACME_AOL_TEST.dbo.StudyType;

                                    SELECT ReferralTypeID, [Name] FROM ACME_AOL_TEST.dbo.ReferralType 
                                    WHERE ISNULL(Hidden, 0) = 0 ORDER BY [Name];

                                    SELECT ReferralID, ReferralTypeID, [Name] FROM ACME_AOL_TEST.dbo.Referral 
                                    WHERE SchoolID = @SchoolID;

                                    SELECT IncomeLevelID, [Name] FROM ACME_AOL_TEST.dbo.IncomeLevel ORDER BY [Name];

                                    SELECT AgeAtEntranceID, [Name] FROM ACME_AOL_TEST.dbo.AgeAtEntrance;

                                    SELECT EducationTypeID, [Name] FROM ACME_AOL_TEST.dbo.EducationType ORDER BY [Name];

                                    SELECT FirstLanguageID, [Name] FROM ACME_AOL_TEST.dbo.FirstLanguage ORDER BY [Name];

                                    SELECT FundingSourceID, [Name] FROM ACME_AOL_TEST.dbo.FundingSource ORDER BY [Name];

                                    SELECT ClientTypeID, [Name] FROM ACME_AOL_TEST.dbo.ClientType ORDER BY [Name];

                                    SELECT CompanyID, [Name] FROM ACME_AOL_TEST.dbo.Company WHERE SchoolID = @SchoolID ORDER BY [Name];

                                    SELECT ReferenceNo FROM ACME_AOL_TEST.dbo.LeadFunding WHERE UserID = @ManageUserID;
                                ";

                using (var multi = dbConnection.connection.QueryMultiple(sqltext, new { ManageUserID = manage_user_id, SchoolID = school_id, CurrentAdminRank = current_admin_rank }))
                {
                    userDetails = multi.Read<UserData>().SingleOrDefault();
                    var roles = multi.Read<Role>().ToList();
                    var locks = multi.Read<Lock>().ToList();
                    var sexTypes = multi.Read<SexType>().ToList();
                    var studyTypes = multi.Read<StudyType>().ToList();
                    var referralTypes = multi.Read<ReferralType>().ToList();
                    var referrals = multi.Read<Referral>().ToList();
                    var incomeLevels = multi.Read<IncomeLevel>().ToList();
                    var ageAtEntrances = multi.Read<AgeAtEntrance>().ToList();
                    var educationTypes = multi.Read<EducationType>().ToList();
                    var firstLanguages = multi.Read<FirstLanguage>().ToList();
                    var fundingSources = multi.Read<FundingSource>().ToList();
                    var clientTypes = multi.Read<ClientType>().ToList();
                    var companies = multi.Read<Company>().ToList();
                    var leadFundingReferenceNo = multi.Read<string>().SingleOrDefault();

                    if (userDetails != null)
                    {
                        userDetails.Roles = roles;
                        userDetails.Locks = locks;
                        userDetails.SexTypes = sexTypes;
                        userDetails.StudyTypes = studyTypes;
                        userDetails.ReferralTypes = referralTypes;
                        userDetails.Referrals = referrals;
                        userDetails.IncomeLevels = incomeLevels;
                        userDetails.AgeAtEntrances = ageAtEntrances;
                        userDetails.EducationTypes = educationTypes;
                        userDetails.FirstLanguages = firstLanguages;
                        userDetails.FundingSources = fundingSources;
                        userDetails.ClientTypes = clientTypes;
                        userDetails.Companies = companies;
                        userDetails.LeadFundingReferenceNo = leadFundingReferenceNo;
                    }
                }

                dbConnection.connection.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return userDetails;
        }

        public static int getUserID(int userID)
        {
            int manage_user_id = 0;
            try
            {
                manage_user_id = userID;
            }
            catch (Exception)
            {
            }
            return manage_user_id;
        }
        public static bool getUserAccess(int userID)
        {
            bool access_granted = false;
            try
            {
                string sqltext = "SELECT U.UserID FROM [User] U INNER JOIN UserModule UM ON UM.UserID = U.UserID WHERE U.UserID = " + userID + " AND U.RoleID > 2 AND UM.ModuleID = 34";

                DBConnection dbConnection = new DBConnection("MAIN");
                dbConnection.connection.Open();
                using (SqlCommand cmd = new SqlCommand(sqltext, dbConnection.connection))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        access_granted = true;
                    }
                }
                dbConnection.connection.Close();
            }
            catch (Exception)
            {
            }
            return access_granted;
        }

        public static int getSchoolIDforUser(int userID)
        {
            int school_id = 0;
            try
            {
                string sqltext = "SELECT SchoolID FROM [User] WHERE UserID = " + userID;

                DBConnection dbConnection = new DBConnection("MAIN");
                dbConnection.connection.Open();
                using (SqlCommand cmd = new SqlCommand(sqltext, dbConnection.connection))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        if (!reader.IsDBNull(0)) { school_id = reader.GetInt32(0); }
                    }
                }
                dbConnection.connection.Close();
            }
            catch (Exception)
            {
            }
            return school_id;
        }

        public static Template GetSchoolTemplateInherited(int SchoolID)
        {
            Template temp = new Template();
            try
            {
                string sqltext =
                "SELECT TemplateTypeID " +
                "FROM ACME_AOL_TEST.dbo.TemplateType " +
                "WHERE TemplateTypeID < 100 ";
                ArrayList TemplateTypeList = new ArrayList();
                DBConnection dbConnection = new DBConnection("MAIN");
                dbConnection.connection.Open();
                using (SqlCommand cmd = new SqlCommand(sqltext, dbConnection.connection))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        int TemplateTypeID = reader.GetInt32(0);
                        TemplateTypeList.Add(TemplateTypeID);
                    }
                }

                ArrayList TemplateIDList = new ArrayList();
                using (SqlCommand cmd = new SqlCommand("ACME_AOL_TEST.dbo.np_GetSchoolTemplateInherited", dbConnection.connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@SchoolID", SqlDbType.Int).Value = SchoolID;
                    cmd.Parameters.Add("@TemplateTypeID", SqlDbType.Int);
                    cmd.Parameters.Add("@CurrTemplateID", SqlDbType.Int).Direction = ParameterDirection.Output;

                    foreach (int TemplateTypeID in TemplateTypeList)
                    {
                        cmd.Parameters["@TemplateTypeID"].Value = TemplateTypeID;
                        cmd.ExecuteNonQuery();
                        int TemplateID = (int)cmd.Parameters["@CurrTemplateID"].Value;
                        TemplateIDList.Add(TemplateID);
                    }
                }

                string sqltextString =
                    "SELECT T.TemplateID, T.TemplateTypeID, T.TemplateName, T.TemplateReportName " +
                    "FROM ACME_AOL_TEST.dbo.Template T " +
                    "WHERE T.TemplateID = @TemplateID ";

                using (SqlCommand cmd = new SqlCommand(sqltextString, dbConnection.connection))
                {
                    cmd.Parameters.Add("@TemplateID", SqlDbType.Int);
                    foreach (int CurrTemplateID in TemplateIDList)
                    {
                        cmd.Parameters["@TemplateID"].Value = CurrTemplateID;
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            temp.TemplateID = reader.GetInt32(0);
                            temp.TemplateTypeID = reader.GetInt32(1);
                            temp.TemplateName = GetStringFromReader(reader, 2);
                            temp.TemplateReportName = GetStringFromReader(reader, 3);
                        }
                        reader.Close();
                    }
                }
                dbConnection.connection.Close();
            }
            catch (Exception ex)
            {

            }
            return temp;
        }
    }
}
