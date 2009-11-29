using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CMusicSearch.MusicService
{
    // NOTE: If you change the interface name "ISearchMusicService" here, you must also update the reference to "ISearchMusicService" in Web.config.
    [ServiceContract]
    public interface ISearchMusicService
    {
        [OperationContract]
        void DoWork();
    }
}
