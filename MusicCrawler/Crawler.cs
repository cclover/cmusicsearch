using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using CMusicSearch.ISearch;
using CMusicSearch.MusicCommon;

namespace CMusicSearch.MusicCrawler
{
    public class Crawler
    {
        #region 方法封装(Music)
        /// <summary>
        /// 获取音乐信息列表
        /// </summary>
        /// <param name="info">歌曲信息的实体类</param>
        /// <param name="objSearch">外部对象</param>
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
                PageRequestResults result = nwr.RequestPage(out _pageHTML, objSearch.CreateMusicUrl(info), objSearch.PageEncode());
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
        /// 获取音乐歌词列表
        /// </summary>
        /// <param name="info">歌词信息的实体类</param>
        /// <param name="objSearch">外部对象</param>
        /// <returns>返回的歌词列表</returns>
        public List<MusicLrcInfo> GetMusicLrcList(SearchMusicInfo info, ILRCSearch objSearch)
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
                PageRequestResults result = nwr.RequestPage(out _pageHTML, objSearch.CreateAllLrcUrl(info), objSearch.PageEncode());
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
        /// 获取音乐歌词内容
        /// </summary>
        /// <param name="info">歌词信息的实体类</param>
        /// <param name="objSearch">外部对象</param>
        /// <returns>歌词字符串</returns>
        public string GetMusicLyric(MusicLrcInfo info, ILRCSearch objSearch)
        {
            string _pageHTML;
            int TryTime = 1;
            int tryCount = 0;   // 已重试次数

            // 重复请求页面，如果成功则跳出，失败则根据重试次数重新请求
            while (true)
            {
                // 请求页面，默认芊芊静听直接返回请求的字符串
                NetworkRequest nwr = new NetworkRequest();
                PageRequestResults result = nwr.RequestPage(out _pageHTML, objSearch.CreateLrcUrl(info),objSearch.PageEncode());
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
        #endregion
    }
}
