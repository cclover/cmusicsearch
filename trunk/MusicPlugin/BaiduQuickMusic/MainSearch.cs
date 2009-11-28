using System.Collections.Generic;
using System.IO;
using System.Xml;
using MusicSearch.ISearch;
using MusicSearch.MusicCommon;
using System.Text;


namespace MusicSearch.BaiduQuickMusic
{
    /// <summary>
    /// 利用百度快速搜索
    /// </summary>
    public class MainSearch : IMusicSearch
    {
        #region 接口实现
         /// <summary>
        /// 分析百度快速搜索
        /// </summary>
        public List<MusicInfo> PageAnalysis(string PageContent)
        {
            List<MusicInfo> musicList = new List<MusicInfo>();
            try
            {
                StringReader pageReader = new StringReader(PageContent);

                // 通过返回的HTML字符构建XML对象
                XmlDocument xmlPage = new XmlDocument();
                xmlPage.Load(pageReader);

                // 每一个url节点是一个歌曲信息，获取所有歌曲信息
                XmlNodeList urlList = xmlPage.SelectNodes("/result/url");

                // 获取每个<URL>标签内的歌曲信息，并添加到列表
                foreach (XmlNode urlNode in urlList)
                {
                    MusicInfo music = MusicInfoBuild(urlNode);
                    if (music != null)
                    {
                        musicList.Add(music);
                    }
                }
            }
            catch
            {

            }
            return musicList;
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
                string baiduQuickUrl = "http://box.zhangmen.baidu.com/x?op=12&count=1&title={0}$${1}$$$$";
                return string.Format(baiduQuickUrl, new object[] { info.MusicName + CommonSymbol.SYMBOL_SPACE + info.SingerName, info.MusicFormat.ToString("D") });
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
        /// 分析每个&lt;URL&gt;标签内的歌曲信息
        /// </summary>
        /// <param name="node">返回的歌曲信息节点</param>
        /// <returns>歌曲信息类对象</returns>
        private MusicInfo MusicInfoBuild(XmlNode node)
        {
            // 从节点中读取歌曲信息
            MusicInfo musicInfo = new MusicInfo();
            string encodeUrl = node.SelectSingleNode("encode").InnerText;
            string decodeName = node.SelectSingleNode("decode").InnerText;
            string musicType = node.SelectSingleNode("type").InnerText.ToUpper();
            string lrcid = node.SelectSingleNode("lrcid").InnerText;
            musicInfo.MusicSource = "百度音乐";

            // 获取真实的歌曲地址，地址或歌名为空时返回null
            if (!string.IsNullOrEmpty(encodeUrl) && !string.IsNullOrEmpty(decodeName))
            {
                musicInfo.MusicUrl = encodeUrl.Replace(encodeUrl.Substring(encodeUrl.LastIndexOf("/") + 1), decodeName);

                // 检查获得的真实地址是否有效，无效则返回null
                //if (MusicOperate.urlIsValid(musicInfo.MusicUrl))
                //{
                // 暂时不检查，因为地址不一定是MP3，rm等结尾，后面可能还有?stdfrom=3等参数
                musicInfo.MusicFileName = MusicOperator.GetMusicFileName(musicInfo.MusicUrl);
                musicInfo.MusicName = EncodeConverter.UrlDecode(musicInfo.MusicFileName);
                musicInfo.MusicFormat = MusicOperator.GetMusicFormat(musicInfo.MusicUrl);
                //}
                //else
                //{
                //    return null;
                //}
            }
            else
            {
                return null;
            }
            try
            {
                // 获取歌词地址
                musicInfo.LyricUrl = "http://box.zhangmen.baidu.com/bdlrc/" + ulong.Parse(lrcid) / 100 + "/" + lrcid + ".lrc";
            }
            catch
            {

            }
            return musicInfo;
        }
    }
}
