using System;

namespace MusicSearch.MusicCommon
{
    /// <summary>
    /// 音乐文件常用格式
    /// </summary>
    [Serializable]
    public enum MusicFormats
    {
        None = -1,
        MP3 = 0,
        RM = 1,
        WMA = 2,
        FLASH = 3,
        WAV = 4,
        MID = 5,
        APE = 6,
        FLV = 7,
        OGG = 8,
    }
}
