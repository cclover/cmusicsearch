using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMusicSearch.ISearch
{
    /// <summary>
    /// 编码接口
    /// </summary>
    public interface IEncoding
    {
        /// <summary>
        /// 指定页面的编码方式
        /// </summary>
        /// <returns>页面编码方式</returns>
        Encoding PageEncode();
    }
}
