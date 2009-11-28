using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Configuration;

using MusicSearch.MusicCommon;
using MusicSearch.MusicCrawler;


namespace MusicSearch.MusicRunner
{
    /// <summary>
    /// 音乐搜索外部调用
    /// TODO:由于程序启动进行一次反射就可以，在这里每次创建Runner时都会去获取一下DLL
    /// </summary>
    public class MSLRCRunner:IDisposable
    {
        /// <summary>
        /// 音乐查找
        /// </summary>
        private List<KeyValuePair<string, IMusicSearch>> musicSearcher = new List<KeyValuePair<string, IMusicSearch>>();
        /// <summary>
        /// 歌词查找
        /// </summary>
        private List<ILRCSearch> lrcSearcher = new List<ILRCSearch>();

        #region 作成请求字符串
        ///// <summary>
        ///// 生成千千静听歌词信息的请求地址
        ///// </summary>
        ///// <param name="info">搜索信息</param>
        ///// <returns>请求地址</returns>
        //private string CreateTTLrcListUrl(SearchMusicInfo info)
        //{
        //    try
        //    {
        //        string ttLrcListUrl = ConfigurationManager.AppSettings["TTLrcListUrl"];
        //        return string.Format(ttLrcListUrl, info.SingerName, info.MusicName);
        //    }
        //    catch
        //    {
        //        return string.Empty;
        //    }
        //}

        ///// <summary>
        ///// 生成千千静听歌词的请求地址
        ///// </summary>
        ///// <returns>请求地址</returns>
        //private string CreateTTLrcUrl(SearchMusicInfo info)
        //{
        //    try
        //    {
        //        string ttLrcUrl = ConfigurationManager.AppSettings["TTLrcUrl"];
        //        return string.Format(ttLrcUrl, info.MusicLrcID);
        //    }
        //    catch
        //    {
        //        return string.Empty;
        //    }
        //}
        #endregion

        #region 方法封装
        public void Initialize()
        {
            musicSearcher.Clear();
            lrcSearcher.Clear();

            string[] filepaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "/Plugin/");
            foreach (string ItemPath in filepaths)
            {
                FileInfo fi = new FileInfo(ItemPath);
                if (fi.Extension.ToLower().Equals(".dll"))
                {
                    Assembly assembly = Assembly.LoadFile(ItemPath);
                    object objSearch = assembly.CreateInstance(string.Format("MusicSearch.{0}.MainSearch", fi.Name.Replace(fi.Extension,string.Empty)), false);
                    if (objSearch == null)
                    {
                        continue;
                    }
                    if (objSearch is IMusicSearch)
                    {
                        musicSearcher.Add(new KeyValuePair<string,IMusicSearch>(Guid.NewGuid().ToString(),(IMusicSearch)objSearch));
                    }
                    else if (objSearch is ILRCSearch)
                    {
                        //lrcSearcher.Add(new KeyValuePair<string,ILRCSearch>(Guid.NewGuid().ToString(),(ILRCSearch)objSearch));
                        lrcSearcher.Add((ILRCSearch)objSearch);
                    }
                }
            }
        }

        public List<MusicInfo> SearchM(SearchMusicInfo info)
        {
            Crawler crawler = new Crawler();
            List<MusicInfo> lstMusic = new List<MusicInfo>();
            foreach (var item in musicSearcher)
            {
                lstMusic.AddRange(crawler.GetMusicList(info, item.Value));
            }
            return lstMusic;
        }

        public List<MusicLrcInfo> SearchL(SearchMusicInfo info)
        {
            Crawler crawler = new Crawler();
            List<MusicLrcInfo> lstMusic = new List<MusicLrcInfo>();
            foreach (var item in lrcSearcher)
            {
                lstMusic.AddRange(crawler.GetMusicLrcList(info,Encoding.UTF8,item));
            }
            return lstMusic;
        }
        public string GetLyricContent(MusicLrcInfo info)
        {
            Crawler crawler = new Crawler();
            //ILRCSearch objsearch = lrcSearcher.Find((x) => x.Key.Trim().Equals(objuid.ToString())).Value;
            if (lrcSearcher.Count < 1)
            {
                return string.Empty;
            }
            ILRCSearch objsearch = lrcSearcher[0];
            return crawler.GetMusicLyric(info,Encoding.UTF8, objsearch);
        }
        #endregion

        #region IDisposable封装

        public void Dispose()
        {
            musicSearcher.Clear();
            lrcSearcher.Clear();
            GC.Collect();
        }

        #endregion
    }
}
