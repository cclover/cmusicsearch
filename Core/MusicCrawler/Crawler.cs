using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using MusicSearch.MusicCommon;

namespace MusicSearch.MusicCrawler
{
    public class Crawler
    {
        #region 属性定义
        /// <summary>
        /// 请求页面的编码方式
        /// 默认GB2312编码
        /// </summary>
        public Encoding CurrentEncoding {set;get;}
        #endregion

        #region 构造函数
        public Crawler()
        {
            //默认GB2312编码
            CurrentEncoding = Encoding.GetEncoding("gb2312");
        }

        public Crawler(string encode)
        {
            CurrentEncoding = Encoding.GetEncoding(encode);
        }

        public Crawler(Encoding encode)
        {
            CurrentEncoding = encode;
        }
        #endregion

        #region 方法封装(Music)
        /// <summary>
        /// 获取音乐信息列表
        /// </summary>
        /// <param name="reqUrl">请求页面地址</param>
        /// <param name="timeOut">请求超时时间</param>
        /// <param name="tryTime">失败重试次数</param>
        /// <param name="code">页面编码</param>
        /// <returns>音乐列表</returns>
        public List<MusicInfo> GetMusicList(SearchMusicInfo info,IMusicSearch objSearch)
        {
            List<MusicInfo> _musicList = new List<MusicInfo>();

            string _pageHTML;
            int tryCount = 0;   // 已重试次数
            int outTimeTry = 1; // 超时重试

            // 重复请求页面，如果成功则跳出，失败则根据重试次数重新请求
            while (true)
            {
                // 请求页面
                NetworkRequest nwr = new NetworkRequest ();
                PageRequestResults result = nwr.RequestPage(out _pageHTML, objSearch.CreateMusicUrl(info), CurrentEncoding);
                if (result == PageRequestResults.Success)
                {
                    // 如果请求成功，分析页面
                    _musicList = objSearch.PageAnalysis(_pageHTML);
                    break;
                }
                else if (result == PageRequestResults.TimeOut)
                {
                    // 超时时自动重试一次
                    if (outTimeTry > 0)
                    {
                        outTimeTry--;
                        continue;
                    }

                    // 根据重试次数重发
                    if (tryCount < SearchConfig.TIME_OUT)
                    {
                        tryCount++;
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                else if (result == PageRequestResults.ReceiveFailure || result == PageRequestResults.SendFailure)
                {
                    // 如果发送或接受失败 ，根据重试次数重发
                    if (tryCount < SearchConfig.TIME_OUT)
                    {
                        tryCount++;
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                else if (result == PageRequestResults.ConnectFailure)
                {
                    // 如果连接失败，不重发
                    break;
                }
                else
                {
                    // 其他情况也不重发
                    break;
                }
            }
            return _musicList;
        }

        /// <summary>
        /// 获取音乐歌词基本信息
        /// </summary>
        /// <param name="reqUrl">请求音乐歌词的地址</param>
        /// <param name="code">页面编码</param>
        /// <returns>返回的歌词列表</returns>
        public List<MusicLrcInfo> GetMusicLrcList(SearchMusicInfo info, Encoding code, ILRCSearch objSearch)
        {
            List<MusicLrcInfo> _lstMusicLrc = new List<MusicLrcInfo>();

            string _pageHTML;
            int TryTime = 1;   // 对于歌词信息之重试一次
            int tryCount = 0;   // 已重试次数

            // 重复请求页面，如果成功则跳出，失败则根据重试次数重新请求
            while (true)
            {
                // 请求页面
                NetworkRequest nwr = new NetworkRequest();
                PageRequestResults result = nwr.RequestPage(out _pageHTML, objSearch.CreateAllLrcUrl(info), code);
                if (result == PageRequestResults.Success)
                {
                    // 如果请求成功，分析页面
                    _lstMusicLrc = objSearch.PageAnalysis(_pageHTML);
                    break;
                }
                else
                {
                    // 如果发送或接受失败 ，根据重试次数重发
                    if (tryCount < TryTime)
                    {
                        tryCount++;
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return _lstMusicLrc;
        }
        /// <summary>
        /// 获取音乐歌词
        /// </summary>
        /// <param name="info">歌词请求地址</param>
        /// <param name="code">页面编码</param>
        /// <param name="objSearch">外部对象</param>
        /// <returns>歌词字符串</returns>
        public string GetMusicLyric(MusicLrcInfo info, Encoding code, ILRCSearch objSearch)
        {
            string _pageHTML;
            int TryTime = 1;
            int tryCount = 0;   // 已重试次数

            // 重复请求页面，如果成功则跳出，失败则根据重试次数重新请求
            while (true)
            {
                // 请求页面，默认芊芊静听直接返回请求的字符串
                NetworkRequest nwr = new NetworkRequest();
                PageRequestResults result = nwr.RequestPage(out _pageHTML, objSearch.CreateLrcUrl(info), code);
                if (result == PageRequestResults.Success)
                {
                    return _pageHTML;
                }
                else
                {
                    // 如果发送或接受失败 ，根据重试次数重发
                    if (tryCount < TryTime)
                    {
                        tryCount++;
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取音乐歌词(GB2312)
        /// </summary>
        /// <param name="reqUrl">歌词请求地址</param>
        /// <returns>歌词字符串</returns>
        public string GetMusicLyric(MusicLrcInfo info,ILRCSearch objSearch)
        {
            return GetMusicLyric(info, Encoding.Unicode, objSearch);
        }
        #endregion
    }
}
