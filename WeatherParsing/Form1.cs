using EntityFramework.Utilities;
using Grib.Api;
using LumenWorks.Framework.IO.Csv;
using Microsoft.Research.Science.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WeatherParsing
{
    public partial class Form1 : Form
    {
        private List<ShipData> shipdata = new List<ShipData> { };

        private WeatherContext db = new WeatherContext();

        public Form1()
        {
            InitializeComponent();
        }

        private void upLoadBtn_Click(object sender, EventArgs e)
        {
            List<string> path = new List<string> { };
            List<string> filename = new List<string> { };
            string dirPath = fileNameBox.Text;

            var wavedataout = WaveParsing(@"c:\gwes00.glo_30m.t18z.grib2");

            if (System.IO.Directory.Exists(dirPath))
            {
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(dirPath);

                foreach (var item in di.GetDirectories())
                {
                    path.Add(dirPath + "\\" + item.Name);
                }

                foreach (var pathname in path)
                {
                    System.IO.DirectoryInfo di2 = new System.IO.DirectoryInfo(pathname);

                    foreach (var fileitem in di2.GetFiles())
                    {
                        //var wavedataout = WaveParsing(@"c:\gwes00.glo_30m.t18z.grib2");

                        EFBatchOperation.For(db, db.WaveWindModels).InsertAll(wavedataout);

                        var count = wavedataout.Count();
                        LogTextBox.AppendText(path + "\\" + fileitem.Name + ".grib2 " + count + " DB Update" + Environment.NewLine);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //LogTextBox.AppendText(fileName + ".grib2 DB Update Start" + Environment.NewLine);

            var oceanout = OceanParsing();

            EFBatchOperation.For(db, db.OceanModels).InsertAll(oceanout);

            var count = oceanout.Count();
            LogTextBox.AppendText(count + " DB Update" + Environment.NewLine);
        }

        private List<OceanModel> OceanParsing()
        {
            List<OceanModel> oceanModel = new List<OceanModel>();

            DataSet ocean = DataSet.Open("2.nc");

            List<Array> box = new List<Array> { };

            foreach (var item in ocean.Variables)
            {
                box.Add(item.GetData());
            }

            float[] tttt = new float[] { };

            var tt = box.ElementAt(0);

            var number = tt.GetLength(0) * tt.GetLength(2) * tt.GetLength(3);
            DateTime[] UTC = new DateTime[number];
            object[] LAT = new object[number];
            object[] LONG = new object[number];
            object[] u = new object[number];
            object[] v = new object[number];

            for (int i = 0; i < tt.GetLength(0); i++)
            {
                for (int j = 0; j < tt.GetLength(2); j++)
                {
                    for (int k = 0; k < tt.GetLength(3); k++)
                    {
                        LONG[i * tt.GetLength(2) * tt.GetLength(3) + j * tt.GetLength(3) + k] = box.ElementAt(2).GetValue(k);

                        LAT[i * tt.GetLength(2) * tt.GetLength(3) + j * tt.GetLength(3) + k] = box.ElementAt(3).GetValue(j);

                        UTC[i * tt.GetLength(2) * tt.GetLength(3) + j * tt.GetLength(3) + k] = new DateTime(2014, 06, 02, 00, 00, 00).AddDays(i * 5);

                        if (Convert.ToString(tt.GetValue(i, 0, j, k)) == "NaN")
                        {
                            v[i * tt.GetLength(2) * tt.GetLength(3) + j * tt.GetLength(3) + k] = 9999;
                        }
                        else
                        {
                            v[i * tt.GetLength(2) * tt.GetLength(3) + j * tt.GetLength(3) + k] = box.ElementAt(0).GetValue(i, 0, j, k);
                        }
                    }
                }
            }

            for (int i = 0; i < tt.GetLength(0); i++)
            {
                for (int j = 0; j < tt.GetLength(2); j++)
                {
                    for (int k = 0; k < tt.GetLength(3); k++)
                    {
                        if (Convert.ToString(tt.GetValue(i, 0, j, k)) == "NaN")
                        {
                            u[i * tt.GetLength(2) * tt.GetLength(3) + j * tt.GetLength(3) + k] = 9999;
                        }
                        else
                        {
                            u[i * tt.GetLength(2) * tt.GetLength(3) + j * tt.GetLength(3) + k] = box.ElementAt(1).GetValue(i, 0, j, k);
                        }
                    }
                }
            }

            for (int i = 0; i < LAT.Length; i++)
            {
                oceanModel.Add(new OceanModel
                {
                    UTC = UTC[i],
                    lat = Convert.ToDouble(LAT[i]),
                    lon = Convert.ToDouble(LONG[i]),
                    Current_UV = Convert.ToDouble(u[i]),
                    Current_VV = Convert.ToDouble(v[i]),
                });
            }

            return oceanModel;
        }

        private List<WaveWindModel> WaveParsing(string path)
        {
            //string path = @"D:\NOAA 데이터\" + fileNameBox.Text +".grib2";

            //using (GribFile file = new GribFile(@"D:\NOAA 데이터\2015020106.grib2"))

            using (GribFile file = new GribFile(path))

            {
                file.Context.EnableMultipleFieldMessages = true;

                DateTime UTC;
                double[] LAT = { };
                double[] LONG = { };
                double[] ICEC = { };
                double[] SWDIR_Seq1 = { };
                double[] SWDIR_Seq2 = { };
                double[] WVDIR = { };
                double[] MWSPER = { };
                double[] SWPER_Seq1 = { };
                double[] SWPER_Seq2 = { };
                double[] WVPER = { };
                double[] DIRPW = { };
                double[] PERPW = { };
                double[] DIRSW = { };
                double[] PERSW = { };
                double[] HTSGW = { };
                double[] SWELL_Seq1 = { };
                double[] SWELL_Seq2 = { };
                double[] WVHGT = { };
                double[] UGRD = { };
                double[] VGRD = { };
                double[] WDIR = { };
                double[] WIND = { };

                List<GribMessage> WaveWind = new List<GribMessage>();

                foreach (GribMessage item in file)
                {
                    if (item.StepRange != "0")
                    {
                        break;
                    }

                    WaveWind.Add(item);
                }

                UTC = WaveWind.ElementAt(1).Time;
                LAT = WaveWind.ElementAt(1).GeoSpatialValues.Select(d => d.Latitude).ToArray();
                LONG = WaveWind.ElementAt(1).GeoSpatialValues.Select(d => d.Longitude).ToArray();
                //WaveWind.ElementAt(4).Values(out ICEC);

                WaveWind.ElementAt(16).Values(out SWDIR_Seq1);
                WaveWind.ElementAt(17).Values(out SWDIR_Seq2);

                WaveWind.ElementAt(7).Values(out WVDIR);
                //WaveWind.ElementAt(6).Values(out MWSPER);
                WaveWind.ElementAt(13).Values(out SWPER_Seq1);
                WaveWind.ElementAt(14).Values(out SWPER_Seq2);

                WaveWind.ElementAt(5).Values(out WVPER);
                WaveWind.ElementAt(8).Values(out DIRPW);
                WaveWind.ElementAt(6).Values(out PERPW);
                WaveWind.ElementAt(15).Values(out DIRSW);
                WaveWind.ElementAt(12).Values(out PERSW);
                WaveWind.ElementAt(4).Values(out HTSGW);
                WaveWind.ElementAt(10).Values(out SWELL_Seq1);
                WaveWind.ElementAt(11).Values(out SWELL_Seq2);

                WaveWind.ElementAt(9).Values(out WVHGT);
                WaveWind.ElementAt(2).Values(out UGRD);
                WaveWind.ElementAt(3).Values(out VGRD);
                WaveWind.ElementAt(1).Values(out WDIR);
                WaveWind.ElementAt(0).Values(out WIND);

                List<WaveWindModel> wavedataout = new List<WaveWindModel> { };

                for (int i = 0; i < LAT.Length; i++)
                {
                    wavedataout.Add(new WaveWindModel
                    {
                        UTC = UTC,
                        lat = LAT[i],
                        lon = LONG[i],
                        //ICEC = ICEC[i],
                        SWDIR_Seq1 = SWDIR_Seq1[i],
                        SWDIR_Seq2 = SWDIR_Seq2[i],
                        WVDIR = WVDIR[i],
                        //MWSPER = MWSPER[i],
                        SWPER_Seq1 = SWPER_Seq1[i],
                        SWPER_Seq2 = SWPER_Seq2[i],
                        WVPER = WVPER[i],
                        DIRPW = DIRPW[i],
                        PERPW = PERPW[i],
                        DIRSW = DIRSW[i],
                        PERSW = PERSW[i],
                        HTSGW = HTSGW[i],
                        SWELL_Seq1 = SWELL_Seq1[i],
                        SWELL_Seq2 = SWELL_Seq2[i],
                        WVHGT = WVHGT[i],
                        UGRD = UGRD[i],
                        VGRD = VGRD[i],
                        WDIR = WDIR[i],
                        WIND = WIND[i]
                    });
                }

                return wavedataout;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog pFileDlg = new OpenFileDialog();
            pFileDlg.Filter = "CSV Files(*.csv)|*.csv|All Files(*.*)|*.*";
            pFileDlg.Title = "조인할 파일을 선택하여 주세요.";
            if (pFileDlg.ShowDialog() == DialogResult.OK)
            {
                String strFullPathFile = pFileDlg.FileName;

                using (CsvReader csv = new CsvReader(
                 new StreamReader(strFullPathFile), false))
                {
                    int d_count = 0;
                    //   string[] headers = csv.GetFieldHeaders();
                   
                }
            }

            this.db.Database.CommandTimeout = 999999;

            //db.Database.ExecuteSqlCommand("set net_write_timeout=99999; set net_read_timeout=99999");

            var wavedata = db.WaveWindModels
                .Where(d => d.UTC >= new DateTime(2014, 06, 01, 00, 00, 00) && d.UTC <= new DateTime(2014, 12, 31, 00, 00, 00));

            var ship = from shipdata in shipdata
                       join wave in wavedata
                       on new { shipdata.UTC } equals new { wave.UTC }
                       select new { shipdata.UTC, wave.VGRD, wave.WIND };
            var outdata = ship.ToList();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog pFileDlg = new OpenFileDialog();
            pFileDlg.Filter = "CSV Files(*.csv)|*.csv|All Files(*.*)|*.*";
            pFileDlg.Title = "업로드 할 파일을 선택하여 주세요.";
            if (pFileDlg.ShowDialog() == DialogResult.OK)
            {
                String strFullPathFile = pFileDlg.FileName;



                //try {
                    using (CsvReader csv = new CsvReader(
                new StreamReader(strFullPathFile), false))
                    {
                        while (csv.ReadNextRecord())
                        {
                            shipdata.Add(new ShipData
                            {
                                shipName                  = csv[0],
                                UTC                       = Convert.ToDateTime(csv[1]),
                                LTC                       = Convert.ToDateTime(csv[2]),
                                Speed_VG                  = csv[3] == "" ? 0 : Convert.ToDouble(csv[3]),
                                Speed_VG_x                = csv[4] == "" ? 0 : Convert.ToDouble(csv[4]),
                                Speed_VG_y                = csv[5] == "" ? 0 : Convert.ToDouble(csv[5]),
                                Speed_VS                  = csv[6] == "" ? 0 : Convert.ToDouble(csv[6]),
                                Speed_VS_x                = csv[7] == "" ? 0 : Convert.ToDouble(csv[7]),
                                Speed_VS_y                = csv[8] == "" ? 0 : Convert.ToDouble(csv[8]),
                                rel_current_speed_Zone1_M = csv[9] == "" ? 0 : Convert.ToDouble(csv[9]),
                                rel_current_dir_Zone1_M   = csv[10] == "" ? 0 : Convert.ToDouble(csv[10]),
                                rel_current_speed_Zone2_M = csv[11] == "" ? 0 : Convert.ToDouble(csv[11]),
                                rel_current_dir_Zone2_M   = csv[12] == "" ? 0 : Convert.ToDouble(csv[12]),
                                abs_current_speed_W       = csv[13] == "" ? 0 : Convert.ToDouble(csv[13]),
                                abs_current_dir_W         = csv[14] == "" ? 0 : Convert.ToDouble(csv[14]),
                                propeller_rpm             = csv[15] == "" ? 0 : Convert.ToDouble(csv[15]),
                                shaft_torque_KNM          = csv[16] == "" ? 0 : Convert.ToDouble(csv[16]),
                                shaft_Power               = csv[17] == "" ? 0 : Convert.ToDouble(csv[17]),
                                brake_Power               = csv[18] == "" ? 0 : Convert.ToDouble(csv[18]),
                                lat_Position              = csv[19] == "" ? 0 : Convert.ToDouble(csv[19]),
                                lat_Position_dir          = csv[20] == "" ? "empty" : Convert.ToString(csv[20]),
                                long_Position             = csv[21] == "" ? 0 : Convert.ToDouble(csv[21]),
                                long_Position_dir         = csv[22] == "" ? "empty" : Convert.ToString(csv[22]),
                                Voyage_Distance           = csv[23] == "" ? 0 : Convert.ToDouble(csv[23]),
                                heading_Gyro              = csv[24] == "" ? 0 : Convert.ToDouble(csv[24]),
                                heading_GPS               = csv[25] == "" ? 0 : Convert.ToDouble(csv[25]),
                                rudder_angle              = csv[26] == "" ? 0 : Convert.ToDouble(csv[26]),
                                seawater_Temp_M           = csv[27] == "" ? 0 : Convert.ToDouble(csv[27]),
                                seawater_Density_M        = csv[28] == "" ? 0 : Convert.ToDouble(csv[28]),
                                rel_wind_speed_M          = csv[29] == "" ? 0 : Convert.ToDouble(csv[29]),
                                rel_wind_direction_M      = csv[30] == "" ? 0 : Convert.ToDouble(csv[30]),
                                abs_wind_speed_W          = csv[31] == "" ? 0 : Convert.ToDouble(csv[31]),
                                abs_wind_direction_W      = csv[32] == "" ? 0 : Convert.ToDouble(csv[32]),
                                wave_height_eye_M         = csv[33] == "" ? 0 : Convert.ToDouble(csv[33]),
                                T_wave_height_M           = csv[34] == "" ? 0 : Convert.ToDouble(csv[34]),
                                T_wave_direction_M        = csv[35] == "" ? 0 : Convert.ToDouble(csv[35]),
                                T_wave_period_M           = csv[36] == "" ? 0 : Convert.ToDouble(csv[36]),
                                L_swell_height_M          = csv[37] == "" ? 0 : Convert.ToDouble(csv[37]),
                                L_swell_direction_M       = csv[38] == "" ? 0 : Convert.ToDouble(csv[38]),
                                L_swell_period_M          = csv[39] == "" ? 0 : Convert.ToDouble(csv[39]),
                                H_sea_height_M            = csv[40] == "" ? 0 : Convert.ToDouble(csv[40]),
                                H_sea_direction_M         = csv[41] == "" ? 0 : Convert.ToDouble(csv[41]),
                                H_sea_period_M            = csv[42] == "" ? 0 : Convert.ToDouble(csv[42]),
                                T_wave_height_W           = csv[43] == "" ? 0 : Convert.ToDouble(csv[43]),
                                T_wave_direction_W        = csv[44] == "" ? 0 : Convert.ToDouble(csv[44]),
                                T_wave_period_W           = csv[45] == "" ? 0 : Convert.ToDouble(csv[45]),
                                L_swell_height_W          = csv[46] == "" ? 0 : Convert.ToDouble(csv[46]),
                                L_swell_direction_W       = csv[47] == "" ? 0 : Convert.ToDouble(csv[47]),
                                L_swell_period_W          = csv[48] == "" ? 0 : Convert.ToDouble(csv[48]),
                                H_sea_height_W            = csv[49] == "" ? 0 : Convert.ToDouble(csv[49]),
                                H_sea_direction_W         = csv[50] == "" ? 0 : Convert.ToDouble(csv[50]),
                                H_sea_period_W            = csv[51] == "" ? 0 : Convert.ToDouble(csv[51]),
                                sea_Depth_M               = csv[52] == "" ? 0 : Convert.ToDouble(csv[52]),
                                sea_depth_W               = csv[53] == "" ? 0 : Convert.ToDouble(csv[53]),
                                fp_drft                   = csv[54] == "" ? 0 : Convert.ToDouble(csv[54]),
                                ap_drft                   = csv[55] == "" ? 0 : Convert.ToDouble(csv[55]),
                                mid_ps_drft               = csv[56] == "" ? 0 : Convert.ToDouble(csv[56]),
                                mid_ss_drft               = csv[57] == "" ? 0 : Convert.ToDouble(csv[57]),
                            });
                        } 
                    }

                var ship = from shipout in shipdata
                            orderby shipout.UTC ascending
                           select shipout;


                foreach (var item in ship) { 

                  
                    int  Weather_Hour = ((item.UTC.Hour*60*60 + item.UTC.Minute*60 + item.UTC.Second) / 23429) * 6;

                    


                    //Weather_Hour = Weather_Hour % 12629 == 0 ? Weather_Hour + 1 : Weather_Hour;
                      
                    item.Weather_Time = new DateTime (item.UTC.Year, item.UTC.Month, item.UTC.Day, Weather_Hour * 3, 0,0);
                }   

                this.db.Database.CommandTimeout = 9999;
                EFBatchOperation.For(db, db.ShipDatas).InsertAll(ship);
                //}
                //catch(Exception t)
                //{
                //    MessageBox.Show(""  + t);
                //}


                }

        }
    }
}