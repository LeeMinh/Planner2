using Newtonsoft.Json;
using Planner2.Module;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planner2.Controllers
{
    public class TableEdittorController : BaseController
    {
        public class MasterTableDAL
        {
            public class columns
            {
                public bool allowEditing { get; set; }
                public string caption { get; set; }
                public string dataField { get; set; }
                public string dataType { get; set; }
            }

            public static string GetJsonFields(String querydata, string tableName)
            {
                string sqlCommand = querydata;
                string primaryKeyName = GetPrimaryKey(tableName);
                var ds = SQLModule.GetTableData("select COLUMN_NAME,DATA_TYPE from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='"+ tableName + "'");
                if (ds != null && ds.Rows.Count > 0)
                {
                    //prepare structure for jtable

                    var ListObj = new List<columns>();
                    ListObj.Add(new columns
                    {
                        caption = primaryKeyName + "(KEY)",
                        dataField = primaryKeyName,
                        allowEditing = false,
                    });
                    for (int i = 0; i < ds.Rows.Count; i++)
                    {
                        var ColumnName = ds.Rows[i]["COLUMN_NAME"].ToString();
                        var dataType = ds.Rows[i]["DATA_TYPE"].ToString();
                        if (ColumnName != primaryKeyName)
                        {
                              dataType = GetDataType(dataType);
                            ListObj.Add(new columns
                            {
                                caption =  ColumnName + "(" + dataType + ")",
                                dataField =  ColumnName,
                                allowEditing = true,
                                dataType = dataType
                            });
                        }
                    }
                    //foreach (DataColumn col in ds.Columns)
                    //{
                    //    if (col.ColumnName != primaryKeyName)
                    //    {
                    //        string dataType = GetDataType(col);
                    //        ListObj.Add(new columns
                    //        {
                    //            caption = col.ColumnName + "(" + dataType + ")",
                    //            dataField = col.ColumnName,
                    //            allowEditing = true,
                    //            dataType = dataType
                    //        });
                    //    }
                    //}

                    return JsonConvert.SerializeObject(ListObj);

                }

                return string.Empty;
            }

            public static string GetDataType(string col)
            {
                switch (col)
                {
                    case "float": return "number";
                    case "int": return "number";
                  
                    case "date": return "datetime";
                    case "datetime": return "datetime";

                    case "bit": return "number";

                    default: return "string";

                }


            }
            public static DataTable GetListOfRecords(string tableName)
            {
                string records = tableName;
                return SQLModule.GetTableData(records);
                //return this.Content(returntext, "application/json");
            }
            public static string AddRecord(string tableName, string fieldList, string fieldValues)
            {
                string sqlCommand = "insert into " + tableName + "(" + fieldList + ") Values (" + fieldValues + "); ";
                SQLModule.ExcuteCommand(sqlCommand);

                //get primary key
                string primaryKey = GetPrimaryKey(tableName);

                //assuming that we will get a table and a row everytime
                string newRecSuccess = "{\"Result\":\"OK\"}";
                return newRecSuccess;
            }
            public static bool UpdateRecord(string tableName, string updateColumnValues)
            {
                string sqlCommand = "update " + tableName + " SET " + updateColumnValues;
                SQLModule.ExcuteCommand(sqlCommand);
                return true;
            }
            public static bool DeleteRecord(string tableName, string primaryKey, string primaryKeyValue)
            {
                string sqlCommand = "delete " + tableName + " WHERE " + primaryKey + " = " + primaryKeyValue;
                SQLModule.ExcuteCommand(sqlCommand);

                return true;
            }

            public static Dictionary<string, string> GetColumns(string tableName)
            {
                  var ds = SQLModule.GetTableData("select COLUMN_NAME,DATA_TYPE from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='" + tableName + "'");

                string primaryKeyName = GetPrimaryKey(tableName);

                //first element will always be primary key
                Dictionary<string, string> ColumnsList = new Dictionary<string, string>();
                ColumnsList.Add(primaryKeyName, "System.Int32");


                if (ds != null && ds.Rows.Count > 0)
                {

                    //prepare primary key; keeping it in a separate loop , not sure what can be the position
                    for (int i = 0; i < ds.Rows.Count; i++)
                    {
                        var ColumnName = ds.Rows[i]["COLUMN_NAME"].ToString();
                        var dataType = ds.Rows[i]["DATA_TYPE"].ToString();
                        if (ColumnName != primaryKeyName)
                        {
                            ColumnsList.Add(ColumnName, dataType);
                        }
                    }
                }

                return ColumnsList;
            }

            public static string GetPrimaryKey(string tableName)
            {
                tableName = tableName.Replace("[", "");
                tableName = tableName.Replace("]", "");
                string primaryKeyQuery = "SELECT  column_name as primarykeycolumn FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TC " +
    "INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KU ON TC.CONSTRAINT_TYPE = 'PRIMARY KEY' AND TC.CONSTRAINT_NAME = KU.CONSTRAINT_NAME and ku.table_name='{0}' " +
    "ORDER BY KU.TABLE_NAME, KU.ORDINAL_POSITION;";
                primaryKeyQuery = String.Format(primaryKeyQuery, tableName);

                var data = SQLModule.GetTableData(primaryKeyQuery);
                string primaryKeyName = "";
                if (data != null && data.Rows.Count > 0)
                {
                    primaryKeyName = data.Rows[0][0].ToString();
                }
                return primaryKeyName;
            }

        }

        // GET: TableEdittor
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ListTable()
        {
            using (Models.Planner2Entities db = new Models.Planner2Entities())
            {
                var nd = (Planner2.Models.User)Session[Planner2.Controllers.LoginAuth.NameSession];
                var data = db.DataViewers.Where(z => z.Show == 1 && z.UserView != null && (z.UserView.Contains("*") || z.UserView.Contains(nd.StaffID))).Select(Z => new { Z.Name, Z.ID }).ToList();
                return JsonMax(data);
            }
        }
        public string ToJson(DataTable dt)
        {
            List<Dictionary<string, object>> lst = new List<Dictionary<string, object>>();
            Dictionary<string, object> item;
            foreach (DataRow row in dt.Rows)
            {
                item = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    item.Add(col.ColumnName, (Convert.IsDBNull(row[col]) ? null : row[col]));
                }
                lst.Add(item);
            }
            return JsonConvert.SerializeObject(lst);
        }
        [HttpPost]
        public ActionResult DataViewer(int ID)
        {
            using (Models.Planner2Entities db = new Models.Planner2Entities())
            {
                var item = db.DataViewers.Where(v => v.ID == ID).FirstOrDefault();
                DataTable data = new DataTable();

                data = SQLModule.GetTableData(item.QueryData);

                var json = ToJson(data);
                return JsonMax(json);
            }
        }
        public ActionResult Viewer(int ID = 0)
        {

            using (Models.Planner2Entities db = new Models.Planner2Entities())
            {
                var item = db.DataViewers.Where(v => v.ID == ID).FirstOrDefault();
                var nd = (Planner2.Models.User)Session[Planner2.Controllers.LoginAuth.NameSession];

                if (item != null && item.UserView != null && (item.UserView.Contains("*") || item.UserView.Contains(nd.StaffID)))
                {
                    string tableName = item.TableEdit;

                    const string listUrl = "/TableEdittor/List?tablename={0}";
                    const string addUrl = "/TableEdittor/add?tablename={0}";
                    const string updateUrl = "/TableEdittor/update?tablename={0}";
                    const string deleteUrl = "/TableEdittor/delete?tablename={0}";

                    ViewBag.title += item.Name;
                    ViewBag.primaryKeyName = MasterTableDAL.GetPrimaryKey(tableName);
                    ViewBag.FieldData = MasterTableDAL.GetJsonFields(item.QueryData, item.TableEdit);
                    ViewBag.ListUrl = String.Format(listUrl, tableName);
                    ViewBag.AddUrl = String.Format(addUrl, tableName);
                    ViewBag.UpdateUrl = String.Format(updateUrl, tableName);
                    ViewBag.DeleteUrl = String.Format(deleteUrl, tableName);
                    return View();

                }
                return View("KHONG CO QUYEN");

            }
        }


        public ActionResult List(string tableName)
        {

            using (Models.Planner2Entities db = new Models.Planner2Entities())
            {
                var item = db.DataViewers.Where(v => v.TableEdit == tableName).FirstOrDefault();
                var records = MasterTableDAL.GetListOfRecords(item.QueryData);
                var json = ToJson(records);
                return JsonMax(json);
            }
        }

        [HttpPost]
        public ActionResult Add(string tableName)
        {

            Dictionary<string, string> ColumnList = MasterTableDAL.GetColumns(tableName);
            List<ColumnFieldValue> columnFieldValueList = new List<ColumnFieldValue>();

            foreach (var item in ColumnList)
            {
                if (item.Key == ColumnList.FirstOrDefault().Key)
                {
                    continue;
                }
                foreach (string key in Request.Form.AllKeys)
                {
                    var newkey = key.Replace("values", "").Replace("[", "").Replace("]", "").ToLower();
                    if (newkey == item.Key.ToLower())
                    {
                        var columnFieldValue = new ColumnFieldValue();
                        columnFieldValue.ColumnName = item.Key;
                        columnFieldValue.ColumnValue = Request.Form[key];
                        columnFieldValue.ColumnType = item.Value;
                        columnFieldValueList.Add(columnFieldValue);
                    }
                }
            }

            //create column string
            string strColumns = String.Join(",", columnFieldValueList.Select(x => x.ColumnName));
            string strValues = "N'" + String.Join("',N'", columnFieldValueList.Select(V => V.ColumnValue)) + "'";

            string jsonResult = MasterTableDAL.AddRecord(tableName, strColumns, strValues);
            return JsonMax("OK");

        }
        [HttpPost]
        public ActionResult Update(string tableName)
        {

            Dictionary<string, string> ColumnList = MasterTableDAL.GetColumns(tableName);
            List<ColumnFieldValue> columnFieldValueList = new List<ColumnFieldValue>();
            var primaryKeyName = MasterTableDAL.GetPrimaryKey(tableName);

            foreach (var item in ColumnList)
            {
                foreach (string key in Request.Form.AllKeys)
                {
                    var newkey = key.Replace("values", "").Replace("[", "").Replace("]", "").ToLower();
                    if (newkey == item.Key.ToLower() && newkey != primaryKeyName.ToLower())
                    {
                        var columnFieldValue = new ColumnFieldValue();
                        columnFieldValue.ColumnName = item.Key;
                        columnFieldValue.ColumnValue = Request.Form[key];
                        columnFieldValue.ColumnType = item.Value;
                        columnFieldValueList.Add(columnFieldValue);
                    }
                }
            }

            //create column string
            string strColumnUpdate = "";

            foreach (var column in columnFieldValueList)
            {
                switch (column.ColumnType)
                {
                    case "float":
                    case "int":
                    case "bit":
                        strColumnUpdate = strColumnUpdate + column.ColumnName + "=" + column.ColumnValue + ",";
                        break;
                     default :
                        strColumnUpdate = strColumnUpdate + column.ColumnName + "=N'" + column.ColumnValue + "',";
                        break;
                }
            }
            strColumnUpdate = strColumnUpdate.TrimEnd(',');

            //audit fields
            strColumnUpdate = strColumnUpdate + " WHERE " + primaryKeyName + " = " + Request.Form["key"];
            MasterTableDAL.UpdateRecord(tableName, strColumnUpdate);
            return Content("{\"Result\":\"OK\"}", "application/json");
        }

        [HttpPost]
        public ActionResult Delete(string tableName)
        {
            string primaryKey = MasterTableDAL.GetPrimaryKey(tableName);

            MasterTableDAL.DeleteRecord(tableName, primaryKey, Request.Form["key"]);

            return Content("{\"Result\":\"OK\"}", "application/json");

        }
        public class ColumnFieldValue
        {
            public string ColumnName { get; set; }
            public string ColumnType { get; set; }
            public string ColumnValue { get; set; }
        }



    }
}