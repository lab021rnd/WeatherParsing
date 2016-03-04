using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherParsing
{
    public class OceanModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public DateTime UTC { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public double DENSITY { get; set; }
        public double SSS { get; set; }
        public double SST { get; set; }
        public double Current_UV { get; set; }
        public double Current_VV { get; set; }
    }
}
