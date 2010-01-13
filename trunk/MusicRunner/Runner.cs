using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Configuration;

using CMusicSearch.MusicCommon;
using CMusicSearch.MusicCrawler;
using CMusicSearch.ISearch;


namespace CMusicSearch.MusicRunner
{
    /// <summary>
    /// 音乐搜索外部调用
    /// TODO:由于程序启动进行一次反射就可以，在这里每次创建Runner时都会去获取一下DLL
    /// </summary>
    public class MSLRCRunner : IDisposable
    {
        MusicClientConfig mcc = MusicClientConfig.GetInstance();
        #region 方法封装
        /// <summary>
        /// Runner初始化
        /// 功能：插件加载
        /// </summary>
        public static bool Initialize()
        {
            MusicClientConfig innermcc = MusicClientConfig.GetInstance();
            //获取“Plugin”目录下的文件
            string[] filepaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "/Plugin/","*.dll");
            foreach (string ItemPath in filepaths)
            {
                FileInfo fi = new FileInfo(ItemPath);
                //找出后缀为DLL的文件，并实例化它
                if (fi.Extension.ToLower().Equals(".dll"))
                {
                    Assembly assembly = Assembly.LoadFile(ItemPath);
                    //动态实例化类库
                    object objSearch = assembly.CreateInstance(string.Format("CMusicSearch.{0}.MainSearch", fi.Name.Replace(fi.Extension, string.Empty)), false);
                    if (objSearch == null)
                    {
                        continue;
                    }
                    //加载音乐搜索插件
                    if (objSearch is IMusicSearch)
                    {
                        innermcc.MusicSearcher.Add(new KeyValuePair<string, IMusicSearch>(Guid.NewGuid().ToString(), (IMusicSearch)objSearch));
                    }
                    //加载歌词搜索插件
                    else if (objSearch is ILRCSearch)
                    {
                        //lrcSearcher.Add(new KeyValuePair<string,ILRCSearch>(Guid.NewGuid().ToString(),(ILRCSearch)objSearch));
                        innermcc.LRCSearcher.Add((ILRCSearch)objSearch);
                    }
                }
            }
            if (innermcc.MusicSearcher.Count < 1)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 搜索歌曲方法
        /// </summary>
        /// <param name="info">搜索信息</param>
        /// <returns>歌曲列表</returns>
        public List<MusicInfo> SearchM(SearchMusicInfo info)
        {
            Crawler crawler = new Crawler();
            List<MusicInfo> lstMusic = new List<MusicInfo>();

            // 遍历插件，搜索音乐
            foreach (var item in mcc.MusicSearcher)
            {
                //根据加载的插件所提供的方法，获取音乐信息
                lstMusic.AddRange(crawler.GetMusicList(info, item.Value));
            }
            MusicDistinctHelper.Distinct(ref lstMusic);
            //TODO:在各自的插件中过滤MusicFormat后，在这里是否最终输出也过滤一次
            return lstMusic;
        }

        /// <summary>
        /// 搜索歌词列表方法
        /// </summary>
        /// <param name="info">搜索信息</param>
        /// <returns>歌词列表</returns>
        public List<MusicLrcInfo> SearchL(SearchMusicInfo info)
        {
            Crawler crawler = new Crawler();
            List<MusicLrcInfo> lstMusic = new List<MusicLrcInfo>();
            foreach (var item in mcc.LRCSearcher)
            {
                //根据加载的插件所提供的方法，获取歌词信息
                lstMusic.AddRange(crawler.GetMusicLrcList(info, item));
            }
            return lstMusic;
        }
        /// <summary>
        /// 获取歌词方法
        /// </summary>
        /// <param name="info">歌词基本信息</param>
        /// <returns>歌词内容</returns>
        public string GetLyricContent(MusicLrcInfo info)
        {
            Crawler crawler = new Crawler();
            //ILRCSearch objsearch = lrcSearcher.Find((x) => x.Key.Trim().Equals(objuid.ToString())).Value;
            if (mcc.LRCSearcher.Count < 1)
            {
                return string.Empty;
            }
            //TODO:
            ILRCSearch objsearch = mcc.LRCSearcher[0];
            return crawler.GetMusicLyric(info, objsearch);
        }
        #endregion

        #region IDisposable封装

        public void Dispose()
        {
            GC.Collect();
        }

        #endregion
    }
}
