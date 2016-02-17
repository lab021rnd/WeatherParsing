using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Grib.Api;
using Microsoft.Research.Science.Data;


namespace WeatherParsing
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            List<WaveWindModel> waveWind = new List<WaveWindModel>();
            List<OceanModel> oceanModel = new List<OceanModel>();
            List<double> dd = new List<double> { };

            using (GribFile file = new GribFile(@"C:\Users\이상봉\Desktop\gwes00.glo_30m.t00z.grib2"))
            {
           
                GribMessage msg = file.First();
                DateTime UTC =  Convert.ToDateTime(msg.Time);

                var lat = file.Where(m => m.Name.Contains("Wind speed")).ElementAt(0);        // elementAt은 날짜 지정
                //var vComp = file.Where(m => m.Name.Contains("Wind speed")).ElementAt(0);



                waveWind.Add(new WaveWindModel
                {
                    //UTC = 

                });



                foreach (var val in msg.GeoSpatialValues)
                {

                    //Console.WriteLine("Lat: {0} Lon: {1} Val: {2}", val.Latitude, val.Longitude, val.Value);
                    dd.Add(val.Value);

                }

            }



        }

        public static void parsing()
        {





        }
    }
}
