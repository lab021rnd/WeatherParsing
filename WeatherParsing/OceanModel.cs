using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherParsing
{
    public class OceanModel
    {
        public Guid Id { get; set; }
        public DateTime UTC { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public double DENSITY { get; set; }
        public double SSS { get; set; }
        public double SST { get; set; }
        public double Current_UV { get; set; }
        public double Current_VV { get; set; }
    }
}
