using System;
using System.Collections.Generic;
using System.Text;

namespace MusicSearch.MusicCommon
{
    /// <summary>
    /// 搜索类型
    /// </summary>
    [Flags]
    [Serializable]
    public enum SearchType
    {
        /// <summary>
        /// 按音乐名称搜索
        /// </summary>
        MusicName = 0x0001,
        /// <summary>
        /// 按歌手搜索
        /// </summary>
        Singer = 0x00010
    }
}
