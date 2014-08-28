using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacebookWrapper.ObjectModel;
using FacebookWrapper;

namespace FacebookFeatures
{
    // This is a Concerete class
    class SubletApartmentHouseBuilder : ISubletHousePostBuilder
    {
        private SubletHousePost m_SubletHousePost;

        public SubletApartmentHouseBuilder()
        {
            m_SubletHousePost = new SubletHousePost(eSubletHouseType.Apartment);
        }       

        public void BuildPostSubletHeader(string i_PostSubletHeader)
        {
            m_SubletHousePost.SubletPostHeader = i_PostSubletHeader;
        }

        public void BuildSubletCountryName(string i_SubletCountryName)
        {
            m_SubletHousePost.SubletCountryName = i_SubletCountryName;
        }

        public void BuildSubletCityName(string i_SubletCityName)
        {
            m_SubletHousePost.SubletCityName = i_SubletCityName;
        }

        public void BuildSubletStreetName(string i_SubletStreetName)
        {
            m_SubletHousePost.SubletStreetName = i_SubletStreetName;
        }

        public void BuildSubletHouseNumber(int i_SubletHouseNumber)
        {
            m_SubletHousePost.SubletHouseNumber = i_SubletHouseNumber;
        }

        public void BuildSubletHouseDescription(string i_SubletHouseDescription)
        {
            m_SubletHousePost.SubletHouseDescription = i_SubletHouseDescription;
        }

        public void BuildSubletHouseRentStartDate(DateTime i_SubletHouseRentStartDate)
        {
            m_SubletHousePost.HouseRentStartDate = i_SubletHouseRentStartDate;
        }

        public void BuildSubletHouseRentEndDate(DateTime i_SubletHouseRentEndDate)
        {
            m_SubletHousePost.HouseRentEndDate = i_SubletHouseRentEndDate;
        }

        public void BuildSubletHouseImagePath(string i_SubletHouseImagePath)
        {
            m_SubletHousePost.SubletHouseImagePath = i_SubletHouseImagePath;
        }       

        public SubletHousePost SubletHousePost
        {
            get { return m_SubletHousePost; }
        }

        public void BuildSubletHousePrice(string i_SubletHousePrice)
        {
            m_SubletHousePost.SubletHousePrice = i_SubletHousePrice;
        }
    }
}
