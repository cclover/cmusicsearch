using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

using CMusicSearch.ISearch;
using CMusicSearch.MusicCommon;

namespace CMusicSearch.SosoMusic
{
    public class MainSearch : IMusicSearch
    {
        #region 常量定义
        /// <summary>
        /// 获得Soso网页中所有歌曲信息的DIV块的正则表达式(已过时)
        /// </summary>
        private readonly string SOSO_DIV_PATTERN = "<div id=\"meta_info_.*?</div>";

        /// <summary>
        /// 获得Soso网页中所有歌曲信息的TR块的正则表达式
        /// </summary>
        private readonly string SOSO_TR_PATTREN = "<tr\\s+onmouseover=.*?</tr>";

        /// <summary>
        /// 获得Soso网页TR块中歌曲信息的正则表达式
        /// </summary>
        private readonly string SOSO_MUSIC_INFO_PATTEN = "<td class=\"song\"><a.*?>(?<MusicName>.*?)\\s*</a>.*?</td>.*?<td class=\"singer\"><a.*?>(?<SingerName>.*?)\\s*</a>.*?</td>.*?<td class=\"ablum\">(<a.*?>(?<Album>.*?)\\s*</a>)?.*?</td>";

        #endregion

        #region 构造函数
        public MainSearch()
        {

        }
        #endregion

        #region 接口实现
        /// <summary>
        /// 分析搜搜音乐搜索
        /// </summary>
        public List<MusicInfo> PageAnalysis(string PageContent)
        {
            List<MusicInfo> lstMusic = new List<MusicInfo>();
            try
            {
                // 获取所有歌曲的DIV块信息，此块包含了MusicInfo信息
                List<string> musicDIV = RegexHelper.GetRegexStringList(PageContent, SOSO_TR_PATTREN, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                if (musicDIV != null)
                {
                    foreach (string div in musicDIV)
                    {
                        // 把每个DIV块中取得的歌曲信息存入歌曲列表
                        lstMusic.AddRange(MusicInfoBuild(div));
                    }
                }
            }
            catch
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
                int ntype = 0;//ALL
                switch (info.MusicFormat)
                {
                    case SearchMusicFormat.MP3:
                        ntype = 1;
                        break;
                    case SearchMusicFormat.WMA:
                        ntype = 2;
                        break;
                }
                string sosoUrl = "http://cgi.music.soso.com/fcgi-bin/m.q?w={0}&p=1&t={1}";
                return string.Format(sosoUrl, new object[] { info.MusicName + CommonSymbol.SYMBOL_SPACE + info.SingerName, ntype.ToString() });
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
        /// <param name="tr">歌曲TR块的HTML信息</param>
        /// <returns>TR块中歌曲信息列表</returns>
        private List<MusicInfo> MusicInfoBuild(string tr)
        {
            List<MusicInfo> musicInfoList = new List<MusicInfo>();

            // 获取音乐地址
            List<string> musicAddressList = MusicOperator.GetMusicUrlList(tr);
            if (musicAddressList != null && musicAddressList.Count > 0)
            {
                // 过滤掉重复地址，如果过滤后为空，则返回null
                var urlList = (from list in musicAddressList select list).Distinct();
                if (urlList.Count() == 0)
                {
                    return null;
                }

                // 如果获得地址成功,获得歌曲信息
                GroupCollection group = RegexHelper.GetRegexGroup(tr, SOSO_MUSIC_INFO_PATTEN, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                if (group != null)
                {
                    // 新建歌曲信息列表，设置歌曲信息
                    MusicInfo musicInfo = new MusicInfo();
                    musicInfo.MusicName = EncodeConverter.HtmlDecode(group["MusicName"].Value);
                    musicInfo.SingerName = EncodeConverter.HtmlDecode(group["SingerName"].Value);
                    musicInfo.Album = EncodeConverter.HtmlDecode(group["Album"].Value);
                    musicInfo.MusicSource = "搜搜音乐";

                    // 设置歌曲地址和格式
                    foreach (string address in urlList)
                    {
                        MusicInfo tmpMusic = musicInfo.Clone() as MusicInfo;
                        // 检查地址是否合法
                        if (MusicOperator.urlIsValid(address))
                        {
                            tmpMusic.MusicUrl = address;
                            tmpMusic.MusicFormat = MusicOperator.GetMusicFormat(tmpMusic.MusicUrl);
                            musicInfoList.Add(tmpMusic);
                        }
                    }
                }
                return musicInfoList;
            }
            else
            {
                return musicInfoList;
            }
        }

    }
}
