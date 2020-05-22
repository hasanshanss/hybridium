using DbUpdater.Models;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DbUpdater.Models
{
    public class CsvMappers
    {
        public sealed class GeoipBlocksMap : ClassMap<GeoipBlocks>
        {
            public GeoipBlocksMap()
            {
                AutoMap(CultureInfo.InvariantCulture);
                Map(m => m.GeoblockId).Ignore();
            }
        }

        public sealed class GeoipLocationsMap : ClassMap<GeoipLocations>
        {
            public GeoipLocationsMap()
            {
                AutoMap(CultureInfo.InvariantCulture);
                Map(m => m.GeolocationId).Ignore();
            }
        }
    }
}
