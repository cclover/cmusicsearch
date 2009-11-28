using System;
using System.Collections.Generic;
using System.Text;

namespace MusicSearch.MusicCommon
{
    public interface ILRCSearch : ISearch
    {
        /// <summary>
        /// 音乐网页采集时
        /// 根据不同的音乐站点的网站结构
        /// 构建不同的采集方式
        /// 主要采集歌词
        /// </summary>
        /// <returns></returns>
        List<MusicLrcInfo> PageAnalysis(string PageContent);

        /// <summary>
        /// 获取所有歌词信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        string CreateAllLrcUrl(SearchMusicInfo info);

        /// <summary>
        /// 获取指定歌词信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        string CreateLrcUrl(MusicLrcInfo info);
    }
}
