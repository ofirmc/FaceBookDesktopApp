namespace FacebookFeatures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    
    public abstract class FacebookManagerDecorator : IFacebookManager
    {
        private IFacebookManager m_FacebookManagerDecorated;

        public FacebookManagerDecorator(IFacebookManager i_IFacebookManagerDecorated)
        {
            m_FacebookManagerDecorated = i_IFacebookManagerDecorated;
        }

        public virtual void PostYouTubePlaylistToFacebook(string i_PlaylistDecription, int i_NumberOfSelectedListBoxItems, 
            StringBuilder i_StrMessagePostAsHrefLinkToNewsFeed, string i_StrMessagePostAsEmbeddedLink)
        {
            m_FacebookManagerDecorated.PostYouTubePlaylistToFacebook(i_PlaylistDecription, i_NumberOfSelectedListBoxItems, i_StrMessagePostAsHrefLinkToNewsFeed, 
                i_StrMessagePostAsEmbeddedLink);
        }
    }
}
