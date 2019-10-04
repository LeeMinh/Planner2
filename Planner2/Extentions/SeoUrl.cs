using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Planner2.Extentions
{
    public   class SeoUrl
    {
        public static string SeoURL(string text)
        {

            //First to lower case
            for (int i = 32; i < 48; i++)
            {

                text = text.Replace(((char)i).ToString(), " ");

            }
            text = text.Replace(".", "-");

            text = text.Replace(" ", "-");

            text = text.Replace(",", "-");

            text = text.Replace(";", "-");

            text = text.Replace(":", "-");

            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");

            string strFormD = text.Normalize(System.Text.NormalizationForm.FormD);

            return regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D').ToLower().Trim();

             //string str = RemoveAccent(phrase).ToLower();
            //// invalid chars           
            //str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            //// convert multiple spaces into one space   
            //str = Regex.Replace(str, @"\s+", " ").Trim();
            //// cut and trim 
            //str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            //str = Regex.Replace(str, @"\s", "-"); // hyphens   
            //return str;
        }
        public static string RemoveAccent(string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
    }
}