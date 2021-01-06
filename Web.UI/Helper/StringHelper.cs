using ICSharpCode.SharpZipLib.GZip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Web.UI.Helper
{
    public static class StringHelper
    {
        public static string ClearHtmlTags(this string html)
        {
            string result;
            if (string.IsNullOrWhiteSpace(html))
            {
                result = html;
            }
            else
            {
                html = html.Trim();
                string[] hs = html.Split("<>".ToArray<char>());
                bool skip = false;
                StringBuilder sb = new StringBuilder();
                string[] array = hs;
                for (int i = 0; i < array.Length; i++)
                {
                    string s = array[i];
                    if (!skip)
                    {
                        sb.Append(s);
                    }
                    skip = !skip;
                }
                result = sb.ToString();
            }
            return result;
        }

        public static string CreateCaptchaKey()
        {
            string chars = "123456789qwertyuiopkjhgfdsazxcvbnm";

            Random rand = new Random();
            string captchaKey = "";

            for (int i = 0; i < 5; ++i)
            {
                captchaKey += chars[rand.Next(0, chars.Length)];
            }
            return captchaKey;
        }

        public static string Capital(this string text)
        {
            string[] words = text.Trim().Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = words[i].CapitalWord();
            }
            return string.Join(" ", words);
        }

        static string CapitalWord(this string text)
        {
            var culture = System.Globalization.CultureInfo.GetCultureInfo("tr-TR");
            string allText = text.ToLower(culture);
            int len = text.Length;
            string first = allText.Substring(0, 1).ToUpper(culture);
            string rest = allText.Substring(1, len - 1);
            return string.Concat(first, rest);
        }

        public static string ClearTrChars(this string text)
        {
            text = text.ToLower().Replace("ı", "i").Replace("ş", "s").Replace("ö", "o").Replace("ü", "u").Replace("ç", "c").Replace("ğ", "g").Replace(" ", "-").Replace(".", "-").Replace("&", "-").Replace("/", "-");
            return Regex.Replace(text, @"(-)\1+", "-");
        }

        public static List<int> ToListInt(this string text)
        {
            string[] dizi = text.Split(',');
            List<int> list = new List<int>();
            foreach (var item in dizi)
            {
                list.Add(Convert.ToInt32(item));
            }
            return list;
        }

        public static string ToStringInt(this List<int> list)
        {
            return string.Join(",", list.ToArray());
        }

        public static string CropText(this string text, int max)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            else
            {
                int len = text.Length;
                if (len <= max)
                    return text;
                else
                    return text.Substring(0, max);
            }
        }


        public static string ClearInputV1(this string text)
        {
            return text
                .Replace("/", "")
                .Replace(".", "")
                .Replace("-", "")
                .Replace("(", "")
                .Replace(")", "")
                .Replace(" ", "")
                .Replace("_", "");
        }

        public static string ClearForTab(this string text)
        {
            return text
                .Replace("\"", "-")
                .Replace("&", "-")
                .Replace("?", "- ")
                .Replace(",", "- ")
                .Replace("'", "-");
        }

        public static string ClearForPhone(this string text)
        {
            return text
                .Replace("(", "")
                .Replace(")", "")
                .Replace("-", "")
                .Replace(" ", "")
                .Replace("+", "");
        }

        public static string ClearSpecial(this string text)
        {
            return text
                .Replace("/", "-")
                .Replace(".", "")
                .Replace("#", "")
                .Replace("$", "")
                .Replace("?", "")
                .Replace(".", "")
                .Replace("+", "-")
                .Replace("-", "-")
                .Replace("&", "-");
        }

        public static string ConvertXmlToHtmlTable(string xml)
        {
            StringBuilder html = new StringBuilder("<table class='table'>\r\n");
            try
            {
                XDocument xDocument = XDocument.Parse(xml);
                XElement root = xDocument.Root;

                var xmlAttributeCollection = root.Elements().Attributes();

                foreach (var ele in root.Elements())
                {
                    if (!ele.HasElements)
                    {
                        string elename = "";
                        html.Append("<tr>");

                        elename = ele.Name.ToString();

                        if (ele.HasAttributes)
                        {
                            IEnumerable<XAttribute> attribs = ele.Attributes();
                            foreach (XAttribute attrib in attribs)
                                elename += Environment.NewLine + attrib.Name.ToString() +
                                  "=" + attrib.Value.ToString();
                        }

                        html.Append("<td>" + elename + "</td>");
                        html.Append("<td>" + ele.Value + "</td>");
                        html.Append("</tr>");
                    }
                    else
                    {
                        string elename = "";
                        html.Append("<tr>");

                        elename = ele.Name.ToString();

                        if (ele.HasAttributes)
                        {
                            IEnumerable<XAttribute> attribs = ele.Attributes();
                            foreach (XAttribute attrib in attribs)
                                elename += Environment.NewLine + attrib.Name.ToString() + "=" + attrib.Value.ToString();
                        }

                        html.Append("<td>" + elename + "</td>");
                        html.Append("<td>" + ConvertXmlToHtmlTable(ele.ToString()) + "</td>");
                        html.Append("</tr>");
                    }
                }

                html.Append("</table>");
            }
            catch (Exception)
            {
                return xml;
            }
            return html.ToString();
        }

        public static byte[] zipText(string text)
        {
            if (text == null)
                return null;

            using (Stream memOutput = new MemoryStream())
            {
                using (GZipOutputStream zipOut = new GZipOutputStream(memOutput))
                {
                    using (StreamWriter writer = new StreamWriter(zipOut))
                    {
                        writer.Write(text);

                        writer.Flush();
                        zipOut.Finish();

                        byte[] bytes = new byte[memOutput.Length];
                        memOutput.Seek(0, SeekOrigin.Begin);
                        memOutput.Read(bytes, 0, bytes.Length);

                        return bytes;
                    }
                }
            }
        }

        public static string unzipText(Stream memInput)
        {
            using (GZipInputStream zipInput = new GZipInputStream(memInput))
            using (StreamReader reader = new StreamReader(zipInput))
            {
                string text = reader.ReadToEnd();

                return text;
            }
        }
        public static string unzipText(byte[] bytes)
        {
            if (bytes == null)
                return null;

            using (Stream memInput = new MemoryStream(bytes))
            using (GZipInputStream zipInput = new GZipInputStream(memInput))
            using (StreamReader reader = new StreamReader(zipInput))
            {
                string text = reader.ReadToEnd();

                return text;
            }
        }
    }
}