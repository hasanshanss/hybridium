using System;
using System.Collections.Generic;

namespace DbUpdater.Models
{
    public partial class GeoipBlocks
    {
        public long GeoblockId { get; set; }
        public string Network { get; set; }
        public long? GeonameId { get; set; }
        public long? RegisteredCountryGeonameId { get; set; }
        public long? RepresentedCountryGeonameId { get; set; }
        public bool? IsAnonymousProxy { get; set; }
        public bool? IsSatelliteProvider { get; set; }
        public string PostalCode { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? AccuracyRadius { get; set; }

    }
}
