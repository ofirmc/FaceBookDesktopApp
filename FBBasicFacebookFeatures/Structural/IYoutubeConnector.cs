using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FacebookFeatures
{
    // This is the Subject Inteface of the Proxy design pattern
    interface IYoutubeConnector
    {
        string FetchYoutubeVideoUrl(string i_StrSearch);
    }
}
