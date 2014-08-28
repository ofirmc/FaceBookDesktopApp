namespace FacebookFeatures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// This is a ConcreteDecorator.
    /// We need this ConcreteDecorator to save the Youtube links if the user chooses to save them as a playlist
    /// at run time when he is posting to Facebook. 
    /// The playlist will be saved to an XML file
    /// </summary>
    public class YoutubeLinksPlaylistSaver : FacebookManagerDecorator
    {
        public YoutubeLinksPlaylistSaver(IFacebookManager i_IFacebookManager) :
            base(i_IFacebookManager) 
        { 
        }

        public bool SaveYoutubeLinks { get; set; }

        public List<string> YoutubeVideoLinks { get; set; }

        public override void PostYouTubePlaylistToFacebook(string i_PlaylistDecription, int i_NumberOfSelectedListBoxItems, StringBuilder i_StrMessagePostAsHrefLinkToNewsFeed, string i_StrMessagePostAsEmbeddedLink)
        {
            base.PostYouTubePlaylistToFacebook(i_PlaylistDecription, i_NumberOfSelectedListBoxItems, i_StrMessagePostAsHrefLinkToNewsFeed, i_StrMessagePostAsEmbeddedLink);

            if (SaveYoutubeLinks)
            {
                ObjectSerializer objectSerializer = new ObjectSerializer(new JsonStrategySerializer());
                objectSerializer.SerializeFromList(YoutubeVideoLinks, "MyYoutubeLinksPlaylist");                               
            }
        }        
    }
}
