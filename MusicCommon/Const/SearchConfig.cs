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
                bool.TryParse(CommonOperator.ReadWinFormConfig("UseProxy"), out useproxy);
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
                return CommonOperator.ReadWinFormConfig("ProxyServer");
            }
        }

        /// <summary>
        /// 代理端口
        /// </summary>
        private static string ProxyPort
        {
            get
            {
                return CommonOperator.ReadWinFormConfig("ProxyPort");
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
                return CommonOperator.ReadWinFormConfig("ProxyUsername");
            }
        }

        /// <summary>
        /// 代理用户密码
        /// </summary>
        private static string ProxyPassword
        {
            get
            {
                return CommonOperator.ReadWinFormConfig("ProxyPassword");
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
                int port = -1;
                string host = SearchConfig.ProxyServer;
                int.TryParse( SearchConfig.ProxyPort,out port);

                if(string.IsNullOrEmpty(host) || port==-1)
                {
                    return null;
                }
                try
                {
                    proxy = new WebProxy(host,port);
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
