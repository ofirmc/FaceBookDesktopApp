namespace FacebookFeatures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    /// <summary>
    /// This is the observer part of the observer pattern
    /// </summary>
    public class ObserverTextBoxSubletPrice : TextBox
    {
        public readonly List<SubletHousePriceUpgrader> m_subletHousePriceUpgraders = new List<SubletHousePriceUpgrader>();

        public ObserverTextBoxSubletPrice()
        {
            SubletHousePriceUpgrader subletHousePriceUpgrader = new SubletHousePriceUpgrader();
            subletHousePriceUpgrader.m_ReportCurrentPriceDelegate += new Action<float>(subletHousePriceUpgrader_m_ReportCurrentPriceDelegate);
            m_subletHousePriceUpgraders.Add(subletHousePriceUpgrader);
        }

        private void subletHousePriceUpgrader_m_ReportCurrentPriceDelegate(float i_RentPrice)
        {
            // We need to invoke to prevent cross thread exception
            this.Invoke(new Action(() =>
                {
                    this.Text = i_RentPrice.ToString();
                    this.Update();
                }));
        }
    }
}
