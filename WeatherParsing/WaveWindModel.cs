using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherParsing
{
    public class WaveWindModel
    {

        public Guid Id { get; set; }
        public DateTime UTC { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public double SWDIR { get; set; }
        public double WVDIR { get; set; }
        public double ICEC { get; set; }
        public double MWSPER { get; set; }
        public double SWPER { get; set; }
        public double WVPER { get; set; }
        public double DIRPW { get; set; }
        public double PERPW { get; set; }
        public double HTSGW { get; set; }
        public double SWELL { get; set; }
        public double WVHGT { get; set; }
        public double WDIR { get; set; }
        public double WIND { get; set; }
     
    }
}
