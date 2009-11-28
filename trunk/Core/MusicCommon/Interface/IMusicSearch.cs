using System;
using System.Collections.Generic;
using System.Text;

namespace MusicSearch.MusicCommon
{
    public interface IMusicSearch : ISearch
    {
        /// <summary>
        /// 音乐网页采集时
        /// 根据不同的音乐站点的网站结构
        /// 构建不同的采集方式
        /// 主要采集歌曲
        /// </summary>
        /// <returns></returns>
        List<MusicInfo> PageAnalysis(string PageContent);

        /// <summary>
        /// 创建音乐获取URL
        /// </summary>
        /// <param name="info">音乐搜索信息</param>
        /// <returns>URL</returns>
        string CreateMusicUrl(SearchMusicInfo info);
    }
}
