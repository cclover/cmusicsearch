using System;

namespace CMusicSearch.MusicCommon
{
    /// <summary>
    /// 常用符号
    /// </summary>
    public class CommonSymbol
    {
        /// <summary>
        /// 逗号
        /// </summary>
        public static readonly string SYMBOL_COMMA = ",";

        /// <summary>
        /// 点号
        /// </summary>
        public static readonly string SYMBOL_DOT = ".";

        /// <summary>
        /// 冒号
        /// </summary>
        public static readonly string SYMBOL_COLON = ":";

        /// <summary>
        /// 空格
        /// </summary>
        public static readonly string SYMBOL_SPACE = " ";

        /// <summary>
        /// 回车换行
        /// </summary>
        public static readonly string SYMBOL_NEW_LINE = Environment.NewLine;
    }

    /// <summary>
    /// 正则表达式文本
    /// </summary>
    public class RegexExpressionText
    {
        /// <summary>
        /// 获得音乐地址的正则表达式
        /// </summary>
        public static readonly string MUSIC_URL_PATTEN = @"(ftp|http)://[^, ]+?/[^, ]+?\.(mp3|rm|wma|flash|wav|mid|ape|ogg|flv){1}";

        /// <summary>
        /// 验证音乐地址中后缀名是否正确的正则表达式
        /// </summary>
        public static readonly string MUSIC_VALID_PATTEN = @"\.(mp3|rm|wma|flash|wav|mid|ape|ogg|flv){1}$";

        /// <summary>
        /// 从音乐地址中获得音乐格式的正则表达式
        /// </summary>
        public static readonly string MUSIC_FORMAT_PATTEN = @"\.(?<MusicFormat>mp3|rm|wma|flash|wav|mid|ape|ogg|flv){1}$";

        /// <summary>
        /// 音乐地址中获得音乐文件名的正则表达式
        /// </summary>
        public static readonly string MUSIC_FILENAME_PATTEN = @"//.+/(?<FileName>.*)\.(mp3|rm|wma|flash|wav|mid|ape|ogg|flv){1}$";

        /// <summary>
        /// 要过滤的页面内容
        /// </summary>
        public static readonly string PAGE_FILTER = "<font.*?>|</font>|<strong.*?>|</strong>";

    }
}
