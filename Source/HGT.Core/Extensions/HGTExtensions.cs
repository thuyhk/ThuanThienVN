
using HGT.Core.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Text.RegularExpressions;

namespace HGT.Core.Extensions
{
    public static class HGTExtensions
    {

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public static string ToXml<T>(this T obj)
        //{
        //    XmlDocument xmlDoc = new XmlDocument();
        //    XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
        //    using (MemoryStream xmlStream = new MemoryStream())
        //    {
        //        xmlSerializer.Serialize(xmlStream, obj);
        //        xmlStream.Position = 0;
        //        xmlDoc.Load(xmlStream);
        //        return xmlDoc.InnerXml;
        //    }
        //}

        //public static object ToObject<T>(this string xmlData)
        //{
        //    XmlSerializer serializer = new XmlSerializer(typeof(AppSettings));
        //    MemoryStream memStream = new MemoryStream(Encoding.UTF8.GetBytes(xmlData));
        //    return serializer.Deserialize(memStream);

        //}        

        public static string GenerateToAlias(string text)
        {
            text = text.ToLower();
            for (int i = 33; i < 48; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }

            for (int i = 58; i < 65; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }

            for (int i = 91; i < 97; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }

            for (int i = 123; i < 127; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }

            text = text.Replace(" ", "-");

            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string strFormD = text.Normalize(System.Text.NormalizationForm.FormD);
            return regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
    }
}
