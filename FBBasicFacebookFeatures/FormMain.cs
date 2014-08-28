using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FacebookWrapper.ObjectModel;
using FacebookWrapper;
using System.Threading;

namespace FacebookFeatures
{
    public partial class FormMain : Form
    {
        private FacebookManager m_FacebookManager;
        private string m_LoginResultErrorMessage;
        private List<String> m_FullFileNames = new List<String>();       
        private List<string> m_OriginalYoutubeQueryLinks = new List<string>();                        
        private ObserverTextBoxSubletPrice observerTextBoxSubletPrice;
        private float m_PercentageAddition = 0;
        public bool m_IsHoseNumberInt = true;
        private string LocalhouseImage { get; set; }
        
        public FormMain()
        {
            InitializeComponent();                                              
        }
               
        private void PrepareWebBrowserDefaultText()
        {
            WebBrowser.Navigate("about:blank");
            WebBrowser.Document.Write("<html><body><div style=\"font-size:25px;color:dimgray; height: 100px; position: absolute; top:0; bottom: 0; left: 0; right: 0; margin: auto;\">View your Youtube playlist here</div></body></html>");
        }

        private void LoginAndInit()
        {
            m_LoginResultErrorMessage = m_FacebookManager.LoginAndInit();

            if (!string.IsNullOrEmpty(m_LoginResultErrorMessage))
            {
                MessageBox.Show(m_LoginResultErrorMessage);
            }

            fetchUserInfo(m_LoginResultErrorMessage);
        }

        private void fetchUserInfo(string loginResultErrorMessage)
        {
            if (m_FacebookManager.LoggedInUser != null)
            {
                pictureSmallPictureBox.LoadAsync(m_FacebookManager.LoggedInUser.PictureNormalURL);
                if (m_FacebookManager.LoggedInUser.Statuses.Count > 0)
                {
                    textBoxStatus.Text = m_FacebookManager.LoggedInUser.Statuses[0].Message;
                }
            }
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            LoginAndInit();
        }

        private void buttonSetStatus_Click(object sender, EventArgs e)
        {
            if (m_FacebookManager.LoggedInUser != null)
            {
                m_FacebookManager.LoggedInUser.PostStatus(textBoxStatus.Text);
            }
        }

        private void linkNewsFeed_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new Thread(() =>
                {
                    if (m_FacebookManager.LoggedInUser != null)
                    {
                        foreach (string strPost in m_FacebookManager.PostMessages)
                        {
                            listBoxNewsFeed.Invoke(new Action(() =>
                            {
                                listBoxNewsFeed.Items.Add(strPost);
                            }));
                        }
                    }
                    else
                    {
                        PrintLoginError();
                    }
                }).Start();
        }       

