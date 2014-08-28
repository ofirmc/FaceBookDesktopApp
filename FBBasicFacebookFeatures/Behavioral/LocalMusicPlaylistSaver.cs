namespace FacebookFeatures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using System.IO;

    /// <summary>
    /// This is a ConcreteDecorator.
    /// We need this ConcreteDecorator to save the paths of the local music files if the user chooses to save them as a playlist
    /// at run time when he is posting to Facebook. 
    /// The playlist will be saved to an XML file
    /// </summary>
    public class LocalMusicPlaylistSaver : FacebookManagerDecorator
    {
        public LocalMusicPlaylistSaver(IFacebookManager i_IFacebookManager) :
            base(i_IFacebookManager) 
        {
        }

        public bool SaveLocalMusicPaths { get; set; }

        public List<string> MusicFullFileNames { get; set; }

        public override void PostYouTubePlaylistToFacebook(string i_PlaylistDecription, int i_NumberOfSelectedListBoxItems, StringBuilder i_StrMessagePostAsHrefLinkToNewsFeed, string i_StrMessagePostAsEmbeddedLink)
        {
            base.PostYouTubePlaylistToFacebook(i_PlaylistDecription, i_NumberOfSelectedListBoxItems, i_StrMessagePostAsHrefLinkToNewsFeed, i_StrMessagePostAsEmbeddedLink);
            
            if (SaveLocalMusicPaths)
            {
                ObjectSerializer objectSerializer = new ObjectSerializer(new XmlStrategySerializer());
                objectSerializer.SerializeFromList(MusicFullFileNames, "MyLocalPlaylist");                            
            }
        }       
    }
}
