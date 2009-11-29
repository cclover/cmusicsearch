using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CMusicSearch.MusicService
{
    // NOTE: If you change the interface name "ISearchLRCService" here, you must also update the reference to "ISearchLRCService" in Web.config.
    [ServiceContract]
    public interface ISearchLRCService
    {
        [OperationContract]
        void DoWork();
    }
}
