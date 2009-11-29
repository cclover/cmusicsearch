using System;


namespace CMusicSearch.MusicCommon
{
    /// <summary>
    /// 歌词信息的实体类
    /// </summary>
    [Serializable]
    public class MusicLrcInfo
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="id">文件ID</param>
        /// <param name="artist">歌手名</param>
        /// <param name="title">歌曲名</param>
        public MusicLrcInfo(string id, string artist, string title)
        {
            ID = id;
            Title = title;
            Artist = artist;
        }

        /// <summary>
        /// 文件ID
        /// </summary>
        public string ID
        { get; set; }

        /// <summary>
        /// 歌手名
        /// </summary>
        public string Artist
        { get; set; }

        /// <summary>
        /// 歌曲名称
        /// </summary>
        public string Title
        { get; set; }

        /// <summary>
        /// 歌词
        /// </summary>
        public string Lyrics
        { get; set; }
    }
}
