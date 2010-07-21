using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

using CMusicSearch.ISearch;
using CMusicSearch.MusicCommon;


namespace CMusicSearch.BaiduMusic
{
    public class MainSearch : IMusicSearch
    {
        #region 常量定义
        /// <summary>
        ///  获得Baidu网页中所有歌曲信息的TR块的正则表达式
        /// </summary>
        private readonly string BAIDU_TR_PATTERN = @"<tr>\s*<td class=tdn>.*?</tr>";
        /// <summary>
        ///  获得TR块中每个TD块信息(歌曲地址、歌曲名、歌手名)
        /// </summary>
        private readonly string BAIDU_MUSIC_INFO_PATTERN = "<td class=tdn>.*?</td><td class=d><a href=\"(?<LinkUrl>.*?)\"\\s+target=.*?>(?<MusicName>.*?)\\s*</a></td><td>(<a.*?>(?<Singer1>.*?)</a>.*?)?(<a.*?>(?<Singer2>.*?)</a>.*?)?(<a.*?>(?<Singer3>.*?)</a>.*?)?(<a.*?>.*?</a>.*?).*?</td>";

        /// <summary>
        /// 获得TR块中每个TD块信息(专辑名)
        /// </summary>
        private readonly string BAIDU_MUSIC_INFO_PATTERN_2 = "<td class=al>(<a.*?>(?<Album>.*?)</a>)?.*?</td>";
        #endregion

        #region 构造函数
        public MainSearch()
        {

        }
        #endregion

        #region 接口实现
        public List<MusicInfo> PageAnalysis(string PageContent)
        {
            List<MusicInfo> lstMusic = new List<MusicInfo>();
            try
            {
                // 获取所有歌曲的TR块信息，此块包含了MusicInfo信息
                List<string> musicTR = RegexHelper.GetRegexStringList(PageContent, BAIDU_TR_PATTERN, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                if (musicTR != null)
                {
                    // 获取每个TR块中的信息
                    foreach (string tr in musicTR)
                    {
                        // 把每个DIV块中取得的歌曲信息存入歌曲列表
                        var list = MusicInfoBuild(tr);
                        if(list != null)
                            lstMusic.AddRange(list);
                    }
                }
            }
            catch(Exception ex)
            {

            }
            return lstMusic;
        }
        /// <summary>
        /// 创建音乐获取URL
        /// </summary>
        /// <param name="info">音乐搜索信息</param>
        /// <returns>URL</returns>
        public string CreateMusicUrl(SearchMusicInfo info)
        {
            try
            {
                string baiduUrl = "http://mp3.baidu.com/m?f=ms&rf=idx&tn=baidump3&ct=134217728&lf=&rn=&word={0}&lm={1}";
                return string.Format(baiduUrl, new object[] { info.MusicName + CommonSymbol.SYMBOL_SPACE + info.SingerName, info.MusicFormat.ToString("D") });
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 指定当前页面编码方式
        /// </summary>
        /// <returns></returns>
        public Encoding PageEncode()
        {
            return Encoding.GetEncoding("GB2312");
        }
        #endregion

        /// <summary>
        /// 分析返回的DIV快内的歌曲信息
        /// </summary>
        /// <param name="tr">歌曲tr块的HTML信息</param>
        /// <returns>TR块中歌曲信息列表</returns>
        private List<MusicInfo> MusicInfoBuild(string tr)
        {
            List<MusicInfo> musicInfoList = new List<MusicInfo>();

            // 从TR块中获取音乐基本信息
            tr = tr.Replace("\n", string.Empty);
            tr = tr.Replace("&nbsp;", string.Empty);
            GroupCollection infoGroup = RegexHelper.GetRegexGroup(tr, BAIDU_MUSIC_INFO_PATTERN, RegexOptions.IgnoreCase);
            if (infoGroup == null)
            {
                return musicInfoList;
            }

            // 设置音乐基本信息
            MusicInfo music = new MusicInfo();
            music.MusicSource = "百度音乐";
            music.MusicName = EncodeConverter.HtmlDecode(infoGroup["MusicName"].Value);

            // 这里单独从TR块中获取专辑信息，是出于性能考虑，如果在infoGroup中搜索专辑名信息，因为正则表达式过长
            // 导致每次搜索要940ms左右，而分离后，两次搜索总时间不到5ms
            music.Album = EncodeConverter.HtmlDecode(RegexHelper.GetRegexGroupItem(tr, BAIDU_MUSIC_INFO_PATTERN_2, "Album", RegexOptions.IgnoreCase));
            music.SingerName = infoGroup["Singer1"].Value + " " + infoGroup["Singer2"].Value + " " + infoGroup["Singer3"].Value;

            // 从歌名的链接地址中获得音乐地址
            string detailLink = infoGroup["LinkUrl"].Value;
            if (!string.IsNullOrEmpty(detailLink))
            {
                List<string> musicUrlList = MusicOperator.GetMusicUrlList(detailLink);  //目前一些文件名后面没有后缀名
                if (musicUrlList == null || musicUrlList.Count == 0)
                {
                    return null;
                }

                // 过滤掉重复地址
                var urlList = musicUrlList;

                // 每个不同的地址，组成一个音乐信息
                foreach (string url in urlList)
                {
                    if (MusicOperator.urlIsValid(url))
                    {
                        MusicInfo tmpMusic = music.Clone() as MusicInfo;
                        tmpMusic.MusicUrl = url;
                        tmpMusic.MusicFormat = MusicOperator.GetMusicFormat(url);
                        musicInfoList.Add(tmpMusic);
                    }
                }
            }
            else
            {
                return musicInfoList;
            }
            return musicInfoList;
        }
    }
}
