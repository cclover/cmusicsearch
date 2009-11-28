using System;


namespace MusicSearch.MusicCommon
{
    /// <summary>
    /// 音乐信息的实体类
    /// </summary>
    [Serializable]
    public class MusicInfo : ICloneable
    {
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
        /// 歌曲地址
        /// </summary>
        public string MusicUrl
        { get; set; }

        /// <summary>
        /// 歌曲专辑
        /// </summary>
        public string Album
        { get; set; }

        /// <summary>
        /// 歌曲格式
        /// </summary>
        public MusicFormats MusicFormat
        { get; set; }

        /// <summary>
        /// 歌词地址
        /// </summary>
        public string LyricUrl
        { get; set; }

        /// <summary>
        /// 歌曲文件名
        /// </summary>
        public string MusicFileName
        { get; set; }

        /// <summary>
        /// 歌曲来源
        /// </summary>
        public string MusicSource
        { get; set; }

        /// <summary>
        /// 歌曲大小
        /// </summary>
        public string MusicSize
        { get; set; }

        /// <summary>
        /// 对象的浅拷贝
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
