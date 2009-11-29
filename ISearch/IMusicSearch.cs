using System.Collections.Generic;
using CMusicSearch.MusicCommon;

namespace CMusicSearch.ISearch
{
    /// <summary>
    /// 获取歌曲信息要实现的接口
    /// </summary>
    public interface IMusicSearch : IEncoding
    {
        /// <summary>
        /// 分析要采集的HTML页面内容，并返回采集到的歌曲信息
        /// </summary>
        /// <param name="PageContent">HTML页面内容</param>
        /// <returns>搜索到的歌曲列表</returns>
        List<MusicInfo> PageAnalysis(string PageContent);

        /// <summary>
        /// 创建采集音乐信息时请求的URL
        /// </summary>
        /// <param name="info">音乐搜索信息</param>
        /// <returns>请求的URL</returns>
        string CreateMusicUrl(SearchMusicInfo info);
    }
}