        private void linkFriends_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new Thread(() =>
                {
                    listBoxFriends.DisplayMember = "Name";

                    if (m_FacebookManager.LoggedInUser != null)
                    {
                        foreach (object objFacebookUser in m_FacebookManager.UserFriends)
                        {
                            listBoxFriends.Invoke(new Action(() =>
                            {
                                listBoxFriends.Items.Add(objFacebookUser);
                            }));
                        }
                    }
                    else
                    {
                        PrintLoginError();
                    }
                }).Start();
        }

        private void listBoxFriends_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxFriends.SelectedItems.Count == 1)
            {
                User facebookUser = listBoxFriends.SelectedItem as User;

                new Thread(() =>
                    {
                        pictureBoxFriend.Invoke(new Action(() =>
                            {
                                if (facebookUser.PictureNormalURL != null)
                                {
                                    pictureBoxFriend.LoadAsync(facebookUser.PictureNormalURL);
                                }
                                else
                                {
                                    pictureSmallPictureBox.Image = pictureSmallPictureBox.ErrorImage;
                                }
                            }));
                    }).Start();
            }
        }       

        private void labelEvents_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new Thread(() =>
                {
                    listBoxEvents.DisplayMember = "Name";

                    if (m_FacebookManager.LoggedInUser != null)
                    {
                        if (m_FacebookManager.FacebookEvents != null)
                        {
                            foreach (object objFacebookEvent in m_FacebookManager.FacebookEvents)
                            {
                                listBoxEvents.Invoke(new Action(() =>
                                    {
                                        listBoxEvents.Items.Add(objFacebookEvent);
                                    }));
                            }
                        }
                    }
                    else
                    {
                        PrintLoginError();
                    }
                });
        }
        
        private void listBoxEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxEvents.SelectedItems.Count == 1)
            {
                Event FacebookEvent = listBoxEvents.SelectedItem as Event;

                if (FacebookEvent.PictureNormalURL != null)
                {
                    pictureBoxEvent.LoadAsync(FacebookEvent.PictureNormalURL);
                }
                else
                {
                    pictureSmallPictureBox.Image = pictureSmallPictureBox.ErrorImage;
                }
            }      
        }

        private void linkCheckins_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new Thread(() =>
                {
                    if (m_FacebookManager.LoggedInUser != null)
                    {
                        if (m_FacebookManager.FacebookCheckins != null)
                        {
                            foreach (object objCheckin in m_FacebookManager.FacebookCheckins)
                            {
                                listBoxCheckins.Invoke(new Action(() =>
                                   {
                                       listBoxCheckins.Items.Add(objCheckin);
                                   }));
                            }
                        }
                    }
                    else
                    {
                        PrintLoginError();
                    }
                }).Start();
        }                
       
        private void linkLabelSendYouTubePlaylist_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FetchYoutubeVideo();
        }

        //Fix video url so that it will be presented as embeded Youtube media player in WebBrowser control.
        private string FixYoutubeVideoUrl(string i_VideoUrl)
        {
            string intermediaryVideoLink = i_VideoUrl.Replace("watch?v=", "v/");
            return intermediaryVideoLink.Replace("&feature=youtube_gdata_player", "?autohide=1&version=3&autoplay=1");             
        }
        
        private void FetchYoutubeVideo()
        {
            if (this.listBoxLocalMusicPlaylist.SelectedItem != null)
            {
                string selectedListBoxItem = listBoxLocalMusicPlaylist.SelectedItem.ToString();

                new Thread(() =>
                    {
                        try
                        {
                            IYoutubeConnector youtubeConnector = new YoutubeConnectorProxy();

                            string videoUrl = youtubeConnector.FetchYoutubeVideoUrl(selectedListBoxItem);
                            m_OriginalYoutubeQueryLinks.Add(videoUrl);                          

                            string finalFixedVideoUrl = FixYoutubeVideoUrl(videoUrl);
                            if (finalFixedVideoUrl != null)
                            {
                                listBoxYoutubeVideoList.Invoke(new Action(() =>
                                    {
                                        listBoxYoutubeVideoList.Items.Add(finalFixedVideoUrl);
                                    }));
                            }

                        }
                        catch (YoutubeRequestException Ex)
                        {
                            MessageBox.Show(Ex.Message);
                        }
                        catch (Exception Ex)
                        {
                            MessageBox.Show(Ex.Message);
                        }
                    }).Start();
            }
            else
            {
                MessageBox.Show("Select a song name from the local music playlist");
            }
        }

        private void linkLabelViewVideo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (listBoxYoutubeVideoList.SelectedItem != null)
            {
                Uri uri = new Uri(listBoxYoutubeVideoList.SelectedItem.ToString());
                WebBrowser.Url = uri;
            }
            else
            {
                MessageBox.Show("Select a Youtube link from the youtube video playlist list");
            }
        }                
           
        private void linkLabelOpenMusicFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.openFileDialog1.Filter = "Music Files (*.mp3,*.ogg,*.wma,*.aac,*.wav) | *.mp3; *.ogg; *.wma; *.aac; *.wav | All files (*.*)|*.*";
            this.openFileDialog1.FileName = "";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (String fileFullName in openFileDialog1.FileNames)
                {
                    m_FullFileNames.Add(fileFullName);
                    listBoxLocalMusicPlaylist.Items.Add(System.IO.Path.GetFileNameWithoutExtension(fileFullName));
                }
            }
        }

        private void linkLabelRemoveFromListboxSelectMedia_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = listBoxLocalMusicPlaylist.SelectedIndices.Count - 1; i >= 0; i--)
            {
                m_FullFileNames.RemoveAt(listBoxLocalMusicPlaylist.SelectedIndices[i]);
                listBoxLocalMusicPlaylist.Items.RemoveAt(listBoxLocalMusicPlaylist.SelectedIndices[i]);
            }

            SerializeLocalMusicPlaylist();           
        }

        private void linkLabelRemoveYoutubeVideo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = listBoxYoutubeVideoList.SelectedIndices.Count - 1; i >= 0; i--)
            {
                m_OriginalYoutubeQueryLinks.RemoveAt(listBoxYoutubeVideoList.SelectedIndices[i]);
                listBoxYoutubeVideoList.Items.RemoveAt(listBoxYoutubeVideoList.SelectedIndices[i]);
            }

            SerializeYoutubeLinksPlaylist();            
        }

        private void linkLabelPostToFacebook_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            StringBuilder strMessagePostAsHrefLinkToNewsFeed = new StringBuilder();
            string strMessagePostAsEmbeddedLink = string.Empty;
            int numberOfSelectedListBoxItems = 0;

            if (m_FacebookManager.LoggedInUser != null)
            {
                if (listBoxYoutubeVideoList.Items.Count < 1)
                {
                    MessageBox.Show("Your Youtube playlist is empty, there is nothing to post");
                }
                else if (listBoxYoutubeVideoList.SelectedIndex > -1)
                {
                    pictureBoxPlaylistLoader.Image = Properties.Resources.loader;

                    // Prepare the post in the format: the first link will be sent as an embedded link(the Youtube player will appear in the post)
                    // the rest of the links will be shown as Href link to Youtube website.
                    foreach (string videoLink in listBoxYoutubeVideoList.SelectedItems)
                    {
                        numberOfSelectedListBoxItems++;
                        if (listBoxYoutubeVideoList.SelectedItems.Count > 1)
                        {
                            if (listBoxYoutubeVideoList.SelectedItems.IndexOf(videoLink) == 0)
                            {
                                strMessagePostAsEmbeddedLink = m_OriginalYoutubeQueryLinks[listBoxYoutubeVideoList.SelectedItems.IndexOf(videoLink)];
                            }
                            else if (listBoxYoutubeVideoList.SelectedItems.IndexOf(videoLink) > 0)
                            {
                                strMessagePostAsHrefLinkToNewsFeed.AppendFormat("{0} {1}", m_OriginalYoutubeQueryLinks[listBoxYoutubeVideoList.SelectedItems.IndexOf(videoLink)],
                                    Environment.NewLine);
                            }
                        }
                        else
                        {
                            strMessagePostAsEmbeddedLink = m_OriginalYoutubeQueryLinks[listBoxYoutubeVideoList.SelectedItems.IndexOf(videoLink)];
                        }
                    }

                    new Thread(() =>
                        {
                            IFacebookManager facebookManager = SavePlaylists();
                            facebookManager.PostYouTubePlaylistToFacebook(textBoxDescribingPlaylist.Text, numberOfSelectedListBoxItems,
                                strMessagePostAsHrefLinkToNewsFeed, strMessagePostAsEmbeddedLink);

                            pictureBoxPlaylistLoader.Invoke(new Action(() =>
                                {
                                    pictureBoxPlaylistLoader.Image = null;
                                }));
                        }).Start();             
                }
                else
                {
                    MessageBox.Show("Select one or more links from the Youtube video playlist to post to Facebook");
                }
            }
            else
            {
                PrintLoginError();
            }

            listBoxYoutubeVideoList.TopIndex = 0;
        }

        private IFacebookManager SavePlaylists()
        {
            //This are the Decorators Implementation                    
            IFacebookManager facebookManager = new YoutubeLinksPlaylistSaver(
                new LocalMusicPlaylistSaver(m_FacebookManager)
                {
                    SaveLocalMusicPaths = this.checkBoxSaveLocalMusicPlaylist.Checked,
                    MusicFullFileNames = this.m_FullFileNames
                })
                {
                    SaveYoutubeLinks = this.checkBoxSaveYoutubelinksPlaylist.Checked,
                    YoutubeVideoLinks = m_OriginalYoutubeQueryLinks
                };

            return facebookManager;
        }

        private void listBoxLocalMusicPlaylist_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.listBoxLocalMusicPlaylist.SelectedIndex > -1)
            {
                string fullFilePath = m_FullFileNames[this.listBoxLocalMusicPlaylist.SelectedIndex];
                WindowsMediaPlayer.URL = fullFilePath;
            }
        }

        private void listBoxYoutubeVideoList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBoxYoutubeVideoList.SelectedIndex > -1)
            {
                Uri uri = new Uri(listBoxYoutubeVideoList.SelectedItem.ToString());
                WebBrowser.Url = uri;
            }
        }

        private void linkLabelFacebookVideos_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (m_FacebookManager.LoggedInUser != null)
            {
                foreach (Post post in m_FacebookManager.LoggedInUser.NewsFeed)
                {
                    if (post.Link != null)
                    {
                        if (post.Type.ToString() == "video")
                        {
                            if (!post.Source.Contains("youtu"))
                            {
                                if (!listBoxFacebookVideos.Items.Contains(post.Source))
                                {
                                    listBoxFacebookVideos.Items.Add(post.Source);
                                }
                            }
                            else
                            {
                                if (!listBoxYouTubeVideos.Items.Contains(post.Source))
                                {
                                    listBoxYouTubeVideos.Items.Add(post.Source);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                PrintLoginError();
            }
        }        

        private void listBoxYouTubeVideos_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBoxYouTubeVideos.SelectedItem != null)
            {
                Uri uri = new Uri(listBoxYouTubeVideos.SelectedItem.ToString());
                YoutubeWebBrowser.Url = uri;
            }
        }

        private void listBoxFacebookVideos_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.listBoxFacebookVideos.SelectedIndex > -1)
            {                
                facebookMediaPlayer.URL = listBoxFacebookVideos.SelectedItem.ToString();
            }
        }

        private void PrintLoginError()
        {
            MessageBox.Show("You are not loged in to Facebook");
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            FetchYoutubeVideo();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            m_FacebookManager = new FacebookManager();
            PrepareWebBrowserDefaultText(); 
            PopulateSubletHouseTypeComboBox();
            PopulateBirthDayComboBoxes();
            LoadPlaylistsFromFiles();

            
        }

        /// <summary>
        /// This method uses both concrete strategies to show the ease of use of a strategy pattern
        /// </summary>
        private void LoadPlaylistsFromFiles()
        {            
            ObjectSerializer objectSerializer = new ObjectSerializer(new JsonStrategySerializer());
            objectSerializer.DeserializeToList(m_OriginalYoutubeQueryLinks, "MyYoutubeLinksPlaylist");

            objectSerializer.FileTypeSerializer = new XmlStrategySerializer();
            objectSerializer.DeserializeToList(m_FullFileNames, "MyLocalPlaylist");

            foreach (string fileFullName in m_FullFileNames)
            {                
                listBoxLocalMusicPlaylist.Items.Add(System.IO.Path.GetFileNameWithoutExtension(fileFullName));
            }

            foreach (string youtubeVideoLink in m_OriginalYoutubeQueryLinks)
            {
                string fixedYoutubeVideoUrl = FixYoutubeVideoUrl(youtubeVideoLink);
                listBoxYoutubeVideoList.Items.Add(fixedYoutubeVideoUrl);
            }
        }

        private void PopulateBirthDayComboBoxes()
        {
            DateTime dateTime = DateTime.Now;

            for (int i = 18; i <= 120; i++)
            {
                comboBoxMinimumAge.Items.Add(i);                
            }            

            comboBoxMinimumAge.SelectedIndex = 0;                        
        }

        private void PopulateSubletHouseTypeComboBox()
        {
            ComboBoxSubletHouseType.Items.AddRange(Enum.GetNames(typeof(eSubletHouseType)));
            ComboBoxSubletHouseType.SelectedIndex = 0;
        }

        private object ParseEnums(Type i_TargetEnum)
        {
            object objEnums = null;
            if (i_TargetEnum.BaseType == typeof(Enum))
            {
                objEnums = Enum.Parse(i_TargetEnum, ComboBoxSubletHouseType.Text);
            }

            return objEnums;
        }

        private void linkLabelPostSubletOnWall_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (m_FacebookManager.LoggedInUser != null)
            {                
                int houseNumber = 0;
                Int32.TryParse(TextBoxSubletHouseNumber.Text, out houseNumber);

                eSubletHouseType subletHouseType = (eSubletHouseType)ParseEnums(typeof(eSubletHouseType));

                //Creating the Director of the builder pattern
                SubletHouseDirector subletHouseDirector = new SubletHouseDirector();
                ISubletHousePostBuilder subletHousePostBuilder = null;
                if (m_IsHoseNumberInt)
                {
                    pictureBoxSubletHouseLoader.Image = Properties.Resources.loader;

                    switch (subletHouseType)
                    {
                        case eSubletHouseType.Apartment:
                            subletHousePostBuilder = new SubletApartmentHouseBuilder();
                            break;
                        case eSubletHouseType.Loft:
                            subletHousePostBuilder = new SubletLoftHouseBuilder();
                            break;
                    }
                    
                    subletHouseDirector.GetSubletHouseDetails(TextBoxSubletPostHeader.Text,
                        TextBoxSubletCountryName.Text, TextBoxSubletCityName.Text, TextBoxSubletStreetName.Text,
                        houseNumber, TextBoxSubletHouseDescription.Text, DateTimePickerHouseRentStartDate.Value.Date,
                        DateDateTimePickerHouseRentEnd.Value.Date, LocalhouseImage, TextBoxSubletHousePrice.Text);

                    subletHouseDirector.Construct(subletHousePostBuilder);
                    
                    if (subletHousePostBuilder.SubletHousePost.SubletHouseImagePath != null)
                    {                        
                        new Thread(() =>
                        {
                            m_FacebookManager.LoggedInUser.PostPhoto(subletHousePostBuilder.SubletHousePost.SubletHouseImagePath,
                                subletHousePostBuilder.SubletHousePost.ToString());

                            pictureBoxSubletHouseLoader.Invoke(new Action(() =>
                                {
                                    pictureBoxSubletHouseLoader.Image = null;
                                }));
                        }).Start();
                    }
                    else
                    {                        
                        new Thread(() =>
                        {
                            m_FacebookManager.LoggedInUser.PostStatus(subletHousePostBuilder.SubletHousePost.ToString());
                                
                            pictureBoxSubletHouseLoader.Invoke(new Action(() =>
                            {
                                pictureBoxSubletHouseLoader.Image = null;
                            }));
                        }).Start();
                    }
                }
                else
                {
                    PrintHouseNumberError();
                }
            }
            else
            {
                PrintLoginError();
            }
        }         

        private void linkLabelLoadHouseImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.openFileDialog1.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png; | All Files (*.*) | *.*";
            this.openFileDialog1.FileName = "";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                PictureBoxHouseImage.ImageLocation = openFileDialog1.FileName;
                LocalhouseImage = openFileDialog1.FileName;
            }
        }

        private void TextBoxSubletHouseNumber_Leave(object sender, EventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(TextBoxSubletHouseNumber.Text, "^[0-9]*$"))
            {
                m_IsHoseNumberInt = false;
                PrintHouseNumberError();                
            }
            else
            {
                m_IsHoseNumberInt = true;
            }
        }

        private void PrintHouseNumberError()
        {
            MessageBox.Show("House Number feild must contain numeric values only");
        }

        private void linkLabelSaveSelectedPlaylist_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (this.checkBoxSaveLocalMusicPlaylist.Checked)
            {
                SerializeLocalMusicPlaylist();                
            }

            if (this.checkBoxSaveYoutubelinksPlaylist.Checked)
            {
                SerializeYoutubeLinksPlaylist();                
            }
        }

        private void SerializeLocalMusicPlaylist()
        {
            ObjectSerializer objectSerializer = new ObjectSerializer(new XmlStrategySerializer());
            objectSerializer.SerializeFromList(m_FullFileNames, "MyLocalPlaylist");
        }

        private void SerializeYoutubeLinksPlaylist()
        {
            ObjectSerializer objectSerializer = new ObjectSerializer(new JsonStrategySerializer());
            objectSerializer.SerializeFromList(m_OriginalYoutubeQueryLinks, "MyYoutubeLinksPlaylist");
        }       

        private void linkLabelCalcCountDown_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            observerTextBoxSubletPrice = new ObserverTextBoxSubletPrice();
            foreach (SubletHousePriceUpgrader subletHousePriceUpgrader in observerTextBoxSubletPrice.m_subletHousePriceUpgraders)
            {
                subletHousePriceUpgrader.SetCountDownTimeAndPrice(DateTimePickerHouseRentStartDate.Value.Date,
                DateDateTimePickerHouseRentEnd.Value.Date, TextBoxSubletHousePrice.Text, m_PercentageAddition);
            }

            observerTextBoxSubletPrice.Visible = false;
            tabPageFeature3.Controls.Add(observerTextBoxSubletPrice);

            lblCurrentPrice.Text = "Waiting for price update";

            observerTextBoxSubletPrice.TextChanged += (o, i) =>
                {
                    TextBoxSubletHousePrice.Text = observerTextBoxSubletPrice.Text;
                    lblCurrentPrice.Text = observerTextBoxSubletPrice.Text;
                };

            lblSubletPriceMessage.Text = string.Empty;
            CalculteCountDownTime();           
        }

        private void CalculteCountDownTime()
        {            
            TimeSpan t = DateDateTimePickerHouseRentEnd.Value.Date - DateTimePickerHouseRentStartDate.Value.Date;
            int countDownTime = (int)t.TotalHours;
            string countDown = countDownTime.ToString();            

            timer1.Interval = 1000;
            timer1.Tick += (o, e) =>
                {
                    lblCurrentTime.Text = countDownTime.ToString();
                    countDownTime--;

                    if (countDownTime == 0)
                    {
                        timer1.Stop();
                        lblCurrentTime.Text = countDownTime.ToString();
                        string subletPriceMessage = string.Format("{0}{1}{2}{3}{4}","Reminder: this is the last day to post your sublet.",
                            Environment.NewLine, "your sublet price has been automatically updated", Environment.NewLine, 
                            "for you in the Sublet House Price textbox");

                        lblSubletPriceMessage.Text = subletPriceMessage;
                    }
                };
            timer1.Start();                        
        }

        private void maskedTextBoxPercentageAddition_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                m_PercentageAddition = float.Parse(maskedTextBoxPercentageAddition.Text);

                if (m_PercentageAddition != 0)
                {
                    m_PercentageAddition /= 100;
                }
            }
            catch
            {
                e.Cancel = true;
                MessageBox.Show("Incorrect input. setting to default(90%)");
                maskedTextBoxPercentageAddition.Text = "90"; 
            }
        }       
    }
}
