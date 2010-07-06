using System;

namespace CMusicSearch.MusicCommon
{
    /// <summary>
    /// 音乐信息的实体类
    /// </summary>
    [Serializable]
    public class DownloadMusicTask : ICloneable
    {

        /// <summary>
        /// 下载编号
        /// </summary>
        public int DownloadID
        { get; set; }


        /// <summary>
        /// 下载GUID
        /// </summary>
        public Guid DownloadGUID
        { get; set; }


        /// <summary>
        /// 歌曲下载地址
        /// </summary>
        public string DownloadUrl
        { get; set; }

        /// <summary>
        /// 下载后的文件名
        /// </summary>
        public string MusicFileName
        { get; set; }

        /// <summary>
        /// 下载后的路径
        /// </summary>
        public string MusicSavePath
        { get; set; }


        /// <summary>
        /// 文件的大小
        /// </summary>
        public long FileSize
        { get; set; }

        /// <summary>
        /// 已下载文件的大小
        /// </summary>
        public long DownloadSize
        { get; set; }

        /// <summary>
        /// 下载状态
        /// </summary>
        public DownloadStatus DownloadStatus
        { get; set; }



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
        /// 对象的浅拷贝
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
