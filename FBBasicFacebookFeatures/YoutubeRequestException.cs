namespace FacebookFeatures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    
    public class YoutubeRequestException : Exception
    {
        private string m_BadConnectionRequestStr = string.Empty;

        public YoutubeRequestException(string i_RequestError)
        {
            m_BadConnectionRequestStr = i_RequestError;
        }

        public override string Message
        {
            get
            {
                return m_BadConnectionRequestStr;
            }
        }
    }
}
