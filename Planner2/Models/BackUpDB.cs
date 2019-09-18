using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Planner2.Models
{
    class BackUpDB
    {
        public static void BackUpNow()
        {
           Task.Run(() => {
                string outputDir = System.IO.Path.GetDirectoryName(MvcApplication.filePath) + @"\FileUpload\";

                string[] fileArray = Directory.GetFiles(outputDir).Where(v=>v.Contains(".bak")).ToArray();
                if (fileArray != null && fileArray.Where(Z => Z.Contains("Planner2_" + DateTime.Now.ToString("yyyyMMdd"))).Count() > 0)
                {
                     return;
                }
              
                var fileurl =  GetTableData(@"DECLARE @fileName VARCHAR(max);
                                    DECLARE @dbName VARCHAR(20);
                                    DECLARE @fileDate VARCHAR(max);
                                    SET @fileName = '" + outputDir + @"';
                                    SET @dbName = 'Planner2';
                                    SET @fileDate = CONVERT(VARCHAR(20), GETDATE(), 112) + '_' + CONVERT(nvarchar(50), newid());
                                    SET @fileName = @fileName + @dbName + '_' + RTRIM(@fileDate) + '.bak';
                                    BACKUP DATABASE @dbName TO DISK = @fileName;
                                    select @fileName").Rows[0][0].ToString();

                
         });
        }
        public static string StrConnection = @"Data Source=18.136.70.89;Initial Catalog=Planner2;User ID=sa;Password=Rorze123456;";
         public static DataTable GetTableData(string SQLSelect)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(StrConnection);

            SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(SQLSelect, conn);
            da.SelectCommand.CommandTimeout = 6699;
            try
            {
                if (conn.State != ConnectionState.Open)
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


    }
}
