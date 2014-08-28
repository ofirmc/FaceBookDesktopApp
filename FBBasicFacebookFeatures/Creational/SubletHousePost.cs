using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace FacebookFeatures
{
    // This is the Product class
    class SubletHousePost
    {
        public SubletHousePost(eSubletHouseType i_SubletHouseType)
        {
            SubletHouseType = i_SubletHouseType;
        }

        public eSubletHouseType SubletHouseType { get; set; }
        public string SubletPostHeader { get; set; }
        public string SubletCountryName { get; set; }
        public string SubletCityName { get; set; }
        public string SubletStreetName { get; set; }
        public int SubletHouseNumber { get; set; }
        public string SubletHouseDescription { get; set; }
        public DateTime HouseRentStartDate { get; set; }
        public DateTime HouseRentEndDate { get; set; }
        public string SubletHouseImagePath { get; set; }
        public string SubletHousePrice { get; set; }

        public override string ToString()
        {
            return string.Format("My {0} for rent{1}{2}{3}{4} {5}, {6} {7}, {8} {9}, {10} {11}{12}{13} {14}{15} {16} {17} {18} {19}{20}{21} {22}{23}",
                Enum.GetName(typeof(eSubletHouseType), SubletHouseType), Environment.NewLine,
                 SubletPostHeader, Environment.NewLine,
                 "Country:", SubletCountryName,
                "City:", SubletCityName, "Sreet:", SubletStreetName,
                "House Number:", SubletHouseNumber.ToString(), Environment.NewLine,
                "House Description:", SubletHouseDescription, Environment.NewLine,
                 "From:", HouseRentStartDate.ToShortDateString(), "To:", HouseRentEndDate.ToShortDateString(), Environment.NewLine,
                 "Price:", SubletHousePrice, Environment.NewLine);                                                                
        }       
    }
}
