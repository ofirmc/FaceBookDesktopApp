using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace FacebookFeatures
{
    // This is the proxy class in the Proxy design pattern
    class YoutubeConnectorProxy : IYoutubeConnector
    {       
        private YoutubeConnector m_YoutubeConnector = null;        

        public string FetchYoutubeVideoUrl(string i_StrSearch)
        {
            if (m_YoutubeConnector == null)
            {
                m_YoutubeConnector = new YoutubeConnector();
            }

            try
            {
                return m_YoutubeConnector.FetchYoutubeVideoUrl(i_StrSearch);
            }
            catch (Google.GData.Client.GDataRequestException)
            {
                throw new YoutubeRequestException("Could not retrive Youtube link");
            }            
        }
    }
}
