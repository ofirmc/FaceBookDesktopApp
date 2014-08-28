using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace FacebookFeatures
{
    class SubletHouseDirector
    {        
        private string SubletPostHeader { get; set; }
        private string SubletCountryName { get; set; }
        private string SubletCityName { get; set; }
        private string SubletStreetName { get; set; }
        private int SubletHouseNumber { get; set; }
        private string SubletHouseDescription { get; set; }
        private DateTime HouseRentStartDate { get; set; }
        private DateTime HouseRentEndDate { get; set; }
        private string SubletHouseImagePath { get; set; }
        private string SubletHousePrice { get; set; }

        public void GetSubletHouseDetails(string i_SubletPostHeader, string i_SubletCountryName,
            string i_SubletCityName, string i_SubletStreetName, int i_SubletHouseNumber, string i_SubletHouseDescription,
             DateTime i_SubletHouseRentStartDate, DateTime i_SubletHouseRentEndDate, string i_SubletHouseImagePath,
            string i_SubletHousePrice)            
        {            
            SubletPostHeader = i_SubletPostHeader;
            SubletCountryName = i_SubletCountryName;
            SubletCityName = i_SubletCityName;
            SubletStreetName = i_SubletStreetName;
            SubletHouseNumber = i_SubletHouseNumber;
            SubletHouseDescription = i_SubletHouseDescription;
            HouseRentStartDate = i_SubletHouseRentStartDate;
            HouseRentEndDate = i_SubletHouseRentEndDate;
            SubletHouseImagePath = i_SubletHouseImagePath;
            SubletHousePrice = i_SubletHousePrice;
        }

        public void Construct(ISubletHousePostBuilder i_SubletHousePostBuilder)
        {            
            i_SubletHousePostBuilder.BuildPostSubletHeader(SubletPostHeader);
            i_SubletHousePostBuilder.BuildSubletCountryName(SubletCountryName);
            i_SubletHousePostBuilder.BuildSubletCityName(SubletCityName);
            i_SubletHousePostBuilder.BuildSubletStreetName(SubletStreetName);
            i_SubletHousePostBuilder.BuildSubletHouseNumber(SubletHouseNumber);
            i_SubletHousePostBuilder.BuildSubletHouseDescription(SubletHouseDescription);
            i_SubletHousePostBuilder.BuildSubletHouseRentStartDate(HouseRentStartDate);
            i_SubletHousePostBuilder.BuildSubletHouseRentEndDate(HouseRentEndDate);
            i_SubletHousePostBuilder.BuildSubletHouseImagePath(SubletHouseImagePath);
            i_SubletHousePostBuilder.BuildSubletHousePrice(SubletHousePrice);
        }
    }
}
