using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FacebookFeatures
{
    // This is the Builder class
    interface ISubletHousePostBuilder
    {        
        void BuildPostSubletHeader(string i_PostSubletHeader);
        void BuildSubletCountryName(string i_SubletCountryName);
        void BuildSubletCityName(string i_SubletCityName);
        void BuildSubletStreetName(string i_SubletStreetName);
        void BuildSubletHouseNumber(int i_SubletHouseNumber);
        void BuildSubletHouseDescription(string i_SubletHouseDescription);
        void BuildSubletHouseRentStartDate(DateTime i_SubletHouseRentStartDate);
        void BuildSubletHouseRentEndDate(DateTime i_SubletHouseRentEndDate);
        void BuildSubletHouseImagePath(string i_SubletHouseImagePath);
        void BuildSubletHousePrice(string i_SubletHousePrice);
        SubletHousePost SubletHousePost { get; }
    }
}
