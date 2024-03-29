﻿using System;

namespace CMusicSearch.MusicCommon
{
    /// <summary>
    /// 音乐下载任务的实体类
    /// </summary>
    [Serializable]
    public class DownloadMusicTask : ICloneable
    {
        public DownloadMusicTask()
        {
            DownloadTaskID = Guid.Empty;
            MusicName = string.Empty;
            DownloadUrl = string.Empty;
            MusicSavePath = string.Empty;
            MusicConfigPath = string.Empty;
            FileSize = 0;
            DownloadSize = 0;
            DownloadStatus = DownloadStatus.ST_NONE;
            DownloadSpeed = 0;
            IsStop = false;
            IsCancle = false;
            Error = null;
        }

        /// <summary>
        /// 下载任务ID
        /// </summary>
        public Guid DownloadTaskID
        { get; set; }

        /// <summary>
        /// 歌曲名
        /// </summary>
        public string MusicName
        { get; set; }

        /// <summary>
        /// 歌曲下载地址
        /// </summary>
        public string DownloadUrl
        { get; set; }

        /// <summary>
        /// 下载文件保存的路径
        /// </summary>
        public string MusicSavePath
        { get; set; }

        /// <summary>
        /// 下载时配置文件
        /// </summary>
        public String MusicConfigPath
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
        /// 下载速度
        /// </summary>
        public int DownloadSpeed
        { get; set; }

        /// <summary>
        /// 下载是否被停止
        /// </summary>
        public bool IsStop
        { get; set; }

        /// <summary>
        /// 下载是否被取消
        /// </summary>
        public bool IsCancle
        { get; set; }


        /// <summary>
        /// 错误信息
        /// </summary>
        public Exception Error
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
