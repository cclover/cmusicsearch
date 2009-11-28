using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MusicSearch.MusicCommon
{
    //MusicOperator
    /// <summary>
    /// 音乐信息的操作类
    /// </summary>
    public static class MusicOperator
    {
        /// <summary>
        /// 通过音乐地址获得歌曲格式
        /// </summary>
        /// <param name="musicUrl">音乐地址</param>
        /// <returns>音乐格式，格式错误则返回None</returns>
        public static MusicFormats GetMusicFormat(string musicUrl)
        {
            string musicFormat = RegexHelper.GetRegexGroupItem(musicUrl, RegexExpressionText.MUSIC_FORMAT_PATTEN, "MusicFormat", RegexOptions.IgnoreCase);
            // 如果获得歌曲格式为空
            if (string.IsNullOrEmpty(musicFormat))
            {
                return MusicFormats.None;
            }

            // 转换为枚举
            object enumObject = EncodeConverter.ConvertStringToEnum(musicFormat, typeof(MusicFormats));
            return enumObject == null ? MusicFormats.None : (MusicFormats)enumObject;
        }

        /// <summary>
        /// 通过音乐地址获得文件名
        /// </summary>
        /// <param name="musicUrl">音乐地址</param>
        /// <returns>音乐文件名</returns>
        public static string GetMusicFileName(string musicUrl)
        {
            return RegexHelper.GetRegexGroupItem(musicUrl, RegexExpressionText.MUSIC_FILENAME_PATTEN, "FileName", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 获取指定字符串中的音乐地址
        /// </summary>
        /// <param name="urlString"></param>
        /// <returns></returns>
        public static List<string> GetMusicUrlList(string urlString)
        {
            return RegexHelper.GetRegexStringList(urlString, RegexExpressionText.MUSIC_URL_PATTEN, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 检验音乐地址是否有效
        /// </summary>
        /// <param name="musicUrl">音乐地址</param>
        /// <returns>true:有效;false:无效</returns>
        public static bool urlIsValid(string musicUrl)
        {
            if (string.IsNullOrEmpty(musicUrl))
            {
                return false;
            }
            // 只根据地址的结尾是否是MusicFormats中元素来判断
            return RegexHelper.IsFormat(musicUrl, RegexExpressionText.MUSIC_VALID_PATTEN, RegexOptions.IgnoreCase);
        }
    }
}
