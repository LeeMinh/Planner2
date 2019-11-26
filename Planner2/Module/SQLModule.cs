using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml;

namespace Planner2.Module
{
    public class SQLModule
    {
         public static string StrConnection = @"Data Source=112.78.2.70,1433;Initial Catalog=cho10227_BDS;User ID=cho10227_admin;Password=Trang262456;";
        public static DataTable GetTableData(string SQLSelect, string StrCon = "")
        {
            if (StrCon == "")
            {
                StrCon = StrConnection;
            }
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(StrCon);

            SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(SQLSelect, conn);
            da.SelectCommand.CommandTimeout = 6699;
            try
            {
                conn.Open();
                da.Fill(dt);
                conn.Close();
                return dt;
            }
            catch (Exception e)
            {
                conn.Close();

                return null;
            }
        }
     

        public static int ExcuteCommand(string CommandStr = "", string StrCon = "")
        {
            if (StrCon == "")
            {
                StrCon = StrConnection;
            }
            int i = 0;
            var CN = new SqlConnection(StrCon);
            try
            {
                var CMD = new SqlCommand(CommandStr, CN);
                CN.Open();
                CMD.CommandTimeout = 9999;
                i = CMD.ExecuteNonQuery();
            }
            catch (Exception e)
            {


                return -1;

            }
            CN.Close();

            return i;
        }



    }
}