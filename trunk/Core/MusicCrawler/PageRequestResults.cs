using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicSearch.MusicCrawler
{
    /// <summary>
    /// 页面请求结果
    /// </summary>
    [Flags]
    [Serializable]
    internal enum PageRequestResults
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 0x0000,

        /// <summary>
        /// 超时
        /// </summary>
        TimeOut = 0x0001,

        /// <summary>
        /// 请求发送失败
        /// </summary>
        SendFailure = 0x0010,

        /// <summary>
        /// 连接失败
        /// </summary>
        ConnectFailure = 0x0011,

        /// <summary>
        /// 接受失败
        /// </summary>
        ReceiveFailure = 0x0100,

        /// <summary>
        /// 请求URL为空
        /// </summary>
        UrlIsNull = 0x0101,

        /// <summary>
        /// 未知错误
        /// </summary>
        UnknowException = 0x1111,
    }
}
