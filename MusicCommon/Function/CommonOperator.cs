using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace CMusicSearch.MusicCommon
{
    /// <summary>
    /// 共同操作类
    /// </summary>
    public class CommonOperator
    {
        /// <summary>
        /// 读取Winform配置文件的AppSettings
        /// </summary>
        /// <param name="key">要读取的键值</param>
        /// <returns>内容</returns>
        public static string ReadWinFormConfig(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key].ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
