using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using CMusicSearch.MusicCommon;

namespace CMusicSearch.MusicService
{
    [ServiceContract]
    public interface ISearchMusicService
    {
        [OperationContract]
        List<MusicInfo> SearchM();
    }
}
