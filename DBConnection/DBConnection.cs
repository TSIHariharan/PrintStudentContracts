using System;
using System.Data.SqlClient;
using System.Data;

namespace PrintStudentContracts
{
    public class DBConnection
    {
        public static string ACME_MAIN_TESTConnectionString => System.Configuration.ConfigurationManager.ConnectionStrings["acme_main_test_CS"].ConnectionString;
        public static string ACME_AOL_TESTConnectionString => System.Configuration.ConfigurationManager.ConnectionStrings["acme_aol_test_CS"].ConnectionString;


        public const string SSES_SCHOOLNAME = "T2202_SchoolName";
        public const string APPL_ALLSCHOOLS = "T2202_AllSchools";

        public SqlConnection connection;

        public DBConnection(string dbName)
        {
            switch (dbName)
            {
                case "AOL":
                    connection = new SqlConnection(ACME_AOL_TESTConnectionString);
                    break;
                case "MAIN":
                    connection = new SqlConnection(ACME_MAIN_TESTConnectionString);
                    break;
                default:
                    connection = new SqlConnection(ACME_AOL_TESTConnectionString);
                    break;
            }

        }

        public static DataTable GetDataTable(string sql, SqlConnection Conn)
        {
            if (Conn.State.ToString().ToLower() != "open")
            {
                Conn.Open();
            }
            SqlDataAdapter da = new SqlDataAdapter(sql, Conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            //release resources
            da.Dispose();
            da = null;
            Conn.Close();
            Conn = null;

            return dt;
        }
    }
}
