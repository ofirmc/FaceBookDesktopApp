namespace FacebookFeatures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;    
    using System.Threading;
    using System.Drawing;
    using FacebookWrapper.ObjectModel;
    using FacebookWrapper;
    
    /// <summary>
    /// This is the ConcreteComponent of the decorator design pattern
    /// </summary>
    public class FacebookManager : IFacebookManager
    {       
        private List<string> m_PostMessages;
        private List<User> m_UserFriends;
        private List<Event> m_FacebookEvents;
        private List<Checkin> m_FacebookCheckin;
        private LoginResult result = new LoginResult();        

        public User LoggedInUser { get; set; }

        public List<string> PostMessages 
        {
            get 
            {
                fetchNewsFeed();
                return m_PostMessages; 
            }
        }

        public List<User> UserFriends
        {
            get 
            {
                fetchFriends();
                return m_UserFriends; 
            }
        }

        public List<Event> FacebookEvents 
        {
            get
            {
                fetchEvents();
                return m_FacebookEvents;
            }            
        }        

        public List<Checkin> FacebookCheckins
        {
            get 
            {
                fetchCheckins();
                return m_FacebookCheckin; 
            }            
        }

        public string LoginAndInit()
        {
            FacebookService.s_CollectionLimit = 2000;
            string resultErrorMessage = string.Empty;
            result = FacebookService.Login("Your FaceBook Access Token",
                "user_about_me", "friends_about_me", "publish_stream", "user_events", "read_stream",
                "user_status, friends_birthday");
           
            if (string.IsNullOrEmpty(result.ErrorMessage))
            {
                LoggedInUser = result.LoggedInUser;                
            }
            else
            {
                resultErrorMessage = result.ErrorMessage;
            }

            return resultErrorMessage;
        }

        public void PostYouTubePlaylistToFacebook(string i_PlaylistDecription, int i_NumberOfSelectedListBoxItems, StringBuilder i_StrMessagePostAsHrefLinkToNewsFeed, string i_StrMessagePostAsEmbeddedLink)
        {
            if (i_NumberOfSelectedListBoxItems > 0)
            {
                if (i_PlaylistDecription == string.Empty)
                {
                    if (i_NumberOfSelectedListBoxItems == 1)
                    {
                        LoggedInUser.PostLink(i_StrMessagePostAsEmbeddedLink, string.Empty);
                    }
                    else if (i_NumberOfSelectedListBoxItems > 1)
                    {
                        LoggedInUser.PostLink(i_StrMessagePostAsEmbeddedLink, i_StrMessagePostAsHrefLinkToNewsFeed.ToString());
                    }
                }
                else
                {
                    if (i_NumberOfSelectedListBoxItems == 1)
                    {
                        LoggedInUser.PostLink(i_StrMessagePostAsEmbeddedLink, i_PlaylistDecription);
                    }
                    else if (i_NumberOfSelectedListBoxItems > 1)
                    {
                        StringBuilder playlistDescription = new StringBuilder();
                        playlistDescription.AppendFormat("{0} {1}", i_PlaylistDecription, Environment.NewLine);
                        i_StrMessagePostAsHrefLinkToNewsFeed.Insert(0, playlistDescription);
                        LoggedInUser.PostLink(i_StrMessagePostAsEmbeddedLink, i_StrMessagePostAsHrefLinkToNewsFeed.ToString());
                    }
                }
            }
        }   

        private void fetchNewsFeed()
        {            
            m_PostMessages = new List<string>();

            foreach (Post post in LoggedInUser.NewsFeed)
            {
                if (post.Message != null)
                {
                    m_PostMessages.Add(post.Message);                    
                }
                else if (post.Caption != null)
                {
                    m_PostMessages.Add(post.Caption);                    
                }
                else
                {
                    m_PostMessages.Add(string.Format("[{0}]", post.Type));                    
                }
            }            
        }

        private void fetchFriends()
        {
            m_UserFriends = new List<User>();
            foreach (User friend in LoggedInUser.Friends)
            {                
                m_UserFriends.Add(friend);
            }
        }

        private void fetchEvents()
        {
            m_FacebookEvents = new List<Event>();
            foreach (Event fbEvent in LoggedInUser.Events)
            {
                m_FacebookEvents.Add(fbEvent);                
            }
        }

        private void fetchCheckins()
        {
            foreach (Checkin checkin in LoggedInUser.Checkins)
            {
                m_FacebookCheckin = new List<Checkin>();
                m_FacebookCheckin.Add(checkin);                
            }
        }             
    }
}
