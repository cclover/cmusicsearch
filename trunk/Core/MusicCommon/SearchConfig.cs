using System;
using System.Collections.Generic;
using System.Text;

namespace MusicSearch.MusicCommon
{
    public class SearchConfig
    {
        /// <summary>
        /// 默认超时时间10秒
        /// </summary>
        public static readonly int TIME_OUT = 10000;

        /// <summary>
        /// 默认重试次数2次
        /// </summary>
        public static readonly int TRY_TIME = 2;
    }
}
