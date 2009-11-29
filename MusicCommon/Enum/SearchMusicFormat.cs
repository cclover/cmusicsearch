using System;

namespace CMusicSearch.MusicCommon
{

    /// <summary>
    /// 可搜索的音乐文件格式
    /// </summary>
    [Serializable]
    public enum SearchMusicFormat
    {
        ALL = -1,
        MP3 = 0,
        RM = 1,
        WMA = 2
    }
}
