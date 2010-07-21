using System;

namespace CMusicSearch.MusicCommon
{
    /// <summary>
    /// 下载状态
    /// </summary>
    [Serializable]
    public enum DownloadStatus
    {
        /// <summary>
        /// 无状态
        /// </summary>
        ST_NONE,

        /// <summary>
        /// 准备下载状态
        /// </summary>
        ST_READY_DOWNLOAD,

        /// <summary>
        /// 下载状态
        /// </summary>
        ST_IS_DOWNLOAD,

        /// <summary>
        /// 等待下载状态
        /// </summary>
        ST_WAIT_DOWNLOAD,

        /// <summary>
        /// 暂停下载状态
        /// </summary>
        ST_STOP_DOWNLOAD,

        /// <summary>
        /// 取消下载状态
        /// </summary>
        ST_CANCEL_DOWNLOAD,

        /// <summary>
        /// 下载错误状态
        /// </summary>
        ST_ERROR_DOWNLOAD,

        /// <summary>
        /// 下载完成
        /// </summary>
        ST_OVER_DOWNLOAD

    }
}
