using System;
using System.Collections.Generic;
using System.Text;

namespace MusicSearch.MusicCommon
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
        /// 获得Soso网页中所有歌曲信息的DIV块的正则表达式(已过时)
        /// </summary>
        public static readonly string SOSO_DIV_PATTERN = "<div id=\"meta_info_.*?</div>";

        /// <summary>
        /// 获得Soso网页中所有歌曲信息的TR块的正则表达式
        /// </summary>
        public static readonly string SOSO_TR_PATTREN = "<tr\\s+onmouseover=.*?</tr>";

        /// <summary>
        /// 获得Soso网页TR块中歌曲信息的正则表达式
        /// </summary>
        public static readonly string SOSO_MUSIC_INFO_PATTEN = "<td class=\"song\"><a.*?>(?<MusicName>.*?)\\s*</a>.*?</td>.*?<td class=\"singer\"><a.*?>(?<SingerName>.*?)\\s*</a>.*?</td>.*?<td class=\"ablum\">(<a.*?>(?<Album>.*?)\\s*</a>)?.*?</td>";

        /// <summary>
        ///  获得Baidu网页中所有歌曲信息的TR块的正则表达式
        /// </summary>
        public static readonly string BAIDU_TR_PATTERN = @"<tr>\s*<td class=tdn>.*?</tr>";

        /// <summary>
        ///  获得TR块中每个TD块信息(歌曲地址、歌曲名、歌手名)
        /// </summary>
        public static readonly string BAIDU_MUSIC_INFO_PATTERN = "<td class=tdn>.*?</td><td class=d><a href=\"(?<LinkUrl>.*?)\"\\s+target=.*?>(?<MusicName>.*?)\\s*</a></td><td>(<a.*?>(?<Singer1>.*?)</a>.*?)?(<a.*?>(?<Singer2>.*?)</a>.*?)?(<a.*?>(?<Singer3>.*?)</a>.*?)?(<a.*?>.*?</a>.*?).*?</td>";

        /// <summary>
        /// 获得TR块中每个TD块信息(专辑名)
        /// </summary>
        public static readonly string BAIDU_MUSIC_INFO_PATTERN_2 = "<td class=al>(<a.*?>(?<Album>.*?)</a>)?.*?</td>";

        /// <summary>
        /// &lt;font&gt;
        /// </summary>
        public static readonly string FONT_FRONT_PATTERN = "<font.*?>";

        /// <summary>
        /// &lt;/font&gt;
        /// </summary>
        public static readonly string FONT_END_PATTERN = "</font>";

        /// <summary>
        /// &lt;strong&gt;
        /// </summary>
        public static readonly string STRONG_FRONT_PATTERN = "<strong.*?>";

        /// <summary>
        /// &lt;/strong&gt;标签
        /// </summary>
        public static readonly string STRONG_END_PATTERN = "</strong>";

    }
}
