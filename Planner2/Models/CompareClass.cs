using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Planner2.Models
{
    public class CompareClass
    {
        public string Name { get; set; }
        public string Before { get; set; }
        public string After { get; set; }
        public static List<CompareClass> ClassWithClass<T>(T Before, T After) where T : class, new()
        {

            T obj = new T();
            List<CompareClass> Change = new List<CompareClass>();
                foreach (var prop in obj.GetType().GetProperties())
            {
                try
                {
                    var info = obj.GetType().GetProperty(prop.Name);
                    var ValA = info.GetValue(Before, null).ToString();
                    var ValB = info.GetValue(After, null).ToString();
                    if (ValA!=ValB)
                    {
                        Change.Add(new CompareClass {
                            Name=prop.Name,
                            Before=ValA,
                            After=ValB
                        });
                    }

                }
                catch
                {
                    continue;
                }
            }
            return Change;

        }

        public static T Clone<T>(T obj) where T : class, new()
        {
            T new_obj = new T();
            foreach (PropertyInfo pi in obj.GetType().GetProperties())
            {
                if (pi.CanRead && pi.CanWrite && pi.PropertyType.IsSerializable)
                {
                    pi.SetValue(new_obj, pi.GetValue(obj, null), null);
                }
            }
            return new_obj;
        }
        public static string ClassWithClassToTableHTML<T>(T Before, T After) where T : class, new()
         {
           var data = Models.CompareClass.ClassWithClass<T>(Before, After);
            if (data.Count==0)
            {
                return null;
            }
            string html = "<br>";
            html += "<table id='CV' border='1' style='width:100%'>";
            html += "<tr><th>Name</th><th>Before</th><th>After</th></tr>";
            foreach (var item in data)
            {
            html += "<tr><td>"+item.Name+ "</td><td>" + item.Before + "</td><td>" + item.After + "</td></tr>";

            }
            html += "</table>";

            return html;
        }
    }
}