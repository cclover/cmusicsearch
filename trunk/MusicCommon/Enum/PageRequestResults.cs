using System;

namespace CMusicSearch.MusicCommon
{
    /// <summary>
    /// 页面请求结果
    /// </summary>
    [Flags]
    [Serializable]
    public enum PageRequestResults
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
        /// 域名解析错误
        /// </summary>
        DNSFailure = 0x0110,

        /// <summary>
        /// 代理失败
        /// </summary>
        ProxyFailure = 0x0111,

        /// <summary>
        /// 未知错误
        /// </summary>
        UnknowException = 0x1111
    }
}
