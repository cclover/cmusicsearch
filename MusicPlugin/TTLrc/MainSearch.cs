using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MusicSearch.MusicCommon;
using System.Xml;

/*
 *  eg.
 *  //<result>
 *  //<lrc id="124962" artist="Jason Mraz" title="I\'m Yours" /> 
 *  //<lrc id="108753" artist="Jason Mraz" title="17 I'm Yours (Original Demo)" /> 
 *  //<lrc id="240096" artist="Jason Mraz (英汉对照)" title="I'm Yours" /> 
 *  //<lrc id="54134" artist="Jason Mraz" title="I'm Yours" /> 
 *  //<lrc id="3252" artist="Jason Mraz" title="I'm Yours (Corrected Album Version)" /> 
 *  //<lrc id="73843" artist="Jason Mraz (Album)" title="I'm Yours" /> 
 *  //<lrc id="60945" artist="Jason Mraz (MV version)" title="I'm Yours" /> 
 *  //<lrc id="57234" artist="smelly" title="I'm yours" /> 
 *  //<lrc id="27861" artist="Lonny Bereal" title="I'm Yours" /> 
 *  //<lrc id="278148" artist="TWO-MIX" title="I'M YOURS" /> 
 *  //</result>
 */

namespace MusicSearch.TTLrc
{
    public class MainSearch:ILRCSearch
    {

        #region ILRCSearch Members

        public List<MusicLrcInfo> PageAnalysis(string PageContent)
        {
            List<MusicLrcInfo> lstLRC = new List<MusicLrcInfo>();
            try
            {
                lstLRC.AddRange(LRCInfoBuild(PageContent));
            }
            catch
            {

            }
            return lstLRC;
        }

        public string CreateAllLrcUrl(SearchMusicInfo info)
        {
            try
            {
                string TTLrcUrl = "http://ttlrcct.qianqian.com/dll/lyricsvr.dll?sh?Artist={0}&Title={1}";
                return string.Format(TTLrcUrl, new object[] { EncodeConverter.ToHexString(info.SingerName), EncodeConverter.ToHexString(info.MusicName) });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace.ToString());
                return string.Empty;
            }
        }

        public string CreateLrcUrl(MusicLrcInfo info)
        {
            try
            {
                string TTLrcUrl = "http://ttlrcct2.qianqian.com/dll/lyricsvr.dll?dl?Id={0}&Code={1}&uid=03&mac=002421585787&hds=WD-WMAV22344505";
                return string.Format(TTLrcUrl, new object[] { info.ID, TTEncode.CreateQianQianCode(info.Artist, info.Title, int.Parse(info.ID)) });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace.ToString());
                return string.Empty;
            }
        }
        #endregion

         /// <summary>
        /// 分析返回的XML的歌词信息
        /// </summary>
        /// <param name="tr">获取TT静听服务器的返回的XML</param>
        /// <returns>TR块中歌词信息列表</returns>
        private List<MusicLrcInfo> LRCInfoBuild(string tr)
        {
            List<MusicLrcInfo> LRCInfoList = new List<MusicLrcInfo>();
            if (tr.Contains("error"))
            {
                return LRCInfoList;
            }
            try
            {
                XmlDocument x = new XmlDocument();
                x.LoadXml(tr);

                XmlNodeList list = x.SelectNodes("/result/lrc");

                foreach (XmlNode xnl in list)
                {
                    string sLrcId = xnl.Attributes["id"].Value;
                    string artist = xnl.Attributes["artist"].Value;
                    string title = xnl.Attributes["title"].Value;
                    LRCInfoList.Add(new MusicLrcInfo(sLrcId, artist, title));
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace.ToString()); 
            }
            return LRCInfoList;
        }
    }
}
