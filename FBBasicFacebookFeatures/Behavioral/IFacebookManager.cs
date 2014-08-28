namespace FacebookFeatures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// This is a the Component of the decorator design pattern
    /// </summary>
    public interface IFacebookManager
    {
        void PostYouTubePlaylistToFacebook(string i_PlaylistDecription, int i_NumberOfSelectedListBoxItems, StringBuilder i_StrMessagePostAsHrefLinkToNewsFeed, string i_StrMessagePostAsEmbeddedLink);
    }
}
