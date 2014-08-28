using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Google.GData.Client;
using Google.GData.Extensions;
using Google.GData.YouTube;
using Google.GData.Extensions.MediaRss;
using Google.YouTube;

namespace FacebookFeatures
{
    /// <summary>
    /// This class is a Youtube connector that returns a query result, It is a Facade design pattern
    /// and also a RealSubject class for a Proxy.
    /// </summary>
    internal class YoutubeConnector : IYoutubeConnector
    {                
        private YouTubeRequest YouTubeRequest { get; set; }

        private YouTubeQuery YoutubeQuery { get; set; }

        private Feed<Video> Feed { get; set; }

        private string VideoUrl { get; set; }

        private void InitializeYoutubeConnection()
        {
            YouTubeRequestSettings settings = new YouTubeRequestSettings("facebook App", string.Empty);
            YouTubeRequest = new YouTubeRequest(settings);
            YoutubeQuery = new YouTubeQuery(YouTubeQuery.DefaultVideoUri);
        }

        public string FetchYoutubeVideoUrl(string i_StrSearch)
        {
            InitializeYoutubeConnection();

            // order results by the relevance of views
            this.YoutubeQuery.OrderBy = "relevance";

            // search for video and include restricted content in the search results
            this.YoutubeQuery.Query = i_StrSearch;
            this.YoutubeQuery.SafeSearch = YouTubeQuery.SafeSearchValues.None;

            this.Feed = YouTubeRequest.Get<Video>(YoutubeQuery);
            this.Feed.Maximum = 1; // Get only one video result from Youtube

            foreach (Video entry in Feed.Entries)
            {
                VideoUrl = entry.WatchPage.ToString();
            }

            return VideoUrl;
        }                       
    }
}
