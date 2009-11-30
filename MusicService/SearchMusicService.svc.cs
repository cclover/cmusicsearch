using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CMusicSearch.MusicService
{
    // NOTE: If you change the class name "SearchMusicService" here, you must also update the reference to "SearchMusicService" in Web.config.
    public class SearchMusicService : ISearchMusicService
    {
        #region ISearchMusicService Members

        public List<CMusicSearch.MusicCommon.MusicInfo> SearchM()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
