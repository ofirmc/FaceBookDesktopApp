namespace FacebookFeatures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Timers;

    /// <summary>
    /// This is the subject part of the observer pattern
    /// </summary>
    public class SubletHousePriceUpgrader
    {        
        private DateTime m_StartTimeOfRent;
        private DateTime m_EndTimeOfRent;          
        private string m_HouseRentPrice;
        private float m_PercentageAddition;
        private Timer m_Timer = new Timer(1000); // Need to change timer interval to 3600000 inorder to really call the Elpased event every 1 hour
        private int m_CountDownTime = 0;               

        public event Action<float> m_ReportCurrentPriceDelegate;            

        public void SetCountDownTimeAndPrice(DateTime i_StartTimeOfRent, DateTime i_EndTimeOfRent, string i_HouseRentPrice, float i_PercentageAddition)
        {
            m_StartTimeOfRent = i_StartTimeOfRent;
            m_EndTimeOfRent = i_EndTimeOfRent;
            m_HouseRentPrice = i_HouseRentPrice;
            m_PercentageAddition = i_PercentageAddition;

            string countDownTimeStr = GetCountDownTime();
            Int32.TryParse(countDownTimeStr, out m_CountDownTime);

            m_Timer.Elapsed += new ElapsedEventHandler(m_Timer_Elapsed);
            m_Timer.Start();            
        }

        private void m_Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            m_CountDownTime--;

            if (m_CountDownTime == 0)
            {
                m_Timer.Stop();                
                float houseRentPrice = 0;
                float.TryParse(m_HouseRentPrice, out houseRentPrice);

                houseRentPrice = (m_PercentageAddition * houseRentPrice) + houseRentPrice;
                if (m_ReportCurrentPriceDelegate != null)
                {
                    m_ReportCurrentPriceDelegate.Invoke(houseRentPrice);
                }
            }
        }

        private string GetCountDownTime()
        {                        
            TimeSpan t = m_EndTimeOfRent - m_StartTimeOfRent;            
            string countDown = t.TotalHours.ToString();
            return countDown;
        }
    }
}
