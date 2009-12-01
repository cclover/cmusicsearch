using System;
using System.Net;
using System.Configuration;
namespace CMusicSearch.MusicCommon
{
    /// <summary>
    /// 搜索配置信息
    /// </summary>
    public class SearchConfig
    {
        /// <summary>
        /// 默认超时时间10秒
        /// </summary>
        public static readonly int TIME_OUT = 10000;

        /// <summary>
        /// 默认重试次数2次
        /// </summary>
        public static readonly int TRY_TIME = 2;

        #region Proxy代理设置

        /// <summary>
        /// 是否使用代理
        /// 默认是关闭的>false
        /// </summary>
        private static bool UseProxy
        {
            get
            {
                bool useproxy = false;
                try
                {
                    useproxy = bool.Parse(ConfigurationManager.AppSettings["UseProxy"].ToString());
                }
                catch
                {
                    return useproxy;
                }
                return useproxy;
            }
        }

        /// <summary>
        /// 代理服务器地址
        /// </summary>
        private static string ProxyServer
        {
            get
            {
                string proxyserver = string.Empty;
                try
                {
                    proxyserver = ConfigurationManager.AppSettings["ProxyServer"].ToString();
                }
                catch
                {
                    return proxyserver;
                }
                return proxyserver;
            }
        }

        /// <summary>
        /// 代理端口
        /// </summary>
        private static string ProxyPort
        {
            get
            {
                string proxyport = string.Empty;
                try
                {
                    proxyport = ConfigurationManager.AppSettings["ProxyPort"].ToString();
                }
                catch
                {
                    return proxyport;
                }
                return proxyport;
            }
        }

        /// <summary>
        /// 代理验证用户名
        /// </summary>0.
        /// 
        private static string ProxyUsername
        {
            get
            {
                string proxyusername = string.Empty;
                try
                {
                    proxyusername = ConfigurationManager.AppSettings["ProxyUsername"].ToString();
                }
                catch
                {
                    return proxyusername;
                }
                return proxyusername;
            }
        }

        /// <summary>
        /// 代理用户密码
        /// </summary>
        private static string ProxyPassword
        {
            get
            {
                string proxypassword = string.Empty;
                try
                {
                    proxypassword = ConfigurationManager.AppSettings["ProxyPassword"].ToString();
                }
                catch
                {
                    return proxypassword;
                }
                return proxypassword;
            }
        }

        /// <summary>
        /// 获取代理配置
        /// </summary>
        /// <returns>IWebProxy代理信息</returns>
        public static IWebProxy GetConfiguredWebProxy()
        {
            WebProxy proxy = null;
            if (SearchConfig.UseProxy)
            {
                try
                {
                    proxy = new WebProxy(SearchConfig.ProxyServer, int.Parse(SearchConfig.ProxyPort));
                    proxy.Credentials = new NetworkCredential(SearchConfig.ProxyUsername, SearchConfig.ProxyPassword);
                }
                catch (UriFormatException ex)
                {
                    throw ex;
                }
            }
            return proxy;
        }
        #endregion
    }
}
