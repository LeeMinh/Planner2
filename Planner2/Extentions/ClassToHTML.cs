using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planner2.Extentions
{
    public class ObjChange
    {

        public string Name { get; set; }
        public string Value { get; set; }

    }

    public static class ClassToHTML
    {

        public static List<ObjChange> FClassToHTML<T>(this T Before) where T : class, new()
        {

            T obj = new T();
            List<ObjChange> Change = new List<ObjChange>();
            foreach (var prop in obj.GetType().GetProperties())
            {
                try
                {
                    var info = obj.GetType().GetProperty(prop.Name);
                    var ValA = info.GetValue(Before, null).ToString();

                    Change.Add(new ObjChange
                    {
                        Name = prop.Name,
                        Value = ValA
                    });


                }
                catch
                {
                    continue;
                }
            }
            return Change;

        }


        public static string ClassToTableHTML<T>(T Before) where T : class, new()
        {
            var data = FClassToHTML<T>(Before);
            if (data.Count == 0)
            {
                return null;
            }
            string html = "<br>";
            html += "<table id='CV' border='1' style='width:100%'>";
            html += "<tr><th>Name</th><th>Value</th></tr>";
            foreach (var item in data)
            {
                html += "<tr><td>" + item.Name + "</td><td>" + item.Value + "</td></tr>";

            }
            html += "</table>";

            return html;
        }
    }
}