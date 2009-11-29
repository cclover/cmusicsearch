using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace CMusicSearch.MusicCommon
{
    /// <summary>
    /// 正则表达式操作类
    /// </summary>
    public static class RegexHelper
    {
        /// <summary>
        /// 获取与正则表达式匹配的字符串
        /// </summary>
        /// <param name="filter">需要过滤的字符串</param>
        /// <param name="patten">正则表达式</param>
        /// <returns>匹配后的字符串</returns>
        public static string GetRegexString(string filter, string patten, RegexOptions option)
        {
            try
            {
                Regex regex = new Regex(patten, option);
                Match match = regex.Match(filter);
                if (match.Success)
                {
                    return match.Value;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 验证字符串与正则表达式是否匹配
        /// </summary>
        /// <param name="filter">需要过滤的字符串</param>
        /// <param name="patten">正则表达式</param>
        /// <returns>匹配结果</returns>
        public static bool IsFormat(string filter, string patten, RegexOptions option)
        {
            try
            {
                Regex regex = new Regex(patten, option);
                return regex.IsMatch(filter);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取与正则表达匹配的字符串列表
        /// </summary>
        /// <param name="filter">需要过滤的字符串</param>
        /// <param name="patten">正则表达式</param>
        /// <param name="option">正则表达式选项的枚举值</param>
        /// <returns></returns>
        public static List<string> GetRegexStringList(string filter, string patten, RegexOptions option)
        {
            List<string> resultList = new List<string>();
            try
            {
                Regex regex = new Regex(patten, option);
                MatchCollection matchList = regex.Matches(filter);
                if (matchList.Count > 0)
                {
                    for (int i = 0; i < matchList.Count; i++)
                    {
                        resultList.Add(matchList[i].Value);
                    }
                }
                return resultList;
            }
            catch
            {
                return resultList;
            }
        }

        /// <summary>
        /// 获取与正则表达匹配的字符串中的分组集合
        /// </summary>
        /// <param name="filter">需要过滤的字符串</param>
        /// <param name="patten">正则表达式</param>
        /// <returns>匹配字符串的分组信息</returns>
        public static GroupCollection GetRegexGroup(string filter, string patten, RegexOptions option)
        {
            try
            {
                Regex regex = new Regex(patten, option);
                Match match = regex.Match(filter);
                if (match.Success)
                {
                    return match.Groups;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取与正则表达匹配的字符串中的指定组的值
        /// </summary>
        /// <param name="filter">需要过滤的字符串</param>
        /// <param name="groupName">要获得的组的字符串</param>
        /// <param name="patten">正则表达式</param>
        /// <returns>匹配字符串的分组信息</returns>
        public static string GetRegexGroupItem(string filter, string patten, string groupName, RegexOptions option)
        {
            try
            {
                Regex regex = new Regex(patten, option);
                Match match = regex.Match(filter);
                if (match.Success)
                {
                    return match.Groups[groupName].Value;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 替换正则表达式为制定字符
        /// </summary>
        /// <param name="filter">要被替换的字符</param>
        /// <param name="patten">正则表达式</param>
        /// <param name="replace">替换的字符</param>
        /// <returns></returns>
        public static string ReplaceRegexString(string filter, string patten, string replace, RegexOptions option)
        {
            try
            {
                if (string.IsNullOrEmpty(filter) || string.IsNullOrEmpty(patten))
                {
                    return filter;
                }
                return Regex.Replace(filter, patten, replace, option);
            }
            catch
            {
                return filter;
            }
        }
    }
}
