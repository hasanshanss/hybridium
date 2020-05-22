using System;
using System.Collections.Generic;

namespace DbUpdater.Models
{
    public partial class GeoipLocations
    {
        public long GeolocationId { get; set; }
        public long? GeonameId { get; set; }
        public string LocaleCode { get; set; }
        public string ContinentCode { get; set; }
        public string ContinentName { get; set; }
        public string CountryIsoCode { get; set; }
        public string CountryName { get; set; }
        public bool? IsInEuropeanUnion { get; set; }

    }
}
