using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;

namespace CMusicSearch.MusicCommon
{
    /// <summary>
    /// 音乐处理类，主要用于对集合的处理
    /// </summary>
    public class MusicDistinctHelper
    {
        /// <summary>
        /// 搜索音乐时去除相同LINK
        /// </summary>
        /// <param name="infos">搜索出来的音乐</param>
        public static void Distinct(ref List<MusicInfo> infos)
        {
            List<MusicInfo> distinctresults = new List<MusicInfo>();
            ECMomparer musiccomparer = new ECMomparer();
            musiccomparer.Match += new ECMomparer.MatchHandle(musiccomparer_Match);
            distinctresults.AddRange(infos.Distinct(musiccomparer));
            infos = distinctresults;
        }
        /// <summary>
        /// 外部Match
        /// </summary>
        /// <param name="x">音乐信息X</param>
        /// <param name="y">音乐信息Y</param>
        /// <returns></returns>
        static bool musiccomparer_Match(MusicInfo x, MusicInfo y)
        {
            //按音乐地址过滤信息，相同的音乐地址只出现一次
            return x.MusicUrl.ToLower().Equals(y.MusicUrl.ToLower());
        }

        #region IEqualityComparer
        /// <summary>
        /// 主要用于对List的Distinct操作，自定义规则使用
        /// </summary>
        internal class ECMomparer : IEqualityComparer<MusicInfo>
        {
            public delegate bool MatchHandle(MusicInfo x, MusicInfo y);
            /// <summary>
            /// 音乐匹配方式
            /// 外部调用事件
            /// </summary>
            public event MatchHandle Match; 

            #region IEqualityComparer<MusicInfo> Members

            public bool Equals(MusicInfo x, MusicInfo y)
            {
                bool bequal = false;
                //两者为空的时候，返回false
                if (x == null && y == null)
                {
                    return bequal;
                }
                if (Match == null)
                {
                    //MusicUrl相同时，返回false
                    return x == y;
                }
                else
                {
                    //外部Match匹配时，返回结果
                    bequal = Match(x, y); 
                }

                return bequal;
            }

            public int GetHashCode(MusicInfo obj)
            {
                return obj.ToString().GetHashCode();
            }

            #endregion
        }
        #endregion
    }
}
