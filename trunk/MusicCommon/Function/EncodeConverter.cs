using System;
using System.Text;
using System.Web;

namespace CMusicSearch.MusicCommon
{
    /// <summary>
    /// 通用操作功能类
    /// </summary>
    public static class EncodeConverter
    {
        #region URL编码和解码
        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="str">要编码的字符串</param>
        /// <param name="str">编码方式</param>
        /// <returns>经过编码的字符串</returns>
        public static string UrlEncode(string str, Encoding code)
        {
            return HttpUtility.UrlEncode(str, code);
        }

        /// <summary>
        /// URL编码（GB2312）
        /// </summary>
        /// <param name="str">要编码的字符串</param>
        /// <returns>经过编码的字符串</returns>
        public static string UrlEncode(string str)
        {
            return UrlEncode(str, Encoding.GetEncoding("GB2312"));
        }

        /// <summary>
        /// URL解码
        /// </summary>
        /// <param name="str">要解码的字符串</param>
        /// <param name="str">编码方式</param>
        /// <returns>经过解码的字符串</returns>
        public static string UrlDecode(string str, Encoding code)
        {
            return HttpUtility.UrlDecode(str, code);
        }

        /// <summary>
        /// URL解码（GB2312）
        /// </summary>
        /// <param name="str">要解码的字符串</param>
        /// <returns>经过解码的字符串</returns>
        public static string UrlDecode(string str)
        {
            return UrlDecode(str, Encoding.GetEncoding("GB2312"));
        }

        #endregion

        #region HTML特殊字符编码和解码
        /// <summary>
        /// 将HTML编码转换为字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>HTML编码的字符串</returns>
        public static string HtmlDecode(string str)
        {
            return HttpUtility.HtmlDecode(str);
        }

        /// <summary>
        /// 将字符串转换为HTML编码
        /// </summary>
        /// <param name="str">HTML编码的字符串</param>
        /// <returns>字符串</returns>
        public static string HtmlEncode(string str)
        {
            return HttpUtility.HtmlEncode(str);
        }
        #endregion

        #region 转化枚举方法
        /// <summary>
        /// 把字符转换为枚举对象
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="type">要转换的枚举类型</param>
        /// <returns>枚举对象</returns>
        public static object ConvertStringToEnum(string str, Type type)
        {
            if (string.IsNullOrEmpty(str) || type == null)
            {
                return null;
            }
            try
            {
                return Enum.Parse(type, str, true);
            }
            catch (ArgumentNullException)
            {
                return null;
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        /// <summary>
        /// 把数值转换为枚举对象
        /// </summary>
        /// <param name="str">要转换的数值</param>
        /// <param name="type">要转换的枚举类型</param>
        /// <returns>枚举对象</returns>
        public static object ConvertNumberToEnum(int num, Type type)
        {
            return ConvertStringToEnum(num.ToString(), type);
        }
        #endregion

        #region 16进制转换
        public static String ToHexString(string strTarget)
        {
            return ToHexString(strTarget, Encoding.Unicode, true);
        }
        /// <summary>
        /// 生成16进制字符串
        /// </summary>
        /// <param name="strTarget">目标字符串</param>
        /// <param name="encoding">编码</param>
        /// <returns>16进制字符串</returns>
        public static String ToHexString(string strTarget, Encoding encoding, bool toReal)
        {
            if (toReal)
            {
                strTarget = FilterBlank(FilterIllegalChar(strTarget.ToLower(),string.Empty));
            }
            if (strTarget == string.Empty)
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder();
            byte[] bytes = encoding.GetBytes(strTarget);
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("X").PadLeft(2, '0'));
            }
            return sb.ToString();
        }
        #endregion

        #region 过滤属性
        /// <summary>
        /// 去除空格
        /// </summary>
        /// <param name="strTarget"></param>
        /// <returns></returns>
        public static String FilterBlank(string strTarget)
        {
            StringBuilder sbstr = new StringBuilder();
            Char[] carry = strTarget.ToCharArray();
            foreach (char cItem in carry)
            {
                if (cItem != ' ')
                {
                    sbstr.Append(cItem);
                }
            }
            return sbstr.ToString();
        }
        /// <summary>
        /// 过滤非法字符
        /// TODO:非法字符过滤，字符传入考虑中。。。
        /// </summary>
        /// <param name="strTarget"></param>
        /// <returns></returns>
        public static String FilterIllegalChar(string strTarget, string strIllegalChar)
        {
            string strillege = strIllegalChar;
            if (strillege == null || strillege.Equals(string.Empty))
            {
                return strTarget;
            }
            Char[] illegechar = strillege.ToCharArray();
            foreach (Char cItem in illegechar)
            {
                strTarget.Replace(cItem, ' ');
            }
            return strTarget;
        }
        #endregion
    }
}
