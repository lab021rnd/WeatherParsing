using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherParsing
{
    public class WaveWindModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public DateTime UTC { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public double ICEC { get; set; }
        public double SWDIR_Seq1 { get; set; }
        public double SWDIR_Seq2 { get; set; }
        public double WVDIR { get; set; }
        public double MWSPER { get; set; }
        public double SWPER_Seq1 { get; set; }
        public double SWPER_Seq2 { get; set; }
        public double WVPER { get; set; }

        public double DIRPW { get; set; }
        public double PERPW { get; set; }


        public double DIRSW { get; set; }
        public double PERSW { get; set; }
        public double HTSGW { get; set; }
        public double SWELL_Seq1 { get; set; }
        public double SWELL_Seq2 { get; set; }
        public double WVHGT { get; set; }
        public double UGRD { get; set; }
        public double VGRD { get; set; }
        public double WDIR { get; set; }
        public double WIND { get; set; }
     
    }
}
