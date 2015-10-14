using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Configuration;
using HGT.Core.Enums;

namespace HGT.Core
{
    public class Permissions
    {
        static bool invalid = false;

        public static List<string> listUrlDenyByUser = new List<string>()
        {
            #region Manager
            //"/admin/user/index|0"+"|"+(int)RoleEnum.Manager,
            //"/admin/user|1"+"|"+(int)RoleEnum.Manager,
            //"/admin/roles/accessright|0"+"|"+(int)RoleEnum.Manager,
            //"/admin/tenant/details|0"+"|"+(int)RoleEnum.Manager,
            //"/admin/tenant/edit|0"+"|"+(int)RoleEnum.Manager,
            //"/admin/tenant|1"+"|"+(int)RoleEnum.Manager,
            //"/admin/tenant/index|0"+"|"+(int)RoleEnum.Manager,
            //"/admin/usergroup/index|0"+"|"+(int)RoleEnum.Manager,
            //"/admin/usergroup|1"+"|"+(int)RoleEnum.Manager,

            "/admin/User/Create|0"+"|"+(int)RoleType.Manager,
            "/admin/User/Edit|1"+"|"+(int)RoleType.Manager,
            "/admin/User/Delete|0"+"|"+(int)RoleType.Manager,

            "/admin/Setting|0"+"|"+(int)RoleType.Manager,
            "/admin/Setting/Edit|0"+"|"+(int)RoleType.Manager,
            #endregion

            #region  Admin
            //"/admin/Setting/details|0"+"|"+(int)RoleEnum.Administrator,
            "/admin/EmailTemplate/edit|0"+"|"+(int)RoleType.Administrator,
            #endregion
            
        };
        public static string ParseNumberToBinary(long number = 0)
        {
            //Variables
            string binaryString = "";
            try
            {
                //Convert number to binary
                binaryString = Convert.ToString(number, 2);
                //and parse that string into char array to reverse string
                char[] charArray = binaryString.ToCharArray();
                Array.Reverse(charArray);
                return new string(charArray);
            }
            catch
            {
                return "0";
            }
        }

        /// <summary>
        /// Get bitmask of user to compare in enum
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static int GetBitMaskOfUser(long number)
        {
            string strBinary = "";
            int bitMask = 0;

            try
            {
                strBinary = ParseNumberToBinary(number);
                for (int i = 4; i < strBinary.Length; i++)
                {
                    if (strBinary[i] == '1')
                        bitMask = Convert.ToInt32(Math.Pow(2, i));
                }

                return bitMask;
            }
            catch
            {
                return 0;
            }

        }

        public static bool IsValidEmail(string strIn)
        {
            invalid = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names. 
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (invalid)
                return false;

            // Return true if strIn is in valid e-mail format. 
            try
            {
                return Regex.IsMatch(strIn,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private static string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }

        public static string GetConfigValue(string key, string defaultValue)
        {
            try
            {
                string value = ConfigurationManager.AppSettings[key].ToString();
                return value;
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}
