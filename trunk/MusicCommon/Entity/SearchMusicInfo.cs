using System;


namespace CMusicSearch.MusicCommon
{
    /// <summary>
    /// 音乐搜索信息的实体类
    /// </summary>
    [Serializable]
    public class SearchMusicInfo
    {
        /// <summary>
        /// 创建音乐搜索信息
        /// </summary>
        public SearchMusicInfo()
        {
            MusicName = string.Empty;
            SingerName = string.Empty;
            MusicFormat = SearchMusicFormat.ALL;
        }

        /// <summary>
        /// 歌曲名
        /// </summary>
        public string MusicName
        { get; set; }

        /// <summary>
        /// 歌手名
        /// </summary>
        public string SingerName
        { get; set; }

        /// <summary>
        /// 搜索的音乐格式
        /// </summary>
        public SearchMusicFormat MusicFormat
        { get; set; }
    }
}
