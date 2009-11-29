using System.Collections.Generic;
using CMusicSearch.MusicCommon;

namespace CMusicSearch.ISearch
{
    /// <summary>
    /// 获取歌词信息时要实现的接口
    /// </summary>
    public interface ILRCSearch : IEncoding
    {
        /// <summary>
        /// 分析要采集的HTML页面内容，并返回采集到的歌词信息
        /// </summary>
        /// <param name="PageContent">HTML页面内容</param>
        /// <returns>搜索到的歌词列表</returns>
        List<MusicLrcInfo> PageAnalysis(string PageContent);

        /// <summary>
        /// 创建采集音乐歌词列表时请求的地址
        /// </summary>
        /// <param name="info">音乐搜索信息</param>
        /// <returns>获取所有歌词信息时请求的URL</returns>
        string CreateAllLrcUrl(SearchMusicInfo info);

        /// <summary>
        /// 创建采集指定歌词内容时请求的地址
        /// </summary>
        /// <param name="info">歌词信息</param>
        /// <returns>获取指定歌词内容时请求的URL</returns>
        string CreateLrcUrl(MusicLrcInfo info);
    }
}
