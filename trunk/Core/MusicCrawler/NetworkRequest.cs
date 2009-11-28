using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using MusicSearch.MusicCommon;

namespace MusicSearch.MusicCrawler
{
    /// <summary>
    /// 网络页面获取
    /// </summary>
    internal class NetworkRequest
    {
        #region 构造函数
        public NetworkRequest()
        {
        }
        #endregion

        #region 页面请求封装(Internal)
        /// <summary>
        /// 对Web页面进行请求，获取页面流
        /// </summary>
        /// <param name="pageHtml">返回的页面HTML字符串</param>
        /// <param name="encode">请求的页面编码</param>
        /// <returns>请求结果</returns>
        internal PageRequestResults RequestPage(out string pageHtml,string ReqUrl,Encoding encode)
        {
            pageHtml = string.Empty;
            if (!string.IsNullOrEmpty(ReqUrl))
            {
                // 设置请求信息，使用GET方式获得数据
                HttpWebRequest musicPageReq = (HttpWebRequest)WebRequest.Create(ReqUrl);
                musicPageReq.AllowAutoRedirect = false;
                musicPageReq.Method = "GET";
                musicPageReq.Timeout = SearchConfig.TIME_OUT;
                try
                {
                    // 获取页面响应
                    using (HttpWebResponse musicPageRes = (HttpWebResponse)musicPageReq.GetResponse())
                    {
                        // 如果HTTP为200，并且是同一页面,获取相应的页面流
                        if (musicPageRes.StatusCode == HttpStatusCode.OK)
                        {
                            // 获取响应的页面流
                            Stream pageStrem = musicPageRes.GetResponseStream();

                            // 读取页面流，获取页面HTML字符串,去除指定的标签
                            StreamReader reader = new StreamReader(pageStrem, encode);
                            pageHtml = ReplaceHtml(reader.ReadToEnd());
                            return PageRequestResults.Success;
                        }
                        else
                        {
                            return PageRequestResults.UnknowException;
                        }
                    }
                }
                catch (WebException webEx)
                {
                    // 异常时返回异常的原因
                    if (webEx.Status == WebExceptionStatus.Timeout)
                    {
                        return PageRequestResults.TimeOut;
                    }
                    else if (webEx.Status == WebExceptionStatus.SendFailure)
                    {
                        return PageRequestResults.SendFailure;
                    }
                    else if (webEx.Status == WebExceptionStatus.ConnectFailure)
                    {
                        return PageRequestResults.ConnectFailure;
                    }
                    else if (webEx.Status == WebExceptionStatus.ReceiveFailure)
                    {
                        return PageRequestResults.ReceiveFailure;
                    }
                    else
                    {
                        return PageRequestResults.UnknowException;
                    }
                }
                catch
                {
                    return PageRequestResults.UnknowException;
                }
            }
            return PageRequestResults.UrlIsNull;
        }
        #endregion

        /// <summary>
        /// 去除网页HTML中指定的标签
        /// </summary>
        /// <param name="filter">要处理的HTML字符串</param>
        /// <returns>消除后的字符串</returns>
        private string ReplaceHtml(string filter)
        {
            string result = RegexHelper.ReplaceRegexString(filter, RegexExpressionText.FONT_FRONT_PATTERN, string.Empty, RegexOptions.IgnoreCase);
            result = RegexHelper.ReplaceRegexString(result, RegexExpressionText.FONT_END_PATTERN, string.Empty, RegexOptions.IgnoreCase);
            result = RegexHelper.ReplaceRegexString(result, RegexExpressionText.STRONG_FRONT_PATTERN, string.Empty, RegexOptions.IgnoreCase);
            result = RegexHelper.ReplaceRegexString(result, RegexExpressionText.STRONG_END_PATTERN, string.Empty, RegexOptions.IgnoreCase);
            return result;
        }
    }
}
