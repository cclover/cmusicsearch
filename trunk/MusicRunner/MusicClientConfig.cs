using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMusicSearch.ISearch;

namespace CMusicSearch.MusicCommon
{
    public class MusicClientConfig
    {
        private static MusicClientConfig _musicClientConfig = null;
        /// <summary>
        /// 存储音乐查找的插件
        /// </summary>
        public List<KeyValuePair<string, IMusicSearch>> MusicSearcher;

        /// <summary>
        /// 存储歌词查找的插件
        /// </summary>
        public List<ILRCSearch> LRCSearcher;
        /// <summary>
        /// 构造函数
        /// </summary>
        protected MusicClientConfig()
        {
            MusicSearcher = new List<KeyValuePair<string, IMusicSearch>>();
            LRCSearcher = new List<ILRCSearch>();
        }
        /// <summary>
        /// 实例化MusicClientConfig
        /// </summary>
        /// <returns></returns>
        public static MusicClientConfig GetInstance()
        {
            if (_musicClientConfig == null)
            {
                _musicClientConfig = new MusicClientConfig ();
            }
            return _musicClientConfig;
        }

    }
}
