using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMusicSearch.MusicCommon
{
    public class MusicClientConfig
    {
        private MusicClientConfig _musicClientConfig = null;
        /// <summary>
        /// 存储音乐查找的插件
        /// </summary>
        public List<KeyValuePair<string, IMusicSearch>> musicSearcher = new List<KeyValuePair<string, IMusicSearch>>();

        /// <summary>
        /// 存储歌词查找的插件
        /// </summary>
        public List<ILRCSearch> lrcSearcher = new List<ILRCSearch>();

        protected MusicClientConfig(){}
        /// <summary>
        /// 实例化MusicClientConfig
        /// </summary>
        /// <returns></returns>
        public MusicClientConfig GetInstance()
        {
            if (_musicClientConfig == null)
            {
                _musicClientConfig = new MusicClientConfig ();
            }
            return _musicClientConfig;
        }

    }
}
